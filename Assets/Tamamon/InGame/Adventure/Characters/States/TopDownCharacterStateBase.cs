using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class TopDownCharacterStateBase
{
    public TopDownCharacterBase m_parentCharacterController;

    public enum State
	{
        INVALID = -1,
        IDOL,
        MOVE,
        WAIT_UI,
        WAIT_EVENT,
	}

    /// <summary>
    /// èâä˙âª
    /// </summary>
    public async UniTask<bool> OnInitialize()
    {
        await UniTask.Yield();
        return true;
    }
}
