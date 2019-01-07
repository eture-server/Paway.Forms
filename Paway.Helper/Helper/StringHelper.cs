using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Paway.Helper
{
    /// <summary>
    ///     对 字符串 的扩展操作
    /// </summary>
    public static class StringHelper
    {
        #region 方法

        /// <summary>
        ///     获取字符串的字符长度
        /// </summary>
        /// <param name="str">需要获取字符长度的字符串</param>
        /// <returns>返回字符串的字符长度</returns>
        public static int GetStrLength(this string str)
        {
            return Encoding.GetEncoding("gb2312").GetBytes(str).Length;
        }

        #region JoinArray 将字符串数组按指定符号连接成字符串

        /// <summary>
        ///     将字符串数组以逗号（，）分隔
        /// </summary>
        /// <param name="array">string[]</param>
        /// <returns>返回分隔后的字符串</returns>
        public static string JoinArray(object[] array)
        {
            return JoinArray(array, ",");
        }

        /// <summary>
        ///     将字符串数据以指定的符号分隔
        /// </summary>
        /// <param name="array">string[]</param>
        /// <param name="split">分隔数组的符号</param>
        /// <returns>返回分隔后的字符串</returns>
        public static string JoinArray(object[] array, string split)
        {
            var sb = new StringBuilder();
            if (array != null && array.Length > 1)
            {
                sb.Append(array[0]);
                for (var i = 1; i < array.Length; i++)
                {
                    sb.Append(split);
                    sb.Append(array[i]);
                }
            }
            return sb.ToString();
        }

        #endregion

        #endregion

        #region 正则检查

        /// <summary>
        ///     使用正则表达式检查字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <returns>返回识别到的字符数</returns>
        public static string RegexChecked(string str, RegexType type)
        {
            string pattern = null;
            switch (type)
            {
                case RegexType.Normal:
                    pattern = @"[\u0022\u0391-\uFFE5\r\n a-zA-Z0-9`=\'\-\\\[\] ;,./~!@#$%^&*()_+|{}:<>?]{0,}";
                    break;
                case RegexType.Password:
                    pattern = @"[\u0022a-zA-Z0-9`=\-\\\[\] ;,./~!@#$%^&*()_+|{}:<>?]{0,}";
                    break;
                case RegexType.Ip:
                    pattern =
                        @"((?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d))))";
                    break;
                case RegexType.PosInt:
                    pattern = @"[0-9]*[1-9][0-9]*";
                    break;
                case RegexType.Number:
                    var sign = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                    pattern = string.Format("[1-9]+[0-9]*([{0}][0-9]+)?|[0-9]([{0}][0-9]+)?", sign);
                    break;
            }
            return RegexChecked(str, pattern);
        }

        /// <summary>
        ///     使用正则表达式检查字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="pattern"></param>
        /// <returns>返回识未别到的字符</returns>
        public static string RegexChecked(string str, string pattern)
        {
            if (pattern == null) return null;
            var regex = new Regex(pattern, RegexOptions.Singleline);
            var match = regex.Match(str);
            if (!match.Success)
            {
                return "检查错误,请重新输入";
            }
            if (match.Groups[0].Value != str)
            {
                if (match.Groups[0].Index != 0)
                {
                    return str.Substring(0, match.Groups[0].Index);
                }
                return "不可以输入字符:" + str.Remove(match.Groups[0].Index, match.Groups[0].Length);
            }
            return null;
        }

        #endregion
    }
}