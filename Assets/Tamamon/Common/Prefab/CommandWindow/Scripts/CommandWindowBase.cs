using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class CommandWindowBase : MonoBehaviour
{
   
    [SerializeField]
    protected int m_commandNum = 0;

    protected int m_selectIndex = 0;

    protected int m_prevSelectIndex = 0;

    protected bool m_isEscapeInput = false;

    protected bool m_isEscape = false;
    public bool IsEscape => m_isEscape;

    public int SelectIndex => m_selectIndex;

    /// <summary>
    /// ������
    /// </summary>
    public virtual void OnInitialize(List<string> comanndTextList, bool isEsCapeInput = false)
    {
        
    }

    /// <summary>
    /// �R�}���h�����X�V
    /// </summary>
    /// <param name="comanndTextList"></param>
    /// <param name="isEsCapeInput"></param>
    public virtual void UpdateCommandText(List<string> comanndTextList, bool isEsCapeInput = false)
    {
        
    }

    /// <summary>
    /// �\��
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Show(System.Action onCallback = null)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));

        onCallback?.Invoke();

        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// ��\��
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Hide(System.Action onCallback = null)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));

        onCallback?.Invoke();

        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// ���������R�}���h���폜
    /// </summary>
    public virtual void OnDestroyCommand()
    {
        
    }

    /// <summary>
    /// �A���[UI�̕\���ؑ�
    /// </summary>
    public virtual void SetArrowActive()
    {
        
    }

    /// <summary>
    /// �A���[UI�̕\���������ɖ߂�
    /// </summary>
    public virtual void ResetArrowActive()
    {
        m_prevSelectIndex = m_selectIndex;
        m_selectIndex = 0;
        SetArrowActive();
    }

    /// <summary>
    /// �R�}���h�I��
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
                
            }
        }
    }
}