using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;

namespace Framework
{
    /// <summary>
    /// �Z�[�u�f�[�^�Ǘ��N���X
    /// </summary>
    public class DataBank : Singleton<DataBank>
    {
        private static Dictionary<string, object> dataBank = new Dictionary<string, object>();

        private static readonly string path = "SaveData";
        private static readonly string fullPath = $"{Application.persistentDataPath}/{path}";
        private static readonly string extension = "dat";

        /// <summary>
        /// �Z�[�u�f�[�^�ۑ��p�X
        /// </summary>
        public string SavePath
        {
            get
            {
                return fullPath;
            }
        }

        /// <summary>
        /// �Z�[�u�f�[�^�̑��݃`�F�b�N
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return dataBank.Count == 0;
        }

        /// <summary>
        /// �L�[�̑��݃`�F�b�N
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsExistsKey(string key)
        {
            return dataBank.ContainsKey(key);
        }

        /// <summary>
        /// �Z�[�u�f�[�^�̍X�V
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void UpdateData(string key, object obj)
        {
            dataBank[key] = obj;
        }

        /// <summary>
        /// �Z�[�u�f�[�^�̍폜
        /// </summary>
        public void Clear()
        {
            dataBank.Clear();
        }

        /// <summary>
        /// �w��L�[�̃f�[�^�폜
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            dataBank.Remove(key);
        }

        /// <summary>
        /// �w��L�[�̃f�[�^��Ԃ�
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
        /// �Z�[�u
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Save(string key)
        {
            // ���݂��Ȃ��f�[�^
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
        /// ���[�h
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