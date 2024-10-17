using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// ���͂ɂ��C�x���g���Ǘ�����
    /// </summary>
    public class InputEventManager : MonoBehaviourSingleton<InputEventManager>
    {
        private Dictionary<InputManager.Key, EventHandler> onKeyEvents = new Dictionary<InputManager.Key, EventHandler>();
        private Dictionary<InputManager.Key, EventHandler> onKeyDownEvents = new Dictionary<InputManager.Key, EventHandler>();
        private Dictionary<InputManager.Key, EventHandler> onKeyUpEvents = new Dictionary<InputManager.Key, EventHandler>();
        private Dictionary<InputManager.Key, EventHandler> onKeyNotPressedEvents = new Dictionary<InputManager.Key, EventHandler>();
        private Dictionary<InputManager.Axes, EventHandler> onAxesEvents = new Dictionary<InputManager.Axes, EventHandler>();
        private Dictionary<InputManager.Axes, EventHandler> onAxesRowEvents = new Dictionary<InputManager.Axes, EventHandler>();

        // ���͂ł��邩�ǂ���
        public bool IsInput { get; set; }

        public override void Awake()
        {
            base.Awake();

            IsInput = true;

            //�L�[�̎�ނ̐������C�x���g�𐶐�����
            foreach (InputManager.Key key in InputManager.Key.AllKeyData)
            {
                onKeyEvents.Add(key, (o, a) => { });
                onKeyDownEvents.Add(key, (o, a) => { });
                onKeyUpEvents.Add(key, (o, a) => { });
                onKeyNotPressedEvents.Add(key, (o, a) => { });
            }

            foreach (InputManager.Axes axes in InputManager.Axes.AllAxesData)
            {
                onAxesEvents.Add(axes, (o, a) => { });
                onAxesRowEvents.Add(axes, (o, a) => { });
            }
        }

        public void Update()
        {
            if (!IsInput)
                return;

            // HACK:EventArgs���p�����ėl�X�ȃf�[�^���󂯓n����悤�ɏo����

            KeyEventInvoke(InputManager.Instance.GetKey, onKeyEvents, new EventArgs());
            KeyEventInvoke(InputManager.Instance.GetKeyDown, onKeyDownEvents, new EventArgs());
            KeyEventInvoke(InputManager.Instance.GetKeyUp, onKeyUpEvents, new EventArgs());
            KeyEventInvoke((key) => { return !InputManager.Instance.GetKey(key); }, onKeyNotPressedEvents, new EventArgs());

            AxesEventInvoke(onAxesEvents, new EventArgs());
            AxesEventInvoke(onAxesRowEvents, new EventArgs());
        }

        /// <summary>
        /// �L�[���̓C�x���g���Z�b�g����
        /// </summary>
        /// <param name="key">�L�[�̎��</param>
        /// <param name="eventHandler">���s����C�x���g</param>
        public void SetKeyEvent(InputManager.Key key, EventHandler eventHandler)
        {
            onKeyEvents[key] += eventHandler;
        }

        /// <summary>
        /// �L�[���͊J�n���C�x���g���Z�b�g����
        /// </summary>
        /// <param name="key">�L�[�̎��</param>
        /// <param name="eventHandler">���s����C�x���g</param>
        public void SetKeyDownEvent(InputManager.Key key, EventHandler eventHandler)
        {
            onKeyDownEvents[key] += eventHandler;
        }

        /// <summary>
        /// �L�[���͏I�����C�x���g���Z�b�g����
        /// </summary>
        /// <param name="key">�L�[�̎��</param>
        /// <param name="eventHandler">���s����C�x���g</param>
        public void SetKeyUpEvent(InputManager.Key key, EventHandler eventHandler)
        {
            onKeyUpEvents[key] += eventHandler;
        }

        /// <summary>
        /// �L�[��������Ă��Ȃ��ꍇ�̃C�x���g���Z�b�g����
        /// </summary>
        /// <param name="key">�L�[�̎��</param>
        /// <param name="eventHandler">���s����C�x���g</param>
        public void SetKeyNotPressedEvent(InputManager.Key key, EventHandler eventHandler)
        {
            onKeyNotPressedEvents[key] += eventHandler;
        }

        /// <summary>
        /// �����͎��C�x���g���Z�b�g����
        /// </summary>
        /// <param name="axes">���̎��</param>
        /// <param name="eventHandler">���s����C�x���g</param>
        public void SetAxesEvent(InputManager.Axes axes, EventHandler eventHandler)
        {
            onAxesEvents[axes] += eventHandler;
        }

        /// <summary>
        /// �����͎��C�x���g���Z�b�g����
        /// </summary>
        /// <param name="axes">���̎��</param>
        /// <param name="eventHandler">���s����C�x���g</param>
        public void SetAxesRowEvent(InputManager.Axes axes, EventHandler eventHandler)
        {
            onAxesRowEvents[axes] += eventHandler;
        }

        /// <summary>
        /// �L�[���̓C�x���g����w�肵���C�x���g���폜����
        /// </summary>
        /// <param name="key">�L�[�̎��</param>
        /// <param name="eventHandler">�폜����C�x���g</param>
        public void RemoveKeyEvent(InputManager.Key key, EventHandler eventHandler)
        {
            onKeyEvents[key] -= eventHandler;
        }

        /// <summary>
        /// �L�[���͎��C�x���g����w�肵���C�x���g���폜����
        /// </summary>
        /// <param name="key">�L�[�̎��</param>
        /// <param name="eventHandler">�폜����C�x���g</param>
        public void RemoveKeyDownEvent(InputManager.Key key, EventHandler eventHandler)
        {
            onKeyDownEvents[key] -= eventHandler;
        }

        /// <summary>
        /// �L�[���͏I�����C�x���g����w�肵���C�x���g���폜����
        /// </summary>
        /// <param name="key">�L�[�̎��</param>
        /// <param name="eventHandler">�폜����C�x���g</param>
        public void RemoveKeyUpEvent(InputManager.Key key, EventHandler eventHandler)
        {
            onKeyUpEvents[key] -= eventHandler;
        }

        /// <summary>
        /// �L�[�����͂���Ă��Ȃ��ꍇ�̃C�x���g����w�肵���C�x���g���폜����
        /// </summary>
        /// <param name="key">�L�[�̎��</param>
        /// <param name="eventHandler">�폜����C�x���g</param>
        public void RemoveKeyNotPressedEvent(InputManager.Key key, EventHandler eventHandler)
        {
            onKeyNotPressedEvents[key] -= eventHandler;
        }

        /// <summary>
        /// �����͎��C�x���g���폜����
        /// </summary>
        /// <param name="axes">���̎��</param>
        /// <param name="eventHandler">�폜����C�x���g</param>
        public void RemoveAxesEvent(InputManager.Axes axes, EventHandler eventHandler)
        {
            onAxesEvents[axes] -= eventHandler;
        }

        /// <summary>
        /// �����͎��C�x���g���폜����
        /// </summary>
        /// <param name="axes">���̎��</param>
        /// <param name="eventHandler">�폜����C�x���g</param>
        public void RemoveAxesRowEvent(InputManager.Axes axes, EventHandler eventHandler)
        {
            onAxesRowEvents[axes] -= eventHandler;
        }

        /// <summary>
        /// �o�^���ꂽ�C�x���g��S�č폜����
        /// </summary>
        public void ClearEvent()
        {
            onKeyEvents.Clear();
            onKeyDownEvents.Clear();
            onKeyUpEvents.Clear();
            onKeyNotPressedEvents.Clear();
            onAxesEvents.Clear();
            onAxesRowEvents.Clear();
        }

        /// <summary>
        /// �L�[�C�x���g�����s����
        /// �L�[�����͂���Ă��Ȃ���Ԃ̎��̓C�x���g�����s���Ȃ�
        /// </summary>
        /// <param name="keyEntryDecision">�L�[���͔�����s���q��</param>
        /// <param name="keyEvent">�L�[���Ƃ̃C�x���g���i�[����n�b�V���}�b�v</param>
        /// <param name="args">�C�x���g���s�ɗp�������</param>
        private void KeyEventInvoke(Func<InputManager.Key, bool> keyEntryDecision, Dictionary<InputManager.Key, EventHandler> keyEvent, EventArgs args)
        {
            foreach (InputManager.Key key in InputManager.Key.AllKeyData)
                if (keyEntryDecision(key))
                    if (keyEvent[key] != null)
                        keyEvent[key](this, args);
        }

        /// <summary>
        /// ���C�x���g�����s����
        /// </summary>
        /// <param name="axesEntryDecision">�����͒l�擾���s���q��</param>
        /// <param name="axesEvent">�����Ƃ̃C�x���g���i�[����n�b�V���}�b�v</param>
        /// <param name="args">�C�x���g���s�ɗp�������</param>
        private void AxesEventInvoke(Dictionary<InputManager.Axes, EventHandler> axesEvent, EventArgs args)
        {
            foreach (InputManager.Axes axes in InputManager.Axes.AllAxesData)
                if (axesEvent[axes] != null)
                    axesEvent[axes](this, args);
        }
    }
}