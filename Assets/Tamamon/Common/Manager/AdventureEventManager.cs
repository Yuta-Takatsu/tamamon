using Cysharp.Threading.Tasks;
using Framework;

namespace Tamamon.InGame.AdventureEvent
{
    /// <summary>
    /// アドベンチャーパートで起こるイベントの管理クラス
    /// </summary>
    public class AdventureEventManager : MonoBehaviourSingleton<AdventureEventManager>
    {
        private AdventureEventController m_controller = default;

        /// <summary>
        /// シーン読み込み
        /// </summary>
        /// <returns></returns>
        public async UniTask LoadScene(UnityEngine.SceneManagement.LoadSceneMode mode = UnityEngine.SceneManagement.LoadSceneMode.Additive)
        {
            await SceneManager.Instance.LoadSceneAsync("AdventureEvent", mode);

            // AdventureEventSceneのAdventureEventControllerを取得
            m_controller = SceneManager.Instance.GetSceneObjectByName("AdventureEvent", "AdventureEventController").GetComponent<AdventureEventController>();

            m_controller.OnInitialize();

            await OnExecute(1);
        }

        /// <summary>
        /// シーン破棄
        /// </summary>
        /// <returns></returns>
        public async UniTask UnLoadScene()
        {
            m_controller.OnFinalize();
            await SceneManager.Instance.UnLoadSceneAsync("AdventureEvent");
        }

        /// <summary>
        /// イベント実行
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async UniTask OnExecute(int eventId)
        {
            await m_controller.OnExecute(eventId);
        }
    }
}