using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace Framework
{
    public class SceneManager : MonoBehaviourSingleton<SceneManager>
    {

        public async UniTask LoadSceneAsync(string name, UnityEngine.SceneManagement.LoadSceneMode mode = UnityEngine.SceneManagement.LoadSceneMode.Single, bool isFade = true)
        {
            if (isFade) await FadeManager.Instance.FadeIn();

            await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name, mode);

            await FadeManager.Instance.FadeOut();
        }

        public async UniTask UnLoadSceneAsync(string name, bool isFade = true)
        {
            if (isFade) await FadeManager.Instance.FadeIn();

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
    }
}