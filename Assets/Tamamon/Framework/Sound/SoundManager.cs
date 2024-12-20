using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Audio;
using Cysharp.Threading.Tasks;

namespace Framework
{
    /// <summary>
    /// 音源管理クラス
    /// </summary>
    public class SoundManager : MonoBehaviourSingleton<SoundManager>
    {

        // BGM管理
        public enum BGM_Type
        {
            Title = 0,
            Adventure = 1,
            Battle = 2,
            SILENCE = 999,
        }

        // SE管理
        public enum SE_Type
        {
            
        }

        // クロスフェード時間
        public const float CROSS_FADE_TIME = 1.0f;

        // ボリューム関連
        public float BGM_Volume = 0.1f;
        public float SE_Volume = 0.2f;
        public bool Mute = false;

        // === AudioClip ===
        public AudioClip[] BGM_Clips;
        public AudioClip[] SE_Clips;

        // SE用AudioMixer  未使用
        public AudioMixer audioMixer;


        // === AudioSource ===
        private AudioSource[] BGM_Sources = new AudioSource[2];
        private AudioSource[] SE_Sources = new AudioSource[16];

        private bool isCrossFading;

        private int currentBgmIndex = 999;

        public override void Awake()
        {
            base.Awake();

            // BGM用 AudioSource追加
            BGM_Sources[0] = gameObject.AddComponent<AudioSource>();
            BGM_Sources[1] = gameObject.AddComponent<AudioSource>();

            // SE用 AudioSource追加
            for (int i = 0; i < SE_Sources.Length; i++)
            {
                SE_Sources[i] = gameObject.AddComponent<AudioSource>();
            }
        }

        void Update()
        {
            // ボリューム設定
            if (!isCrossFading)
            {
                BGM_Sources[0].volume = BGM_Volume;
                BGM_Sources[1].volume = BGM_Volume;
            }

            foreach (AudioSource source in SE_Sources)
            {
                source.volume = SE_Volume;
            }
        }

        /// <summary>
        /// BGM再生
        /// </summary>
        /// <param name="bgmType"></param>
        /// <param name="loopFlg"></param>
        public void PlayBGM(BGM_Type bgmType, bool loopFlg = true,bool isCrossFade = true)
        {
            // BGMなしの状態にする場合            
            if ((int)bgmType == 999)
            {
                StopBGM();
                return;
            }

            int index = (int)bgmType;
            currentBgmIndex = index;

            if (index < 0 || BGM_Clips.Length <= index)
            {
                return;
            }

            // 同じBGMの場合は何もしない
            if (BGM_Sources[0].clip != null && BGM_Sources[0].clip == BGM_Clips[index])
            {
                return;
            }
            else if (BGM_Sources[1].clip != null && BGM_Sources[1].clip == BGM_Clips[index])
            {
                return;
            }

            // フェードでBGM開始
            if (BGM_Sources[0].clip == null && BGM_Sources[1].clip == null)
            {
                BGM_Sources[0].loop = loopFlg;
                BGM_Sources[0].clip = BGM_Clips[index];
                BGM_Sources[0].Play();
            }
            else
            {
                if (isCrossFade)
                {
                    // クロスフェード処理
                    CrossFadeChangeBMG(index, loopFlg).Forget();
                }
                else
                {
                    ChangeBGM(index, loopFlg);
                }
            }
        }

        /// <summary>
        /// BGM切り替え
        /// </summary>
        /// <param name="index"></param>
        /// <param name="loopFlg"></param>
        private void ChangeBGM(int index, bool loopFlg)
        {
            if (BGM_Sources[0].clip != null)
            {
                // [0]が再生されている場合、[1]を新しい曲として再生
                BGM_Sources[0].volume = 0;
                BGM_Sources[0].Stop();
                BGM_Sources[0].clip = null;

                BGM_Sources[1].volume = BGM_Volume;
                BGM_Sources[1].clip = BGM_Clips[index];
                BGM_Sources[1].loop = loopFlg;
                BGM_Sources[1].Play();
            }
            else
            {
                // [1]が再生されている場合、[0]を新しい曲として再生
                BGM_Sources[1].volume = 0;
                BGM_Sources[1].Stop();
                BGM_Sources[1].clip = null;

                BGM_Sources[0].volume = BGM_Volume;
                BGM_Sources[0].clip = BGM_Clips[index];
                BGM_Sources[0].loop = loopFlg;
                BGM_Sources[0].Play();
            }
        }

