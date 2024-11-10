using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

namespace Tamamon.InGame.AdventureEvent
{
    /// <summary>
    /// アドベンチャーパートのviewクラス
    /// </summary>
    public class AdventureEventView : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_textWindow = default;
        [SerializeField]
        private TextMeshProUGUI m_messageText = default;
        [SerializeField]
        private CommandWindowBase m_selectCommandWindow = default;

        private TypeWriteEffect m_typeWriteEffect = default;

        /// <summary>
        /// 初期化
        /// </summary>
        public void OnInitialize()
        {
            m_typeWriteEffect = new TypeWriteEffect();

            ClearText();
        }

        // <summary>
        /// テキストクリア
        /// </summary>
        public void ClearText()
        {
            m_messageText.text = string.Empty;
        }

        /// <summary>
        /// テキストウィンドウ表示切替
        /// </summary>
        /// <param name="isActive"></param>
        public void SetWindow(bool isActive)
        {
            m_textWindow.SetActive(isActive);
        }

        /// <summary>
        /// テキスト表示(1文字送り)
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async UniTask ShowMessage(string message)
        {
            await m_typeWriteEffect.ShowTextMessage(m_messageText, message);
        }

        /// <summary>
        /// テキスト表示アニメーション再生状況
        /// </summary>
        /// <returns></returns>
        public bool IsMessageAnimation()
        {
            return m_typeWriteEffect.IsAnimation;
        }

        public void ShowSelectCommandWindow(List<string> comanndTextList)
        {
            m_selectCommandWindow.OnInitialize(comanndTextList);
        }
    }
}