using System;
using Cysharp.Threading.Tasks;
using Framework;

namespace Tamamon.InGame.AdventureEvent
{
    /// <summary>
    /// 会話イベント
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
            // ウィンドウ表示
            m_adventureEventView.SetWindow(true);

            // 前テキスト削除
            m_adventureEventView.ClearText();

            // テキスト表示
            await m_adventureEventView.ShowMessage(str);

            // 入力待ち
            await UniTask.WaitUntil(() => InputManager.Instance.GetKeyDown(InputManager.Key.Decision));

            return 0;
        }

        public void OnFinalize()
        {
            // ウィンドウ非表示
            m_adventureEventView.SetWindow(false);

            InputEventManager.Instance.RemoveKeyDownEvent(InputManager.Key.Decision, m_handler);
        }
    }
}