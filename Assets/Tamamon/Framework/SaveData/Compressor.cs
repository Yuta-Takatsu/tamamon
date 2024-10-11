using System.IO;
using System.IO.Compression;

namespace Framework
{
    /// <summary>
    /// �o�C�i���f�[�^��gzip�t�H�[�}�b�g�ň��k�A��
    /// </summary>
    public class Compressor
    {
        /// <summary>
        /// ���k
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] rawData)
        {
            byte[] result = null;

            using (MemoryStream compressedStream = new MemoryStream())
            {
                using (GZipStream gZipStream = new GZipStream(compressedStream, CompressionMode.Compress))
                {
                    gZipStream.Write(rawData, 0, rawData.Length);
                }
                result = compressedStream.ToArray();
            }

            return result;
        }

        /// <summary>
        /// ��
        /// </summary>
        /// <param name="compressedData"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] compressedData)
        {
            byte[] result = null;

            using (MemoryStream compressedStream = new MemoryStream(compressedData))
            {
                using (MemoryStream decompressedStream = new MemoryStream())
                {
                    using (GZipStream gZipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                    {
                        gZipStream.CopyTo(decompressedStream);
                    }
                    result = decompressedStream.ToArray();
                }
            }

            return result;
        }
    }
}