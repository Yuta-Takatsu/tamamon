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

    private string m_encountMessage = "�쐶�� {0} �����ꂽ!";

    private string m_bringOutMessage = "�s�� ! {0} !!";

    [SerializeField]
    private BattleUIView m_battleUIView = default;

    public async UniTask OnInitialize(Tamamon.TamamonDataInfo enemyTamamon, Tamamon.TamamonDataInfo playerTamamon)
    {
        // �����^�}�����ɓn��
        m_enemyTamamon.SetTamamonData(enemyTamamon);
        m_playerTamamon.SetTamamonData(playerTamamon);

        // �G���J�E���g�A�j���[�V����������
        m_enemyTamamon.OnEncountAnimationInitialize(false);
        m_playerTamamon.OnEncountAnimationInitialize(true);

        // ����UI�ɓn��
        m_battleUIView.SetEnemyUI(enemyTamamon.Name, enemyTamamon.Sex, enemyTamamon.Level, enemyTamamon.MaxHP, enemyTamamon.NowHP);
        m_battleUIView.SetPlayerUI(playerTamamon.Name, playerTamamon.Sex, playerTamamon.Level, playerTamamon.MaxExp, playerTamamon.NowExp, playerTamamon.MaxHP, playerTamamon.NowHP);

        // �G���J�E���g�A�j���[�V�����Đ�
        m_enemyTamamon.OnEncountAnimation(false);

        await UniTask.WaitWhile(() => m_enemyTamamon.IsAnimation);

        // �e�L�X�g�\��
        TypeWriteEffect typeWriteEffect = new TypeWriteEffect();
        m_encountMessage = string.Format(m_encountMessage, enemyTamamon.Name);
        await typeWriteEffect.ShowTextMessage(m_messageText, m_encountMessage);

        await UniTask.WaitWhile(() => typeWriteEffect.IsAnimation);

        // �f�B���C�������Ă��玟�ɍs��
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        // ���g�̃^�}�����G���J�E���g�A�j���[�V�����Đ�
        m_playerTamamon.OnEncountAnimation(true);

        // �O�̃e�L�X�g�������Ă���\��
        m_messageText.text = string.Empty;
        m_bringOutMessage = string.Format(m_bringOutMessage, playerTamamon.Name);
        await typeWriteEffect.ShowTextMessage(m_messageText, m_bringOutMessage);

        await UniTask.WaitWhile(() => m_playerTamamon.IsAnimation);
        await UniTask.WaitWhile(() => typeWriteEffect.IsAnimation);

        Debug.Log("End");

    }
}