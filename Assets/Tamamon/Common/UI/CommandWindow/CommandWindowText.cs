using UnityEngine;
using UnityEngine.Pool;
using TMPro;
using DG.Tweening;
using Framework;

namespace Tamamon.UI
{
    public class CommandWindowText : MonoBehaviour, IPool<CommandWindowText>
    {
        [SerializeField]
        private TextMeshProUGUI m_commandText = default;

        [SerializeField]
        private CanvasGroup m_commandArrowcanvasGroup = new CanvasGroup();

        private Tween m_flashTween = default;

        private IObjectPool<CommandWindowText> m_objectPool;
        public IObjectPool<CommandWindowText> ObjectPool { set => m_objectPool = value; }

        /// <summary>
        /// 初期化
        /// </summary>
        public void OnInitialize()
        {

        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="commandText"></param>
        public void OnInitialize(string commandText)
        {
            m_commandText.text = commandText;

            SetActiveArrow(false);

            if (m_flashTween == null)
            {
                PlayFlashAnimation();
            }
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void OnFinalize()
        {
            m_objectPool.Release(this);
        }

        /// <summary>
        /// アローUIの表示切替
        /// </summary>
        /// <param name="isActive"></param>
        public void SetActiveArrow(bool isActive)
        {
            m_commandArrowcanvasGroup.gameObject.SetActive(isActive);
        }

        /// <summary>
        /// 点滅アニメーション
        /// </summary>
        /// <param name="obj"></param>
        public void PlayFlashAnimation()
        {
            m_commandArrowcanvasGroup.alpha = 1.0f;
            m_flashTween = m_commandArrowcanvasGroup.DOFade(0.0f, 1f).SetEase(Ease.InCubic).SetLoops(-1, LoopType.Restart).SetLink(gameObject);
        }
    }
}