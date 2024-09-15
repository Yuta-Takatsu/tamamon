using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Cysharp.Threading.Tasks;

public class CommandWindowBase : MonoBehaviour
{
    [SerializeField]
    protected List<TextMeshProUGUI> m_commandTextList = new List<TextMeshProUGUI>();
    [SerializeField]
    protected List<CanvasGroup> m_arrowUIObjectList = new List<CanvasGroup>();

    protected int m_selectIndex = 0;
    public int SelectIndex => m_selectIndex;

    protected bool m_isEscape = false;
    public bool IsEscape => m_isEscape;

    protected int m_commandNum = 0;
    protected CanvasGroup m_prevArrowUIObject = default;

    private Tween m_flashTween = default;

    /// <summary>
    /// 初期化
    /// </summary>
    public virtual void OnInitialize(List<string> comanndTextList)
    {
        int index = 0;
        m_commandNum = comanndTextList.Count;
        foreach (var command in m_commandTextList)
        {
            if (index < m_commandNum)
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
            if (m_flashTween == null)
            {
                PlayFlashAnimation(obj);
            }
        }

        ShowArrowUI(0);
    }

    /// <summary>
    /// 矢印UI表示
    /// </summary>
    /// <param name="index"></param>
    public virtual void ShowArrowUI(int index)
    {
        if (index >= m_commandNum || index < 0) return;

        if (m_prevArrowUIObject != null)
        {
            m_prevArrowUIObject.gameObject.SetActive(false);
        }

        m_arrowUIObjectList[index].gameObject.SetActive(true);

        m_prevArrowUIObject = m_arrowUIObjectList[index];
        m_selectIndex = index;
    }

    /// <summary>
    /// コマンド選択
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask<bool> SelectCommand()
    {
        await UniTask.WaitUntil(() => Input.anyKeyDown);

        foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ShowArrowUI(m_selectIndex - 1);
                break;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ShowArrowUI(m_selectIndex + 1);
                break;
            }
        }
        return false;
    }

    /// <summary>
    /// 点滅アニメーション
    /// </summary>
    /// <param name="obj"></param>
    public virtual void PlayFlashAnimation(CanvasGroup obj)
    {
        obj.alpha = 1.0f;
        m_flashTween = obj.DOFade(0.0f, 1f).SetEase(Ease.InCubic).SetLoops(-1, LoopType.Restart).SetLink(gameObject);
    }
}