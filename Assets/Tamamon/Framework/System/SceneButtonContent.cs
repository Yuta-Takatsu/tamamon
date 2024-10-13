using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
            // コールバック登録
            Button titleButton = Instantiate(m_button, m_button.transform.parent);
            titleButton.GetComponentInChildren<TextMeshProUGUI>().text = "Title";
            Button adventureButton = Instantiate(m_button, m_button.transform.parent);
            adventureButton.GetComponentInChildren<TextMeshProUGUI>().text = "Adventure";
            Button battleButton = Instantiate(m_button, m_button.transform.parent);
            battleButton.GetComponentInChildren<TextMeshProUGUI>().text = "Battle";

            titleButton.onClick.AddListener(async () => await SceneManager.Instance.LoadSceneAsync("Title"));
            adventureButton.onClick.AddListener(async () => await SceneManager.Instance.LoadSceneAsync("Cty001"));
            battleButton.onClick.AddListener(async () => await SceneManager.Instance.LoadSceneAsync("Battle"));

            m_button.gameObject.SetActive(false);
        }
    }
}