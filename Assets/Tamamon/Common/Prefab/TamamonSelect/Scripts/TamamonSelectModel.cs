using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// タマモン選択画面モデルクラス
/// </summary>
public class TamamonSelectModel
{
    public int SelectIndex { get; set; } = 0;

    public int PrevSelectIndex { get; set; } = 0;

    public int MaxSelectIndex { get; set; } = 0;

    public bool IsShow { get; set; } = false;

    /// <summary>
    /// 入力待機
    /// </summary>
    /// <returns></returns>
    public async UniTask OnInput(System.Action onCallback = null)
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
                    PrevSelectIndex = SelectIndex;
                    SelectIndex = MaxSelectIndex;
                    onCallback?.Invoke();
                    break;
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (SelectIndex != 0 && SelectIndex != MaxSelectIndex)
                    {
                        PrevSelectIndex = SelectIndex;
                        SelectIndex = 0;
                        onCallback?.Invoke();
                    }
                    break;
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (SelectIndex == 0)
                    {
                        PrevSelectIndex = SelectIndex;
                        SelectIndex = 1;
                        onCallback?.Invoke();
                    }
                    break;
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (SelectIndex > 0)
                    {
                        PrevSelectIndex = SelectIndex;
                        SelectIndex--;
                        onCallback?.Invoke();
                    }
                    break;
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (SelectIndex < MaxSelectIndex)
                    {
                        PrevSelectIndex = SelectIndex;
                        SelectIndex++;
                        onCallback?.Invoke();
                    }
                    break;
                }
            }
        }
    }
}