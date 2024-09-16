using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CommandWindowText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_commandText = default;

    [SerializeField]
    private CanvasGroup m_commandArrowcanvasGroup = new CanvasGroup();

    private Tween m_flashTween = default;

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