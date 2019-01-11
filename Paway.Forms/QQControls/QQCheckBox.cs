using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Paway.Helper;
using Paway.Resource;

namespace Paway.Forms
{
    /// <summary>
    ///     CheckBox
    /// </summary>
    [DefaultProperty("Checked")]
    [ToolboxBitmap(typeof(CheckBox))]
    public class QQCheckBox : CheckBox
    {
        #region 变量

        /// <summary>
        ///     当前的属标状态
        /// </summary>
        private TMouseState _mouseState = TMouseState.Normal;

        #endregion

        #region 构造函数

        /// <summary>
        /// </summary>
        public QQCheckBox()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.Selectable |
                ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, false);
            UpdateStyles();
            TConfig.Init(this);
            this.AutoSize = true;
        }

        #endregion

        #region 属性

        /// <summary>
        ///     文本区域
        /// </summary>
        internal Rectangle TextRect
        {
            get { return new Rectangle(17, 0, Width - 17, Height); }
        }

        /// <summary>
        ///     图片显示区域
        /// </summary>
        internal Rectangle ImageRect
        {
            get { return new Rectangle(0, (Height - 17) / 2, 17, 17); }
        }

        /// <summary>
        ///     鼠标状态
        /// </summary>
        [Description("鼠标状态"), DefaultValue(typeof(TMouseState), "Normal")]
        internal TMouseState MouseState
        {
            get { return _mouseState; }
            set
            {
                _mouseState = value;
                Invalidate();
            }
        }

        /// <summary>
        ///     重写CheckBox的Text属性
        /// </summary>
        [DefaultValue("QQCheckBox")]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        /// <summary>
        ///     重载AutoSize属性
        /// </summary>
        [DefaultValue(true)]
        public override bool AutoSize
        {
            get { return base.AutoSize; }
            set { base.AutoSize = value; }
        }

        /// <summary>
        ///     获取或设置控件的背景色
        /// </summary>
        [Description("获取或设置控件的背景色"), DefaultValue(typeof(Color), "Transparent")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                if (value == Color.Empty || value == SystemColors.Control)
                {
                    value = Color.Transparent;
                }
                base.BackColor = value;
            }
        }

        /// <summary>
        ///     获取或设置控件的前景色。
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

        private readonly Image normalImage = AssemblyHelper.GetImage("QQ.CheckBox.normal.png");
        private readonly Image tick_normalImage = AssemblyHelper.GetImage("QQ.CheckBox.tick_normal.png");
        private readonly Image tick_highlightImage = AssemblyHelper.GetImage("QQ.CheckBox.tick_highlight.png");
        private readonly Image hightlightImage = AssemblyHelper.GetImage("QQ.CheckBox.hightlight.png");
        private readonly Image _tick_normalImage = AssemblyHelper.GetImage("QQ.CheckBox._tick_normal.png");
        private readonly Image _tick_highlightImage = AssemblyHelper.GetImage("QQ.CheckBox._tick_highlight.png");

        #endregion

        #region Override 方法

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;

            var flags = TextFormatFlags.Left |
                        TextFormatFlags.SingleLine |
                        TextFormatFlags.VerticalCenter;
            var color = Enabled ? ForeColor : Color.LightGray;
            TextRenderer.DrawText(e.Graphics, Text, Font, TextRect, color, flags);
            if (!Enabled)
            {
                switch (CheckState)
                {
                    case CheckState.Checked:
                        break;
                    case CheckState.Indeterminate:
                        break;
                    case CheckState.Unchecked:
                        g.DrawImage(normalImage, ImageRect);
                        break;
                }
            }
            else
            {
                switch (_mouseState)
                {
                    case TMouseState.Normal:
                    case TMouseState.Leave:
                        if (Checked)
                        {
                            g.DrawImage(tick_normalImage, ImageRect);
                        }
                        else
                        {
                            g.DrawImage(normalImage, ImageRect);
                        }
                        break;
                    case TMouseState.Down:
                    case TMouseState.Up:
                    case TMouseState.Move:
                        if (Checked)
                        {
                            g.DrawImage(tick_highlightImage, ImageRect);
                        }
                        else
                        {
                            g.DrawImage(hightlightImage, ImageRect);
                        }
                        break;
                }
                if (CheckState == CheckState.Indeterminate)
                {
                    if (MouseState == TMouseState.Down || MouseState == TMouseState.Move)
                    {
                        g.DrawImage(_tick_normalImage, ImageRect);
                    }
                    else
                    {
                        g.DrawImage(_tick_highlightImage, ImageRect);
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="eventargs"></param>
        protected override void OnMouseEnter(EventArgs eventargs)
        {
            base.OnMouseEnter(eventargs);
            MouseState = TMouseState.Move;
        }

        /// <summary>
        /// </summary>
        /// <param name="mevent"></param>
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            MouseState = TMouseState.Down;
        }

        /// <summary>
        /// </summary>
        /// <param name="eventargs"></param>
        protected override void OnMouseLeave(EventArgs eventargs)
        {
            base.OnMouseLeave(eventargs);
            MouseState = TMouseState.Leave;
        }

        /// <summary>
        /// </summary>
        /// <param name="mevent"></param>
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            MouseState = TMouseState.Up;
        }

        #endregion

        #region Dispose
        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (normalImage != null)
                    normalImage.Dispose();
                if (tick_normalImage != null)
                    tick_normalImage.Dispose();
                if (tick_highlightImage != null)
                    tick_highlightImage.Dispose();
                if (hightlightImage != null)
                    hightlightImage.Dispose();
                if (_tick_normalImage != null)
                    _tick_normalImage.Dispose();
                if (_tick_highlightImage != null)
                    _tick_highlightImage.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}