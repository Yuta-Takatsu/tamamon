using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Tamamon.Framework
{
    public class SceneManager : MonoBehaviourSingleton<SceneManager>
    {
        public async UniTask LoadSceneAsync(string name, UnityEngine.SceneManagement.LoadSceneMode mode = UnityEngine.SceneManagement.LoadSceneMode.Single)
        {
            await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name, mode);
        }

        public async UniTask UnLoadSceneAsync(string name)
        {
            await UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(name);
            await Resources.UnloadUnusedAssets();
        }
    }
}