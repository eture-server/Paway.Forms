using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing.Imaging;
using Paway.Resource;
using Paway.Helper;

namespace Paway.Forms
{
    /// <summary>
    /// CheckBox
    /// </summary>
    [DefaultEvent("CheckedChanged")]
    [DefaultProperty("Checked")]
    [ToolboxBitmap(typeof(CheckBox))]
    public class QQCheckBox : CheckBox
    {
        #region 变量
        /// <summary>
        /// 当前的属标状态
        /// </summary>
        private TMouseState _mouseState = TMouseState.Normal;

        #endregion

        #region 构造函数
        /// <summary>
        /// 
        /// </summary>
        public QQCheckBox()
        {
            this.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.Selectable |
                ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.Opaque, false);
            this.UpdateStyles();
            TConfig.Init(this);
        }

        #endregion

        #region 属性

        /// <summary>
        /// 文本区域
        /// </summary>
        internal Rectangle TextRect
        {
            get { return new Rectangle(17, 0, this.Width - 17, this.Height); }
        }
        /// <summary>
        /// 图片显示区域
        /// </summary>
        internal Rectangle ImageRect
        {
            get { return new Rectangle(0, (this.Height - 17) / 2, 17, 17); }
        }
        /// <summary>
        /// 鼠标状态
        /// </summary>
        [Description("鼠标状态"), DefaultValue(typeof(TMouseState), "Normal")]
        internal TMouseState MouseState
        {
            get { return this._mouseState; }
            set
            {
                this._mouseState = value;
                base.Invalidate();
            }
        }

        /// <summary>
        /// 重写CheckBox的Text属性
        /// </summary>
        [DefaultValue("QQCheckBox")]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }
        /// <summary>
        /// 获取或设置控件的背景色
        /// </summary>
        [Description("获取或设置控件的背景色"), DefaultValue(typeof(Color), "Transparent")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                if (value == Color.Empty)
                {
                    value = Color.Transparent;
                }
                base.BackColor = value;
            }
        }
        /// <summary>
        /// 获取或设置控件的前景色。
        /// </summary>
        [Description("获取或设置控件的前景色"), DefaultValue(typeof(Color), "Black")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                if (value == Color.Empty)
                {
                    value = Color.Black;
                }
                base.ForeColor = value;
            }
        }

        #endregion

        #region Override 方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            TextFormatFlags flags = TextFormatFlags.Left |
                                    TextFormatFlags.SingleLine |
                                    TextFormatFlags.VerticalCenter;
            Color color = this.Enabled ? this.ForeColor : Color.LightGray;
            TextRenderer.DrawText(e.Graphics, this.Text, this.Font, this.TextRect, color, flags);
            if (!base.Enabled)
            {
                switch (this.CheckState)
                {
                    case CheckState.Checked:
                        break;
                    case CheckState.Indeterminate:
                        break;
                    case CheckState.Unchecked:
                        Bitmap bitmap = new Bitmap(
                            AssemblyHelper.GetImage("QQ.CheckBox.normal.png"));
                        //bitmap = RGB2Gray(bitmap);
                        g.DrawImage(bitmap, this.ImageRect);
                        break;
                }
            }
            else
            {
                switch (this._mouseState)
                {
                    case TMouseState.Normal:
                    case TMouseState.Leave:
                        if (base.Checked)
                        {
                            using (Image normal = AssemblyHelper.GetImage("QQ.CheckBox.tick_normal.png"))
                            {
                                g.DrawImage(normal, this.ImageRect);
                            }
                        }
                        else
                        {
                            using (Image normal = AssemblyHelper.GetImage("QQ.CheckBox.normal.png"))
                            {
                                g.DrawImage(normal, this.ImageRect);
                            }
                        }
                        break;
                    case TMouseState.Down:
                    case TMouseState.Up:
                    case TMouseState.Move:
                        if (base.Checked)
                        {
                            using (Image high = AssemblyHelper.GetImage("QQ.CheckBox.tick_highlight.png"))
                            {
                                g.DrawImage(high, this.ImageRect);
                            }
                        }
                        else
                        {
                            using (Image high = AssemblyHelper.GetImage("QQ.CheckBox.hightlight.png"))
                            {
                                g.DrawImage(high, this.ImageRect);
                            }
                        }
                        break;
                }
                if (base.CheckState == CheckState.Indeterminate)
                {
                    if (this.MouseState == TMouseState.Down || this.MouseState == TMouseState.Move)
                    {
                        using (Image normal = AssemblyHelper.GetImage("QQ.CheckBox._tick_normal.png"))
                        {
                            g.DrawImage(normal, this.ImageRect);
                        }
                    }
                    else
                    {
                        using (Image high = AssemblyHelper.GetImage("QQ.CheckBox._tick_highlight.png"))
                        {
                            g.DrawImage(high, this.ImageRect);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventargs"></param>
        protected override void OnMouseEnter(EventArgs eventargs)
        {
            base.OnMouseEnter(eventargs);
            this.MouseState = TMouseState.Move;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mevent"></param>
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            this.MouseState = TMouseState.Down;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventargs"></param>
        protected override void OnMouseLeave(EventArgs eventargs)
        {
            base.OnMouseLeave(eventargs);
            this.MouseState = TMouseState.Leave;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mevent"></param>
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            this.MouseState = TMouseState.Up;
        }

        #endregion
    }
}
