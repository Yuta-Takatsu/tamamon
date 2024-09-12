using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUIView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_enemyNameText = default;
    [SerializeField]
    private TextMeshProUGUI m_enemySexText = default;
    [SerializeField]
    private TextMeshProUGUI m_enemyLevelText = default;

    [SerializeField]
    private RectMask2D m_enemyHpBar = default;

    [SerializeField]
    private TextMeshProUGUI m_playerNameText = default;
    [SerializeField]
    private TextMeshProUGUI m_playerSexText = default;
    [SerializeField]
    private TextMeshProUGUI m_playerLevelText = default;
    [SerializeField]
    private TextMeshProUGUI m_playerHpText = default;

    [SerializeField]
    private RectMask2D m_playerHpBar = default;
    [SerializeField]
    private RectMask2D m_playerExpBar = default;

    [SerializeField]
    private TextMeshProUGUI m_messageText = default;


    private readonly int MaxHpAdjustValue = 80;
    private readonly int MinHpAdjustValue = 312;

    private readonly int MaxExpAdjustValue = 67;
    private readonly int MinExpAdjustValue = 369;

    public void SetEnemyUI(string name, Tamamon.SexType sexType, int level, int maxHP, int nowHP)
    {
        m_enemyNameText.text = name;

        switch (sexType)
        {
            case Tamamon.SexType.None:
                m_enemySexText.text = string.Empty;
                break;

            case Tamamon.SexType.Male:
                m_enemySexText.text = "Åâ";
                break;
            case Tamamon.SexType.Female:
                m_enemySexText.text = "Åä";
                break;
        }

        m_enemyLevelText.text = $"Lv:{level}";

        SetEnemyHpBar(maxHP, nowHP);
    }

    public void SetPlayerUI(string name, Tamamon.SexType sexType, int level, int maxExp, int nowExp, int maxHP, int nowHP)
    {
        m_playerNameText.text = name;

        switch (sexType)
        {
            case Tamamon.SexType.None:
                m_playerSexText.text = string.Empty;
                break;

            case Tamamon.SexType.Male:
                m_playerSexText.text = "Åâ";
                break;
            case Tamamon.SexType.Female:
                m_playerSexText.text = "Åä";
                break;
        }

        m_playerLevelText.text = $"Lv:{level}";

        m_playerHpText.text = $"{nowHP}/{maxHP}";

        SetPlayerExpBar(maxExp, nowExp);
        SetPlayerHpBar(maxHP, nowHP);
    }

    public void SetEnemyHpBar(int maxHP, int nowHP)
    {
        // HPäÑçáåvéZ
        float value = MinHpAdjustValue - MaxHpAdjustValue;
        if (maxHP == nowHP)
        {
            value = MaxHpAdjustValue;
        }
        else
        {
            float ratio = 1f - ((float)nowHP / (float)maxHP);
            value = value * ratio + MaxHpAdjustValue;
        }
        m_enemyHpBar.padding = new Vector4(0, 0, value, 0);
    }

    public void SetPlayerHpBar(int maxHP, int nowHP)
    {
        // HPäÑçáåvéZ
        float value = MinHpAdjustValue - MaxHpAdjustValue;
        if (maxHP == nowHP)
        {
            value = MaxHpAdjustValue;
        }
        else
        {
            float ratio = 1f - ((float)nowHP / (float)maxHP);
            value = value * ratio + MaxHpAdjustValue;
        }
        m_playerHpBar.padding = new Vector4(0, 0, value, 0);
    }

    public void SetPlayerExpBar(int maxExp, int nowExp)
    {
        // ExpäÑçáåvéZ
        float value = MinExpAdjustValue - MaxExpAdjustValue;
        if (maxExp == nowExp)
        {
            value = MinExpAdjustValue;
        }
        else
        {
            float ratio = 1f - ((float)nowExp / (float)maxExp);
            value = value * ratio + MaxExpAdjustValue;
        }
        m_playerExpBar.padding = new Vector4(0, 0, value, 0);
    }

    public void UpdateEnemyHpBar(int maxHP, int nowHP, int damage)
    {

    }

    public void UpdatePlayerHpBar(int maxHP, int nowHP, int damage)
    {

    }

    public void UpdatePlayerExpBar(int maxExp, int nowExp, int exp)
    {

    }
}