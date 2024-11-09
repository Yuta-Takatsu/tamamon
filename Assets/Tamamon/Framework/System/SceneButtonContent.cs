using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Tamamon.InGame.AdventureEvent;

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
            Button adventureEventButton = Instantiate(m_button, m_button.transform.parent);
            adventureEventButton.GetComponentInChildren<TextMeshProUGUI>().text = "AdventureEvent";
            Button battleButton = Instantiate(m_button, m_button.transform.parent);
            battleButton.GetComponentInChildren<TextMeshProUGUI>().text = "Battle";
            
            titleButton.onClick.AddListener(async () => await SceneManager.Instance.LoadSceneAsync("Title"));
            adventureButton.onClick.AddListener(async () => await SceneManager.Instance.LoadSceneAsync("Cty001"));
            adventureEventButton.onClick.AddListener(async () => await AdventureEventManager.Instance.LoadScene(UnityEngine.SceneManagement.LoadSceneMode.Single));
            battleButton.onClick.AddListener(async () => await BattleManager.Instance.LoadScene(3, UnityEngine.SceneManagement.LoadSceneMode.Single));

            m_button.gameObject.SetActive(false);
        }
    }
}