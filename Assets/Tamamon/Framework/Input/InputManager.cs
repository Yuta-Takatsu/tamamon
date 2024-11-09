using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 入力関連の管理
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

            // デフォルト設定を読み込み
            SetDefaultKeyConfig();
        }

        /// <summary>
        /// 押下中のkeyCodeリストを返す
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
        /// 押されているキーを設定
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

            // 既に設定されているキーと同じキーが押下されている場合
            if (keyCode.Count > currentInputKeyList.Count && currentInputKeyList.All(code => keyCode.Contains(code)))
            {
                return false;
            }

            RemoveKey(key);
            return SetKey(key, currentInputKeyList);
        }

        /// <summary>
        /// コンフィグにキーをセット
        /// </summary>
        /// <param name="key"></param>
        /// <param name="keyCodeList"></param>
        /// <returns></returns>
        public bool SetKey(Key key, List<KeyCode> keyCodeList)
        {
            return m_keyConfig.SetKey(key.InputType, keyCodeList);
        }

        /// <summary>
        /// コンフィグから値を消去
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool RemoveKey(Key key)
        {
            return m_keyConfig.RemoveKey(key.InputType);
        }

        /// <summary>
        /// 指定されたキーに割りつけられているキータイプを返す
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<KeyCode> GetKeyCode(Key key)
        {
            return m_keyConfig.GetKeyCode(key.InputType);
        }

        /// <summary>
        /// デフォルトのキー設定
        /// </summary>
        public void SetDefaultKeyConfig()
        {
            foreach (var key in Key.AllKeyData)
                SetKey(key, key.DefaultKeyCode);
        }

        /// <summary>
        /// 指定したキーが入力されたか(長押し)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool GetKey(Key key)
        {
            return m_keyConfig.GetKey(key.InputType);
        }

        /// <summary>
        /// 指定したキーが入力されたか(単押し)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool GetKeyDown(Key key)
        {
            return m_keyConfig.GetKeyDown(key.InputType);
        }

        /// <summary>
        /// 指定されたキーが離されたかどうか
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool GetKeyUp(Key key)
        {
            return m_keyConfig.GetKeyUp(key.InputType);
        }

        /// <summary>
        /// 軸入力に対する値を返す
        /// </summary>
        /// <param name="axes"></param>
        /// <returns></returns>
        public float GetAxes(Axes axes)
        {
            return Input.GetAxis(axes.AxesToString());
        }

        /// <summary>
        /// 軸入力に対する値を返す
        /// </summary>
        /// <param name="axes"></param>
        /// <returns></returns>
        public float GetAxesRaw(Axes axes)
        {
            return Input.GetAxisRaw(axes.AxesToString());
        }

        /// <summary>
        /// 入力値の基底クラス
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
        /// 使用するキーを表すクラス
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
        /// 使用する軸入力を表すクラス
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