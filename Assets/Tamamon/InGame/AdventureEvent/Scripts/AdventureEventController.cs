using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using Cysharp.Threading.Tasks;

namespace Tamamon.InGame.AdventureEvent
{
    /// <summary>
    /// �A�h�x���`���[�p�[�g�ŋN����C�x���g�̃R���g���[���[�N���X
    /// </summary>
    public class AdventureEventController : MonoBehaviour
    {

        [SerializeField]
        private AdventureEventView m_adventureEventView = default;
        private AdventureEventModel m_adventureEventModel = default;

        private Script m_functionScript = default;

        private static AdventureEventView m_view = default;

        public void OnInitialize()
        {
            m_adventureEventModel = new AdventureEventModel();
            m_adventureEventView.OnInitialize();

            m_view = m_adventureEventView;

            // lua�X�N���v�g�ǂݍ���
            m_functionScript = new Script();
            m_functionScript.DoFile("function.lue");

            //OnExecute("SetWindow", true);
            OnExecute("talk", "testtesttesttesttest�e�X�g");
        }

        public void OnExecute(string command, string value)
        {
            DynValue result = m_functionScript.Call(m_functionScript.Globals[command], value);
            Debug.Log(result.Number);
        }

        public static void SetWindow(bool isActive)
        {
            m_view.SetWindow(isActive);
        }

        public static void ShowMessage(string message)
        {
            m_view.ShowMessage(message).Forget();
        }
    }
}