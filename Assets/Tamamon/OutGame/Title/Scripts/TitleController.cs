using UnityEngine;
using Tamamon.Framework;
using UniRx;

namespace Tamamon.OutGame.Title
{
    public class TitleController : MonoBehaviour
    {
        [SerializeField]
        private OpeningView m_openingView = default;

        [SerializeField]
        private TitleView m_titleView = default;

        private TitleModel m_titleModel = default;

        private OpeningState m_openingState = default;
        private TitleState m_titleState = default;

        void Start()
        {
            OnInitialize();
        }

        public void OnInitialize()
        {
            m_titleModel = new TitleModel();

            m_openingState = new OpeningState();
            m_titleState = new TitleState();

            m_openingState.OnInitialize(m_openingView);
            m_titleState.OnInitialize(m_titleView);

            m_openingView.SetCallback(() =>
            {
                // ステート切り替え
                m_titleModel.SetTitleState(m_titleState);
            });

            m_titleModel.Observable.Skip(1).Subscribe(state => m_titleModel.OnExecute());

            // BGM再生
            SoundManager.Instance.PlayBGM(SoundManager.BGM_Type.Title);

            // ステート切り替え
            m_titleModel.SetTitleState(m_openingState);
        }
    }
}