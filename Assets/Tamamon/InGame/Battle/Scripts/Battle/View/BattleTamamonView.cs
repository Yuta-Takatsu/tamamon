using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;

/// <summary>
/// �^�}�����\���N���X
/// </summary>
public class BattleTamamonView : MonoBehaviour
{
    [SerializeField]
    private TamamonData m_enemyTamamon = default;

    [SerializeField]
    private TamamonData m_playerTamamon = default;

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="enemyTamamon"></param>
    /// <param name="playerTamamon"></param>
    public void OnInitialize(TamamonData.TamamonDataInfomation enemyTamamon, TamamonData.TamamonDataInfomation playerTamamon)
    {
        // �����^�}�����ɓn��
        m_enemyTamamon.SetTamamonData(enemyTamamon);
        m_playerTamamon.SetTamamonData(playerTamamon);

        // �G���J�E���g�A�j���[�V����������
        m_enemyTamamon.OnEncountAnimationInitialize(false);
        m_playerTamamon.OnEncountAnimationInitialize(true);
    }

    /// <summary>
    /// �G�l�~�[�G���J�E���g�A�j���[�V�����Đ�
    /// </summary>
    public void PlayEncountEnemyAnimation()
    {
        m_enemyTamamon.OnEncountAnimation(false);
    }

    /// <summary>
    /// �v���C���[�G���J�E���g�A�j���[�V�����Đ�
    /// </summary>
    public void PlayEncountPlayerAnimation()
    {
        m_playerTamamon.OnEncountAnimation(true);
    }

    /// <summary>
    /// �G�l�~�[�G���J�E���g�A�j���[�V�����Đ���
    /// </summary>
    /// <returns></returns>
    public bool IsEncountEnemyAnimation()
    {
        return m_enemyTamamon.IsAnimation;
    }

    /// <summary>
    /// �v���C���[�G���J�E���g�A�j���[�V�����Đ���
    /// </summary>
    /// <returns></returns>
    public bool IsEncountPlayerAnimation()
    {
        return m_playerTamamon.IsAnimation;
    }
}