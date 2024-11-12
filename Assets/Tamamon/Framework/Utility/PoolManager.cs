using UnityEngine;
using UnityEngine.Pool;

namespace Framework
{
    /// <summary>
    /// Pool�I�u�W�F�N�g�������}�l�[�W���[�N���X
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class PoolManager<T> : MonoBehaviour where T : MonoBehaviour, IPool<T>
    {
        [SerializeField]
        protected T m_poolObject = default;

        protected IObjectPool<T> m_objectPool = default;

        [SerializeField]
        private bool m_isCollection = true;

        [SerializeField]
        private int m_defaultCapacity = 32;
        [SerializeField]
        private int m_maxSize = 100;

        /// <summary>
        /// ������
        /// </summary>
        public virtual void OnInitialize()
        {
            m_objectPool = new ObjectPool<T>(
                OnCreate,
                OnGetFromPool,
                OnReleaseToPool,
                OnDestroyPoolObject,
                m_isCollection,
                m_defaultCapacity,
                m_maxSize);

            m_poolObject.gameObject.SetActive(false);
        }

        /// <summary>
        /// �I�u�W�F�N�g����
        /// </summary>
        /// <returns></returns>
        protected virtual T OnCreate()
        {
            T instance = Instantiate(m_poolObject, m_poolObject.transform.parent);
            instance.ObjectPool = m_objectPool;
            instance.gameObject.SetActive(true);
            return instance;
        }

        /// <summary>
        /// �I�u�W�F�N�g���v�[���ɑ���
        /// </summary>
        /// <param name="poolObject"></param>
        protected virtual void OnReleaseToPool(T poolObject)
        {
            poolObject.gameObject.SetActive(false);
        }

        /// <summary>
        /// �I�u�W�F�N�g���v�[��������o��
        /// </summary>
        /// <param name="poolObject"></param>
        protected virtual void OnGetFromPool(T poolObject)
        {
            poolObject.gameObject.SetActive(true);
        }

        /// <summary>
        /// �I�u�W�F�N�g�̔j��
        /// </summary>
        /// <param name="poolObject"></param>
        protected virtual void OnDestroyPoolObject(T poolObject)
        {
            poolObject.OnFinalize();
            Destroy(poolObject.gameObject);
        }
    }
}