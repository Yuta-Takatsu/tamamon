using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// �o�g�����f���N���X
/// </summary>
public class BattleModel
{
    private ReactiveProperty<BattleStateType> m_battleState = new();
    public BattleStateType BattleState { get => m_battleState.Value; set => m_battleState.Value = value; }
    public IObservable<BattleStateType> BattleStateObserver => m_battleState;

    private Dictionary<BattleStateType, System.Action> m_stateCallbackDictionary = new Dictionary<BattleStateType, System.Action>();

    /// <summary>
    /// �o�g�����t���[�X�e�[�g
    /// </summary>
    public enum BattleStateType
    {
        None,
        Encount,         // �o��
        ActionSelect,    // �s���I��
        TechniqueSelect, // �Z�I��
        Execute,         // �퓬
        TurnEnd,         // �^�[���I��
        TamamonSelect,   // �^�}�����I��
        ItemSelect,       // �A�C�e���I��
        Result,          // �퓬�I��
    }

    private BattleExecuteType m_battleExecuteState = default;
    public BattleExecuteType BattleExecuteState { get => m_battleExecuteState; set => m_battleExecuteState = value; }

    private Dictionary<BattleExecuteType, System.Action> m_enemyBattleStateCallbackDictionary = new Dictionary<BattleExecuteType, System.Action>();
    private Dictionary<BattleExecuteType, System.Action> m_playerBattleStateCallbackDictionary = new Dictionary<BattleExecuteType, System.Action>();
    /// <summary>
    /// �퓬�s���X�e�[�g
    /// </summary>
    public enum BattleExecuteType
    {
        None = 0,
        Technique = 1, // �Z�g�p
        Item = 2,      // �A�C�e���g�p
        Change = 3,    // ����ւ�
        Escape = 4,    // ������
    }

    private BattleTurnEndType m_battleTurnEndState = default;
    public BattleTurnEndType BattleTurnEndState { get => m_battleTurnEndState; set => m_battleTurnEndState = value; }

    /// <summary>
    /// �^�[���I�����X�e�[�g
    /// </summary>
    public enum BattleTurnEndType
    {
        None,
        EnemyDown,
        PlayerDown,
        AllDown,
    }

    private BattleEndType m_battleEndState = default;
    public BattleEndType BattleEndState { get => m_battleEndState; set => m_battleEndState = value; }

    /// <summary>
    /// �o�g���I�����X�e�[�g
    /// </summary>
    public enum BattleEndType
    {
        None,
        Win,
        Lose,
        Escape,
    }

    private BattleEventType m_battleEventState = default;
    public BattleEventType BattleEventState { get => m_battleEventState; set => m_battleEventState = value; }

    /// <summary>
    /// �o�g���C�x���g�X�e�[�g
    /// </summary>
    public enum BattleEventType
    {
        None,
        Trainer,
        Wild,
    }

    private WeaknessType m_weaknessTypeState = default;
    public WeaknessType WeaknessTypeState => m_weaknessTypeState;
    /// <summary>
    /// �^�C�v����
    /// </summary>
    public enum WeaknessType
    {
        None,
        Effective,
        NotEffective,
        DontAffective,
    }

    private TamamonStatusData m_enemyStatusData = default;
    public TamamonStatusData EnemyStatusData { get => m_enemyStatusData; set => m_enemyStatusData = value; }

    private TamamonStatusData m_playerStatusData = default;
    public TamamonStatusData PlayerStatusData { get => m_playerStatusData; set => m_playerStatusData = value; }

    private List<TamamonStatusData> m_enemyStatusDataList = new List<TamamonStatusData>();

    private List<TamamonStatusData> m_playerStatusDataList = new List<TamamonStatusData>();

    /// <summary>
    /// �X�e�[�g�؂�ւ����ɌĂ΂��R�[���o�b�N��o�^
    /// </summary>
    public void OnStateExecute()
    {
        m_battleState.Skip(1).Subscribe(state => OnExecute(state));
    }

    /// <summary>
    /// �o�g���X�e�[�g���s�R�[���o�b�N
    /// </summary>
    /// <param name="state"></param>
    /// <param name="isPlayerTurn"></param>
    public void OnBattkeStateExecute(BattleExecuteType state, bool isPlayerTurn)
    {
        if (isPlayerTurn)
        {
            m_playerBattleStateCallbackDictionary[state]?.Invoke();
        }
        else
        {
            m_enemyBattleStateCallbackDictionary[state]?.Invoke();
        }
    }

    /// <summary>
    /// �X�e�[�g�ύX�����s�R�[���o�b�N���Z�b�g
    /// </summary>
    /// <param name="state"></param>
    /// <param name="onCallback"></param>
    public void SetCallbackDictionary(BattleStateType state, System.Action onCallback)
    {
        m_stateCallbackDictionary.Add(state, onCallback);
    }

    /// <summary>
    /// �o�g�����s�R�[���o�b�N���Z�b�g
    /// </summary>
    /// <param name="state"></param>
    /// <param name="onCallback"></param>
    public void SetEnemyBattleStateCallbackDictionary(BattleExecuteType state, System.Action onCallback)
    {
        m_enemyBattleStateCallbackDictionary.Add(state, onCallback);
    }

    /// <summary>
    /// �o�g�����s�R�[���o�b�N���Z�b�g
    /// </summary>
    /// <param name="state"></param>
    /// <param name="onCallback"></param>
    public void SetPlayerBattleStateCallbackDictionary(BattleExecuteType state, System.Action onCallback)
    {
        m_playerBattleStateCallbackDictionary.Add(state, onCallback);
    }

