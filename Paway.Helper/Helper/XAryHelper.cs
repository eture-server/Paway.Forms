using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Paway.Helper
{
    /// <summary>
    /// X进制处理
    /// 本文档为34进制(0-9，A-Z，不含字母IO)
    /// </summary>
    public abstract class XAryHelper
    {
        //34进制(不含字母IO)
        private const string digits = "0123456789ABCDEFGHJKLMNPQRSTUVWXYZ";
        /// <summary>
        /// 将指定基数的数字的 System.String 表示形式转换为等效的 32 位有符号整数。
        /// </summary>
        /// <param name="value">包含数字的 System.String。</param>
        public static int Value(string value)
        {
            if (value.IsNullOrEmpty()) return 0;
            int result = 0;
            for (int i = 0; i < value.Length; i++)
            {
                if (!digits.Contains(value[i]))
                {
                    throw new ArgumentException(string.Format("The argument \"{0}\" is not in system.", value[i]));
                }
                try
                {
                    result += (int)Math.Pow(digits.Length, i) * digits.IndexOf(value[value.Length - i - 1]);
                }
                catch
                {
                    throw new OverflowException("Arithmetic overflow");
                }
            }
            return result;
        }

        /// <summary>
        /// int转化为X进制(默认取5位)
        /// </summary>
        public static string Value(int value, int length = 5)
        {
            value = Math.Abs(value);
            char[] temp = new char[length];
            for (int i = 0; i < temp.Length; i++) temp[i] = '0';
            for (int index = 0; index < length; index++)
            {
                if (value == 0) break;

                temp[temp.Length - index - 1] = digits[value % digits.Length];
                value /= digits.Length;
            }
            return new string(temp);
        }
    }
}