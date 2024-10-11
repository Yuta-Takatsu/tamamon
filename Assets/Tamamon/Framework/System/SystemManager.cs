using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework
{
    /// <summary>
    /// �Q�[���J�n(�N��)���Ɉ�x������������������N���X
    /// </summary>
    public class SystemManager : MonoBehaviour
    {

        [SerializeField]
        private List<GameObject> m_managerList = new List<GameObject>();

        // ����������������������
        public static bool IsInitialized { get; private set; } = false;

        // �Q�[���J�n��(�V�[���ǂݍ��ݑO�AAwake�O)�Ɏ��s�����
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