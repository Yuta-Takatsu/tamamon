using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// セーブしたいデータを定義
    /// </summary>
    [System.Serializable]
    public class SaveData
    {
        public string PlayerName;
        public List<int> Party;

        public override string ToString()
        {
            return $"{base.ToString()} {JsonUtility.ToJson(this)}";
        }
    }
}