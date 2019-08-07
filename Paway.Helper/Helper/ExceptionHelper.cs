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
    public abstract class ExceptionHelper
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string text;
        private static Form form;

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
        public static void Show(string msg, LeveType type, bool sync = true)
        {
            Show(null, null, msg, type, sync);
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
        public static void Show(Control obj, string msg, LeveType type, bool sync = true)
        {
            Show(obj, null, msg, type, sync);
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
                title = text;
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
                    obj = form;
                    if (obj == null)
                    {
                        IntPtr handle = NativeMethods.GetForegroundWindow();
                        obj = Control.FromChildHandle(handle);
                    }
                    if (obj == null)
                    {
                        for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
                        {
                            var item = Application.OpenForms[i];
                            if (item.GetType().Name != "SkinForm")
                            {
                                obj = item;
                                break;
                            }
                        }
                    }
                }
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