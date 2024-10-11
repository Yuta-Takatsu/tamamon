using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace Framework
{
    public class SceneManager : MonoBehaviourSingleton<SceneManager>
    {
        [SerializeField]
        private CanvasGroup m_fadePanel = default;

        private bool m_isFade = false;
        private readonly float FadeTime = 1.5f;

        public async UniTask LoadSceneAsync(string name, UnityEngine.SceneManagement.LoadSceneMode mode = UnityEngine.SceneManagement.LoadSceneMode.Single, bool isFade = true)
        {
            if (isFade) await FadeIn();

            await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name, mode);

            await SceneManager.Instance.FadeOut();
        }

        public async UniTask UnLoadSceneAsync(string name, bool isFade = true)
        {
            if (isFade) await FadeIn();

            await UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(name);
            await Resources.UnloadUnusedAssets();
        }

        public UnityEngine.SceneManagement.Scene GetSceneByName(string sceneName)
        {
            return UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
        }

        public GameObject GetSceneObjectByName(string sceneName, string name)
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);

            foreach (var obj in scene.GetRootGameObjects())
            {
                if(obj.name == name)
                {
                    return obj;
                }
            }
            return null;
        }

        public async UniTask FadeIn()
        {
            m_isFade = true;
            m_fadePanel.gameObject.SetActive(true);
            m_fadePanel.alpha = 0f;
            m_fadePanel.DOFade(1f, FadeTime)
                .OnComplete(() =>
            {
                m_fadePanel.alpha = 1f;
                m_isFade = false;
            });

            await UniTask.WaitWhile(() => m_isFade);
        }

        public async UniTask FadeOut()
        {
            m_isFade = true;
            m_fadePanel.alpha = 1f;
            m_fadePanel.DOFade(0f, FadeTime)
                .OnComplete(() =>
                {
                    m_fadePanel.gameObject.SetActive(false);
                    m_fadePanel.alpha = 0f;
                    m_isFade = false;
                });

            await UniTask.WaitWhile(() => m_isFade);
        }
    }
}