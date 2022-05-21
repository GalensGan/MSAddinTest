using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Utils
{
    internal class FileHelper
    {
        /// <summary>
        /// 获取文件的 HASH256 值
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileHash(string filePath)
        {
            if (!File.Exists(filePath)) return "";

            var hash = SHA512.Create();
            var stream = new FileStream(filePath, FileMode.Open);
            byte[] bytes = hash.ComputeHash(stream);
            stream.Close();
            return BitConverter.ToString(bytes);
        }
    }
}
