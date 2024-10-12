using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Framework;

namespace Tamamon.OutGame.Title
{
    public class OpeningState : ITitleState
    {
        private OpeningView m_openingView = default;

        public void OnInitialize(ITitleView openingView)
        {
            m_openingView = (OpeningView)openingView;
            m_openingView.OnInitialize();
        }

        public void OnExecute()
        {
            // BGMÄ¶
            SoundManager.Instance.PlayBGM(SoundManager.BGM_Type.Title);

            m_openingView.OnExecute();

            // “ü—ÍƒCƒxƒ“ƒg“o˜^
            EventHandler handler = null;
            handler = (object sender, EventArgs e) =>
            {
                m_openingView.NextState();
                InputEventManager.Instance.SetKeyDownEvent(InputManager.Key.Decision, handler);
            };
            InputEventManager.Instance.SetKeyDownEvent(InputManager.Key.Decision, handler);
        }

        public void OnFinalize()
        {
            m_openingView.OnFinalize();
        }
    }
}