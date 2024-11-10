using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

namespace Tamamon.InGame.AdventureEvent
{
    /// <summary>
    /// �A�h�x���`���[�p�[�g��view�N���X
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
        /// ������
        /// </summary>
        public void OnInitialize()
        {
            m_typeWriteEffect = new TypeWriteEffect();

            ClearText();
        }

        // <summary>
        /// �e�L�X�g�N���A
        /// </summary>
        public void ClearText()
        {
            m_messageText.text = string.Empty;
        }

        /// <summary>
        /// �e�L�X�g�E�B���h�E�\���ؑ�
        /// </summary>
        /// <param name="isActive"></param>
        public void SetWindow(bool isActive)
        {
            m_textWindow.SetActive(isActive);
        }

        /// <summary>
        /// �e�L�X�g�\��(1��������)
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async UniTask ShowMessage(string message)
        {
            await m_typeWriteEffect.ShowTextMessage(m_messageText, message);
        }

        /// <summary>
        /// �e�L�X�g�\���A�j���[�V�����Đ���
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