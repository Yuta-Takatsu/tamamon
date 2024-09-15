using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class TamamonSelectModel
{
    public int SelectIndex { get; set; } = 0;

    public int PrevSelectIndex { get; set; } = 0;

    public int MaxSelectIndex { get; set; } = 0;

    public bool IsShow { get; set; } = false;

    /// <summary>
    /// “ü—Í‘Ò‹@
    /// </summary>
    /// <returns></returns>
    public async UniTask OnInput()
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
                    if (SelectIndex == MaxSelectIndex)
                    {
                        isDecision = true;
                        break;
                    }
                    SelectIndex = MaxSelectIndex;
                    break;
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (SelectIndex != 0 && SelectIndex != MaxSelectIndex)
                    {
                        SelectIndex = 0;
                    }
                    break;
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (SelectIndex == 0)
                    {
                        SelectIndex = 1;
                    }
                    break;
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (SelectIndex > 0)
                    {
                        SelectIndex--; ;
                    }
                    break;
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (SelectIndex < MaxSelectIndex)
                    {
                        SelectIndex++;
                    }
                    break;
                }
            }
        }
    }
}