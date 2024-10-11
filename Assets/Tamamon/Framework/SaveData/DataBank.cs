using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;

namespace Framework
{
    /// <summary>
    /// セーブデータ管理クラス
    /// </summary>
    public class DataBank : Singleton<DataBank>
    {
        private static Dictionary<string, object> dataBank = new Dictionary<string, object>();

        private static readonly string path = "SaveData";
        private static readonly string fullPath = $"{Application.persistentDataPath}/{path}";
        private static readonly string extension = "dat";

        /// <summary>
        /// セーブデータ保存パス
        /// </summary>
        public string SavePath
        {
            get
            {
                return fullPath;
            }
        }

        /// <summary>
        /// セーブデータの存在チェック
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return dataBank.Count == 0;
        }

        /// <summary>
        /// キーの存在チェック
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsExistsKey(string key)
        {
            return dataBank.ContainsKey(key);
        }

        /// <summary>
        /// セーブデータの更新
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void UpdateData(string key, object obj)
        {
            dataBank[key] = obj;
        }

        /// <summary>
        /// セーブデータの削除
        /// </summary>
        public void Clear()
        {
            dataBank.Clear();
        }

        /// <summary>
        /// 指定キーのデータ削除
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            dataBank.Remove(key);
        }

        /// <summary>
        /// 指定キーのデータを返す
        /// </summary>
        /// <typeparam name="DataType"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public DataType Get<DataType>(string key)
        {
            if (IsExistsKey(key))
            {
                return (DataType)dataBank[key];
            }
            else
            {
                return default(DataType);
            }
        }

        public void SaveAll()
        {
            foreach(string key in dataBank.Keys)
            {
                Save(key);
            }
        }

        /// <summary>
        /// セーブ
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Save(string key)
        {
            // 存在しないデータ
            if (!IsExistsKey(key))
            {
                return false;
            }

            string filePath = $"{fullPath}/{key}.{extension}";

            string json = JsonUtility.ToJson(dataBank[key]);

            byte[] data = Encoding.UTF8.GetBytes(json);
            data = Compressor.Compress(data);
            data = Cryptor.Encrypt(data);

            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            using (FileStream fileStream = File.Create(filePath))
            {
                fileStream.Write(data, 0, data.Length);
            }

            return true;
        }

        /// <summary>
        /// ロード
        /// </summary>
        /// <typeparam name="DataType"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Load<DataType>(string key)
        {
            string filePath = $"{fullPath}/{key}.{extension}";

            if (!File.Exists(filePath))
            {
                return false;
            }

            byte[] data = null;
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                data = new byte[fileStream.Length];
                fileStream.Read(data, 0, data.Length);
            }

            data = Cryptor.Decrypt(data);
            data = Compressor.Decompress(data);

            string json = Encoding.UTF8.GetString(data);

            dataBank[key] = JsonUtility.FromJson<DataType>(json);

            return true;
        }
    }
}