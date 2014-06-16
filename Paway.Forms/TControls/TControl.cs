using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    /// 自定义基控件
    /// </summary>
    public class TControl : UserControl, IControl
    {
        #region 变量
        private List<Locate> tList;
        private Size normal = Size.Empty;
        /// <summary>
        /// 指定窗体窗口如何显示
        /// </summary>
        protected FormWindowState _windowState = FormWindowState.Normal;

        #endregion

        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        public TControl()
        {
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.DoubleBuffer |
                ControlStyles.SupportsTransparentBackColor, true);
            this.UpdateStyles();
            this.BackColor = Color.Transparent;
        }

        #endregion

        #region 属性
        private Color _color;
        /// <summary>
        /// 获取或设置控件的背景色
        /// </summary>
        [Description("获取或设置控件的背景色"), DefaultValue(typeof(Color), "Transparent")]
        public override Color BackColor
        {
            get
            {
                if (_color == Color.Empty)
                    _color = Color.Transparent;
                return _color;
            }
            set
            {
                _color = value;
                if (value.A > _trans)
                {
                    base.BackColor = Color.FromArgb(_trans, value.R, value.G, value.B);
                }
            }
        }

        private Color _colorMoveBack = Color.Transparent;
        /// <summary>
        /// 鼠标移入状态的背景颜色
        /// 背景色为Color.White时使用默认背景
        /// </summary>
        [Description("鼠标移入状态的背景颜色,背景色为Color.Transparent时使用默认背景")]
        [DefaultValue(typeof(Color), "Transparent")]
        public Color ColorMoveBack
        {
            get { return this._colorMoveBack; }
            set
            {
                _colorMoveBack = value;
                if (value.A > _trans)
                {
                    _colorMoveBack = Color.FromArgb(_trans, value.R, value.G, value.B);
                }
                Invalidate(true);
            }
        }

        private Color _colorDownBack = Color.Transparent;
        /// <summary>
        /// 选中状态的背景颜色
        /// 背景色为Color.White时使用默认背景
        /// </summary>
        [Description("选中状态的背景颜色,背景色为Color.Transparent时使用默认背景")]
        [DefaultValue(typeof(Color), "Transparent")]
        public Color ColorDownBack
        {
            get { return this._colorDownBack; }
            set
            {
                _colorDownBack = value;
                if (value.A > _trans)
                {
                    _colorDownBack = Color.FromArgb(_trans, value.R, value.G, value.B);
                }
                Invalidate(true);
            }
        }

        private int _trans = 255;
        /// <summary>
        /// 控件透明度
        /// </summary>
        [Description("透明度"), DefaultValue(255)]
        public int Trans
        {
            get { return _trans; }
            set
            {
                if (value < 0 || value > 255)
                {
                    value = 255;
                }
                _trans = value;
            }
        }

        #endregion

        #region 接口
        /// <summary>
        /// 坐标点是否包含在项中
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool Contain(Point p) { return false; }

        #endregion

        #region 在窗体上固定控件位置
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
}
