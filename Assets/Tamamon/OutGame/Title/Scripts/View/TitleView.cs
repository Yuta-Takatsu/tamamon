using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace Tamamon.OutGame.Title
{
    public class TitleView : MonoBehaviour, ITitleView
    {
        [SerializeField]
        private Image m_titleBg = default;

        [SerializeField]
        private GameObject m_titleRoot = default;

        [SerializeField]
        private Image m_titleLogoImage = default;

        [SerializeField]
        private CanvasGroup m_tapTextObj = default;

        [SerializeField]
        private Image m_tamamonImage = default;

        [SerializeField]
        private SpriteAtlas m_tamamonSpriteAtlas = default;
      
        public AudioClip TitleBGM = default;

        public void OnInitialize()
        {
            m_titleBg.gameObject.SetActive(false);
            m_titleRoot.SetActive(false);
        }

        public void OnExecute()
        {
            m_titleBg.gameObject.SetActive(true);
            m_titleRoot.SetActive(true);
            PlayTitleLogoMoveAnimation(m_titleLogoImage);
            PlayTextFadeAnimation(m_tapTextObj);
            PlayTamamonMoveAnimation(m_tamamonImage);
        }

        public void OnFinalize()
        {
            m_titleBg.gameObject.SetActive(false);
            m_titleRoot.SetActive(false);
        }

        private void PlayTitleLogoMoveAnimation(Image titleLogo)
        {
            titleLogo.transform.localPosition = new Vector2(1800f, titleLogo.transform.localPosition.y);
            titleLogo.transform.DOLocalMove(new Vector3(0, titleLogo.transform.localPosition.y, 0), 0.5f).SetEase(Ease.OutBack).SetLink(gameObject);
        }

        private void PlayTextFadeAnimation(CanvasGroup transform)
        {
            transform.alpha = 1f;
            transform.DOFade(0.0f, 1f).SetEase(Ease.InCubic).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);
        }

        private void PlayTamamonMoveAnimation(Image image)
        {
            PlayLeftOutAnimation(image).Forget();
        }

        private async UniTask PlayLeftOutAnimation(Image image)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(3f));

            UnityEngine.Random.InitState(DateTime.Now.Millisecond);
            int id = UnityEngine.Random.Range(1, m_tamamonSpriteAtlas.spriteCount + 1);
            image.sprite = m_tamamonSpriteAtlas.GetSprite($"tamamon_{id}");

            image.transform.localScale = new Vector3(1, 1, 1);
            image.transform.DOLocalJump(new Vector3(-2000f, image.transform.localPosition.y, 0), 50, 20, 7f).OnComplete(async () => await PlayRightOutAnimation(image)).SetLink(gameObject); ;
        }

        private async UniTask PlayRightOutAnimation(Image image)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(3f));

            UnityEngine.Random.InitState(DateTime.Now.Millisecond);
            int id = UnityEngine.Random.Range(1, m_tamamonSpriteAtlas.spriteCount + 1);
            image.sprite = m_tamamonSpriteAtlas.GetSprite($"tamamon_{id}");

            image.transform.localScale = new Vector3(-1, 1, 1);
            image.transform.DOLocalJump(new Vector3(2000f, image.transform.localPosition.y, 0), 50, 20, 5f).OnComplete(async () => await PlayLeftOutAnimation(image)).SetLink(gameObject); ;
        }
    }
}