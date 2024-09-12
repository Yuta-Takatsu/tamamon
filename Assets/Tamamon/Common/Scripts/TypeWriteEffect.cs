using System;
using Cysharp.Threading.Tasks;
using TMPro;

public class TypeWriteEffect
{

    private bool m_isAnimation = false;
    public bool IsAnimation => m_isAnimation;
    /// <summary>
    /// テキストを一文字ずつ表示
    /// </summary>
    /// <param name="text"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public async UniTask ShowTextMessage(TextMeshProUGUI text, string message)
    {
        m_isAnimation = true;
        foreach (char c in message)
        {
            text.text += c;
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        }
        m_isAnimation = false;
    }
}