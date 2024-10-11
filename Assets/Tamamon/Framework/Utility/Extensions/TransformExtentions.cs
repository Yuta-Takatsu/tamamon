using UnityEngine;

namespace Framework
{
    public static class TransformExtentions
    {
        /// <summary>
        /// 子オブジェクトの追加
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="children"></param>
        public static void AddChildren(this Transform transform,params GameObject[] children)
        {
            foreach(var child in children)
            {
                child.transform.parent = transform;
            }
        }

        /// <summary>
        /// 子オブジェクトの追加
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="children"></param>
        public static void AddChildren(this Transform transform, params Component[] children)
        {
            foreach (var child in children)
            {
                child.transform.parent = transform;
            }
        }

        /// <summary>
        /// 座標の初期化
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="space"></param>
        public static void Reset(this Transform transform , Space space = Space.Self)
        {
            switch (space)
            {
                case Space.Self:
                    transform.localPosition = Vector3.zero;
                    transform.localRotation = Quaternion.identity;
                    break;

                case Space.World:
                    transform.position = Vector3.zero;
                    transform.rotation = Quaternion.identity;
                    break;
            }

            transform.localScale = Vector3.one;
        }

        /// <summary>
        /// 座標の初期化
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="recursive"></param>
        public static void ResetChildPositions(this Transform transform, bool recursive = false)
        {
            foreach (Transform child in transform)
            {
                child.position = Vector3.zero;

                if (recursive)
                {
                    child.ResetChildPositions(true);
                }
            }
        }

        /// <summary>
        /// 子オブジェクトのレイヤーをセット
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="layerName"></param>
        /// <param name="recursive"></param>
        public static void SetChildLayers(this Transform transform, string layerName, bool recursive = false)
        {
            var layer = LayerMask.NameToLayer(layerName);
            SetChildLayersHelper(transform, layer, recursive);
        }

        /// <summary>
        /// 子オブジェクトのレイヤーをセット
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="layer"></param>
        /// <param name="recursive"></param>
        private static void SetChildLayersHelper(Transform transform, int layer, bool recursive)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = layer;

                if (recursive)
                {
                    SetChildLayersHelper(child, layer, true);
                }
            }
        }

        /// <summary>
        /// ローカルポジションのセット
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public static void SetLocalPosition(this Transform transform, float? x = null, float? y = null, float? z = null)
        {
            var localPosition = transform.localPosition;

            if (x.HasValue)
            {
                localPosition.x = x.Value;
            }

            if (y.HasValue)
            {
                localPosition.y = y.Value;
            }

            if (z.HasValue)
            {
                localPosition.z = z.Value;
            }

            transform.localPosition = localPosition;
        }

        /// <summary>
        /// ポジションのセット
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public static void SetPosition(this Transform transform, float? x = null, float? y = null, float? z = null)
        {
            var position = transform.position;

            if (x.HasValue)
            {
                position.x = x.Value;
            }

            if (y.HasValue)
            {
                position.y = y.Value;
            }

            if (z.HasValue)
            {
                position.z = z.Value;
            }

            transform.position = position;
        }

        /// <summary>
        /// X座標のセット
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="x"></param>
        public static void SetX(this Transform transform, float x)
        {
            var position = transform.position;
            transform.position = new Vector3(x, position.y, position.z);
        }

        /// <summary>
        /// Y座標のセット
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="y"></param>
        public static void SetY(this Transform transform, float y)
        {
            var position = transform.position;
            transform.position = new Vector3(position.x, y, position.z);
        }

        /// <summary>
        /// Z座標のセット
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="z"></param>
        public static void SetZ(this Transform transform, float z)
        {
            var position = transform.position;
            transform.position = new Vector3(position.x, position.y, z);
        }
    }
}