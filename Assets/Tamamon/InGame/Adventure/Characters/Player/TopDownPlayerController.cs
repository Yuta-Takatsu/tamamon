using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�R���g���[���[(�����낵���_)
/// </summary>
public class TopDownPlayerController : TopDownCharacterController
{
    /// <summary>
    /// ������
    /// </summary>
    public override void OnInitialize(TopDownCharacterBase controllCharacter)
    {
        m_characterClass = controllCharacter;
        m_state = new TopDownPlayerState();

        m_state.OnInitialize(TopDownCharacterState.State.WAIT_KEY, this);

        var info = new TopDownCharacterState.StateBredgeInfo() {
            nextState = TopDownCharacterState.State.WAIT_KEY
		};
		m_state.RequestSwitchState(info);
    }
}
