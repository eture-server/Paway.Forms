using log4net;
using Paway.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;

namespace Paway.Helper
{
    /// <summary>
    /// 一些公共业务方法
    /// </summary>
    public partial class TMethod
    {
        #region EventHelper
        /// <summary>
        /// 判断事件是否已注册
        /// </summary>
        public static bool Exist(Delegate handler, Delegate del)
        {
            if (handler == null) return false;
            foreach (Delegate item in handler.GetInvocationList())
            {
                if (item.Equals(del))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 移除事件所有已注册
        /// </summary>
        public static void Clear<T>(T t, string eventName)
        {
            var invokeList = GetObjectEventList(t, eventName);
            foreach (Delegate del in invokeList)
            {
                typeof(T).GetEvent(eventName).RemoveEventHandler(t, del);
            }
        }
        /// <summary>
        /// 获取对象事件
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="eventName">事件名</param>
        /// <returns></returns>
        private static Delegate[] GetObjectEventList(object obj, string eventName)
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;
            var type = obj.GetType();
            FieldInfo field = type.GetField(eventName, flags);
            while (field == null && type.BaseType != null)
            {
                type = type.BaseType;
                field = type.GetField(eventName, flags);
            }
            if (field != null)
            {
                object fieldValue = field.GetValue(obj);
                if (fieldValue != null && fieldValue is Delegate)
                {
                    return ((Delegate)fieldValue).GetInvocationList();
                }
            }
            return new Delegate[0];
        }

        #endregion

        #region 公共业务相关
        /// <summary>
        /// 自动调整图片显示模式(图像比控件大=Zoom,其它=CenterImage)
        /// </summary>
        public static void AutoSizeMode(PictureBox pic)
        {
            if (pic.Image == null) return;

            if (pic.Image.Width > pic.Width || pic.Image.Height > pic.Height)
                pic.SizeMode = PictureBoxSizeMode.Zoom;
            else
                pic.SizeMode = PictureBoxSizeMode.CenterImage;
        }

        /// <summary>
        /// 根据指定List更新目标List
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="type">操作类型</param>
        /// <param name="tList">要更新的目标List</param>
        /// <param name="fList">更新源List</param>
        public static void Update<T>(OperType type, List<T> tList, List<T> fList) where T : IId
        {
            switch (type)
            {
                case OperType.Insert:
                case OperType.Update:
                    for (int i = 0; i < fList.Count; i++)
                    {
                        var temp = tList.Find(c => c.Id == fList[i].Id);
                        if (temp != null) fList[i].Clone(temp);
                        else tList.Add(fList[i]);
                    }
                    break;
                case OperType.Delete:
                    var cList = fList.ConvertAll(c => c.Id);
                    for (int i = 0; i < cList.Count; i++)
                    {
                        var temp = tList.Find(c => c.Id == cList[i]);
                        if (temp != null) tList.Remove(temp);
                    }
                    break;
                case OperType.Reset:
                    tList.Clear();
                    tList.AddRange(fList);
                    break;
            }
        }

        #endregion

        #region MessageBox.Ask
        /// <summary>
        /// 弹出Warning对话框
        /// </summary>
        public static bool Ask(string format, params object[] args)
        {
            return Ask(null, format, args);
        }
        /// <summary>
        /// 弹出Warning对话框
        /// </summary>
        public static bool Ask(Control obj, string format, params object[] args)
        {
            return Dialog(obj, string.Format(format, args), MessageBoxIcon.Warning) == DialogResult.OK;
        }
        /// <summary>
        /// 弹出对话框
        /// </summary>
        public static DialogResult Dialog(Control obj, string msg, MessageBoxIcon icon)
        {
            obj = TopForm(obj);
            var text = ExceptionHelper.Text;
            if (text.IsNullOrEmpty() && obj != null && !obj.IsDisposed) text = obj.Text;
            return MessageBox.Show(obj, msg, text, MessageBoxButtons.OKCancel, icon);
        }
        /// <summary>
        /// 获取顶层窗体
        /// </summary>
        public static Control TopForm(Control obj = null)
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
                obj = ExceptionHelper.Form;
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
            return obj;
        }

        #endregion
    }
}