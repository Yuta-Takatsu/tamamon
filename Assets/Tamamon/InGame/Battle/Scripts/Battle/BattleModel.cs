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

    /// <summary>
    /// バトル中フローステート
    /// </summary>
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
    public BattleExecuteType BattleExecuteState { get => m_battleExecuteState; set => m_battleExecuteState = value; }

    /// <summary>
    /// 戦闘行動ステート
    /// </summary>
    public enum BattleExecuteType
    {
        Technique, // 技使用
        Change,    // 入れ替え
        Item,      // アイテム使用
        Escape,    // 逃げる
    }

    private BattleTurnEndType m_battleTurnEndState = default;
    public BattleTurnEndType BattleTurnEndState { get => m_battleTurnEndState; set => m_battleTurnEndState = value; }

    /// <summary>
    /// ターン終了時ステート
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
    /// バトルイベントステート
    /// </summary>
    public enum BattleEventType
    {
        Trainer,
        Wild,
    }

    /// <summary>
    /// 初期化
    /// </summary>
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