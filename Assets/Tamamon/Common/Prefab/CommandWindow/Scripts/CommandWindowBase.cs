using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class CommandWindowBase : MonoBehaviour
{
    [SerializeField]
    protected CommandWindowText m_commandWindowText = default;

    [SerializeField]
    protected int m_commandNum = 0;

    protected int m_selectIndex = 0;

    protected int m_prevSelectIndex = 0;

    protected bool m_isEscapeInput = false;

    protected bool m_isEscape = false;
    public bool IsEscape => m_isEscape;

    protected List<CommandWindowText> m_commandWindowTextList = new List<CommandWindowText>();
    protected List<CommandWindowText> m_commandObjectList = new List<CommandWindowText>();
    public int SelectIndex => m_selectIndex;

    /// <summary>
    /// 初期化
    /// </summary>
    public virtual void OnInitialize(List<string> comanndTextList, bool isEsCapeInput = false)
    {
        m_isEscapeInput = isEsCapeInput;

        if (m_commandNum == 0)
        {
            foreach (var text in comanndTextList)
            {
                CommandWindowText commandWindowText = Instantiate(m_commandWindowText, m_commandWindowText.transform.parent);
                commandWindowText.OnInitialize(text);
                m_commandWindowTextList.Add(commandWindowText);
            }
        }
        else
        {
            for (int i = 0; i < m_commandNum; i++)
            {
                CommandWindowText commandWindowText = Instantiate(m_commandWindowText, m_commandWindowText.transform.parent);
                if (i < comanndTextList.Count)
                {
                    commandWindowText.OnInitialize(comanndTextList[i]);
                    m_commandWindowTextList.Add(commandWindowText);
                    m_commandObjectList.Add(commandWindowText);
                }
                else
                {
                    commandWindowText.OnInitialize("-");
                    m_commandObjectList.Add(commandWindowText);
                }
            }
        }
        ResetArrowActive();

        m_commandWindowText.gameObject.SetActive(false);
    }

    /// <summary>
    /// コマンド情報を更新
    /// </summary>
    /// <param name="comanndTextList"></param>
    /// <param name="isEsCapeInput"></param>
    public virtual void UpdateCommandText(List<string> comanndTextList, bool isEsCapeInput = false)
    {
        m_isEscapeInput = isEsCapeInput;

        for (int i = 0; i < comanndTextList.Count; i++)
        {
            if (i < m_commandWindowTextList.Count)
            {
                m_commandWindowTextList[i].OnInitialize(comanndTextList[i]);
            }
            else
            {
                CommandWindowText commandWindowText = m_commandObjectList[i];
                commandWindowText.OnInitialize(comanndTextList[i]);
                commandWindowText.gameObject.SetActive(true);
                m_commandWindowTextList.Add(commandWindowText);
            }
        }

        for (int i = m_commandWindowTextList.Count - 1; i > -1; i--)
        {
            if (i >= comanndTextList.Count)
            {
                m_commandWindowTextList[i].OnInitialize("-");
                m_commandWindowTextList.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// 表示
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Show(System.Action onCallback = null)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));

        onCallback?.Invoke();

        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// 非表示
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Hide(System.Action onCallback = null)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));

        onCallback?.Invoke();

        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 生成したコマンドを削除
    /// </summary>
    public virtual void OnDestroyCommand()
    {
        foreach(var obj in m_commandWindowTextList)
        {
            Destroy(obj.gameObject);
        }
        m_commandWindowTextList.Clear();
    }

    /// <summary>
    /// アローUIの表示切替
    /// </summary>
    public virtual void SetArrowActive()
    {
        if (m_prevSelectIndex < m_commandWindowTextList.Count)
        {
            m_commandWindowTextList[m_prevSelectIndex].SetActiveArrow(false);
        }
        m_commandWindowTextList[m_selectIndex].SetActiveArrow(true);
    }

    /// <summary>
    /// アローUIの表示を初期に戻す
    /// </summary>
    public virtual void ResetArrowActive()
    {
        m_prevSelectIndex = m_selectIndex;
        m_selectIndex = 0;
        SetArrowActive();
    }

    /// <summary>
    /// コマンド選択
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask SelectCommand()
    {
        m_isEscape = false;
        bool isDecision = false;

        while (!isDecision)
        {
            await UniTask.WaitUntil(() => Input.anyKeyDown);

            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    isDecision = true;
                    break;
                }
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (m_isEscapeInput)
                    {
                        m_isEscape = true;
                        isDecision = true;
                    }
                    break;
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (m_selectIndex > 0)
                    {
                        m_prevSelectIndex = m_selectIndex;
                        m_selectIndex--;
                        SetArrowActive();
                    }
                    break;
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (m_selectIndex < m_commandWindowTextList.Count)
                    {
                        m_prevSelectIndex = m_selectIndex;
                        m_selectIndex++;
                        SetArrowActive();
                    }
                    break;
                }
            }
        }
    }
}