using System;
using System.Collections.Generic;
using Framework;
using Cysharp.Threading.Tasks;

namespace Tamamon.UI
{
    /// <summary>
    /// �����R�}���hUI�̐���p�x�[�X�N���X
    /// </summary>
    public class CommandWindowBase : PoolManager<CommandWindowText>
    {
        // �R�}���h���
        protected List<CommandWindowText> m_commandWindowTextList = new List<CommandWindowText>();
        protected List<string> m_commandStringList = new List<string>();

        // �I�����
        public int SelectIndex { get; protected set; } = default;
        public int PrevSelectIndex { get; protected set; } = default;

        public bool IsShow { get; protected set; } = false;

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="commandStringList"></param>
        public virtual void OnInitialize(List<string> commandStringList)
        {
            if (m_objectPool == null)
            {
                base.OnInitialize();
            }

            m_commandStringList = commandStringList;

            // �R�}���h����
            foreach (string str in m_commandStringList)
            {
                OnCreateCommand(str);
            }

            // �I�u�W�F�N�g���ёւ�
            for (int i = m_commandWindowTextList.Count - 1; i >= 0; i--)
            {
                m_commandWindowTextList[i].transform.SetSiblingIndex(i);
            }

            // �A���[UI������
            ResetArrowActive();

            // ���̓C�x���g
            AddInputEvent();
        }

        /// <summary>
        /// �I������
        /// </summary>
        public virtual void OnFinarize()
        {
            var tmpList = new List<CommandWindowText>(m_commandWindowTextList);
            foreach (var command in tmpList)
            {
                OnReleaseCommand(command);
            }
        }

        /// <summary>
        /// �R�}���hUI����
        /// </summary>
        /// <param name="command"></param>
        public virtual void OnCreateCommand(string command)
        {
            CommandWindowText commandWindowText = m_objectPool.Get();
            commandWindowText.OnInitialize(command);
            m_commandWindowTextList.Add(commandWindowText);
        }

        /// <summary>
        /// �R�}���hUI�j��
        /// </summary>
        /// <param name="commandWindowText"></param>
        public virtual void OnReleaseCommand(CommandWindowText commandWindowText)
        {
            m_commandWindowTextList.Remove(commandWindowText);
            commandWindowText.OnFinalize();
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
            m_commandWindowTextList[SelectIndex].SetActiveArrow(true);
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

        protected EventHandler m_decisionInputEventHandler = null;
        protected EventHandler m_topInputEventHandler = null;
        protected EventHandler m_BottomInputEventHandler = null;

        /// <summary>
        /// ���͎��̏�������
        /// </summary>
        public virtual void AddInputEvent()
        {
            m_decisionInputEventHandler = async (object sender, EventArgs e) =>
            {
                await Hide();
            };

            m_topInputEventHandler = (object sender, EventArgs e) =>
            {
                if (SelectIndex > 0)
                {
                    PrevSelectIndex = SelectIndex;
                    SelectIndex--;
                    SetArrowActive();
                }
            };

            m_BottomInputEventHandler = (object sender, EventArgs e) =>
            {
                if (SelectIndex < m_commandWindowTextList.Count - 1)
                {
                    PrevSelectIndex = SelectIndex;
                    SelectIndex++;
                    SetArrowActive();
                }
            };
        }

        /// <summary>
        /// �\��
        /// </summary>
        public virtual async UniTask Show()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));

            // ���̓C�x���g�o�^
            InputEventManager.Instance.SetKeyDownEvent(InputManager.Key.Decision, m_decisionInputEventHandler);
            InputEventManager.Instance.SetKeyDownEvent(InputManager.Key.Top, m_topInputEventHandler);
            InputEventManager.Instance.SetKeyDownEvent(InputManager.Key.Bottom, m_BottomInputEventHandler);

            IsShow = true;
            this.gameObject.SetActive(true);
        }

        /// <summary>
        /// ��\��
        /// </summary>
        protected virtual async UniTask Hide()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));

            // ���̓C�x���g�j��
            InputEventManager.Instance.RemoveKeyDownEvent(InputManager.Key.Decision, m_decisionInputEventHandler);
            InputEventManager.Instance.RemoveKeyDownEvent(InputManager.Key.Top, m_topInputEventHandler);
            InputEventManager.Instance.RemoveKeyDownEvent(InputManager.Key.Bottom, m_BottomInputEventHandler);

            IsShow = false;
            this.gameObject.SetActive(false);
        }
    }
}