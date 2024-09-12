using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class TamamonSelectTextWindow : CommandWindowBase
{
    /// <summary>
    /// コマンド選択
    /// </summary>
    /// <returns></returns>
    public override async UniTask<int> SelectCommand()
    {
        await UniTask.WaitUntil(() => Input.anyKeyDown);

        foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                return m_selectIndex;
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                return CloseIndex;
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
        return -1;
    }
}