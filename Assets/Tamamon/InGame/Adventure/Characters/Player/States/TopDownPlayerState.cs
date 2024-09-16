using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// プレイヤーステート(見下ろし視点)
/// </summary>
public partial class TopDownPlayerState : TopDownCharacterState
{
    /// <summary>
    /// ステート完了イベント
    /// <para></para>
    /// </summary>
    protected override void OnCompleteState(StateBredgeInfo nextStateInfo)
    {
        Debug.Log("終わったね");
        var nextState = nextStateInfo.nextState;

        RequestSwitchState(nextStateInfo);
    }
}
