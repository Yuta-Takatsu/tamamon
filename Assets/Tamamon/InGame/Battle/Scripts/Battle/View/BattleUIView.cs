using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

/// <summary>
/// タマモン情報表示クラス
/// </summary>
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

    private bool m_isEnemyHpBarAnimation = false;
    public bool IsEnemyHpBarAnimation => m_isEnemyHpBarAnimation;

    private bool m_isPlayerHpBarAnimation = false;
    public bool IsPlayerHpBarAnimation => m_isPlayerHpBarAnimation;

    private bool m_isPlayerExpBarAnimation = false;
    public bool IsPlayerExpBarAnimation => m_isPlayerExpBarAnimation;

    private readonly int MaxHpAdjustValue = 80;
    private readonly int MinHpAdjustValue = 312;

    private readonly int MaxExpAdjustValue = 67;
    private readonly int MinExpAdjustValue = 369;

    /// <summary>
    /// エネミー情報表示
    /// </summary>
    /// <param name="name"></param>
    /// <param name="sexType"></param>
    /// <param name="level"></param>
    /// <param name="maxHP"></param>
    /// <param name="nowHP"></param>
    public void ShowEnemyUI(string name, TamamonData.SexType sexType, int level, int maxHP, int nowHP)
    {
        m_enemyNameText.text = name;

        switch (sexType)
        {
            case TamamonData.SexType.None:
                m_enemySexText.text = string.Empty;
                break;

            case TamamonData.SexType.Male:
                m_enemySexText.text = "♂";
                break;
            case TamamonData.SexType.Female:
                m_enemySexText.text = "♀";
                break;
        }

        m_enemyLevelText.text = $"Lv:{level}";

        ShowEnemyHpBar(maxHP, nowHP);
    }

    /// <summary>
    /// プレイヤー情報表示
    /// </summary>
    /// <param name="name"></param>
    /// <param name="sexType"></param>
    /// <param name="level"></param>
    /// <param name="maxExp"></param>
    /// <param name="nowExp"></param>
    /// <param name="maxHP"></param>
    /// <param name="nowHP"></param>
    public void ShowPlayerUI(string name, TamamonData.SexType sexType, int level, int maxExp, int nowExp, int maxHP, int nowHP)
    {
        m_playerNameText.text = name;

        switch (sexType)
        {
            case TamamonData.SexType.None:
                m_playerSexText.text = string.Empty;
                break;

            case TamamonData.SexType.Male:
                m_playerSexText.text = "♂";
                break;
            case TamamonData.SexType.Female:
                m_playerSexText.text = "♀";
                break;
        }

        m_playerLevelText.text = $"Lv:{level}";

        m_playerHpText.text = $"{nowHP}/{maxHP}";

        ShowPlayerExpBar(maxExp, nowExp);
        ShowPlayerHpBar(maxHP, nowHP);
    }

    /// <summary>
    /// エネミーHP表示
    /// </summary>
    /// <param name="maxHP"></param>
    /// <param name="nowHP"></param>
    public void ShowEnemyHpBar(int maxHP, int nowHP)
    {
        // HP割合計算
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

    /// <summary>
    /// プレイヤーHP表示
    /// </summary>
    /// <param name="maxHP"></param>
    /// <param name="nowHP"></param>
    public void ShowPlayerHpBar(int maxHP, int nowHP)
    {
        // HP割合計算
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

    /// <summary>
    /// プレイヤーEXP表示
    /// </summary>
    /// <param name="maxExp"></param>
    /// <param name="nowExp"></param>
    public void ShowPlayerExpBar(int maxExp, int nowExp)
    {
        // Exp割合計算
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

    /// <summary>
    /// エネミーHP更新
    /// </summary>
    /// <param name="maxHP"></param>
    /// <param name="nowHP"></param>
    /// <param name="damage"></param>
    public void UpdateEnemyHpBar(int maxHP, int nowHP, int damage)
    {
        m_isEnemyHpBarAnimation = true;
        float value = MinHpAdjustValue - MaxHpAdjustValue;
        nowHP -= damage;

        float ratio = 1f - ((float)nowHP / (float)maxHP);
        value = value * ratio + MaxHpAdjustValue;
        float damageValue = 0f;
        DOTween.To(
            () => m_enemyHpBar.padding.z,
            x => damageValue = x,
            value,
            1f)
            .OnUpdate(() => m_enemyHpBar.padding = new Vector4(0, 0, damageValue, 0))
            .OnComplete(() =>
            {
                m_enemyHpBar.padding = new Vector4(0, 0, value, 0);
                m_isEnemyHpBarAnimation = false;
            });
    }

    /// <summary>
    /// プレイヤーHP更新
    /// </summary>
    /// <param name="maxHP"></param>
    /// <param name="nowHP"></param>
    /// <param name="damage"></param>
    public void UpdatePlayerHpBar(int maxHP, int nowHP, int damage)
    {
        m_isPlayerHpBarAnimation = true;
        float value = MinHpAdjustValue - MaxHpAdjustValue;
        int hp = nowHP;
        nowHP -= damage;

        float ratio = 1f - ((float)nowHP / (float)maxHP);
        value = value * ratio + MaxHpAdjustValue;
        float damageValue = 0f;
        DOTween.To(
            () => m_playerHpBar.padding.z,
            x => damageValue = x,
            value,
            1f)
            .OnUpdate(() => m_playerHpBar.padding = new Vector4(0, 0, damageValue, 0))
            .OnComplete(() =>
            {
                m_playerHpBar.padding = new Vector4(0, 0, value, 0);
                m_isPlayerHpBarAnimation = false;
            });
        DOTween.To(
            () => hp,
            x => hp = x,
            nowHP,
            1f)
            .OnUpdate(() =>
            {
                if (hp < 1)
                {
                    hp = 0;
                }
                m_playerHpText.text = $"{hp}/{maxHP}";
            })
            .OnComplete(() =>
            {
                m_playerHpText.text = $"{hp}/{maxHP}";
            });
    }

    /// <summary>
    /// プレイヤーEXP更新
    /// </summary>
    /// <param name="maxExp"></param>
    /// <param name="nowExp"></param>
    /// <param name="exp"></param>
    public void UpdatePlayerExpBar(int maxExp, int nowExp, int exp)
    {
        m_isPlayerExpBarAnimation = true;
        float value = MinExpAdjustValue - MaxExpAdjustValue;
        nowExp += exp;

        float ratio = 1f - ((float)nowExp / (float)maxExp);
        value = value * ratio + MaxExpAdjustValue;
        float expValue = 0f;
        DOTween.To(
            () => m_playerExpBar.padding.z,
            x => expValue = x,
            value,
            1f)
            .OnUpdate(() => m_playerExpBar.padding = new Vector4(0, 0, expValue, 0))
            .OnComplete(() =>
            {
                m_playerExpBar.padding = new Vector4(0, 0, value, 0);
                m_isPlayerExpBarAnimation = false;
            });
    }
}