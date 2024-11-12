using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Framework;

namespace Tamamon.InGame.AdventureEvent
{
    /// <summary>
    /// �I�����C�x���g
    /// </summary>
    public class SelectStartEvent : IAdventureEvent
    {
        private AdventureEventView m_adventureEventView = default;
        private AdventureEventModel m_adventureEventModel = default;
        private int m_index = -1;

        public void OnInitialize(AdventureEventView view, AdventureEventModel model)
        {
            m_adventureEventView = view;
            m_adventureEventModel = model;
        }

        public async UniTask<int> OnExecute(string str)
        {
            List<string> commandList = new List<string>();
            

            commandList.Add("�I����1");
            commandList.Add("�I����2");
            if (m_index < 0)
            {
                commandList.Add("�I����3");
                m_index = 1;
            }
            else if (m_index > 0)
            {
                m_index = -1;
            }

            await m_adventureEventView.OnOpenSelectCommandWindow(commandList);

            await UniTask.WaitWhile(() => m_adventureEventView.IsShow());

            int index = m_adventureEventView.OnCloseSelectCommandWindow();

            m_adventureEventModel.AddSelectEvent(index);
            return index;
        }
    }
}