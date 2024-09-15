using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Cysharp.Threading.Tasks;

/// <summary>
/// バトル用行動コマンドUIウィンドウ
/// </summary>
public class BattleUICommandTextWindow : CommandWindowBase
{
    /// <summary>
    /// コマンド選択
    /// </summary>
    /// <returns></returns>
    public override async UniTask SelectCommand()
    {
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
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (m_selectIndex == 0 || m_selectIndex == 2) break;

                    m_prevSelectIndex = m_selectIndex;
                    m_selectIndex--;
                    SetArrowActive();
                    break;
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (m_selectIndex == 1 || m_selectIndex == 3) break;
                    if (m_selectIndex + 1 >= m_commandWindowTextList.Count) break;

                    m_prevSelectIndex = m_selectIndex;
                    m_selectIndex++;
                    SetArrowActive();
                    break;
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (m_selectIndex == 0 || m_selectIndex == 1) break;

                    m_prevSelectIndex = m_selectIndex;
                    m_selectIndex -= 2;
                    SetArrowActive();
                    break;
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (m_selectIndex == 2 || m_selectIndex == 3) break;
                    if (m_selectIndex + 2 >= m_commandWindowTextList.Count) break;

                    m_prevSelectIndex = m_selectIndex;
                    m_selectIndex += 2;
                    SetArrowActive();
                    break;
                }
            }
        }
    }
}