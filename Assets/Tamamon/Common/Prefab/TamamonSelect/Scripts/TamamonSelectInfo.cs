using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
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

    [SerializeField]
    private SpriteAtlas m_tamamonSpriteAtlas = default;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="level"></param>
    /// <param name="sexType"></param>
    /// <param name="maxHP"></param>
    /// <param name="nowHP"></param>
    public void OnInitialize(int id, string name, int level, TamamonData.SexType sexType, int maxHP, int nowHP)
    {
        m_iconImage.sprite = m_tamamonSpriteAtlas.GetSprite($"tamamon_{id}");
        m_nameText.text = name;
        m_levelText.text = $"Lv:{level}";

        switch (sexType)
        {
            case TamamonData.SexType.None:
                m_sexText.text = string.Empty;
                break;

            case TamamonData.SexType.Male:
                m_sexText.text = "♂";
                break;
            case TamamonData.SexType.Female:
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