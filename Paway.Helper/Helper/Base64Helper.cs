using System;

namespace Paway.Helper
{
    /// <summary>
    ///     BASE64 - 0x3F - 111111
    ///     Base32 - 0x1F - 11111
    /// </summary>
    public abstract class Base64Helper
    {
        private const string Base64 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz+/";

        /// <summary>
        ///     生成两位的BASE64字符串。
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string NumToBase64ByTwo(int number)
        {
            if (number > Base64.Length * Base64.Length)
                throw new ArgumentException("超出了两位BASE64表示范围。");
            return string.Concat(Base64[(number >> 6) & 0x3F], Base64[number & 0x3F]);
        }

        /// <summary>
        ///     从两位BASE64字符转换为数值。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int NumFromBase64ByTwo(string str)
        {
            if (str == null || str.Length != 2)
                throw new ArgumentException("只接受两位BASE64字符。");
            return (Base64.IndexOf(str[0]) << 6) + Base64.IndexOf(str[1]);
        }

        /// <summary>
        ///     生成一位的BASE64字符串。
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string NumToBase64(int number)
        {
            if (number > Base64.Length)
                throw new ArgumentException("超出了两位BASE64表示范围。");
            return Base64[number & 0x3F].ToString();
        }

        /// <summary>
        ///     从一位BASE64字符转换为数值。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int NumFromBase64(string str)
        {
            if (str == null || str.Length != 1)
                throw new ArgumentException("只接受一位BASE64字符。");
            return Base64.IndexOf(str[0]);
        }

        /// <summary>
        ///     生成n位的BASE64字符串。
        /// </summary>
        public static string NumToBase64(int number, int n)
        {
            if (n == 0) return null;
            var length = Base64.Length;
            for (var i = 1; i < n; i++)
            {
                length *= Base64.Length;
            }
            if (number > length)
                throw new ArgumentException(string.Format("超出了{0}位BASE64表示范围。", n));
            string result = null;
            for (var i = 0; i < n; i++)
            {
                result = string.Format("{0}{1}", result, Base64[(number >> ((n - i - 1) * 6)) & 0x3F]);
            }
            return result;
        }

        /// <summary>
        ///     从n位BASE64字符转换为数值。
        /// </summary>
        public static int NumFromBase64(string str, int n)
        {
            if (str == null) return 0;
            var result = 0;
            for (var i = 0; i < str.Length; i++)
            {
                result += Base64.IndexOf(str[i]) << ((n - i - 1) * 6);
            }
            return result;
        }
    }
}