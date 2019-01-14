using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Paway.Helper;
using System.Windows.Forms;
using Paway.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Media;
using log4net;
using System.Reflection;

namespace Paway.Test
{
    public static partial class Method
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Math关于四舍五入
        /// <summary>
        /// 关于货币格式化
        /// </summary>
        public static string Money(double money)
        {
            return string.Format("{0:C2}", money);
        }
        /// <summary>
        /// 关于数字格式化
        /// </summary>
        public static string Round(double value)
        {
            return Round(value, 2);
        }
        /// <summary>
        /// 关于数字格式化
        /// </summary>
        public static string Round(double value, int decimals = 2)
        {
            string length = string.Empty;
            for (int i = 0; i < decimals && i < 2; i++)
                length += "0";
            for (int i = 2; i < decimals; i++)
                length += "#";
            return string.Format("{0:0." + length + "}", value);
        }

        #endregion
    }
}