        /// <summary>
        /// BGMのクロスフェード処理
        /// </summary>
        /// <param name="index">AudioClipの番号</param>
        /// <param name="loopFlg">ループ設定。ループしない場合だけfalse指定</param>
        /// <returns></returns>
        private async UniTask CrossFadeChangeBMG(int index, bool loopFlg)
        {
            isCrossFading = true;
            if (BGM_Sources[0].clip != null)
            {
                // [0]が再生されている場合、[0]の音量を徐々に下げて、[1]を新しい曲として再生
                BGM_Sources[1].volume = 0;
                BGM_Sources[1].clip = BGM_Clips[index];
                BGM_Sources[1].loop = loopFlg;
                BGM_Sources[1].Play();
                BGM_Sources[1].DOFade(BGM_Volume, CROSS_FADE_TIME).SetEase(Ease.Linear);
                BGM_Sources[0].DOFade(0, CROSS_FADE_TIME).SetEase(Ease.Linear);

                await UniTask.Delay(TimeSpan.FromSeconds(CROSS_FADE_TIME));

                BGM_Sources[0].Stop();
                BGM_Sources[0].clip = null;
            }
            else
            {
                // [1]が再生されている場合、[1]の音量を徐々に下げて、[0]を新しい曲として再生
                BGM_Sources[0].volume = 0;
                BGM_Sources[0].clip = BGM_Clips[index];
                BGM_Sources[0].loop = loopFlg;
                BGM_Sources[0].Play();
                BGM_Sources[0].DOFade(BGM_Volume, CROSS_FADE_TIME).SetEase(Ease.Linear);
                BGM_Sources[1].DOFade(0, CROSS_FADE_TIME).SetEase(Ease.Linear);

                await UniTask.Delay(TimeSpan.FromSeconds(CROSS_FADE_TIME));

                BGM_Sources[1].Stop();
                BGM_Sources[1].clip = null;
            }
            isCrossFading = false;
        }

        /// <summary>
        /// BGM完全停止
        /// </summary>
        public void StopBGM()
        {
            BGM_Sources[0].Stop();
            BGM_Sources[1].Stop();

            BGM_Sources[0].clip = null;
            BGM_Sources[1].clip = null;
        }
        /// <summary>
        /// BGM完全停止 非同期
        /// </summary>
        public async UniTask StopBGMAsync()
        {
            BGM_Sources[0].DOFade(0, CROSS_FADE_TIME).SetEase(Ease.Linear);
            BGM_Sources[1].DOFade(0, CROSS_FADE_TIME).SetEase(Ease.Linear);

            await UniTask.Delay(TimeSpan.FromSeconds(CROSS_FADE_TIME));

            BGM_Sources[0].volume = 0;
            BGM_Sources[1].volume = 0;

            BGM_Sources[0].Stop();
            BGM_Sources[1].Stop();

            BGM_Sources[0].clip = null;
            BGM_Sources[1].clip = null;
        }

        /// <summary>
        /// SE再生
        /// </summary>
        /// <param name="seType"></param>
        public void PlaySE(SE_Type seType)
        {
            int index = (int)seType;
            if (index < 0 || SE_Clips.Length <= index)
            {
                return;
            }

            // 再生中ではないAudioSourceをつかってSEを鳴らす
            foreach (AudioSource source in SE_Sources)
            {

                // 再生中の AudioSource の場合には次のループ処理へ移る
                if (source.isPlaying)
                {
                    continue;
                }

                // 再生中でない AudioSource に Clip をセットして SE を鳴らす
                source.clip = SE_Clips[index];
                source.Play();
                break;
            }
        }

        /// <summary>
        /// SE停止
        /// </summary>
        public void StopSE()
        {
            // 全てのSE用のAudioSourceを停止する
            foreach (AudioSource source in SE_Sources)
            {
                source.Stop();
                source.clip = null;
            }
        }

        /// <summary>
        /// BGM一時停止
        /// </summary>
        public void MuteBGM()
        {
            BGM_Sources[0].Stop();
            BGM_Sources[1].Stop();
        }

        /// <summary>
        /// BGMの更新
        /// </summary>
        /// <param name="bgmType"></param>
        /// <param name="audioClip"></param>
        public void UpdateBGM(BGM_Type bgmType, AudioClip audioClip)
        {
            BGM_Clips[(int)bgmType] = audioClip;
        }

        /// <summary>
        /// 一時停止した同じBGMを再生(再開)
        /// </summary>
        public void ResumeBGM()
        {
            BGM_Sources[0].Play();
            BGM_Sources[1].Play();
        }

        /// <summary>
        /// AudioMixer設定
        /// </summary>
        /// <param name="vol"></param>
        public void SetAudioMixerVolume(float vol)
        {
            if (vol == 0)
            {
                audioMixer.SetFloat("volumeSE", -80);
            }
            else
            {
                audioMixer.SetFloat("volumeSE", 0);
            }
        }
    }
}