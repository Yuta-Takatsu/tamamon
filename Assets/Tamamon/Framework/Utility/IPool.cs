using UnityEngine.Pool;

namespace Framework
{
    /// <summary>
    /// ObjectPool用インターフェース
    /// </summary>
    public interface IPool<T> where T : class
    {
        public IObjectPool<T> ObjectPool { set; }

        public void OnInitialize();

        public void OnFinalize();
    }
}