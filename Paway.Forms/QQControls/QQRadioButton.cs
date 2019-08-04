using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Paway.Forms.Properties;
using Paway.Helper;

namespace Paway.Forms
{
    /// <summary>
    /// </summary>
    [ToolboxBitmap(typeof(RadioButton))]
    public class QQRadioButton : RadioButton
    {
        #region 变量
        private TMouseState _mouseState = TMouseState.Normal;
        private readonly Image tick_normalImage = Resources.QQ_RadioButton_tick_normal;
        private readonly Image normalImage = Resources.QQ_RadioButton_normal;
        private readonly Image tick_highlightImage = Resources.QQ_RadioButton_tick_highlight;
        private readonly Image highlightImage = Resources.QQ_RadioButton_highlight;

        #endregion

        #region 构造函数
        /// <summary>
        /// </summary>
        public QQRadioButton()
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
            ForeColor = Color.Black;
            BackColor = Color.Transparent;
            this.AutoSize = true;
        }

        #endregion

        #region 重载属性
        /// <summary>
        /// </summary>
        [Description("鼠标状态")]
        [DefaultValue(TMouseState.Normal)]
        private TMouseState MouseState
        {
            set
            {
                _mouseState = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 文本区域
        /// </summary>
        private Rectangle TextRect
        {
            get { return new Rectangle(17, 0, Width - 17, Height); }
        }

        /// <summary>
        /// 图片显示区域
        /// </summary>
        private Rectangle ImageRect
        {
            get { return new Rectangle(0, (Height - 17) / 2, 17, 17); }
        }

        /// <summary>
        /// 获取或设置控件的背景色
        /// </summary>
        [Description("获取或设置控件的背景色")]
        [DefaultValue(typeof(Color), "Transparent")]
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
        /// 获取或设置控件的前景色。
        /// </summary>
        [Description("获取或设置控件的前景色")]
        [DefaultValue(typeof(Color), "Black")]
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

        #region 重绘
        /// <summary>
        /// 重绘
        /// </summary>
        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;

            var foreColor = Enabled ? ForeColor : Color.Gray;
            var flags = TextFormatFlags.Left |
                        TextFormatFlags.SingleLine |
                        TextFormatFlags.VerticalCenter;
            TextRenderer.DrawText(g, Text, Font, TextRect, foreColor, flags);

            switch (_mouseState)
            {
                case TMouseState.Leave:
                case TMouseState.Normal:
                    if (Checked)
                    {
                        g.DrawImage(tick_normalImage, ImageRect);
                    }
                    else
                    {
                        g.DrawImage(normalImage, ImageRect);
                    }

                    break;
                case TMouseState.Move:
                case TMouseState.Down:
                case TMouseState.Up:
                    if (Checked)
                    {
                        g.DrawImage(tick_highlightImage, ImageRect);
                    }
                    else
                    {
                        g.DrawImage(highlightImage, ImageRect);
                    }
                    break;
            }
        }

        /// <summary>
        /// </summary>
        protected override void OnMouseEnter(EventArgs eventargs)
        {
            base.OnMouseEnter(eventargs);
            MouseState = TMouseState.Move;
        }

        /// <summary>
        /// </summary>
        protected override void OnMouseLeave(EventArgs eventargs)
        {
            base.OnMouseLeave(eventargs);
            MouseState = TMouseState.Leave;
        }

        /// <summary>
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            MouseState = TMouseState.Up;
        }

        /// <summary>
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            MouseState = TMouseState.Down;
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
            }
            if (tick_normalImage != null) tick_normalImage.Dispose();
            if (normalImage != null) normalImage.Dispose();
            if (tick_highlightImage != null) tick_highlightImage.Dispose();
            if (highlightImage != null) highlightImage.Dispose();
            base.Dispose(disposing);
        }

        #endregion
    }
}