    /// <summary>
    /// �G�l�~�[�̎莝�����X�g���擾
    /// </summary>
    /// <param name="list"></param>
    public void SetEnemyList(List<TamamonStatusData> list)
    {
        m_enemyStatusDataList = list;
    }

    /// <summary>
    /// �G�l�~�[�̎莝����ǉ�
    /// </summary>
    /// <param name="data"></param>
    public void AddEnemyList(TamamonStatusData data)
    {
        m_enemyStatusDataList.Add(data);
    }

    /// <summary>
    /// �G�l�~�[�̎莝�����X�g��Ԃ�
    /// </summary>
    /// <returns></returns>
    public List<TamamonStatusData> GetEnemyList()
    {
        return m_enemyStatusDataList;
    }

    /// <summary>
    /// �v���C���[�̎莝�����X�g���擾
    /// </summary>
    /// <param name="list"></param>
    public void SetPlayerList(List<TamamonStatusData> list)
    {
        m_playerStatusDataList = list;
    }

    /// <summary>
    /// �v���C���[�̎莝����ǉ�
    /// </summary>
    /// <param name="data"></param>
    public void AddPlayerList(TamamonStatusData data)
    {
        m_playerStatusDataList.Add(data);
    }

    /// <summary>
    /// �v���C���[�̎莝�����X�g��Ԃ�
    /// </summary>
    /// <returns></returns>
    public List<TamamonStatusData> GetPlayerList()
    {
        return m_playerStatusDataList;
    }

    /// <summary>
    /// �_���[�W�v�Z�����Ă��̒l��Ԃ�
    /// </summary>
    /// <returns></returns>
    public int GetDamageValue(int index,bool isPlayer)
    {
        TamamonStatusData attackTamamon = m_playerStatusData;
        TamamonStatusData defenseTamamon = m_enemyStatusData;
        if (isPlayer)
        {
             attackTamamon = m_playerStatusData;
             defenseTamamon = m_enemyStatusData;
        }
        else
        {
             attackTamamon = m_enemyStatusData;
             defenseTamamon = m_playerStatusData;
        }

        // ��
        // �З� * 0.8 * �^�C�v��v�{�[�i�X * ����
        int power = attackTamamon.TamamonStatusDataInfo.TechniqueList[index].TechniqueData.Power;
        float adjustValue = 0.8f;
        float typeBonus = 1.0f;
        float weaknessBonus = 1.0f;

        // �g�p�Z�^�C�v
        TypeData.Type techniqueType = attackTamamon.TamamonStatusDataInfo.TechniqueList[index].TechniqueData.Type;

        m_weaknessTypeState = WeaknessType.None;

        // �^�C�v�����v�Z
        foreach (var enemyType in defenseTamamon.TamamonStatusDataInfo.tamamonDataInfomation.TypeList)
        {
            // ����
            foreach (var effectiveType in TypeData.DontAffectDictionary[techniqueType])
            {
                if (enemyType == effectiveType)
                {
                    m_weaknessTypeState = WeaknessType.DontAffective;
                    return 0;
                }
            }

            // ���Q
            foreach (var effectiveType in TypeData.EffectiveDictionary[techniqueType])
            {
                if (enemyType == effectiveType)
                {
                    m_weaknessTypeState = WeaknessType.Effective;
                    weaknessBonus *= 2f;
                    break;
                }
            }

            //���܂ЂƂ�
            foreach (var effectiveType in TypeData.NotEffectiveDictionary[techniqueType])
            {
                if (enemyType == effectiveType)
                {
                    m_weaknessTypeState = WeaknessType.NotEffective;
                    weaknessBonus *= 0.5f;
                    break;
                }
            }
        }

        // �^�C�v��v�{�[�i�X�v�Z
        foreach (var playerType in attackTamamon.TamamonStatusDataInfo.tamamonDataInfomation.TypeList)
        {
            if (playerType == techniqueType)
            {
                typeBonus = 1.5f;
            }
        }
        return (int)(power * adjustValue * typeBonus * weaknessBonus);
    }

    /// <summary>
    /// �m���̃G�l�~�[�^�}���������邩
    /// </summary>
    /// <returns></returns>
    public bool IsEnemyFainting()
    {
        return m_enemyStatusData.TamamonStatusDataInfo.NowHP <= 0;
    }

    /// <summary>
    /// �m���̃v���C���[�^�}���������邩
    /// </summary>
    /// <returns></returns>
    public bool IsPlayerFainting()
    {
        return m_playerStatusData.TamamonStatusDataInfo.NowHP <= 0;
    }

    /// <summary>
    /// ��Ԉُ�̃^�}���������邩
    /// </summary>
    /// <returns></returns>
    public bool IsStatusAilment()
    {
        return false;
    }

    /// <summary>
    /// �S������Ă��邩�ǂ���
    /// </summary>
    /// <returns></returns>
    public bool IsBind()
    {
        return false;
    }

    /// <summary>
    /// �V�󂪕ς���Ă��邩�ǂ���
    /// </summary>
    /// <returns></returns>
    public bool IsWeather()
    {
        return false;
    }

    /// <summary>
    /// �t�B�[���h���ς���Ă��邩�ǂ���
    /// </summary>
    /// <returns></returns>
    public bool IsField()
    {
        return false;
    }

    /// <summary>
    /// �ݒu�������邩�ǂ���
    /// </summary>
    /// <returns></returns>
    public bool IsInstallation()
    {
        return false;
    }

    /// <summary>
    /// ���s
    /// </summary>
    /// <param name="state"></param>
    private void OnExecute(BattleStateType state)
    {
        m_stateCallbackDictionary[state]?.Invoke();
    }
}