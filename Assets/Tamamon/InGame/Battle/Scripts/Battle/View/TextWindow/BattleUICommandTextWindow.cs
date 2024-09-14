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
    public override async UniTask<bool> SelectCommand()
    {
        await UniTask.WaitUntil(() => Input.anyKeyDown);

        foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (m_selectIndex == 0 || m_selectIndex == 2) break;

                ShowArrowUI(m_selectIndex - 1);
                break;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (m_selectIndex == 1 || m_selectIndex == 3) break;

                ShowArrowUI(m_selectIndex + 1);
                break;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ShowArrowUI(m_selectIndex - 2);
                break;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ShowArrowUI(m_selectIndex + 2);
                break;
            }
        }
        return false;
    }
}