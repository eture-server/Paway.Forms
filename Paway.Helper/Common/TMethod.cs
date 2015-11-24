using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Paway.Helper
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class TMethod
    {
        /// <summary>
        /// 获取内部异常
        /// </summary>
        public static string InnerMessage(ref Exception ex)
        {
            string msg = ex.Message;
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                if (!string.IsNullOrEmpty(ex.Message))
                    msg = string.Format("{0}\r\n{1}", msg, ex.Message);
            }
            return msg;
        }
    }
}
