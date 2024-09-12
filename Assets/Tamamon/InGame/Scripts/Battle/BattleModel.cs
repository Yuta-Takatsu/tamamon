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
    public BattleExecuteType BattleExecuteState => m_battleExecuteState;

    public enum BattleExecuteType
    {
        Technique,
        Change,
        Item,
        Escape,
    }

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