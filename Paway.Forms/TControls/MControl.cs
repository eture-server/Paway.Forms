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
    ///     事件委托方法(OnChanged)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void MethodDelegate(object sender, EventArgs e);

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
        [Description("控件数据"), DefaultValue(null)]
        public new virtual object Tag { get; set; }

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
        public void InitDelegate(Delegate method)
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
            if (ChangeEvent != null)
            {
                ChangeEvent(sender, e);
            }
        }

        #endregion

        #region 界面切换控制

        /// <summary>
        ///     基类控件列表
        /// </summary>
        private static readonly Dictionary<string, MControl> _iList = new Dictionary<string, MControl>();

        /// <summary>
        ///     控件列表
        /// </summary>
        public static Dictionary<string, MControl> List
        {
            get { return _iList; }
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
            if (!Licence.Checking()) return null;

            MControl control = null;
            try
            {
                //不重复加载
                if (Current != null)
                {
                    if (Current.GetType() == type && Current.Parent == parent) return null;
                }
                //拒绝移除
                if (Current != null && !Current.UnLoad())
                {
                    return null;
                }
                //加载控件
                if (_iList.ContainsKey(type.FullName))
                {
                    control = _iList[type.FullName];
                    if (control.ILoad) return control;
                }

                parent.SuspendLayout();
                //加载控件
                if (control == null)
                {
                    var asmb = Assembly.GetAssembly(type);
                    control = asmb.CreateInstance(type.FullName) as MControl;
                }
                if (control == null)
                {
                    throw new ArgumentException(string.Format("{0} 不是有效的MControl", type.FullName));
                }
                if (e != null)
                {
                    control.Tag = e;
                }
                if (method != null)
                {
                    control.InitDelegate(method);
                }
                if (direction == TMDirection.None)
                {
                    direction = control.MDirection;
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
                if (!_iList.ContainsKey(type.FullName))
                {
                    _iList.Add(type.FullName, control);
                }
                parent.BackgroundImage = null;
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
            if (!_iList.ContainsKey(type.FullName))
            {
                _iList.Add(type.FullName, control);
                return control;
            }
            return _iList[type.FullName];
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
            for (var i = 0; i < _iList.Count; i++)
            {
                var item = _iList.Keys.ElementAt(i);
                if (_iList[item].Parent == parent)
                {
                    return _iList[item];
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
            for (var i = _iList.Count - 1; i >= 0; i--)
            {
                var item = _iList.Keys.ElementAt(i);
                if (_iList[item] == Current)
                {
                    Current = null;
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
        ///     重置控件上所有子控件
        /// </summary>
        public static void ReSet(Control parent)
        {
            for (var i = _iList.Count - 1; i >= 0; i--)
            {
                var item = _iList.Keys.ElementAt(i);
                if (_iList[item].Parent == parent)
                {
                    if (_iList[item] == Current)
                    {
                        Current = null;
                    }
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
        ///     重置所有子控件
        /// </summary>
        public static void ReSetAll()
        {
            for (var i = _iList.Count - 1; i >= 0; i--)
            {
                var item = _iList.Keys.ElementAt(i);
                if (!_iList[item].IsDisposed)
                {
                    _iList[item].Dispose();
                }
                _iList[item] = null;
            }
            Current = null;
            _iList.Clear();
        }

        /// <summary>
        ///     刷新所有控件数据
        /// </summary>
        public static void RefreshAll(object sender, EventArgs e)
        {
            for (var i = 0; i < _iList.Count; i++)
            {
                var item = _iList.Keys.ElementAt(i);
                _iList[item].Refresh(sender, e);
            }
        }

        #endregion
    }
}