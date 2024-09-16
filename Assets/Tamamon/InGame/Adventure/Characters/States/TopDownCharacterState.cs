using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// キャラクターコントローラー(見下ろし視点)
/// </summary>
public partial class TopDownCharacterState
{
    public TopDownCharacterController m_parentCharacterController;
    public IState m_currentState;

    public TopDownCharacterState_Idol m_idolState;
    public TopDownCharacterState_WaitKey m_waitKeyState;
    public TopDownCharacterState_MoveKey m_moveKeyState;

    public enum State
	{
        INVALID = -1,
        IDOL,
        WAIT_KEY,
        MOVE_KEY,
        WAIT_UI,
        WAIT_EVENT,
	}

    public interface IState
	{
        /// <summary>
        /// 初期化
        /// </summary>
        public void OnInitialize(StateBredgeInfo beforeStateInfo, TopDownCharacterController controller);

        /// <summary>
        /// 開始
        /// </summary>
        public UniTask<StateBredgeInfo> OnExecute();

        /// <summary>
        /// 終了
        /// </summary>
        public StateBredgeInfo GetFinishData();
	}

    public struct StateBredgeInfo
	{
        public State nextState;
        public KeyCode keycode;
	}

    public IState CurrentState { get { return m_currentState; } private set { } }

    /// <summary>
    /// 初期化
    /// <para></para>
    /// </summary>
    public virtual void OnInitialize(State initState, TopDownCharacterController controller)
    {
        m_parentCharacterController = controller;

        var stateInfo = new StateBredgeInfo(){
            nextState = initState,
		};

        m_idolState = new TopDownCharacterState_Idol();
        m_idolState.OnInitialize(stateInfo, m_parentCharacterController);

        m_waitKeyState = new TopDownCharacterState_WaitKey();
        m_waitKeyState.OnInitialize(stateInfo, m_parentCharacterController);

        m_moveKeyState = new TopDownCharacterState_MoveKey();
        m_moveKeyState.OnInitialize(stateInfo, m_parentCharacterController);


		switch ( initState ) {
            case State.IDOL:
                m_currentState = m_idolState;
                break;
            case State.WAIT_KEY:
                m_currentState = m_waitKeyState;
                break;
            case State.MOVE_KEY:
                m_currentState = m_moveKeyState;
                break;
		}
    }

    /// <summary>
    /// ステート変更命令
    /// <para></para>
    /// </summary>
    public virtual void RequestSwitchState(StateBredgeInfo requestStateInfo)
    {
        switch ( requestStateInfo.nextState ) {
            case State.IDOL:
                m_currentState = m_idolState;
                break;
            case State.WAIT_KEY:
                m_currentState = m_waitKeyState;
                break;
            case State.MOVE_KEY:
                m_currentState = m_moveKeyState;
                break;
		}

        m_currentState.OnInitialize(requestStateInfo, m_parentCharacterController);

        ExecuteState();
    }

    /// <summary>
    /// ステート実行
    /// <para></para>
    /// </summary>
    protected virtual async void ExecuteState()
    {
        if( m_currentState == null ) return;

        var info = await m_currentState.OnExecute();

        OnCompleteState(info);
    }

    /// <summary>
    /// ステート完了イベント
    /// <para></para>
    /// </summary>
    protected virtual void OnCompleteState(StateBredgeInfo nextStateInfo)
    {
        Debug.Log("終わったね");
        nextStateInfo.nextState = State.IDOL;

        RequestSwitchState(nextStateInfo);
    }
}
