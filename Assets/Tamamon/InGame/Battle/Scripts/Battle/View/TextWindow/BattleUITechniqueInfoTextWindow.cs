using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// �Z���e�L�X�g�\���N���X
/// </summary>
public class BattleUITechniqueInfoTextWindow : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_ppValueText = default;

    [SerializeField]
    private TextMeshProUGUI m_typeText = default;

    /// <summary>
    /// �e�L�X�g�\��
    /// </summary>
    public void ShowText(int maxPP, int nowPP, string type)
    {
        m_ppValueText.text = $"{nowPP}/{maxPP}";
        m_typeText.text = type;
    }
}