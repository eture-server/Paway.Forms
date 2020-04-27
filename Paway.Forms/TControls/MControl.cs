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
    /// 多控件切换方法
    /// </summary>
    public class MControl : TControl
    {
        #region 事件
        /// <summary>
        /// 当控件数据更新时发生
        /// </summary>
        public static event EventHandler ChangeEvent;

        #endregion

        #region 字段与属性
        /// <summary>
        /// 控件数据
        /// </summary>
        [Description("控件数据")]
        [DefaultValue(null)]
        protected EventArgs Args { get; set; }
        /// <summary>
        /// 控件列表
        /// </summary>
        public static Dictionary<Type, MControl> List { get; } = new Dictionary<Type, MControl>();
        /// <summary>
        /// 当前控件
        /// </summary>
        public static MControl Current { get; private set; }

        #endregion

        #region virtual Method
        /// <summary>
        /// 从其它控件切换过来时重新激活
        /// </summary>
        protected virtual void ReLoad(bool first)
        {
            ILoad = true;
        }
        /// <summary>
        /// 移除当前界面时，是否允许移除
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

        #endregion

        #region 界面切换控制
        /// <summary>
        /// 引发ChangeEvent事件
        /// </summary>
        public static void OnChanged(object sender, EventArgs e)
        {
            ChangeEvent?.Invoke(sender, e);
        }
        /// <summary>
        /// 切换界面控件
        /// </summary>
        public static MControl ReLoad(Control parent, Type type)
        {
            return ReLoad(parent, type, EventArgs.Empty, TMDirection.None);
        }
        /// <summary>
        /// 切换界面控件
        /// </summary>
        public static MControl ReLoad(Control parent, Type type, EventArgs e)
        {
            return ReLoad(parent, type, e, TMDirection.None);
        }
        /// <summary>
        /// 切换界面控件
        /// </summary>
        public static MControl ReLoad(Control parent, Type type, TMDirection direction)
        {
            return ReLoad(parent, type, EventArgs.Empty, direction);
        }
        /// <summary>
        /// 切换界面控件
        /// 如已加载，则调用ReLoad()
        /// </summary>
        public static MControl ReLoad(Control parent, Type type, EventArgs e, TMDirection direction, int intervel = -1)
        {
            var first = false;
            MControl control = null;
            try
            {
                //不重复加载
                if (Current != null)
                {
                    if (Current.GetType() == type && Current.Parent == parent) return Current;
                }
                //加载控件
                if (List.ContainsKey(type))
                {
                    if (List[type].ILoad) return control = List[type];
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
                if (List.ContainsKey(type))
                {
                    control = List[type];
                }
                parent.SuspendLayout();
                //加载控件
                if (control == null)
                {
                    first = true;
                    control = (MControl)Activator.CreateInstance(type);
                }
                if (control == null)
                {
                    throw new ArgumentException($"{type.FullName} Not a valid MControl.");
                }
                if (e != null)
                {
                    control.Args = e;
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
                if (!List.ContainsKey(type))
                {
                    List.Add(type, control);
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
                if (e != null) Current.Args = e;
                Current.Focus();
                Current.ReLoad(first);
            }
        }

        /// <summary>
        /// 将已定义控件加入列表
        /// </summary>
        /// <param name="control"></param>
        /// <returns>成功返回列表中的已加入的MControl控件</returns>
        public static MControl Add(MControl control)
        {
            var type = control.GetType();
            if (!List.ContainsKey(type))
            {
                List.Add(type, control);
            }
            return List[type];
        }
        /// <summary>
        /// 将指定类型控件加入列表
        /// </summary>
        /// <returns>成功返回列表中的已加入的MControl控件</returns>
        public static MControl Add<T>() where T : MControl
        {
            return Add(typeof(T));
        }
        /// <summary>
        /// 将指定类型控件加入列表
        /// </summary>
        /// <returns>成功返回列表中的已加入的MControl控件</returns>
        public static MControl Add(Type type)
        {
            if (!List.ContainsKey(type))
            {
                MControl control = (MControl)Activator.CreateInstance(type);
                if (control == null) throw new ArgumentException($"{type.FullName} Not a valid MControl.");
                List.Add(type, control);
            }
            return List[type];
        }
        /// <summary>
        /// 返回控件上的当前子控件
        /// </summary>
        public static MControl Get(Control parent)
        {
            for (var i = 0; i < List.Count; i++)
            {
                var item = List.Keys.ElementAt(i);
                if (List[item].Parent == parent)
                {
                    return List[item];
                }
            }
            return null;
        }
        /// <summary>
        /// 返回指定类型控件
        /// </summary>
        public static MControl Get<T>() where T : MControl
        {
            Type type = typeof(T);
            for (var i = 0; i < List.Count; i++)
            {
                var item = List.Keys.ElementAt(i);
                if (item == type)
                {
                    return List[item];
                }
            }
            return null;
        }
        /// <summary>
        /// 重置控件上所有子控件（不指定父控件则重置所有）
        /// </summary>
        public static void Reset(Control parent = null)
        {
            for (var i = List.Count - 1; i >= 0; i--)
            {
                var item = List.Keys.ElementAt(i);
                if (parent == null || List[item].Parent == parent)
                {
                    if (List[item] == Current)
                    {
                        Current = null;
                    }
                    if (!List[item].IsDisposed)
                    {
                        List[item].Dispose();
                    }
                    List[item] = null;
                    List.Remove(item);
                }
            }
        }

        #endregion
    }
}