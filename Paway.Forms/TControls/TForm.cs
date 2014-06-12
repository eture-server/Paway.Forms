using Paway.Helper;
using Paway.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    /// 窗体自定义基类
    /// </summary>
    public partial class TForm : Form
    {
        #region 属性
        private List<Locate> tList;
        private Size normal = Size.Empty;
        /// <summary>
        /// 指定窗体窗口如何显示
        /// </summary>
        protected FormWindowState _windowState = FormWindowState.Normal;

        #endregion

        #region 移动窗体
        /// <summary>
        /// 移动窗体
        /// </summary>
        /// <param name="control"></param>
        protected void TMouseMove(Control control)
        {
            if (control == null) return;
            control.MouseDown += control_MouseDown;
        }
        void control_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            IControl icontrol = sender as IControl;
            if (icontrol != null)
            {
                if (icontrol.Contain(e.Location)) return;
            }
            if (this._windowState != FormWindowState.Maximized)
            {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(Handle, 274, 61440 + 9, 0);
            }
        }

        #endregion

        #region MyRegion
        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ToLocate();
        }
        /// <summary>
        /// 在窗体上固定控件位置
        /// </summary>
        protected void AddLocate(Control control)
        {
            AddLocate(control, StringAlignment.Center, StringAlignment.Center);
        }
        /// <summary>
        /// 在窗体上固定控件位置
        /// </summary>
        protected void AddLocate(Control control, StringAlignment lLocation)
        {
            AddLocate(control, lLocation, lLocation);
        }
        /// <summary>
        /// 在窗体上固定控件位置
        /// </summary>
        protected void AddLocate(Control control, StringAlignment xLocation, StringAlignment yLocation)
        {
            if (normal == Size.Empty)
            {
                normal = this.Size;
            }
            if (tList == null)
            {
                tList = new List<Locate>();
            }
            tList.Add(new Locate(control, control.Location, xLocation, yLocation));
        }
        private void ToLocate()
        {
            if (tList == null) return;
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
                int x = this.Width * (tList[i].Point.X + left) / normal.Width;
                int y = this.Height * (tList[i].Point.Y + top) / normal.Height;
                tList[i].Control.Location = new Point(x - left, y - top);
            }
        }
        #endregion
    }
    /// <summary>
    /// 定位属性
    /// </summary>
    public class Locate
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
        public Locate(Control control, Point point, StringAlignment xLocation, StringAlignment yLocation)
        {
            this.Control = control;
            this.Point = point;
            this.XLocation = xLocation;
            this.YLocation = yLocation;
        }
    }
}
