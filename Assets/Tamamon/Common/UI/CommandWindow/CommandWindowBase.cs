using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace Tamamon.UI
{
    /// <summary>
    /// �����R�}���hUI�̐���p�x�[�X�N���X
    /// </summary>
    public class CommandWindowBase : PoolManager<CommandWindowText>
    {
        // �������v���n�u
        [SerializeField]
        protected CommandWindowText m_commandWindowText = default;

        // �R�}���h���
        protected List<CommandWindowText> m_commandWindowTextList = new List<CommandWindowText>();
        protected List<string> m_commandStringList = new List<string>();

        // �I�����
        public int SelectIndex { get; protected set; } = default;
        public int PrevSelectIndex { get; protected set; } = default;

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="commandStringList"></param>
        public virtual void OnInitialize(List<string> commandStringList)
        {
            m_commandStringList = commandStringList;

            // �R�}���h����
            foreach (string str in m_commandStringList)
            {
                CommandWindowText commandWindowText = Instantiate(m_commandWindowText, m_commandWindowText.transform.parent);
                commandWindowText.OnInitialize(str);
                m_commandWindowTextList.Add(commandWindowText);
            }

            // �A���[UI������
            ResetArrowActive();

            // �������v���n�u���\��
            m_commandWindowText.gameObject.SetActive(false);
        }

        /// <summary>
        /// �A���[UI�̕\���ؑ�
        /// </summary>
        public virtual void SetArrowActive()
        {
            if (PrevSelectIndex < m_commandWindowTextList.Count)
            {
                m_commandWindowTextList[PrevSelectIndex].SetActiveArrow(false);
            }
            m_commandWindowTextList[PrevSelectIndex].SetActiveArrow(true);
        }

        /// <summary>
        /// �A���[UI�̕\���������ɖ߂�
        /// </summary>
        public virtual void ResetArrowActive()
        {
            PrevSelectIndex = SelectIndex;
            SelectIndex = 0;
            SetArrowActive();
        }
    }
}