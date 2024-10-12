using UnityEngine;

namespace Framework
{
    public static class ComponentExtensions
    {
        /// <summary>
        /// �w��R���|�[�l���g�̒ǉ�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        /// <returns></returns>
        public static T AddComponent<T>(this Component component) where T : Component
        {
            return component.gameObject.AddComponent<T>();
        }

        /// <summary>
        /// �w��R���|�[�l���g�̎擾
        /// ������Βǉ����ĕԂ�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this Component component) where T : Component
        {
            if (!component.TryGetComponent<T>(out var attachedComponent))
            {
                attachedComponent = component.AddComponent<T>();
            }

            return attachedComponent;
        }

        /// <summary>
        /// �w��R���|�[�l���g�̑��݃`�F�b�N
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        /// <returns></returns>
        public static bool HasComponent<T>(this Component component) where T : Component
        {
            return component.TryGetComponent<T>(out _);
        }
    }
}