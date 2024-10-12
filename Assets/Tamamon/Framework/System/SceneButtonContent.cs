using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

namespace Framework
{
    public class SceneButtonContent : MonoBehaviour
    {
        [SerializeField]
        private Button m_button = default;

        public void Start()
        {
            CreateChangeSceneButton();
        }

        /// <summary>
        /// 指定シーンに遷移するボタンを生成
        /// </summary>
        public void CreateChangeSceneButton()
        {
            // NOTE: EditorBuildSettingsがあるとビルドが通らないので、代替案が出るまで封印
            /*foreach(var scene in EditorBuildSettings.scenes)
            {
                // パス名からシーン名を取得
                string scenePath = scene.path;
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

                // Bootシーンなら飛ばす
                if(sceneName == "Boot")
                {
                    continue;
                }

                // コールバック登録
                Button button = Instantiate(m_button, m_button.transform.parent);
                button.GetComponentInChildren<TextMeshProUGUI>().text = sceneName;

                button.onClick.AddListener(async () => await SceneManager.Instance.LoadSceneAsync($"{sceneName}"));
            }

            m_button.gameObject.SetActive(false);*/
        }
    }
}