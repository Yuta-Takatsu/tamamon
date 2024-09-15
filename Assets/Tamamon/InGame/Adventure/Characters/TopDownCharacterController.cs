using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// �L�����N�^�[�R���g���[���[(�����낵���_)
/// </summary>
public class TopDownCharacterController
{

    private TopDownCharacterBase m_characterClass;
    private TopDownCharacterState m_state;

    /// <summary>
    /// ������
    /// </summary>
    public virtual void OnInitialize(TopDownCharacterBase controllCharacter)
    {
        m_characterClass = controllCharacter;
        m_state = new TopDownCharacterState();
        m_state.OnInitialize(TopDownCharacterState.State.IDOL);

        m_state.StartState();
    }

    // Update is called once per frame
    public virtual void OnUpdate()
    {
    }
}
