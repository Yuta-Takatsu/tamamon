using UnityEngine;

namespace Framework
{
    /// <summary>
    /// GameObject��Extention�N���X
    /// </summary>
    public static class GameObjectExtentions
    {
        /// <summary>
        /// �R���|�[�l���g���擾
        /// ������Βǉ����ăR���|�[�l���g��Ԃ�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            if (!gameObject.TryGetComponent<T>(out var component))
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }

        /// <summary>
        /// �w��R���|�[�l���g�̑��݃`�F�b�N
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static bool HasComponent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.TryGetComponent<T>(out _);
        }

        /// <summary>
        /// �R���|�[�l���g�I�u�W�F�N�g�̕\���ؑ�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject"></param>
        /// <param name="isActive"></param>
        public static void SetActive<T>(this GameObject gameObject, bool isActive) where T : Component
        {
            gameObject.SetActive(isActive);
        }

        /// <summary>
        /// ���C���[�`�F�b�N
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="layerMask"></param>
        /// <returns></returns>
        public static bool IsInCullingMask(this GameObject gameObject, LayerMask layerMask)
        {
            return (layerMask & (1 << gameObject.layer)) != 0;
        }
    }
}