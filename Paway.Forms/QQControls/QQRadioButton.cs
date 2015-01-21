using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using Paway.Resource;
using Paway.Helper;

namespace Paway.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [DefaultEvent("CheckedChanged")]
    [ToolboxBitmap(typeof(RadioButton))]
    public class QQRadioButton : RadioButton
    {
        #region 变量
        /// <summary>
        /// 
        /// </summary>
        private TMouseState _mouseState = TMouseState.Normal;

        #endregion

        #region 构造函数
        /// <summary>
        /// 
        /// </summary>
        public QQRadioButton()
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
            InitMethod.Init(this);
        }

        #endregion

        #region 属性
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
        /// 
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
        /// <param name="pevent"></param>
        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            Color foreColor = base.Enabled ? this.ForeColor : Color.Gray;
            TextFormatFlags flags = TextFormatFlags.VerticalCenter |
                                    TextFormatFlags.Left |
                                    TextFormatFlags.SingleLine;
            TextRenderer.DrawText(g, this.Text, this.Font, this.TextRect, foreColor, flags);

            switch (this.MouseState)
            {
                case TMouseState.Leave:
                case TMouseState.Normal:
                    if (base.Checked)
                    {
                        using (Image normal = AssemblyHelper.GetImage("QQ.RadioButton.tick_normal.png"))
                        {
                            g.DrawImage(normal, this.ImageRect);
                        }
                    }
                    else
                    {
                        using (Image normal = AssemblyHelper.GetImage("QQ.RadioButton.normal.png"))
                        {
                            g.DrawImage(normal, this.ImageRect);
                        }
                    }

                    break;
                case TMouseState.Move:
                case TMouseState.Down:
                case TMouseState.Up:
                    if (base.Checked)
                    {
                        using (Image high = AssemblyHelper.GetImage("QQ.RadioButton.tick_highlight.png"))
                        {
                            g.DrawImage(high, this.ImageRect);
                        }
                    }
                    else
                    {
                        using (Image high = AssemblyHelper.GetImage("QQ.RadioButton.highlight.png"))
                        {
                            g.DrawImage(high, this.ImageRect);
                        }
                    }
                    break;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mevent"></param>
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            this.MouseState = TMouseState.Down;
        }
        #endregion
    }
}
