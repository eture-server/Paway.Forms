using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Paway.Helper;

namespace Paway.Forms
{
    /// <summary>
    ///     多控件切换方法
    /// </summary>
    public class MControl : TControl
    {
        #region 事件

        /// <summary>
        ///     当控件数据更新时发生
        /// </summary>
        public event EventHandler ChangeEvent;

        #endregion

        #region virtual Method
        private Delegate method;

        /// <summary>
        ///     控件数据
        /// </summary>
        [Description("控件数据")]
        [DefaultValue(null)]
        public EventArgs Args { get; set; }

        /// <summary>
        ///     从其它控件切换过来时重新激活
        /// </summary>
        public virtual void ReLoad()
        {
            ILoad = true;
        }

        /// <summary>
        ///     移除当前界面时，是否允许移除
        /// </summary>
        public virtual bool UnLoad()
        {
            for (var i = 0; i < Controls.Count; i++)
            {
                if (Controls[i] is Panel || Controls[i] is TControl)
                {
                    var panel = Controls[i];
                    for (var j = 0; j < panel.Controls.Count; j++)
                    {
                        if (panel.Controls[j] is TControl)
                        {
                            (panel.Controls[j] as TControl).MStop();
                        }
                    }
                }
            }
            MStop();
            ILoad = false;
            return true;
        }

