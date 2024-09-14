using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using Tamamon.Framework;

namespace Tamamon.OutGame
{
    public class TitleView : MonoBehaviour
    {
        [SerializeField]
        private Image m_titleLogoImage = default;

        [SerializeField]
        private CanvasGroup m_tapTextObj = default;

        [SerializeField]
        private Image m_tamamonImage = default;

        [SerializeField]
        private SpriteAtlas m_tamamonSpriteAtlas = default;


        public async UniTask OnInitialize()
        {
            m_titleLogoImage.transform.localPosition = new Vector2(1800f, m_titleLogoImage.transform.localPosition.y);

            ChangeScene().Forget();

            await UniTask.Delay(TimeSpan.FromSeconds(13f));

            m_titleLogoImage.transform.DOLocalMove(new Vector3(0, m_titleLogoImage.transform.localPosition.y, 0), 0.5f).SetEase(Ease.OutBack).SetLink(gameObject);

            m_tapTextObj.alpha = 1f;
            m_tapTextObj.DOFade(0.0f, 1f).SetEase(Ease.InCubic).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject); ;

            await PlayLeftOutAnimation();
        }

        public async UniTask ChangeScene()
        {
            await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
            await SceneManager.Instance.LoadSceneAsync("Battle",UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }

        public async UniTask PlayLeftOutAnimation()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(3f));

            UnityEngine.Random.InitState(DateTime.Now.Millisecond);
            int id = UnityEngine.Random.Range(1, m_tamamonSpriteAtlas.spriteCount + 1);        
            m_tamamonImage.sprite = m_tamamonSpriteAtlas.GetSprite($"tamamon_{id}");

            m_tamamonImage.transform.localScale = new Vector3(1, 1, 1);
            m_tamamonImage.transform.DOLocalJump(new Vector3(-2000f, m_tamamonImage.transform.localPosition.y, 0), 50, 20, 7f).OnComplete(async () => await PlayRightOutAnimation()).SetLink(gameObject); ;
        }

        public async UniTask PlayRightOutAnimation()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(3f));

            UnityEngine.Random.InitState(DateTime.Now.Millisecond);
            int id = UnityEngine.Random.Range(1, m_tamamonSpriteAtlas.spriteCount + 1);
            m_tamamonImage.sprite = m_tamamonSpriteAtlas.GetSprite($"tamamon_{id}");

            m_tamamonImage.transform.localScale = new Vector3(-1, 1, 1);
            m_tamamonImage.transform.DOLocalJump(new Vector3(2000f, m_tamamonImage.transform.localPosition.y, 0), 50, 20, 5f).OnComplete(async () => await PlayLeftOutAnimation()).SetLink(gameObject); ;
        }
    }
}