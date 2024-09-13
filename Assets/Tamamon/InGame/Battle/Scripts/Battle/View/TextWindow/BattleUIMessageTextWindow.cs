using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;

/// <summary>
/// メッセージ関連テキスト表示クラス
/// </summary>
public class BattleUIMessageTextWindow : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_messageText = default;

    private TypeWriteEffect m_typeWriteEffect = default;


    /// <summary>
    /// 初期化
    /// </summary>
    public void OnInitialize()
    {
        m_typeWriteEffect = new TypeWriteEffect();

        ClearText();
    }

    /// <summary>
    /// テキストクリア
    /// </summary>
    public void ClearText()
    {
        m_messageText.text = string.Empty;
    }

    /// <summary>
    /// テキスト表示
    /// </summary>
    /// <param name="message"></param>
    public void ShowMessageText(string message)
    {
        m_messageText.text = message;
    }

    /// <summary>
    /// テキスト表示(1文字送り)
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public async UniTask ShowMessageTextAsync(string message)
    {
        await m_typeWriteEffect.ShowTextMessage(m_messageText, message);
    }

    /// <summary>
    /// テキスト表示アニメーション再生状況
    /// </summary>
    /// <returns></returns>
    public bool IsMessageAnimation()
    {
        return m_typeWriteEffect.IsAnimation;
    }
}