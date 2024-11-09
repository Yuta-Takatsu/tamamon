using System;
using Framework;
using Tamamon.InGame.AdventureEvent;

namespace Tamamon.OutGame.Title
{
    public class TitleState : ITitleState
    {
        private TitleView m_titleView = default;

        public void OnInitialize(ITitleView titleView)
        {
            m_titleView = (TitleView)titleView;
            m_titleView.OnInitialize();
        }

        public void OnExecute()
        {
            // BGMÄ¶
            SoundManager.Instance.UpdateBGM(SoundManager.BGM_Type.Title, m_titleView.TitleBGM);
            SoundManager.Instance.PlayBGM(SoundManager.BGM_Type.Title, isCrossFade: false);

            m_titleView.OnExecute();

            // “ü—ÍƒCƒxƒ“ƒg“o˜^
            EventHandler handler = null;
            handler = async (object sender, EventArgs e) =>
            {
                InputEventManager.Instance.RemoveKeyDownEvent(InputManager.Key.Decision, handler);
                await BattleManager.Instance.LoadScene(3);
            };
            InputEventManager.Instance.SetKeyDownEvent(InputManager.Key.Decision, handler);
        }

        public void OnFinalize()
        {
            m_titleView.OnFinalize();
        }
    }
}