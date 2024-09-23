using UnityEngine;
using Cysharp.Threading.Tasks;

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
            SkipScene().Forget();
            m_openingView.OnExecute();
        }

        public void OnFinalize()
        {
            m_openingView.OnFinalize();
        }

        public async UniTask SkipScene()
        {
            await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
            m_openingView.NextState();
        }
    }
}