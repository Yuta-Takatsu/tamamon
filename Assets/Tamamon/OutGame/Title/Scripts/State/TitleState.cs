using UnityEngine;
using Cysharp.Threading.Tasks;

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
            ChangeScene().Forget();
            m_titleView.OnExecute();
        }

        public void OnFinalize()
        {
            m_titleView.OnFinalize();
        }

        // ƒV[ƒ“Ø‚è‘Ö‚¦
        public async UniTask ChangeScene()
        {
            await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
            await BattleManager.Instance.LoadBattleScene(3);
        }
    }
}