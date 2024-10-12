using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework
{
    /// <summary>
    /// ゲーム開始(起動)時に一度だけ初期化処理するクラス
    /// </summary>
    public class SystemManager : MonoBehaviour
    {

        [SerializeField]
        private List<GameObject> m_managerList = new List<GameObject>();

        // 初期化処理が完了したか
        public static bool IsInitialized { get; private set; } = false;

        // ゲーム開始時(シーン読み込み前、Awake前)に実行される
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnInitialize()
        {
            string sceneName = "Boot";

            if (!UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName).IsValid())
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);

            foreach (GameObject manager in m_managerList)
            {
                Instantiate(manager);
            }

            IsInitialized = true;
        }
    }
}