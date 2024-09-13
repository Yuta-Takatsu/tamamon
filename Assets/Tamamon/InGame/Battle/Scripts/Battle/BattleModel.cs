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
    public IObservable<BattleStateType> BattleStateObservar => m_battleState;

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

    /// <summary>
    /// �퓬�s���X�e�[�g
    /// </summary>
    public enum BattleExecuteType
    {
        Technique, // �Z�g�p
        Change,    // ����ւ�
        Item,      // �A�C�e���g�p
        Escape,    // ������
    }

    private BattleTurnEndType m_battleTurnEndState = default;
    public BattleTurnEndType BattleTurnEndState { get => m_battleTurnEndState; set => m_battleTurnEndState = value; }

    /// <summary>
    /// �^�[���I�����X�e�[�g
    /// </summary>
    public enum BattleTurnEndType
    {
        Win,
        Lose,
        EnemyDown,
        PlayerDown,
    }

    private BattleEventType m_battleEventState = default;
    public BattleEventType BattleEventState { get => m_battleEventState; set => m_battleEventState = value; }

    /// <summary>
    /// �o�g���C�x���g�X�e�[�g
    /// </summary>
    public enum BattleEventType
    {
        Trainer,
        Wild,
    }

    /// <summary>
    /// ������
    /// </summary>
    public void OnInitialize()
    {
        // �X�e�[�g�̊Ď�
        m_battleState.Skip(1).Subscribe(state => OnExecute(state));
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
    /// ���s
    /// </summary>
    /// <param name="state"></param>
    public void OnExecute(BattleStateType state)
    {
        m_stateCallbackDictionary[state]?.Invoke();
    }
}