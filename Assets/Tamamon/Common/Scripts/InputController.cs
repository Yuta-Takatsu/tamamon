using System.Collections;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class InputController : MonoBehaviourSingleton<InputController>
{

    private bool m_isInput = false;
    public bool IsInput => m_isInput;

    private KeyCode m_inputKey = KeyCode.None;
    public KeyCode InputKey => m_inputKey;

    public async UniTask OnInitialize()
    {
        m_isInput = true;
        while (m_isInput)
        {
            await UniTask.DelayFrame(60);

            if (Input.anyKeyDown)
            {
                foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(code))
                    {
                        m_inputKey = code;
                    }
                }
            }
            else
            {
                m_inputKey = KeyCode.None;
            }
        }
    }
}