using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Audio;
using Cysharp.Threading.Tasks;

namespace Tamamon.Framework
{
    /// <summary>
    /// �����Ǘ��N���X
    /// </summary>
    public class SoundManager : MonoBehaviourSingleton<SoundManager>
    {

        // BGM�Ǘ�
        public enum BGM_Type
        {
            Title = 0,
            Adventure = 1,
            Battle = 2,
            SILENCE = 999,
        }

        // SE�Ǘ�
        public enum SE_Type
        {
            
        }

        // �N���X�t�F�[�h����
        public const float CROSS_FADE_TIME = 1.0f;

        // �{�����[���֘A
        public float BGM_Volume = 0.1f;
        public float SE_Volume = 0.2f;
        public bool Mute = false;

        // === AudioClip ===
        public AudioClip[] BGM_Clips;
        public AudioClip[] SE_Clips;

        // SE�pAudioMixer  ���g�p
        public AudioMixer audioMixer;


        // === AudioSource ===
        private AudioSource[] BGM_Sources = new AudioSource[2];
        private AudioSource[] SE_Sources = new AudioSource[16];

        private bool isCrossFading;

        private int currentBgmIndex = 999;

        public override void Awake()
        {
            base.Awake();

            // BGM�p AudioSource�ǉ�
            BGM_Sources[0] = gameObject.AddComponent<AudioSource>();
            BGM_Sources[1] = gameObject.AddComponent<AudioSource>();

            // SE�p AudioSource�ǉ�
            for (int i = 0; i < SE_Sources.Length; i++)
            {
                SE_Sources[i] = gameObject.AddComponent<AudioSource>();
            }
        }

        void Update()
        {
            // �{�����[���ݒ�
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
        /// BGM�Đ�
        /// </summary>
        /// <param name="bgmType"></param>
        /// <param name="loopFlg"></param>
        public void PlayBGM(BGM_Type bgmType, bool loopFlg = true)
        {
            // BGM�Ȃ��̏�Ԃɂ���ꍇ            
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

            // ����BGM�̏ꍇ�͉������Ȃ�
            if (BGM_Sources[0].clip != null && BGM_Sources[0].clip == BGM_Clips[index])
            {
                return;
            }
            else if (BGM_Sources[1].clip != null && BGM_Sources[1].clip == BGM_Clips[index])
            {
                return;
            }

            // �t�F�[�h��BGM�J�n
            if (BGM_Sources[0].clip == null && BGM_Sources[1].clip == null)
            {
                BGM_Sources[0].loop = loopFlg;
                BGM_Sources[0].clip = BGM_Clips[index];
                BGM_Sources[0].Play();
            }
            else
            {
                // �N���X�t�F�[�h����
                CrossFadeChangeBMG(index, loopFlg).Forget();
            }
        }

        /// <summary>
        /// BGM�̃N���X�t�F�[�h����
        /// </summary>
        /// <param name="index">AudioClip�̔ԍ�</param>
        /// <param name="loopFlg">���[�v�ݒ�B���[�v���Ȃ��ꍇ����false�w��</param>
        /// <returns></returns>
        private async UniTask CrossFadeChangeBMG(int index, bool loopFlg)
        {
            isCrossFading = true;
            if (BGM_Sources[0].clip != null)
            {
                // [0]���Đ�����Ă���ꍇ�A[0]�̉��ʂ����X�ɉ����āA[1]��V�����ȂƂ��čĐ�
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
                // [1]���Đ�����Ă���ꍇ�A[1]�̉��ʂ����X�ɉ����āA[0]��V�����ȂƂ��čĐ�
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
        /// BGM���S��~
        /// </summary>
        public void StopBGM()
        {
            BGM_Sources[0].Stop();
            BGM_Sources[1].Stop();
            BGM_Sources[0].clip = null;
            BGM_Sources[1].clip = null;
        }

        /// <summary>
        /// SE�Đ�
        /// </summary>
        /// <param name="seType"></param>
        public void PlaySE(SE_Type seType)
        {
            int index = (int)seType;
            if (index < 0 || SE_Clips.Length <= index)
            {
                return;
            }

            // �Đ����ł͂Ȃ�AudioSource��������SE��炷
            foreach (AudioSource source in SE_Sources)
            {

                // �Đ����� AudioSource �̏ꍇ�ɂ͎��̃��[�v�����ֈڂ�
                if (source.isPlaying)
                {
                    continue;
                }

                // �Đ����łȂ� AudioSource �� Clip ���Z�b�g���� SE ��炷
                source.clip = SE_Clips[index];
                source.Play();
                break;
            }
        }

        /// <summary>
        /// SE��~
        /// </summary>
        public void StopSE()
        {
            // �S�Ă�SE�p��AudioSource���~����
            foreach (AudioSource source in SE_Sources)
            {
                source.Stop();
                source.clip = null;
            }
        }

        /// <summary>
        /// BGM�ꎞ��~
        /// </summary>
        public void MuteBGM()
        {
            BGM_Sources[0].Stop();
            BGM_Sources[1].Stop();
        }

        /// <summary>
        /// �ꎞ��~��������BGM���Đ�(�ĊJ)
        /// </summary>
        public void ResumeBGM()
        {
            BGM_Sources[0].Play();
            BGM_Sources[1].Play();
        }


        ////* ���g�p *////


        /// <summary>
        /// AudioMixer�ݒ�
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