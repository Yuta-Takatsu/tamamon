using System;
using Cysharp.Threading.Tasks;
using Framework;

namespace Tamamon.InGame.AdventureEvent
{
    /// <summary>
    /// �I�����C�x���g
    /// </summary>
    public class SelectOrEvent : IAdventureEvent
    {
        private AdventureEventModel m_adventureEventModel = default;

        public void OnInitialize(AdventureEventModel model)
        {
            m_adventureEventModel = model;
        }

        public UniTask<int> OnExecute(string str)
        {
            m_adventureEventModel.AddSelectOrEventCount();
            return UniTask.FromResult(0);
        }
    }
}