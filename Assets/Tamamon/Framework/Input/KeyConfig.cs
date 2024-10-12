using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class KeyConfig
    {
        private Dictionary<InputManager.InputEnum, List<KeyCode>> m_keyConfig = new Dictionary<InputManager.InputEnum, List<KeyCode>>();

        /// <summary>
        /// �w�肵���L�[�����͂��ꂽ��
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
        /// �w�肵���L�[�����͂��ꂽ��(�P����)
        /// </summary>
        /// <param name="inputType"></param>
        /// <returns></returns>
        public bool GetKey(InputManager.InputEnum inputType)
        {
            return IsInputKey(inputType, Input.GetKey);
        }

        /// <summary>
        /// �w�肵���L�[�����͂��ꂽ��(������)
        /// </summary>
        /// <param name="inputType"></param>
        /// <returns></returns>
        public bool GetKeyDown(InputManager.InputEnum inputType)
        {
            return IsInputKey(inputType, Input.GetKeyDown);
        }

        /// <summary>
        /// �w�肵���L�[�������ꂽ���ǂ���
        /// </summary>
        /// <param name="inputType"></param>
        /// <returns></returns>
        public bool GetKeyUp(InputManager.InputEnum inputType)
        {
            return IsInputKey(inputType, Input.GetKeyUp);
        }

        /// <summary>
        /// �w�肳�ꂽ�L�[�Ɋ�������Ă���L�[�^�C�v��Ԃ�
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
        /// �L�[�^�C�v�̐ݒ�
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
        /// �R���t�B�O����L�[�^�C�v���폜
        /// </summary>
        /// <param name="inputType"></param>
        /// <returns></returns>
        public bool RemoveKey(InputManager.InputEnum inputType)
        {
            return m_keyConfig.Remove(inputType);
        }
    }
}