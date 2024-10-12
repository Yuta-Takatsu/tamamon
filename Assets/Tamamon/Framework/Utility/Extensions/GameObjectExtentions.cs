using UnityEngine;

namespace Framework
{
    /// <summary>
    /// GameObjectのExtentionクラス
    /// </summary>
    public static class GameObjectExtentions
    {
        /// <summary>
        /// コンポーネントを取得
        /// 無ければ追加してコンポーネントを返す
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
        /// 指定コンポーネントの存在チェック
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static bool HasComponent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.TryGetComponent<T>(out _);
        }

        /// <summary>
        /// コンポーネントオブジェクトの表示切替
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject"></param>
        /// <param name="isActive"></param>
        public static void SetActive<T>(this GameObject gameObject, bool isActive) where T : Component
        {
            gameObject.SetActive(isActive);
        }

        /// <summary>
        /// レイヤーチェック
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