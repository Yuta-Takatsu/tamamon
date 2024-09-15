using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// キャラクターコントローラー(見下ろし視点)
/// </summary>
public class TopDownCharacterController
{

    private TopDownCharacterBase m_characterClass;
    private TopDownCharacterState m_state;

    /// <summary>
    /// 初期化
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
