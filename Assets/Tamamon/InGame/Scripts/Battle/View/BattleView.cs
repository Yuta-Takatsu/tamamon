using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;

public class BattleView : MonoBehaviour
{
    [SerializeField]
    private Tamamon m_enemyTamamon = default;

    [SerializeField]
    private Tamamon m_playerTamamon = default;

    [SerializeField]
    private TextMeshProUGUI m_messageText = default;

    private string m_encountMessage = "野生の {0} が現れた!";

    private string m_bringOutMessage = "行け ! {0} !!";

    [SerializeField]
    private BattleUIView m_battleUIView = default;

    public async UniTask OnInitialize(Tamamon.TamamonDataInfo enemyTamamon, Tamamon.TamamonDataInfo playerTamamon)
    {
        // 情報をタマモンに渡す
        m_enemyTamamon.SetTamamonData(enemyTamamon);
        m_playerTamamon.SetTamamonData(playerTamamon);

        // エンカウントアニメーション初期化
        m_enemyTamamon.OnEncountAnimationInitialize(false);
        m_playerTamamon.OnEncountAnimationInitialize(true);

        // 情報をUIに渡す
        m_battleUIView.SetEnemyUI(enemyTamamon.Name, enemyTamamon.Sex, enemyTamamon.Level, enemyTamamon.MaxHP, enemyTamamon.NowHP);
        m_battleUIView.SetPlayerUI(playerTamamon.Name, playerTamamon.Sex, playerTamamon.Level, playerTamamon.MaxExp, playerTamamon.NowExp, playerTamamon.MaxHP, playerTamamon.NowHP);

        // エンカウントアニメーション再生
        m_enemyTamamon.OnEncountAnimation(false);

        await UniTask.WaitWhile(() => m_enemyTamamon.IsAnimation);

        // テキスト表示
        TypeWriteEffect typeWriteEffect = new TypeWriteEffect();
        m_encountMessage = string.Format(m_encountMessage, enemyTamamon.Name);
        await typeWriteEffect.ShowTextMessage(m_messageText, m_encountMessage);

        await UniTask.WaitWhile(() => typeWriteEffect.IsAnimation);

        // ディレイをかけてから次に行く
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        // 自身のタマモンエンカウントアニメーション再生
        m_playerTamamon.OnEncountAnimation(true);

        // 前のテキストを消してから表示
        m_messageText.text = string.Empty;
        m_bringOutMessage = string.Format(m_bringOutMessage, playerTamamon.Name);
        await typeWriteEffect.ShowTextMessage(m_messageText, m_bringOutMessage);

        await UniTask.WaitWhile(() => m_playerTamamon.IsAnimation);
        await UniTask.WaitWhile(() => typeWriteEffect.IsAnimation);

        Debug.Log("End");

    }
}