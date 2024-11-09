using System;
using Cysharp.Threading.Tasks;
using Framework;

namespace Tamamon.InGame.AdventureEvent
{
    /// <summary>
    /// ループイベント
    /// </summary>
    public class LoopPointEvent : IAdventureEvent
    {
        private AdventureEventModel m_adventureEventModel = default;

        public void OnInitialize(AdventureEventModel model)
        {
            m_adventureEventModel = model;
        }

        public UniTask<int> OnExecute(string str)
        {
            m_adventureEventModel.AddLoopEventIndexKey(1);

            return UniTask.FromResult(0);
        }
    }
}