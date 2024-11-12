using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Tamamon.InGame.AdventureEvent
{
    /// <summary>
    /// �A�h�x���`���[�p�[�g��controller�N���X
    /// </summary>
    public class AdventureEventController : MonoBehaviour
    {

        [SerializeField]
        private AdventureEventView m_adventureEventView = default;
        private AdventureEventModel m_adventureEventModel = default;

        private Dictionary<string, IAdventureEvent> m_onAdventureEvents = new Dictionary<string, IAdventureEvent>();

        // �e��C�x���g
        private TalkEvent m_talkEvent = new TalkEvent();
        private SelectStartEvent m_selectStartEvent = new SelectStartEvent();
        private SelectOrEvent m_selectOrEvent = new SelectOrEvent();
        private SelectEndEvent m_selectEndEvent = new SelectEndEvent();
        private LoopPointEvent m_loopPointEvent = new LoopPointEvent();
        private LoopEvent m_loopEvent = new LoopEvent();

        // �R�}���h����Ԃ����Index
        private int m_commandIndex = -1;

        /// <summary>
        /// ������
        /// </summary>
        public void OnInitialize()
        {
            m_adventureEventModel = new AdventureEventModel();
            m_adventureEventView.OnInitialize();

            // �C�x���g������
            m_talkEvent.OnInitialize(m_adventureEventView);
            m_selectStartEvent.OnInitialize(m_adventureEventView, m_adventureEventModel);
            m_selectOrEvent.OnInitialize(m_adventureEventModel);
            m_selectEndEvent.OnInitialize(m_adventureEventModel);
            m_loopPointEvent.OnInitialize(m_adventureEventModel);
            m_loopEvent.OnInitialize(m_adventureEventModel);

            // �e��C�x���g�ǉ�
            m_onAdventureEvents.Add(m_adventureEventModel.CommandTexts[AdventureEventModel.CommandEnum.Talk], m_talkEvent);
            m_onAdventureEvents.Add(m_adventureEventModel.CommandTexts[AdventureEventModel.CommandEnum.SelectStart], m_selectStartEvent);
            m_onAdventureEvents.Add(m_adventureEventModel.CommandTexts[AdventureEventModel.CommandEnum.SelectOr], m_selectOrEvent);
            m_onAdventureEvents.Add(m_adventureEventModel.CommandTexts[AdventureEventModel.CommandEnum.SelectEnd], m_selectEndEvent);
            m_onAdventureEvents.Add(m_adventureEventModel.CommandTexts[AdventureEventModel.CommandEnum.LoopPoint], m_loopPointEvent);
            m_onAdventureEvents.Add(m_adventureEventModel.CommandTexts[AdventureEventModel.CommandEnum.Loop], m_loopEvent);
        }

        /// <summary>
        /// �w��C�x���gID���s
        /// </summary>
        /// <param name="eventId"></param>
        public async UniTask OnExecute(int eventId)
        {
            /*
            foreach (string str in m_adventureEventModel.GetEventList(eventId))
            {
                m_adventureEventModel.SetCurrentEventString(str);
                m_commandSelectId = await m_onAdventureEvents[m_adventureEventModel.CurrentCommand].OnExecute(m_adventureEventModel.CurrentText);
            }
            */

            var eventList = m_adventureEventModel.GetEventList(eventId);

            while (m_adventureEventModel.CurrentIndex < m_adventureEventModel.MaxEventCommandCount)
            {
                m_adventureEventModel.SetCurrentEventCommand(eventList[m_adventureEventModel.CurrentIndex]);
                m_adventureEventModel.SetCurrentEventString(eventList[m_adventureEventModel.CurrentIndex]);

                if (m_adventureEventModel.IsExecute())
                {
                    m_commandIndex = await m_onAdventureEvents[m_adventureEventModel.CurrentCommand].OnExecute(m_adventureEventModel.CurrentText);
                    Debug.Log(m_adventureEventModel.CurrentCommand);
                }
                m_adventureEventModel.OnCommandEvent();
            }
        }

        /// <summary>
        /// �I������
        /// </summary>
        public void OnFinalize()
        {
            // �C�x���g�I������
            m_talkEvent.OnFinalize();

            m_onAdventureEvents.Clear();
        }
    }
}