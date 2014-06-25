using Paway.Helper;
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
        #region 属性
        /// <summary>
        /// 加载标记
        /// </summary>
        private bool iLoad = false;

        #endregion

        #region 事件
        /// <summary>
        /// 当控件数据更新时发生
        /// </summary>
        public event EventHandler ChangeEvent;

        #endregion

        #region virtual Method
        /// <summary>
        /// 控件数据
        /// </summary>
        [Description("控件数据"), DefaultValue(null)]
        public new virtual Object Tag { get; set; }

        /// <summary>
        /// 从其它控件切换过来时重新激活
        /// </summary>
        public virtual void ReLoad() { }

        /// <summary>
        /// 刷新数据
        /// </summary>
        public virtual void Refresh(object sender, EventArgs e) { }

        /// <summary>
        /// 引发ChangeEvent事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnChanged(object sender, EventArgs e)
        {
            if (ChangeEvent != null)
            {
                ChangeEvent(sender, e);
            }
        }

        #endregion

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
        /// 切换界面控件
        /// 如已加载，则调用ReLoad()
        /// </summary>
        public static MControl ReLoad(Control parent, Type type)
        {
            if (!Licence.Checking()) return null;

            MControl control = null;
            try
            {
                //不重复加载
                if (Current != null)
                {
                    if (Current.GetType() == type) return null;
                }

                //加载控件
                if (_iList.ContainsKey(type.Name))
                {
                    control = _iList[type.Name] as MControl;
                    if (control.iLoad) return control;
                }

                parent.SuspendLayout();
                if (parent.Controls.Count > 0)
                {
                    if (parent.Controls[0] is MControl)
                    {
                        (parent.Controls[0] as MControl).iLoad = false;
                    }
                    parent.Controls.Clear();
                }
                //加载控件
                if (control == null)
                {
                    Assembly asmb = Assembly.GetAssembly(type);
                    control = asmb.CreateInstance(type.FullName) as MControl;
                }
                if (control == null)
                {
                    throw new ArgumentException(string.Format("{0} 不是有效的MControl", type.GetType().FullName));
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
                return control;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (control != null)
                {
                    Current = control;
                }
                Current.iLoad = true;
                Current.ReLoad();
            }
        }
        /// <summary>
        /// 将已定义控件加入列表
        /// </summary>
        /// <param name="control"></param>
        public static void Add(MControl control)
        {
            if (!_iList.ContainsKey(control.Name))
            {
                _iList.Add(control.Name, control);
            }
        }
        /// <summary>
        /// 重置子控件
        /// </summary>
        public static void ReSet()
        {
            for (int i = _iList.Count; i >= 0; i--)
            {
                string item = _iList.Keys.ElementAt<string>(i);
                if (_iList[item] == Current)
                {
                    if (!_iList[item].IsDisposed)
                    {
                        _iList[item].Dispose();
                    }
                    _iList[item] = null;
                    _iList.Remove(item);
                    break;
                }
            }
        }
        /// <summary>
        /// 重置控件上所有子控件
        /// </summary>
        public static void ReSet(Control parent)
        {
            for (int i = _iList.Count; i >= 0; i--)
            {
                string item = _iList.Keys.ElementAt<string>(i);
                if (_iList[item].Parent == parent)
                {
                    if (!_iList[item].IsDisposed)
                    {
                        _iList[item].Dispose();
                    }
                    _iList[item] = null;
                    _iList.Remove(item);
                }
            }
        }
        /// <summary>
        /// 重置所有子控件
        /// </summary>
        public static void ReSetAll()
        {
            for (int i = _iList.Count; i >= 0; i--)
            {
                string item = _iList.Keys.ElementAt<string>(i);
                if (!_iList[item].IsDisposed)
                {
                    _iList[item].Dispose();
                }
                _iList[item] = null;
            }
            _iList.Clear();
        }
        /// <summary>
        /// 刷新所有控件数据
        /// </summary>
        public static void RefreshAll(object sender, EventArgs e)
        {
            for (int i = 0; i < _iList.Count; i++)
            {
                string item = _iList.Keys.ElementAt<string>(i);
                _iList[item].Refresh(sender, e);
            }
        }

        #endregion
    }
}
