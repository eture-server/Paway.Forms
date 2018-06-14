using log4net;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Paway.Helper
{
    /// <summary>
    ///     异常弹出信息
    /// </summary>
    public abstract class ExceptionHelper
    {
        /// <summary>
        /// </summary>
        protected static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string text;
        private static Form form;
        /// <summary>
        /// 获取一个前台窗口的句柄
        /// </summary>
        [DllImport("user32.dll")]
        internal static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init(Form form, string text)
        {
            ExceptionHelper.form = form;
            ExceptionHelper.text = text;
        }
        /// <summary>
        /// 弹出
        /// </summary>
        public static void Show(object ex, bool sync = true)
        {
            Show(null, null, ex, LeveType.None, sync);
        }
        /// <summary>
        /// 弹出
        /// </summary>
        public static void Show(object ex, LeveType type, bool sync = true)
        {
            Show(null, null, ex, type, sync);
        }
        /// <summary>
        /// 弹出
        /// </summary>
        public static void Show(string title, object ex, bool sync = true)
        {
            Show(null, title, ex, LeveType.None, sync);
        }
        /// <summary>
        /// 弹出
        /// </summary>
        public static void Show(string title, object ex, LeveType type, bool sync = true)
        {
            Show(null, title, ex, type, sync);
        }
        /// <summary>
        /// 弹出
        /// </summary>
        public static void Show(Control obj, object ex, bool sync = true)
        {
            Show(obj, null, ex, LeveType.None, sync);
        }
        /// <summary>
        /// 弹出
        /// </summary>
        public static void Show(Control obj, object ex, LeveType type, bool sync = true)
        {
            Show(obj, null, ex, type, sync);
        }
        /// <summary>
        /// 弹出
        /// </summary>
        public static void Show(Control obj, string title, object ex, bool sync = true)
        {
            Show(obj, title, ex, LeveType.None, sync);
        }
        /// <summary>
        /// 弹出
        /// </summary>
        public static void Show(Control obj, string title, object ex, LeveType type, bool sync = true)
        {
            if (ex == null) return;
            string msg = ex.ToString();
            if (ex is Exception)
            {
                if (!(ex is WarningException))
                    log.Error(ex);
                msg = (ex as Exception).InnerMessage();
                if (!title.IsNullOrEmpty())
                    msg = string.Format("{0}\r\n{1}", title, msg);
                if (type == LeveType.None)
                    type = ex is WarningException ? LeveType.Warn : LeveType.Error;
            }
            else if (type == LeveType.None)
            {
                type = LeveType.Warn;
            }
            try
            {
                while (obj is Control && !(obj is Form))
                {
                    if (obj.Parent == null) break;
                    obj = obj.Parent;
                }
                if (obj == null || !obj.Visible || obj.IsDisposed || !(obj is Form))
                {
                    obj = LoadHelper.Form;
                }
                if (obj == null || !obj.Visible || obj.IsDisposed || !(obj is Form))
                {
                    if (form == null)
                    {
                        IntPtr handle = GetForegroundWindow();
                        form = Control.FromChildHandle(handle) as Form;
                        if (form != null) text = form.Text;
                    }
                    obj = form;
                }
                if (sync)
                    new Action<Control, string, LeveType>(Show).BeginInvoke(obj, msg, type, null, null);
                else
                    Show(obj, msg, type);
            }
            finally
            {
                Application.DoEvents();
            }
        }
        private static void Show(Control obj, string msg, LeveType type)
        {
            MessageBoxIcon icon = MessageBoxIcon.Information;
            switch (type)
            {
                case LeveType.Warn: icon = MessageBoxIcon.Warning; break;
                case LeveType.Error: icon = MessageBoxIcon.Error; break;
            }
            if (obj == null || !obj.Visible || obj.IsDisposed)
            {
                Show(null, msg, icon);
            }
            else
            {
                obj.Invoke(new Action<Control, string, MessageBoxIcon>(Show), new object[] { obj, msg, icon });
            }
        }
        private static void Show(Control obj, string msg, MessageBoxIcon icon)
        {
            if (obj == null || !obj.Visible || obj.IsDisposed)
                MessageBox.Show(msg, text, MessageBoxButtons.OK, icon);
            else
                MessageBox.Show(obj, msg, text, MessageBoxButtons.OK, icon);
        }
    }
}