        /// <summary>
        ///     刷新数据
        /// </summary>
        public virtual void Refresh(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     调用委托
        /// </summary>
        /// <param name="method"></param>
        internal void InitDelegate(Delegate method)
        {
            this.method = method;
        }

        /// <summary>
        ///     引发ChangeEvent事件
        ///     引发委托事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnChanged(object sender, EventArgs e)
        {
            if (method != null)
            {
                Invoke(method, sender, e);
            }
            ChangeEvent?.Invoke(sender, e);
        }

        #endregion

        #region 界面切换控制

        /// <summary>
        ///     基类控件列表
        /// </summary>
        private static readonly Dictionary<string, MControl> _list = new Dictionary<string, MControl>();

        /// <summary>
        ///     控件列表
        /// </summary>
        public static Dictionary<string, MControl> List
        {
            get { return _list; }
        }

        /// <summary>
        ///     当前控件
        /// </summary>
        public static MControl Current { get; private set; }

        /// <summary>
        ///     切换界面控件
        /// </summary>
        public static MControl ReLoad(Control parent, Type type)
        {
            return ReLoad(parent, type, EventArgs.Empty, TMDirection.None);
        }

        /// <summary>
        ///     切换界面控件
        /// </summary>
        public static MControl ReLoad(Control parent, Type type, EventArgs e)
        {
            return ReLoad(parent, type, e, TMDirection.None);
        }

        /// <summary>
        ///     切换界面控件
        /// </summary>
        public static MControl ReLoad(Control parent, Type type, TMDirection direction)
        {
            return ReLoad(parent, type, EventArgs.Empty, direction);
        }

        /// <summary>
        ///     切换界面控件
        /// </summary>
        public static MControl ReLoad(Control parent, Type type, Delegate method)
        {
            return ReLoad(parent, type, EventArgs.Empty, TMDirection.None, method);
        }

        /// <summary>
        ///     切换界面控件
        ///     如已加载，则调用ReLoad()
        ///     如调用委托，要求参数：object sender ,EventArgs e
        /// </summary>
        public static MControl ReLoad(Control parent, Type type, EventArgs e, TMDirection direction, Delegate method = null, int intervel = -1)
        {
            MControl control = null;
            try
            {
                //不重复加载
                if (Current != null)
                {
                    if (Current.GetType() == type && Current.Parent == parent) return null;
                }
                //加载控件
                if (_list.ContainsKey(type.FullName))
                {
                    if (_list[type.FullName].ILoad) return control = _list[type.FullName];
                }
                //移除旧控件
                var temp = parent;
                if (parent.Controls.Count == 1)
                {
                    if (parent.Controls[0] is MControl)
                    {
                        temp = parent.Controls[0];
                        //拒绝移除
                        if (!(temp as MControl).UnLoad())
                        {
                            return null;
                        }
                    }
                }

                //加载控件
                if (_list.ContainsKey(type.FullName))
                {
                    control = _list[type.FullName];
                }
                parent.SuspendLayout();
                //加载控件
                if (control == null)
                {
                    control = Assembly.GetAssembly(type).CreateInstance(type.FullName) as MControl;
                }
                if (control == null)
                {
                    throw new ArgumentException(string.Format("{0} 不是有效的MControl", type.FullName));
                }
                if (e != null)
                {
                    control.Args = e;
                }
                if (method != null)
                {
                    control.InitDelegate(method);
                }
                if (direction == TMDirection.None)
                {
                    direction = control.MDirection;
                }

                //特效显示
                switch (direction)
                {
                    case TMDirection.Transparent:
                    case TMDirection.T3DLeft:
                    case TMDirection.T3DLeftToRight:
                    case TMDirection.T3DRight:
                    case TMDirection.T3DRightToLeft:
                    case TMDirection.T3DUp:
                    case TMDirection.T3DUpToDown:
                    case TMDirection.T3DDown:
                    case TMDirection.T3DDownToUp:
                        if (temp.Width > 0 && temp.Height > 0)
                        {
                            var bitmap = new Bitmap(temp.Width, temp.Height);
                            temp.DrawToBitmap(bitmap, new Rectangle(0, 0, temp.Width, temp.Height));
                            control.TranImage = bitmap;
                            parent.BackgroundImageLayout = ImageLayout.Stretch;
                            parent.BackgroundImage = control.TranImage;
                        }
                        break;
                }
                parent.Controls.Clear();

                //加载新控件属性
                control.MDirection = TMDirection.None;
                if (intervel != -1)
                {
                    control.MInterval = intervel;
                }
                control.Dock = DockStyle.Fill;
                parent.Controls.Add(control);
                control.MDirection = direction;
                control.MChild();
                if (!_list.ContainsKey(type.FullName))
                {
                    _list.Add(type.FullName, control);
                }
                parent.BackgroundImage = null;
                parent.ResumeLayout();

                return control;
            }
            finally
            {
                if (control != null)
                {
                    Current = control;
                }
                Current.Focus();
                Current.ReLoad();
            }
        }

        /// <summary>
        ///     将已定义控件加入列表
        /// </summary>
        /// <param name="control"></param>
        /// <returns>成功返回列表中的已加入的MControl控件</returns>
        public static MControl Add(MControl control)
        {
            var type = control.GetType();
            if (!_list.ContainsKey(type.FullName))
            {
                _list.Add(type.FullName, control);
                return control;
            }
            return _list[type.FullName];
        }

        /// <summary>
        ///     将指定类型控件加入列表
        /// </summary>
        /// <returns>成功返回列表中的已加入的MControl控件</returns>
        public static MControl Add<T>() where T : MControl
        {
            Type type = typeof(T);
            if (!MControl.List.ContainsKey(type.FullName))
            {
                var asmb = Assembly.GetAssembly(type);
                MControl control = asmb.CreateInstance(type.FullName) as MControl;
                MControl.List.Add(type.FullName, control);
                return control;
            }
            return MControl.List[type.FullName];
        }

        /// <summary>
        ///     返回控件上的当前子控件
        /// </summary>
        public static MControl Get(Control parent)
        {
            for (var i = 0; i < _list.Count; i++)
            {
                var item = _list.Keys.ElementAt(i);
                if (_list[item].Parent == parent)
                {
                    return _list[item];
                }
            }
            return null;
        }

        /// <summary>
        ///     返回指定类型控件
        /// </summary>
        public static MControl Get<T>() where T : MControl
        {
            Type type = typeof(T);
            for (var i = 0; i < MControl.List.Count; i++)
            {
                var item = MControl.List.Keys.ElementAt(i);
                if (MControl.List[item].GetType() == type)
                {
                    return MControl.List[item];
                }
            }
            return null;
        }

        /// <summary>
        ///     重置子控件
        /// </summary>
        public static void ReSet()
        {
            for (var i = _list.Count - 1; i >= 0; i--)
            {
                var item = _list.Keys.ElementAt(i);
                if (_list[item] == Current)
                {
                    Current = null;
                    if (!_list[item].IsDisposed)
                    {
                        _list[item].Dispose();
                    }
                    _list[item] = null;
                    _list.Remove(item);
                    break;
                }
            }
        }

        /// <summary>
        ///     重置控件上所有子控件
        /// </summary>
        public static void ReSet(Control parent)
        {
            for (var i = _list.Count - 1; i >= 0; i--)
            {
                var item = _list.Keys.ElementAt(i);
                if (_list[item].Parent == parent)
                {
                    if (_list[item] == Current)
                    {
                        Current = null;
                    }
                    if (!_list[item].IsDisposed)
                    {
                        _list[item].Dispose();
                    }
                    _list[item] = null;
                    _list.Remove(item);
                }
            }
        }

        /// <summary>
        ///     重置所有子控件
        /// </summary>
        public static void ReSetAll()
        {
            for (var i = _list.Count - 1; i >= 0; i--)
            {
                var item = _list.Keys.ElementAt(i);
                if (!_list[item].IsDisposed)
                {
                    _list[item].Dispose();
                }
                _list[item] = null;
            }
            Current = null;
            _list.Clear();
        }

        /// <summary>
        ///     刷新所有控件数据
        /// </summary>
        public static void RefreshAll(object sender, EventArgs e)
        {
            for (var i = 0; i < _list.Count; i++)
            {
                var item = _list.Keys.ElementAt(i);
                _list[item].Refresh(sender, e);
            }
        }

        #endregion
    }
}