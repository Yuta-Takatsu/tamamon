using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Framework;

namespace Tamamon.InGame.AdventureEvent
{
    /// <summary>
    /// 選択肢イベント
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
            m_adventureEventModel.AddSelectEvent(0);
            return 0;

            // 入力待ち
            await UniTask.WaitUntil(() => InputManager.Instance.GetKeyDown(InputManager.Key.Decision));
            
            m_index = 0;
            
            return m_index;
        }
    }
}