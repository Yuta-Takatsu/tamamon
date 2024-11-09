using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// ���͊֘A�̊Ǘ�
    /// </summary>
    public class InputManager : MonoBehaviourSingleton<InputManager>
    {
        public enum InputEnum
        {
            None,
            Top,
            Bottom,
            Left,
            Right,
            Decision,
            Cancel,
            Select,
            Horizontal,
            Vertical,
        }

        private Array m_keyCodeValues;
        private KeyConfig m_keyConfig;

        public override void Awake()
        {
            base.Awake();
            m_keyConfig = new KeyConfig();

            // �f�t�H���g�ݒ��ǂݍ���
            SetDefaultKeyConfig();
        }

        /// <summary>
        /// ��������keyCode���X�g��Ԃ�
        /// </summary>
        /// <returns></returns>
        private List<KeyCode> GetCurrentInputKey()
        {
            List<KeyCode> keyCodeList = new List<KeyCode>();

            if (m_keyCodeValues == null)
            {
                m_keyCodeValues = Enum.GetValues(typeof(KeyCode));
            }

            foreach (var keyCode in m_keyCodeValues)
            {
                if (Input.GetKey((KeyCode)(int)keyCode))
                {
                    keyCodeList.Add((KeyCode)(int)keyCode);
                }
            }
            return keyCodeList;
        }

        /// <summary>
        /// ������Ă���L�[��ݒ�
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool SetCurrentInputKey(Key key)
        {
            List<KeyCode> currentInputKeyList = GetCurrentInputKey();

            if (currentInputKeyList == null || currentInputKeyList.Count < 1)
            {
                return false;
            }

            var keyCode = m_keyConfig.GetKeyCode(key.InputType);

            // ���ɐݒ肳��Ă���L�[�Ɠ����L�[����������Ă���ꍇ
            if (keyCode.Count > currentInputKeyList.Count && currentInputKeyList.All(code => keyCode.Contains(code)))
            {
                return false;
            }

            RemoveKey(key);
            return SetKey(key, currentInputKeyList);
        }

        /// <summary>
        /// �R���t�B�O�ɃL�[���Z�b�g
        /// </summary>
        /// <param name="key"></param>
        /// <param name="keyCodeList"></param>
        /// <returns></returns>
        public bool SetKey(Key key, List<KeyCode> keyCodeList)
        {
            return m_keyConfig.SetKey(key.InputType, keyCodeList);
        }

        /// <summary>
        /// �R���t�B�O����l������
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool RemoveKey(Key key)
        {
            return m_keyConfig.RemoveKey(key.InputType);
        }

        /// <summary>
        /// �w�肳�ꂽ�L�[�Ɋ�������Ă���L�[�^�C�v��Ԃ�
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<KeyCode> GetKeyCode(Key key)
        {
            return m_keyConfig.GetKeyCode(key.InputType);
        }

        /// <summary>
        /// �f�t�H���g�̃L�[�ݒ�
        /// </summary>
        public void SetDefaultKeyConfig()
        {
            foreach (var key in Key.AllKeyData)
                SetKey(key, key.DefaultKeyCode);
        }

        /// <summary>
        /// �w�肵���L�[�����͂��ꂽ��(������)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool GetKey(Key key)
        {
            return m_keyConfig.GetKey(key.InputType);
        }

        /// <summary>
        /// �w�肵���L�[�����͂��ꂽ��(�P����)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool GetKeyDown(Key key)
        {
            return m_keyConfig.GetKeyDown(key.InputType);
        }

        /// <summary>
        /// �w�肳�ꂽ�L�[�������ꂽ���ǂ���
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool GetKeyUp(Key key)
        {
            return m_keyConfig.GetKeyUp(key.InputType);
        }

        /// <summary>
        /// �����͂ɑ΂���l��Ԃ�
        /// </summary>
        /// <param name="axes"></param>
        /// <returns></returns>
        public float GetAxes(Axes axes)
        {
            return Input.GetAxis(axes.AxesToString());
        }

        /// <summary>
        /// �����͂ɑ΂���l��Ԃ�
        /// </summary>
        /// <param name="axes"></param>
        /// <returns></returns>
        public float GetAxesRaw(Axes axes)
        {
            return Input.GetAxisRaw(axes.AxesToString());
        }

        /// <summary>
        /// ���͒l�̊��N���X
        /// </summary>
        public class InputValue
        {
            public readonly InputEnum InputType;

            protected InputValue(InputEnum type)
            {
                InputType = type;
            }
        }

        /// <summary>
        /// �g�p����L�[��\���N���X
        /// </summary>
        public class Key : InputValue
        {
            public readonly List<KeyCode> DefaultKeyCode;
            public readonly static List<Key> AllKeyData = new List<Key>();

            private Key(InputEnum inputType, List<KeyCode> defaultKeyCode)
                : base(inputType)
            {
                DefaultKeyCode = defaultKeyCode;
                AllKeyData.Add(this);
            }

            public static readonly Key Top = new Key(InputEnum.Top, new List<KeyCode>() { KeyCode.UpArrow });
            public static readonly Key Bottom = new Key(InputEnum.Bottom, new List<KeyCode>() { KeyCode.DownArrow });
            public static readonly Key Left = new Key(InputEnum.Left, new List<KeyCode>() { KeyCode.LeftArrow });
            public static readonly Key Right = new Key(InputEnum.Right, new List<KeyCode>() { KeyCode.RightArrow });
            public static readonly Key Decision = new Key(InputEnum.Decision, new List<KeyCode>() { KeyCode.Z });
            public static readonly Key Cancel = new Key(InputEnum.Cancel, new List<KeyCode>() { KeyCode.X });
            public static readonly Key Select = new Key(InputEnum.Select, new List<KeyCode>() { KeyCode.C });
        }

        /// <summary>
        /// �g�p���鎲���͂�\���N���X
        /// </summary>
        public class Axes : InputValue
        {
            public readonly static List<Axes> AllAxesData = new List<Axes>();

            private Axes(InputEnum inputType)
                : base(inputType)
            {
                AllAxesData.Add(this);
            }

            public string AxesToString()
            {
                if (InputType == InputEnum.Horizontal)
                {
                    return "Horizontal";
                }
                else if (InputType == InputEnum.Vertical)
                {
                    return "Vertical";
                }
                else
                {
                    return string.Empty;
                }
            }

            public static Axes Horizontal = new Axes(InputEnum.Horizontal);
            public static Axes Vertical = new Axes(InputEnum.Vertical);
        }
    }
}