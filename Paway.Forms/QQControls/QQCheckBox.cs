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
    [DefaultEvent("CheckedChanged")]
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
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

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
                        var bitmap = new Bitmap(
                            AssemblyHelper.GetImage("QQ.CheckBox.normal.png"));
                        //bitmap = RGB2Gray(bitmap);
                        g.DrawImage(bitmap, ImageRect);
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
                            using (var normal = AssemblyHelper.GetImage("QQ.CheckBox.tick_normal.png"))
                            {
                                g.DrawImage(normal, ImageRect);
                            }
                        }
                        else
                        {
                            using (var normal = AssemblyHelper.GetImage("QQ.CheckBox.normal.png"))
                            {
                                g.DrawImage(normal, ImageRect);
                            }
                        }
                        break;
                    case TMouseState.Down:
                    case TMouseState.Up:
                    case TMouseState.Move:
                        if (Checked)
                        {
                            using (var high = AssemblyHelper.GetImage("QQ.CheckBox.tick_highlight.png"))
                            {
                                g.DrawImage(high, ImageRect);
                            }
                        }
                        else
                        {
                            using (var high = AssemblyHelper.GetImage("QQ.CheckBox.hightlight.png"))
                            {
                                g.DrawImage(high, ImageRect);
                            }
                        }
                        break;
                }
                if (CheckState == CheckState.Indeterminate)
                {
                    if (MouseState == TMouseState.Down || MouseState == TMouseState.Move)
                    {
                        using (var normal = AssemblyHelper.GetImage("QQ.CheckBox._tick_normal.png"))
                        {
                            g.DrawImage(normal, ImageRect);
                        }
                    }
                    else
                    {
                        using (var high = AssemblyHelper.GetImage("QQ.CheckBox._tick_highlight.png"))
                        {
                            g.DrawImage(high, ImageRect);
                        }
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
    }
}