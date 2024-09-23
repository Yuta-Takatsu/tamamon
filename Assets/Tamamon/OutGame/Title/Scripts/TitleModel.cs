using System;
using UniRx;

namespace Tamamon.OutGame.Title
{
    public class TitleModel
    {
        private ReactiveProperty<ITitleState> m_titleStateProperty = new();
        public IObservable<ITitleState> Observable => m_titleStateProperty;

        private ITitleState m_prevTitleState = default;

        public void SetTitleState(ITitleState state)
        {
            m_titleStateProperty.Value = state;
        }

        public void OnExecute()
        {
            if (m_prevTitleState != null)
            {
                m_prevTitleState.OnFinalize();
            }
            m_prevTitleState = m_titleStateProperty.Value;
            m_titleStateProperty.Value.OnExecute();

        }
    }
}