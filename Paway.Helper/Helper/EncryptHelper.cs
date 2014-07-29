using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace Paway.Helper
{
    /// <summary>
    /// 加密
    /// </summary>
    public abstract class EncryptHelper
    {
        #region 字符加解密

        /// <summary>
        /// Base64(MD5)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncryptMD5(string str)
        {
            string result = null;
            try
            {
                MD5 md = new MD5CryptoServiceProvider();
                byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(str);
                byte[] inArray = md.ComputeHash(bytes);
                //result = System.Convert.ToBase64String(inArray, 0, inArray.Length);

                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < inArray.Length; i++)
                {
                    sBuilder.Append(inArray[i].ToString("x2"));
                }
                result = sBuilder.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Encryption.GetSign(string) :: " + ex.Message);
                throw;
            }
            return result;
        }
        /// <summary>
        /// 取16位MD5码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncryptMD5_16(string str)
        {
            return EncryptMD5(str).Substring(8, 16);
        }
        /// <summary>
        /// 取8位MD5码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncryptMD5_8(string str)
        {
            return EncryptMD5(str).Substring(8, 8);
        }
        /// <summary>
        /// 取4位MD5码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncryptMD5_4(string str)
        {
            return EncryptMD5(str).Substring(8, 4);
        }

        /// <summary>
        /// 加密 Base64(3DES(加密内容))
        /// </summary>
        /// <param name="content"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encrypt3DES(string content, string key)
        {
            string result = string.Empty;
            try
            {
                TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
                byte[] buffer3 = Encoding.GetEncoding("utf-8").GetBytes(content);
                provider.Key = Encoding.GetEncoding("utf-8").GetBytes(key);
                provider.Mode = CipherMode.ECB;
                MemoryStream stream = new MemoryStream();
                ICryptoTransform transform = provider.CreateEncryptor();
                CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
                stream2.Write(buffer3, 0, buffer3.Length);
                stream2.FlushFinalBlock();
                result = System.Convert.ToBase64String(stream.ToArray());
                stream.Flush();
                stream.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Encryption.Encrypt3DES(string, string) :: " + ex.Message);
                throw;
            }

            return result;
        }

        /// <summary>
        /// 解密 Base64(3DES(加密内容))
        /// </summary>
        /// <param name="sourceData"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Decrypt3DES(string sourceData, string key)
        {
            string result = string.Empty;
            try
            {
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
                byte[] content = System.Convert.FromBase64String(sourceData);
                des.Key = Encoding.GetEncoding("utf-8").GetBytes(key);
                des.Mode = CipherMode.ECB;
                MemoryStream ms = new MemoryStream();
                ICryptoTransform transform = des.CreateDecryptor();
                CryptoStream cs = new CryptoStream(ms, transform, CryptoStreamMode.Write);
                cs.Write(content, 0, content.Length);
                cs.FlushFinalBlock();
                byte[] b = ms.ToArray();
                result = Encoding.GetEncoding("utf-8").GetString(b, 0, b.Length);

                ms.Flush();
                ms.Close();

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Encryption.Decrypt3DES(string, string) :: " + ex.Message);
                throw;
            }
            return result;
        }

        /// <summary>
        /// 默认密钥向量 
        /// </summary>
        private static byte[] _key1 = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        /// <summary>
        /// 加密AES算法
        /// </summary>
        /// <param name="text">明文字符串</param>
        /// <param name="key">私钥</param>
        /// <returns>返回加密后的密文字符串</returns>
        public static string EncryptAES(string text, string key)
        {
            byte[] dtat = Encoding.UTF8.GetBytes(text);//得到需要加密的字节数组	
            byte[] bytes = EncryptAES(dtat, key);
            return Convert.ToBase64String(bytes);
        }
        /// <summary>
        /// AES加密算法
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns>返回加密后的密文字节数组</returns>
        public static byte[] EncryptAES(byte[] data, string key)
        {
            //分组加密算法
            SymmetricAlgorithm des = Rijndael.Create();
            //设置密钥及密钥向量
            des.Key = Encoding.UTF8.GetBytes(key);
            des.IV = _key1;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();
            byte[] bytes = ms.ToArray();//得到加密后的字节数组
            cs.Close();
            return bytes;
        }

        /// <summary>
        /// 解密AES
        /// </summary>
        /// <param name="text">密文字节数组</param>
        /// <param name="key">私钥</param>
        /// <returns>返回解密后的字符串</returns>
        public static string DecryptAES(string text, string key)
        {
            byte[] bytes = System.Convert.FromBase64String(text);
            byte[] decryptBytes = DecryptAES(bytes, key);
            return Encoding.UTF8.GetString(decryptBytes);
        }
        /// <summary>
        /// 解密AES
        /// </summary>
        /// <param name="bytes">密文字节数组</param>
        /// <param name="key">私钥</param>
        /// <returns>返回解密后的字符串</returns>
        public static byte[] DecryptAES(byte[] bytes, string key)
        {
            SymmetricAlgorithm des = Rijndael.Create();
            des.Key = Encoding.UTF8.GetBytes(key);
            des.IV = _key1;
            byte[] decryptBytes = new byte[bytes.Length];
            MemoryStream ms = new MemoryStream(bytes);
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read);
            cs.Read(decryptBytes, 0, decryptBytes.Length);
            cs.Close();
            return decryptBytes;
        }

        #endregion

        #region MD5文件与数组加密
        /// <summary>
        /// 实现对一个文件md5的读取，path为文件路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetMD5(string path)
        {
            try
            {
                using (Stream file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return GetMD5(file);
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
        /// <summary>
        /// 获取流的 MD5 值
        /// </summary>
        /// <param name="s">流</param>
        /// <returns>MD5 值</returns>
        public static string GetMD5(Stream s)
        {
            byte[] hash_byte;
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                hash_byte = md5.ComputeHash(s);
            }
            return GetMD5String(hash_byte);
        }
        /// <summary>
        /// 获取数组的 MD5 值
        /// </summary>
        /// <param name="buffer">数组</param>
        /// <returns>MD5 值</returns>
        public static string GetMD5(byte[] buffer)
        {
            byte[] hash_byte;
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                hash_byte = md5.ComputeHash(buffer);
            }
            return GetMD5String(hash_byte);
        }
        /// <summary>
        /// 获取数组的 MD5 值
        /// </summary>
        /// <param name="buffer">数组</param>
        /// <param name="offset">偏移</param>
        /// <param name="count">长度</param>
        /// <returns>MD5 值</returns>
        public static string GetMD5(byte[] buffer, int offset, int count)
        {
            byte[] hash_byte;
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                hash_byte = md5.ComputeHash(buffer, offset, count);
            }
            return GetMD5String(hash_byte);
        }
        /// <summary>
        /// 获取数组的 MD5 值
        /// </summary>
        /// <param name="hash_byte">数组</param>
        /// <returns>MD5 值</returns>
        private static string GetMD5String(byte[] hash_byte)
        {
            string resule = BitConverter.ToString(hash_byte);
            resule = resule.Replace("-", "");
            return resule;
        }

        #endregion

        #region 文件加解密
        private static object fileLock = new object();
        private static string keys = "ningboyichang@#$";//密钥,128位
        private static string MSVersions = "Tinn01";
        /// <summary>
        /// 加密AES文件
        /// </summary>
        public static void EncryptFileAES(string encryptFile, string decryptFile)
        {
            lock (fileLock)
            {
                byte[] data = GetFileData(encryptFile);
                if (data.Length == 0)
                    throw new Exception("需要加密的文件不存在");

                byte[] encryptBytes = EncryptAES(data, keys);
                List<byte> listData = new List<byte>(encryptBytes);
                byte[] versionsData = Encoding.UTF8.GetBytes(MSVersions);
                for (int i = 0; i < versionsData.Length; i++)
                {
                    listData.Add(versionsData[i]);
                }

                FileStream fs1 = null;
                try
                {
                    fs1 = File.Create(decryptFile);
                    fs1.Write(listData.ToArray(), 0, listData.Count);
                }
                finally
                {
                    if (fs1 != null)
                    {
                        fs1.Close();
                        fs1 = null;
                    }
                }
            }
        }
        /// <summary>
        /// 获取文件流
        /// </summary>
        /// <param name="encryptFile"></param>
        /// <returns></returns>
        private static byte[] GetFileData(string encryptFile)
        {
            if (File.Exists(encryptFile))
            {
                FileStream fs = File.OpenRead(encryptFile);
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                if (fs != null)
                {
                    fs.Close();
                    fs = null;
                }
                return data;
            }
            else
            {
                return new byte[0];
            }
        }
        /// <summary>
        /// 解密AES文件
        /// </summary>
        /// <param name="decryptFile">要解密的文件</param>
        /// <param name="encryptFile">解密后存放的路劲</param>
        public static void DecryptFileAES(string decryptFile, string encryptFile)
        {
            lock (fileLock)
            {
                List<byte> listData = new List<byte>(GetFileData(decryptFile));
                int num = Encoding.UTF8.GetBytes(MSVersions).Length;
                for (int i = 0; i < num; i++)
                {
                    int j = listData.Count - 1;
                    listData.RemoveAt(j);
                }
                byte[] data = listData.ToArray();
                if (data.Length == 0)
                    throw new Exception("数据文件不存在");

                byte[] decryptBytes = DecryptAES(data, keys);
                if (!File.Exists(encryptFile))
                    throw new Exception("数据文件不存在");

                FileStream fs1 = null;
                try
                {
                    fs1 = File.OpenWrite(encryptFile);
                    fs1.Write(decryptBytes, 0, decryptBytes.Length);
                }
                finally
                {
                    if (fs1 != null)
                    {
                        fs1.Close();
                        fs1 = null;
                    }
                }
            }
        }
        #endregion
    }
}
