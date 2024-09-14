using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class TamamonSelectTextWindow : CommandWindowBase
{
    /// <summary>
    /// �R�}���h�I��
    /// </summary>
    /// <returns></returns>
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
}