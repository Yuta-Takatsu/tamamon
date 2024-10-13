using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class KeyConfig
    {
        private Dictionary<InputManager.InputEnum, List<KeyCode>> m_keyConfig = new Dictionary<InputManager.InputEnum, List<KeyCode>>();

        /// <summary>
        /// 指定したキーが入力されたか
        /// </summary>
        /// <param name="inputType"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        private bool IsInputKey(InputManager.InputEnum inputType, Func<KeyCode, bool> predicate)
        {
            foreach (var keyCode in m_keyConfig[inputType])
            {
                if (predicate(keyCode))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 指定したキーが入力されたか(単押し)
        /// </summary>
        /// <param name="inputType"></param>
        /// <returns></returns>
        public bool GetKey(InputManager.InputEnum inputType)
        {
            return IsInputKey(inputType, Input.GetKey);
        }

        /// <summary>
        /// 指定したキーが入力されたか(長押し)
        /// </summary>
        /// <param name="inputType"></param>
        /// <returns></returns>
        public bool GetKeyDown(InputManager.InputEnum inputType)
        {
            return IsInputKey(inputType, Input.GetKeyDown);
        }

        /// <summary>
        /// 指定したキーが離されたかどうか
        /// </summary>
        /// <param name="inputType"></param>
        /// <returns></returns>
        public bool GetKeyUp(InputManager.InputEnum inputType)
        {
            return IsInputKey(inputType, Input.GetKeyUp);
        }

        /// <summary>
        /// 指定されたキーに割りつけられているキータイプを返す
        /// </summary>
        /// <param name="inputType"></param>
        /// <returns></returns>
        public List<KeyCode> GetKeyCode(InputManager.InputEnum inputType)
        {
            if (m_keyConfig.ContainsKey(inputType))
            {
                return new List<KeyCode>(m_keyConfig[inputType]);
            }
            return new List<KeyCode>();
        }

        /// <summary>
        /// キータイプの設定
        /// </summary>
        /// <param name="inputType"></param>
        /// <param name="keyCode"></param>
        /// <returns></returns>
        public bool SetKey(InputManager.InputEnum inputType, List<KeyCode> keyCode)
        {
            if (inputType == InputManager.InputEnum.None || keyCode.Count < 1)
            {
                return false;
            }
            m_keyConfig[inputType] = keyCode;
            return true;
        }

        /// <summary>
        /// コンフィグからキータイプを削除
        /// </summary>
        /// <param name="inputType"></param>
        /// <returns></returns>
        public bool RemoveKey(InputManager.InputEnum inputType)
        {
            return m_keyConfig.Remove(inputType);
        }
    }
}