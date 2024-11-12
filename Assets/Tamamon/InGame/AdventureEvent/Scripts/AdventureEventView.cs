using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using Tamamon.UI;

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
        private UI.CommandWindowBase m_selectCommandWindow = default;

        private TypeWriteEffect m_typeWriteEffect = default;

        /// <summary>
        /// ������
        /// </summary>
        public void OnInitialize()
        {
            m_typeWriteEffect = new TypeWriteEffect();

            // UI��\��
            m_textWindow.SetActive(false);
            m_selectCommandWindow.gameObject.SetActive(false);

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

        /// <summary>
        /// �R�}���h�E�B���h�E�\������
        /// </summary>
        /// <param name="comanndTextList"></param>
        /// <returns></returns>
        public async UniTask OnOpenSelectCommandWindow(List<string> comanndTextList)
        {
            m_selectCommandWindow.OnInitialize(comanndTextList);
            await m_selectCommandWindow.Show();
        }

        /// <summary>
        /// �R�}���h�E�B���h�E��\������
        /// </summary>
        public int OnCloseSelectCommandWindow()
        {
            m_selectCommandWindow.OnFinarize();
            Debug.Log(m_selectCommandWindow.SelectIndex);
            return m_selectCommandWindow.SelectIndex;
        }

        /// <summary>
        /// �R�}���h�E�B���h�E���\�������ǂ���
        /// </summary>
        /// <returns></returns>
        public bool IsShow()
        {
            return m_selectCommandWindow.IsShow;
        }
    }
}