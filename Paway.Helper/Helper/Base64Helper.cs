using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Helper
{
    /// <summary>
    /// BASE64
    /// </summary>
    public abstract class Base64Helper
    {
        private static string BASE64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        /// <summary>
        /// 生成两位的BASE64字符串。
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string NumberToBase64(int number)
        {
            if (number > 4095)
                throw new ArgumentException("超出了两位BASE64表示范围。", "number");
            return string.Concat(BASE64[(number >> 6) & 0x3F], BASE64[number & 0x3F]);
        }
        /// <summary>
        /// 从两位BASE64字符转换为数值。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int NumberFromBase64(string str)
        {
            if (str == null || str.Length != 2)
                throw new ArgumentException("只接受两位BASE64字符。", "str");
            return (BASE64.IndexOf(str[0]) << 6) + BASE64.IndexOf(str[1]);
        }
    }
}
