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
    /// </summary>
    [DefaultEvent("CheckedChanged")]
    [ToolboxBitmap(typeof(RadioButton))]
    public class QQRadioButton : RadioButton
    {
        #region 变量

        /// <summary>
        /// </summary>
        private TMouseState _mouseState = TMouseState.Normal;

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
            TConfig.Init(this);
        }

        #endregion

        #region 属性

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
        ///     获取或设置控件的背景色
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

        #endregion

        #region Override 方法

        /// <summary>
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            var foreColor = Enabled ? ForeColor : Color.Gray;
            var flags = TextFormatFlags.VerticalCenter |
                        TextFormatFlags.Left |
                        TextFormatFlags.SingleLine;
            TextRenderer.DrawText(g, Text, Font, TextRect, foreColor, flags);

            switch (MouseState)
            {
                case TMouseState.Leave:
                case TMouseState.Normal:
                    if (Checked)
                    {
                        using (var normal = AssemblyHelper.GetImage("QQ.RadioButton.tick_normal.png"))
                        {
                            g.DrawImage(normal, ImageRect);
                        }
                    }
                    else
                    {
                        using (var normal = AssemblyHelper.GetImage("QQ.RadioButton.normal.png"))
                        {
                            g.DrawImage(normal, ImageRect);
                        }
                    }

                    break;
                case TMouseState.Move:
                case TMouseState.Down:
                case TMouseState.Up:
                    if (Checked)
                    {
                        using (var high = AssemblyHelper.GetImage("QQ.RadioButton.tick_highlight.png"))
                        {
                            g.DrawImage(high, ImageRect);
                        }
                    }
                    else
                    {
                        using (var high = AssemblyHelper.GetImage("QQ.RadioButton.highlight.png"))
                        {
                            g.DrawImage(high, ImageRect);
                        }
                    }
                    break;
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

        /// <summary>
        /// </summary>
        /// <param name="mevent"></param>
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            MouseState = TMouseState.Down;
        }

        #endregion
    }
}