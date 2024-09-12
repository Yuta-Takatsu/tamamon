using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// バトルモデルクラス
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
        Encount,         // 出現
        ActionSelect,    // 行動選択
        TechniqueSelect, // 技選択
        Execute,         // 戦闘
        TurnEnd,         // ターン終了
        TamamonSelect,   // タマモン選択
        ItemSelect,       // アイテム選択
        Result,          // 戦闘終了
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
        // ステートの監視
        m_battleState.Skip(1).Subscribe(state => OnExecute(state));
    }

    /// <summary>
    /// ステート変更時実行コールバックをセット
    /// </summary>
    /// <param name="state"></param>
    /// <param name="onCallback"></param>
    public void SetCallbackDictionary(BattleStateType state, System.Action onCallback)
    {
        m_stateCallbackDictionary.Add(state, onCallback);
    }

    /// <summary>
    /// 実行
    /// </summary>
    /// <param name="state"></param>
    public void OnExecute(BattleStateType state)
    {
        m_stateCallbackDictionary[state]?.Invoke();
    }
}