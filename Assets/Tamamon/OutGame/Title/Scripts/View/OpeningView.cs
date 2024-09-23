using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace Tamamon.OutGame.Title
{
    public class OpeningView : MonoBehaviour, ITitleView
    {
        [SerializeField]
        private Image m_openingBg = default;

        [SerializeField]
        private GameObject m_openingRoot = default;

        [SerializeField]
        private CanvasGroup m_presentsText = default;

        [SerializeField]
        private CanvasGroup m_openingImage = default;

        private System.Action m_nextStateCallback = null;

        public void OnInitialize()
        {
            m_openingBg.gameObject.SetActive(true);
            m_openingRoot.SetActive(true);
            m_openingImage.alpha = 0f;
        }
        public void OnExecute()
        {
            UniTask.Void(async () =>
            {
                await PlayFadeInAnimation(m_presentsText, 2.0f);
                await PlayFadeOutAnimation(m_presentsText, 2.0f);

                await PlayFadeInAnimation(m_openingImage, 5.0f);

                await UniTask.Delay(TimeSpan.FromSeconds(3.0f));

                await PlayOpeningScaleAnimation(m_openingImage.transform);
                NextState();
            });
        }

        public void OnFinalize()
        {
            m_openingBg.gameObject.SetActive(false);
            m_openingRoot.SetActive(false);
        }

        public void SetCallback(System.Action nextStateCallback)
        {
            m_nextStateCallback = nextStateCallback;
        }

        public void NextState()
        {
            m_nextStateCallback?.Invoke();
        }

        private bool m_isAnimation = false;
        private async UniTask PlayFadeInAnimation(CanvasGroup transform, float time)
        {
            m_isAnimation = false;
            transform.alpha = 0f;
            transform.DOFade(1.0f, time).SetEase(Ease.InCubic).OnComplete(() => m_isAnimation = true).SetLink(gameObject);

            await UniTask.WaitUntil(() => m_isAnimation);
        }

        private async UniTask PlayFadeOutAnimation(CanvasGroup transform, float time)
        {
            m_isAnimation = false;
            transform.alpha = 1f;
            transform.DOFade(0.0f, time).SetEase(Ease.InCubic).OnComplete(() => m_isAnimation = true).SetLink(gameObject);

            await UniTask.WaitUntil(() => m_isAnimation);
        }

        private async UniTask PlayOpeningScaleAnimation(Transform transform)
        {
            m_isAnimation = false;
            transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            transform.DOScale(new Vector3(2.0f, 2.0f, 1.0f), 0.5f).SetEase(Ease.OutBack).OnComplete(() => m_isAnimation = true).SetLink(gameObject);

            await UniTask.WaitUntil(() => m_isAnimation);
        }
    }
}