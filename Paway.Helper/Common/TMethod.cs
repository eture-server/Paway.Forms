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
        #region 在窗体上固定控件位置
        private static List<LocateInfo> tList;
        private static Size normal = Size.Empty;

        /// <summary>
        /// 在窗体上固定控件位置
        /// </summary>
        /// <param name="form"></param>
        /// <param name="control"></param>
        public static void AddLocate(this ContainerControl form, Control control)
        {
            form.AddLocate(control, StringAlignment.Center, StringAlignment.Center);
        }
        /// <summary>
        /// 在窗体上固定控件位置
        /// </summary>
        /// <param name="form"></param>
        /// <param name="control"></param>
        /// <param name="lLocation"></param>
        public static void AddLocate(this ContainerControl form, Control control, StringAlignment lLocation)
        {
            form.AddLocate(control, lLocation, lLocation);
        }
        /// <summary>
        /// 在窗体上固定控件位置
        /// </summary>
        /// <param name="form"></param>
        /// <param name="control"></param>
        /// <param name="xLocation"></param>
        /// <param name="yLocation"></param>
        public static void AddLocate(this ContainerControl form, Control control, StringAlignment xLocation, StringAlignment yLocation)
        {
            form.SizeChanged -= form_SizeChanged;
            form.SizeChanged += form_SizeChanged;
            if (normal == Size.Empty)
            {
                normal = form.Size;
            }
            if (tList == null)
            {
                tList = new List<LocateInfo>();
            }
            tList.Add(new LocateInfo(control, control.Location, xLocation, yLocation));
        }
        static void form_SizeChanged(object sender, EventArgs e)
        {
            if (tList == null) return;
            ContainerControl form = sender as ContainerControl;
            for (int i = 0; i < tList.Count; i++)
            {
                int left = 0;
                int top = 0;
                switch (tList[i].XLocation)
                {
                    case StringAlignment.Near:
                        left = 0;
                        break;
                    case StringAlignment.Center:
                        left = tList[i].Control.Width / 2;
                        break;
                    case StringAlignment.Far:
                        left = tList[i].Control.Width;
                        break;
                }
                switch (tList[i].YLocation)
                {
                    case StringAlignment.Near:
                        top = 0;
                        break;
                    case StringAlignment.Center:
                        top = tList[i].Control.Height / 2;
                        break;
                    case StringAlignment.Far:
                        top = tList[i].Control.Height;
                        break;
                }
                int x = form.Width * (tList[i].Point.X + left) / normal.Width;
                int y = form.Height * (tList[i].Point.Y + top) / normal.Height;
                tList[i].Control.Location = new Point(x - left, y - top);
            }
        }

        #endregion

        /// <summary>
        /// 定位属性
        /// </summary>
        private class LocateInfo
        {
            /// <summary>
            /// 控件
            /// </summary>
            public Control Control { get; set; }

            /// <summary>
            /// 原坐标
            /// </summary>
            public Point Point { get; set; }

            /// <summary>
            /// x点对齐方式
            /// </summary>
            public StringAlignment XLocation { get; set; }

            /// <summary>
            /// y点对齐方式
            /// </summary>
            public StringAlignment YLocation { get; set; }

            /// <summary>
            /// 构造
            /// </summary>
            public LocateInfo(Control control, Point point, StringAlignment xLocation, StringAlignment yLocation)
            {
                this.Control = control;
                this.Point = point;
                this.XLocation = xLocation;
                this.YLocation = yLocation;
            }
        }
    }
}
