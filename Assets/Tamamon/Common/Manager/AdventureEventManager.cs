using Cysharp.Threading.Tasks;
using Framework;

namespace Tamamon.InGame.AdventureEvent
{
    /// <summary>
    /// �A�h�x���`���[�p�[�g�ŋN����C�x���g�̊Ǘ��N���X
    /// </summary>
    public class AdventureEventManager : MonoBehaviourSingleton<AdventureEventManager>
    {
        private AdventureEventController m_controller = default;

        /// <summary>
        /// �V�[���ǂݍ���
        /// </summary>
        /// <returns></returns>
        public async UniTask LoadScene(UnityEngine.SceneManagement.LoadSceneMode mode = UnityEngine.SceneManagement.LoadSceneMode.Additive)
        {
            await SceneManager.Instance.LoadSceneAsync("AdventureEvent", mode);

            // AdventureEventScene��AdventureEventController���擾
            m_controller = SceneManager.Instance.GetSceneObjectByName("AdventureEvent", "AdventureEventController").GetComponent<AdventureEventController>();

            m_controller.OnInitialize();

            await OnExecute(1);
        }

        /// <summary>
        /// �V�[���j��
        /// </summary>
        /// <returns></returns>
        public async UniTask UnLoadScene()
        {
            m_controller.OnFinalize();
            await SceneManager.Instance.UnLoadSceneAsync("AdventureEvent");
        }

        /// <summary>
        /// �C�x���g���s
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async UniTask OnExecute(int eventId)
        {
            await m_controller.OnExecute(eventId);
        }
    }
}