using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// �L�����N�^�[�R���g���[���[(�����낵���_)
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
        /// ������
        /// </summary>
        public void OnInitialize(StateBredgeInfo beforeStateInfo, TopDownCharacterController controller);

        /// <summary>
        /// �J�n
        /// </summary>
        public UniTask<StateBredgeInfo> OnExecute();

        /// <summary>
        /// �I��
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
    /// ������
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
    /// �X�e�[�g�ύX����
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
    /// �X�e�[�g���s
    /// <para></para>
    /// </summary>
    protected virtual async void ExecuteState()
    {
        if( m_currentState == null ) return;

        var info = await m_currentState.OnExecute();

        OnCompleteState(info);
    }

    /// <summary>
    /// �X�e�[�g�����C�x���g
    /// <para></para>
    /// </summary>
    protected virtual void OnCompleteState(StateBredgeInfo nextStateInfo)
    {
        Debug.Log("�I�������");
        nextStateInfo.nextState = State.IDOL;

        RequestSwitchState(nextStateInfo);
    }
}
