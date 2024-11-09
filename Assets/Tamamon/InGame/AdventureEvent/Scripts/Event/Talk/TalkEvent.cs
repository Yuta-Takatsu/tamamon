using System;
using Cysharp.Threading.Tasks;
using Framework;

namespace Tamamon.InGame.AdventureEvent
{
    /// <summary>
    /// ��b�C�x���g
    /// </summary>
    public class TalkEvent : IAdventureEvent
    {

        private AdventureEventView m_adventureEventView = default;

        private EventHandler m_handler = null;



        public void OnInitialize(AdventureEventView view)
        {
            m_adventureEventView = view;
        }

        public async UniTask<int> OnExecute(string str)
        {
            // �E�B���h�E�\��
            m_adventureEventView.SetWindow(true);

            // �O�e�L�X�g�폜
            m_adventureEventView.ClearText();

            // �e�L�X�g�\��
            await m_adventureEventView.ShowMessage(str);

            // ���͑҂�
            await UniTask.WaitUntil(() => InputManager.Instance.GetKeyDown(InputManager.Key.Decision));

            return 0;
        }

        public void OnFinalize()
        {
            // �E�B���h�E��\��
            m_adventureEventView.SetWindow(false);

            InputEventManager.Instance.RemoveKeyDownEvent(InputManager.Key.Decision, m_handler);
        }
    }
}