using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace Framework
{
    /// <summary>
    /// フェード管理クラス
    /// </summary>
    public class FadeManager : MonoBehaviourSingleton<FadeManager>
    {
        [SerializeField]
        private CanvasGroup m_fadePanel = default;

        private bool m_isFade = false;
        private readonly float FadeTime = 1.5f;

        public enum FadeType
        {
            None,
        }

        public async UniTask FadeIn(FadeType fadeType = FadeType.None)
        {
            m_isFade = true;
            m_fadePanel.gameObject.SetActive(true);

            if (fadeType == FadeType.None)
            {
                m_fadePanel.alpha = 0f;
                m_fadePanel.DOFade(1f, FadeTime)
                    .OnComplete(() =>
                    {
                        m_fadePanel.alpha = 1f;
                        m_isFade = false;
                    });
            }

            await UniTask.WaitWhile(() => m_isFade);
        }

        public async UniTask FadeOut(FadeType fadeType = FadeType.None)
        {
            m_isFade = true;

            if (fadeType == FadeType.None)
            {
                m_fadePanel.alpha = 1f;
                m_fadePanel.DOFade(0f, FadeTime)
                    .OnComplete(() =>
                    {
                        m_fadePanel.gameObject.SetActive(false);
                        m_fadePanel.alpha = 0f;
                        m_isFade = false;
                    });
            }

            await UniTask.WaitWhile(() => m_isFade);
        }
    }
}