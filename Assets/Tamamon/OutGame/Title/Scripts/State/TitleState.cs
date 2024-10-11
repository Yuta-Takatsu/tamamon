using UnityEngine;
using Cysharp.Threading.Tasks;
using Framework.Sound;

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
            // BGM�Đ�
            SoundManager.Instance.UpdateBGM(SoundManager.BGM_Type.Title, m_titleView.TitleBGM);
            SoundManager.Instance.PlayBGM(SoundManager.BGM_Type.Title, isCrossFade: false);

            ChangeScene().Forget();
            m_titleView.OnExecute();
        }

        public void OnFinalize()
        {
            m_titleView.OnFinalize();
        }

        // �V�[���؂�ւ�
        public async UniTask ChangeScene()
        {
            await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
            await BattleManager.Instance.LoadBattleScene(3);
        }
    }
}