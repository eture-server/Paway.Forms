using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Paway.Helper
{
    /// <summary>
    /// 加解密
    /// </summary>
    public abstract class EncryptHelper
    {
        #region 密钥
        /// <summary>
        /// 默认密钥向量(AES)
        /// </summary>
        private static readonly byte[] _key1 =
        {
            0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF
        };
        private static readonly byte[] _key2 =
        {
            0x37, 0x67, 0xF6, 0x4F, 0x24, 0x63, 0xA7, 0x1E
        };
        private static readonly string _key3 = "ningboyichang@#$"; //密钥,128位

        #endregion

        #region SHA
        /// <summary>
        /// sha256加密
        /// </summary>
        /// <returns>返回字符串长度64(以十六进制字符串表示形式)</returns>
        public static string EncryptSHA256(string content)
        {
            byte[] data = Encoding.UTF8.GetBytes(content);
            using (var Sha256 = new SHA256Managed())
            {
                byte[] buffer = Sha256.ComputeHash(data);
                return Encrypt(buffer);
            }
        }
        /// <summary>
        /// sha1加密，返回字节组
        /// </summary>
        /// <returns>返回字符串长度40(以十六进制字符串表示形式)</returns>
        public static string EncryptSHA1(string content)
        {
            var data = Encoding.UTF8.GetBytes(content);
            using (var sha1 = SHA1.Create())
            {
                var buffer = sha1.ComputeHash(data);
                return Encrypt(buffer);
            }
        }
        /// <summary>
        /// 加密后生成字符串
        /// </summary>
        private static string Encrypt(byte[] buffer)
        {
            return BitConverter.ToString(buffer).Replace("-", string.Empty);
        }
        /// <summary>
        /// 解析加密字符串
        /// </summary>
        private static byte[] Decrypt(string content)
        {
            byte[] buffer = new byte[content.Length / 2];
            for (int x = 0; x < buffer.Length; x++)
            {
                int i = Convert.ToInt32(content.Substring(x * 2, 2), 16);
                buffer[x] = (byte)i;
            }
            return buffer;
        }

        #endregion

        #region DES
        /// <summary>
        /// DES加密
        /// <para>密钥：8位</para>
        /// </summary>
        /// <returns>返回字符串(以十六进制字符串表示形式)</returns>
        public static string EncryptDES(string content, string key)
        {
            if (key == null || key.Length < 8) throw new ArgumentException("key长度错误");
            var bKey = new byte[8];
            Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);
            using (var des = new DESCryptoServiceProvider())
            {
                des.Key = bKey;
                des.IV = _key2;
                byte[] buffer = Encoding.Default.GetBytes(content);
                var ms = new MemoryStream();
                using (var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(buffer, 0, buffer.Length);
                    cs.FlushFinalBlock();
                }
                return Encrypt(ms.ToArray());
            }
        }
        /// <summary>
        /// DES解密
        /// <para>密钥：8位</para>
        /// </summary>
        public static string DecryptDES(string content, string key)
        {
            if (key == null || key.Length < 8) throw new ArgumentException("key长度错误");
            var bKey = new byte[8];
            Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);
            using (var des = new DESCryptoServiceProvider())
            {
                byte[] buffer = Decrypt(content);
                des.Key = bKey;
                des.IV = _key2;
                var ms = new MemoryStream();
                using (var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(buffer, 0, buffer.Length);
                    cs.FlushFinalBlock();
                }
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        #endregion

        #region 3DES
        /// <summary>
        /// 3DES加密
        /// <para>密钥：16/24位</para>
        /// </summary>
        /// <returns>返回字符串(以十六进制字符串表示形式)</returns>
        public static string Encrypt3DES(string content, string key)
        {
            if (key == null || key.Length < 8) throw new ArgumentException("key长度错误");
            var bKey = new byte[key.Length <= 16 ? 16 : 24];
            Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);
            using (var provider = new TripleDESCryptoServiceProvider())
            {
                provider.Key = bKey;
                provider.Mode = CipherMode.ECB;
                var buffer = Encoding.UTF8.GetBytes(content);
                var ms = new MemoryStream();
                using (var cs = new CryptoStream(ms, provider.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(buffer, 0, buffer.Length);
                    cs.FlushFinalBlock();
                }
                return Encrypt(ms.ToArray());
            }
        }
        /// <summary>
        /// 3DES解密
        /// <para>密钥：16/24位</para>
        /// </summary>
        public static string Decrypt3DES(string content, string key)
        {
            if (key == null || key.Length < 8) throw new ArgumentException("key长度错误");
            var bKey = new byte[key.Length <= 16 ? 16 : 24];
            Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);
            using (var des = new TripleDESCryptoServiceProvider())
            {
                byte[] buffer = Decrypt(content);
                des.Key = bKey;
                des.Mode = CipherMode.ECB;
                var ms = new MemoryStream();
                using (var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(buffer, 0, buffer.Length);
                    cs.FlushFinalBlock();
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }

        #endregion

        #region AES
        /// <summary>
        /// 加密AES算法
        /// <para>密钥：16/24/32位</para>
        /// </summary>
        /// <returns>返回字符串(以十六进制字符串表示形式)</returns>
        public static string EncryptAES(string content, string key)
        {
            var data = Encoding.UTF8.GetBytes(content); //得到需要加密的字节数组	
            var buffer = EncryptAES(data, key);
            return Encrypt(buffer);
        }
        /// <summary>
        /// AES加密算法
        /// <para>密钥：16/24/32位</para>
        /// </summary>
        public static byte[] EncryptAES(byte[] data, string key)
        {
            if (key == null || key.Length < 8) throw new ArgumentException("key长度错误");
            var bKey = new byte[key.Length <= 16 ? 16 : (key.Length <= 24 ? 24 : 32)];
            Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);
            //分组加密算法
            using (var des = Rijndael.Create())
            {
                //设置密钥及密钥向量
                des.Key = bKey;
                des.IV = _key1;
                var ms = new MemoryStream();
                using (var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray(); //得到加密后的字节数组
                }
            }
        }

        /// <summary>
        /// 解密AES
        /// </summary>
        /// <param name="content">密文字节数组</param>
        /// <param name="key">私钥</param>
        /// <returns>返回解密后的字符串</returns>
        public static string DecryptAES(string content, string key)
        {
            byte[] buffer = Decrypt(content);
            var decryptBytes = DecryptAES(buffer, key);
            return Encoding.UTF8.GetString(decryptBytes);
        }
        /// <summary>
        /// 解密AES
        /// </summary>
        public static byte[] DecryptAES(byte[] data, string key)
        {
            if (key == null || key.Length < 8) throw new ArgumentException("key长度错误");
            var bKey = new byte[key.Length <= 16 ? 16 : (key.Length <= 24 ? 24 : 32)];
            Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);
            using (var des = Rijndael.Create())
            {
                des.Key = bKey;
                des.IV = _key1;
                var buffer = new byte[data.Length];
                var ms = new MemoryStream(data);
                using (var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    int length = cs.Read(buffer, 0, buffer.Length);
                    byte[] result = new byte[length];
                    Array.Copy(buffer, 0, result, 0, length);
                    return result;
                }
            }
        }

        #endregion

        #region MD5
        /// <summary>
        /// 获取字符串MD5检验码
        /// </summary>
        /// <returns>返回字符串(以十六进制字符串表示形式)</returns>
        public static string MD5(string content)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var data = Encoding.UTF8.GetBytes(content);
                var buffer = md5.ComputeHash(data);
                return Encrypt(buffer);
            }
        }
        /// <summary>
        /// 取16位MD5码(8,16)
        /// </summary>
        public static string MD5_16(string str)
        {
            return MD5(str).Substring(8, 16);
        }
        /// <summary>
        /// 取8位MD5码(8,8)
        /// </summary>
        public static string MD5_8(string str)
        {
            return MD5(str).Substring(8, 8);
        }
        /// <summary>
        /// byte数组MD5值
        /// </summary>
        public static string MD5(byte[] data, int offset = 0, int count = 0)
        {
            if (count == 0) count = data.Length;
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var buffer = md5.ComputeHash(data, offset, count);
                return Encrypt(buffer);
            }
        }
        /// <summary>
        /// 文件MD5校验码
        /// </summary>
        public static string FileMD5(string file)
        {
            using (var fs = new FileStream(file, FileMode.Open))
            {
                using (var md5 = new MD5CryptoServiceProvider())
                {
                    byte[] buffer = md5.ComputeHash(fs);
                    return Encrypt(buffer);
                }
            }
        }

        #endregion

        #region 文件加解密(AES)
        /// <summary>
        /// 加密AES文件
        /// </summary>
        public static void FileEncryptAES(string encryptFile, string decryptFile)
        {
            var data = GetFileData(encryptFile);
            if (data.Length == 0) throw new ArgumentException("数据文件不存在");
            var buffer = EncryptAES(data, _key3);
            using (var fs = File.Create(decryptFile))
            {
                fs.Write(buffer, 0, buffer.Length);
                fs.Flush();
            }
        }
        /// <summary>
        /// 获取文件流
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static byte[] GetFileData(string file)
        {
            if (File.Exists(file))
            {
                using (var fs = File.OpenRead(file))
                {
                    var data = new byte[fs.Length];
                    fs.Read(data, 0, data.Length);
                    return data;
                }
            }
            return new byte[0];
        }
        /// <summary>
        /// 解密AES文件
        /// </summary>
        /// <param name="decryptFile">要解密的文件</param>
        /// <param name="encryptFile">解密后存放的路劲</param>
        public static void FileDecryptAES(string decryptFile, string encryptFile)
        {
            var data = GetFileData(decryptFile);
            if (data.Length == 0) throw new ArgumentException("数据文件不存在");
            var buffer = DecryptAES(data, _key3);
            using (var fs = File.Create(encryptFile))
            {
                fs.Write(buffer, 0, buffer.Length);
                fs.Flush();
            }
        }

        #endregion
    }
}