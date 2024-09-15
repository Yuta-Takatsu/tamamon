using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// �L�����N�^�[�R���g���[���[(�����낵���_)
/// </summary>
public partial class TopDownCharacterState
{
    public TopDownCharacterBase m_parentCharacterController;
    public IState m_currentState;

    public TopDownCharacterState_Idol m_idolState;

    public enum State
	{
        INVALID = -1,
        IDOL,
        MOVE,
        WAIT_UI,
        WAIT_EVENT,
	}

    public interface IState
	{
        /// <summary>
        /// ������
        /// </summary>
        public void OnInitialize(State beforeState);

        /// <summary>
        /// �J�n
        /// </summary>
        public void OnEntry();

        /// <summary>
        /// �I��
        /// </summary>
        public NextStatePassInfo OnFinish();
	}

    public struct NextStatePassInfo
	{
        public State nextState;
        public KeyCode keycode;
	}

    public IState CurrentState { get { return m_currentState; } private set { } }

    /// <summary>
    /// ������
    /// <para></para>
    /// </summary>
    public virtual void OnInitialize(State initState)
    {
        m_idolState = new TopDownCharacterState_Idol();
        m_idolState.OnInitialize(initState);


		switch ( initState ) {
            case State.IDOL:
                m_currentState = m_idolState;
                break;
		}
    }

    // Update is called once per frame
    public virtual void StartState()
    {
        if( m_currentState != null ) {
            m_currentState.OnEntry();
		}
    }
}
