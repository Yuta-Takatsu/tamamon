using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;

/// <summary>
/// ���b�Z�[�W�֘A�e�L�X�g�\���N���X
/// </summary>
public class BattleUIMessageTextWindow : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_messageText = default;

    private TypeWriteEffect m_typeWriteEffect = default;


    /// <summary>
    /// ������
    /// </summary>
    public void OnInitialize()
    {
        m_typeWriteEffect = new TypeWriteEffect();

        ClearText();
    }

    /// <summary>
    /// �e�L�X�g�N���A
    /// </summary>
    public void ClearText()
    {
        m_messageText.text = string.Empty;
    }

    /// <summary>
    /// �e�L�X�g�\��
    /// </summary>
    /// <param name="message"></param>
    public void ShowMessageText(string message)
    {
        m_messageText.text = message;
    }

    /// <summary>
    /// �e�L�X�g�\��(1��������)
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public async UniTask ShowMessageTextAsync(string message)
    {
        await m_typeWriteEffect.ShowTextMessage(m_messageText, message);
    }

    /// <summary>
    /// �e�L�X�g�\���A�j���[�V�����Đ���
    /// </summary>
    /// <returns></returns>
    public bool IsMessageAnimation()
    {
        return m_typeWriteEffect.IsAnimation;
    }
}