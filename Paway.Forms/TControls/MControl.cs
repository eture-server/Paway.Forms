﻿using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    /// 多控件切换方法
    /// </summary>
    public class MControl : TControl
    {
        /// <summary>
        /// 从其它控件切换过来时重新激活
        /// </summary>
        public virtual void ReLoad() { }

        /// <summary>
        /// 控件数据
        /// </summary>
        [Description("控件数据"), DefaultValue(null)]
        public new virtual Object Tag { get; set; }

        /// <summary>
        /// 刷新数据
        /// </summary>
        public virtual void Refresh(object sender, EventArgs e) { }

        /// <summary>
        /// 控件传出事件
        /// </summary>
        public event EventHandler ChangeEvent;
        /// <summary>
        /// 引发传出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnEfferent(object sender, EventArgs e)
        {
            if (ChangeEvent != null)
            {
                ChangeEvent(sender, e);
            }
        }

        #region 界面切换
        /// <summary>
        /// 基类控件列表
        /// </summary>
        private static Dictionary<string, MControl> _iList = new Dictionary<string, MControl>();
        /// <summary>
        /// 当前控件
        /// </summary>
        public static MControl Current { get; private set; }
        /// <summary>
        /// 切换主界面控件
        /// </summary>
        public static MControl ReLoad(Control parent, Type type)
        {
            Licence.Checking();
            //不重复加载
            if (Current != null)
            {
                if (Current.GetType() == type)
                {
                    Current.ReLoad();
                    return null;
                }
            }

            parent.SuspendLayout();
            parent.Controls.Clear();
            //加载控件
            MControl control = null;
            if (_iList.ContainsKey(type.Name))
            {
                control = _iList[type.Name] as MControl;
            }
            else
            {
                Assembly asmb = Assembly.GetAssembly(type);
                control = asmb.CreateInstance(type.FullName) as MControl;
            }
            if (control == null)
            {
                throw new ArgumentException(string.Format("{0} 不是有效的IControl", type.GetType().FullName));
            }
            else
            {
                parent.Controls.Add(control);
                control.Dock = System.Windows.Forms.DockStyle.Fill;
                if (!_iList.ContainsKey(control.Name))
                {
                    _iList.Add(control.Name, control);
                }
            }
            parent.ResumeLayout();
            Current = control;

            return control;
        }
        /// <summary>
        /// 重置
        /// </summary>
        public static void ReSet()
        {
            if (Current != null && !Current.IsDisposed)
            {
                Current.Dispose();
            }
            Current = null;
            _iList.Clear();
        }
        /// <summary>
        /// 刷新所有控件数据
        /// </summary>
        public static void RefreshAll(object sender, EventArgs e)
        {
            for (int i = 0; i < _iList.Count; i++)
            {
                var item = _iList.ElementAt(i);
                item.Value.Refresh(sender, e);
            }
        }

        #endregion
    }
}