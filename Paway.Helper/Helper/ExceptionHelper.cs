using log4net;
using Paway.Win32;
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
    /// 异常弹出信息
    /// </summary>
    public static class ExceptionHelper
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// 主窗口
        /// </summary>
        public static Form Form { get; private set; }
        /// <summary>
        /// 主标题
        /// </summary>
        public static string Text { get; private set; }

        /// <summary>
        /// 记录日志
        /// </summary>
        public static void Log(this object ex, string title = null)
        {
            if (ex is Exception exc)
            {
                if (exc is WarningException) log.Warn(exc.Message());
                else if (title != null) log.Error(title, exc);
                else log.Error(exc);
            }
            else log.Debug(ex);
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init(Form form, string text)
        {
            Form = form;
            Text = text;
        }
        /// <summary>
        /// 弹出
        /// </summary>
        public static void Show(this object ex, bool sync = true)
        {
            Show(ex, null, null, LeveType.None, sync);
        }
        /// <summary>
        /// 弹出
        /// </summary>
        public static void Show(this string msg, LeveType type, bool sync = true)
        {
            Show(msg, null, null, type, sync);
        }
        /// <summary>
        /// 弹出
        /// </summary>
        public static void Show(this object ex, string title, bool sync = true)
        {
            Show(ex, null, title, LeveType.None, sync);
        }
        /// <summary>
        /// 弹出
        /// </summary>
        public static void Show(this object ex, string title, LeveType type, bool sync = true)
        {
            Show(ex, null, title, type, sync);
        }
        /// <summary>
        /// 弹出
        /// </summary>
        public static void Show(this object ex, Control obj, bool sync = true)
        {
            Show(ex, obj, null, LeveType.None, sync);
        }
        /// <summary>
        /// 弹出
        /// </summary>
        public static void Show(this string msg, Control obj, LeveType type, bool sync = true)
        {
            Show(msg, obj, null, type, sync);
        }
        /// <summary>
        /// 弹出
        /// </summary>
        public static void Show(this object ex, Control obj, string title, bool sync = true)
        {
            Show(ex, obj, title, LeveType.None, sync);
        }
        /// <summary>
        /// 弹出
        /// </summary>
        public static void Show(this object ex, Control obj, string title, LeveType type, bool sync = true)
        {
            if (ex == null) return;
            string msg = ex.ToString();
            if (ex is Exception exc)
            {
                Log(exc);
                msg = exc.Message();
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
                obj = TMethod.TopForm(obj);
                title = Text;
                if (title.IsNullOrEmpty() && obj != null && !obj.IsDisposed) title = obj.Text;
                if (sync)
                    new Action<Control, string, string, LeveType>(Show).BeginInvoke(obj, title, msg, type, null, null);
                else
                    Show(obj, title, msg, type);
            }
            finally
            {
                Application.DoEvents();
            }
        }
        private static void Show(Control obj, string title, string msg, LeveType type)
        {
            MessageBoxIcon icon = MessageBoxIcon.Information;
            switch (type)
            {
                case LeveType.Warn: icon = MessageBoxIcon.Warning; break;
                case LeveType.Error: icon = MessageBoxIcon.Error; break;
            }
            if (obj == null || !obj.Visible || obj.IsDisposed)
            {
                Show(null, title, msg, icon);
            }
            else
            {
                obj.Invoke(new Action<Control, string, string, MessageBoxIcon>(Show), new object[] { obj, title, msg, icon });
            }
        }
        private static void Show(Control obj, string title, string msg, MessageBoxIcon icon)
        {
            if (obj == null || !obj.Visible || obj.IsDisposed)
                MessageBox.Show(msg, title, MessageBoxButtons.OK, icon);
            else
            {
                Win32Helper.ActiveForm(obj.Handle);
                MessageBox.Show(obj, msg, title, MessageBoxButtons.OK, icon);
            }
        }
    }
}