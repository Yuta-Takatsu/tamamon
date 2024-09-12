using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

/// <summary>
/// 行動コマンドUIウィンドウ
/// </summary>
public class BattleUICommandTextWindow : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> m_commandTextList = new List<TextMeshProUGUI>(); 
    [SerializeField]
    private List<CanvasGroup> m_arrowUIObjectList = new List<CanvasGroup>();

    private CanvasGroup m_prevArrowUIObject = default;
    /// <summary>
    /// 初期化
    /// </summary>
    public void OnInitialize(List<string> comanndTextList)
    {
        int index = 0;
        foreach (var command in m_commandTextList)
        {
            if (m_commandTextList[index] != null)
            {
                command.text = comanndTextList[index];
            }
            else
            {
                command.text = "-";
            }
            index++;
        }

        foreach (var obj in m_arrowUIObjectList)
        {
            obj.gameObject.SetActive(false);
            PlayFlashAnimation(obj);
        }

        ShowArrowUI(0);
    }

    /// <summary>
    /// 矢印UI表示
    /// </summary>
    /// <param name="index"></param>
    public void ShowArrowUI(int index)
    {
        if (index > m_arrowUIObjectList.Count || index < 0) return;

        m_arrowUIObjectList[index].gameObject.SetActive(true);

        if (m_prevArrowUIObject != null)
        {
            m_prevArrowUIObject.gameObject.SetActive(false);
        }

        m_prevArrowUIObject = m_arrowUIObjectList[index];
    }

    public void PlayFlashAnimation(CanvasGroup obj)
    {
        obj.DOFade(0.0f, 1f).SetEase(Ease.InCubic).SetLoops(-1, LoopType.Restart);
    }
}