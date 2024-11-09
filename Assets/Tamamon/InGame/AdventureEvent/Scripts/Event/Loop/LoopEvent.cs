using System;
using Cysharp.Threading.Tasks;
using Framework;

namespace Tamamon.InGame.AdventureEvent
{
    /// <summary>
    /// ループイベント
    /// </summary>
    public class LoopEvent : IAdventureEvent
    {
        private AdventureEventModel m_adventureEventModel = default;

        public void OnInitialize(AdventureEventModel model)
        {
            m_adventureEventModel = model;
        }

        public UniTask<int> OnExecute(string str)
        {
            m_adventureEventModel.RemoveSelectEvent();

            int index = (m_adventureEventModel.LoopEventIndexs[1] + 1) * -1;
            m_adventureEventModel.SetCurrentIndex(index);
            return UniTask.FromResult(index);
        }
    }
}