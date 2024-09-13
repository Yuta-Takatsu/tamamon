using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;

/// <summary>
/// タマモン表示クラス
/// </summary>
public class BattleTamamonView : MonoBehaviour
{
    [SerializeField]
    private TamamonData m_enemyTamamon = default;

    [SerializeField]
    private TamamonData m_playerTamamon = default;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="enemyTamamon"></param>
    /// <param name="playerTamamon"></param>
    public void OnInitialize(TamamonData.TamamonDataInfomation enemyTamamon, TamamonData.TamamonDataInfomation playerTamamon)
    {
        // 情報をタマモンに渡す
        m_enemyTamamon.SetTamamonData(enemyTamamon);
        m_playerTamamon.SetTamamonData(playerTamamon);

        // エンカウントアニメーション初期化
        m_enemyTamamon.OnEncountAnimationInitialize(false);
        m_playerTamamon.OnEncountAnimationInitialize(true);
    }

    /// <summary>
    /// エネミーエンカウントアニメーション再生
    /// </summary>
    public void PlayEncountEnemyAnimation()
    {
        m_enemyTamamon.OnEncountAnimation(false);
    }

    /// <summary>
    /// プレイヤーエンカウントアニメーション再生
    /// </summary>
    public void PlayEncountPlayerAnimation()
    {
        m_playerTamamon.OnEncountAnimation(true);
    }

    /// <summary>
    /// エネミーエンカウントアニメーション再生状況
    /// </summary>
    /// <returns></returns>
    public bool IsEncountEnemyAnimation()
    {
        return m_enemyTamamon.IsAnimation;
    }

    /// <summary>
    /// プレイヤーエンカウントアニメーション再生状況
    /// </summary>
    /// <returns></returns>
    public bool IsEncountPlayerAnimation()
    {
        return m_playerTamamon.IsAnimation;
    }
}