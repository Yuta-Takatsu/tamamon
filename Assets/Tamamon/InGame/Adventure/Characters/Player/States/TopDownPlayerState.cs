using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// �v���C���[�X�e�[�g(�����낵���_)
/// </summary>
public partial class TopDownPlayerState : TopDownCharacterState
{
    /// <summary>
    /// �X�e�[�g�����C�x���g
    /// <para></para>
    /// </summary>
    protected override void OnCompleteState(StateBredgeInfo nextStateInfo)
    {
        Debug.Log("�I�������");
        var nextState = nextStateInfo.nextState;

        RequestSwitchState(nextStateInfo);
    }
}
