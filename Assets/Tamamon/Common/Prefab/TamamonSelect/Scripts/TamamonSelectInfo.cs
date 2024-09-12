using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TamamonSelectInfo : MonoBehaviour
{
    [SerializeField]
    private Image m_iconImage = default;

    [SerializeField]
    private Image m_frameImage = default;

    [SerializeField]
    private TextMeshProUGUI m_nameText = default;

    [SerializeField]
    private TextMeshProUGUI m_levelText = default;

    [SerializeField]
    private TextMeshProUGUI m_sexText = default;

    [SerializeField]
    private TextMeshProUGUI m_hpText = default;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="index"></param>
    /// <param name="name"></param>
    /// <param name="level"></param>
    /// <param name="sexType"></param>
    /// <param name="maxHP"></param>
    /// <param name="nowHP"></param>
    public void OnInitialize(int index, string name, int level, Tamamon.SexType sexType, int maxHP, int nowHP)
    {
        m_nameText.text = name;
        m_levelText.text = $"Lv:{level}";

        switch (sexType)
        {
            case Tamamon.SexType.None:
                m_sexText.text = string.Empty;
                break;

            case Tamamon.SexType.Male:
                m_sexText.text = "♂";
                break;
            case Tamamon.SexType.Female:
                m_sexText.text = "♀";
                break;
        }

        m_hpText.text = $"{nowHP}/{maxHP}";
    }

    /// <summary>
    /// フレームの表示切替
    /// </summary>
    /// <param name="isActive"></param>
    public void SetFrame(bool isActive)
    {
        m_frameImage.gameObject.SetActive(isActive);
    }
}