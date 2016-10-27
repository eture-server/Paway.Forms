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
    ///     Button
    /// </summary>
    [DefaultEvent("Click")]
    [DefaultProperty("Text")]
    [ToolboxBitmap(typeof(Button))]
    public class QQButton : Button
    {
        #region 变量

        /// <summary>
        ///     文本对齐方式
        /// </summary>
        private TextFormatFlags _textAlign = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;

        #endregion

        #region 构造函数

        /// <summary>
        ///     实例化 Paway.Forms.QQButton 新的实例。
        /// </summary>
        public QQButton()
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
            Validated += QQButton_LostFocus;
        }

        #endregion

        #region 方法

        private void QQButton_LostFocus(object sender, EventArgs e)
        {
            MouseState = TMouseState.Leave;
        }

        #endregion

        #region 属性

        /// <summary>
        ///     默认大小
        /// </summary>
        [Description("默认大小"), DefaultValue(typeof(Size), "75, 28")]
        protected override Size DefaultSize
        {
            get { return new Size(75, 28); }
        }

        /// <summary>
        ///     默认时的按钮图片
        /// </summary>
        private Image _normalImage;

        /// <summary>
        ///     默认图片
        /// </summary>
        [Description("默认时的按钮图片")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual Image NormalImage
        {
            get
            {
                if (_normalImage == null)
                    _normalImage = AssemblyHelper.GetImage("QQ.Button.normal.png");
                return _normalImage;
            }
            set
            {
                _normalImage = value;
                Invalidate();
            }
        }

        /// <summary>
        ///     鼠标按下时的图片
        /// </summary>
        private Image _downImage;

        /// <summary>
        ///     鼠标按下时的图片
        /// </summary>
        [Description("鼠标按下时的图片")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual Image DownImage
        {
            get
            {
                if (_downImage == null)
                    _downImage = AssemblyHelper.GetImage("QQ.Button.down.png");
                return _downImage;
            }
            set
            {
                _downImage = value;
                Invalidate();
            }
        }

        /// <summary>
        ///     鼠标划过时的图片
        /// </summary>
        private Image _moveImage;

        /// <summary>
        ///     鼠标划过时的图片
        /// </summary>
        [Description("鼠标划过时的图片")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual Image MoveImage
        {
            get
            {
                if (_moveImage == null)
                    _moveImage = AssemblyHelper.GetImage("QQ.Button.highlight.png");
                return _moveImage;
            }
            set
            {
                _moveImage = value;

                Invalidate();
            }
        }

        /// <summary>
        ///     是否按下了鼠标
        /// </summary>
        private bool _isShowBorder = true;

        /// <summary>
        ///     是否显示发光边框
        /// </summary>
        [Description("是否显示发光边框"), DefaultValue(true)]
        public virtual bool IsShowBorder
        {
            get { return _isShowBorder; }
            set { _isShowBorder = value; }
        }

        /// <summary>
        ///     与控件相关的文本
        /// </summary>
        [DefaultValue("QQButton")]
        public override string Text
        {
            get { return base.Text; }
            set
            {
                if (base.Text != value)
                {
                    base.Text = value;
                    Invalidate(TextRect);
                }
            }
        }

        /// <summary>
        ///     按钮上显示的图片
        /// </summary>
        [Description("按钮上显示的图片")]
        public new virtual Image Image
        {
            get { return base.Image; }
            set
            {
                base.Image = value;
                Invalidate();
            }
        }

        /// <summary>
        ///     按钮上文字的对齐方式
        /// </summary>
        [Description("按钮上文字的对齐方式")]
        public new ContentAlignment TextAlign
        {
            get { return base.TextAlign; }
            set
            {
                base.TextAlign = value;
                switch (base.TextAlign)
                {
                    case ContentAlignment.BottomCenter:
                        _textAlign = TextFormatFlags.Bottom |
                                     TextFormatFlags.HorizontalCenter |
                                     TextFormatFlags.SingleLine;
                        break;
                    case ContentAlignment.BottomLeft:
                        _textAlign = TextFormatFlags.Bottom |
                                     TextFormatFlags.Left |
                                     TextFormatFlags.SingleLine;
                        break;
                    case ContentAlignment.BottomRight:
                        _textAlign = TextFormatFlags.Bottom |
                                     TextFormatFlags.Right |
                                     TextFormatFlags.SingleLine;
                        break;
                    case ContentAlignment.MiddleCenter:
                        _textAlign = TextFormatFlags.SingleLine |
                                     TextFormatFlags.HorizontalCenter |
                                     TextFormatFlags.VerticalCenter;
                        break;
                    case ContentAlignment.MiddleLeft:
                        _textAlign = TextFormatFlags.Left |
                                     TextFormatFlags.VerticalCenter |
                                     TextFormatFlags.SingleLine;
                        break;
                    case ContentAlignment.MiddleRight:
                        _textAlign = TextFormatFlags.Right |
                                     TextFormatFlags.VerticalCenter |
                                     TextFormatFlags.SingleLine;
                        break;
                    case ContentAlignment.TopCenter:
                        _textAlign = TextFormatFlags.Top |
                                     TextFormatFlags.HorizontalCenter |
                                     TextFormatFlags.SingleLine;
                        break;
                    case ContentAlignment.TopLeft:
                        _textAlign = TextFormatFlags.Top |
                                     TextFormatFlags.Left |
                                     TextFormatFlags.SingleLine;
                        break;
                    case ContentAlignment.TopRight:
                        _textAlign = TextFormatFlags.Top |
                                     TextFormatFlags.Right |
                                     TextFormatFlags.SingleLine;
                        break;
                }
                Invalidate(TextRect);
            }
        }

        /// <summary>
        ///     整个按钮的区域
        /// </summary>
        internal Rectangle AllRect
        {
            get { return new Rectangle(0, 0, Width, Height); }
        }

        /// <summary>
        ///     文字区域
        /// </summary>
        internal Rectangle TextRect
        {
            get { return new Rectangle(2, 2, AllRect.Width - 4, AllRect.Height - 4); }
        }

        /// <summary>
        ///     鼠标状态
        /// </summary>
        private TMouseState _mouseState = TMouseState.Normal;

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
        ///     引发 System.Windows.Forms.Form.Paint 事件。
        /// </summary>
        /// <param name="pevent">包含事件数据的 System.Windows.Forms.PaintEventArgs。</param>
        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            //g.DrawImage(AssemblyHelper.GetAssemblyImage("Button.all_btn_White-side.png"), this.AllRect);
            if (Enabled)
            {
                if (IsShowBorder)
                {
                    if (Focused) //得到焦点的时候，绘制边框
                    {
                        using (var light = AssemblyHelper.GetImage("QQ.Button.Light.png"))
                        {
                            //g.DrawImage(light, this.AllRect);
                            DrawHelper.RendererBackground(g, AllRect, light, true);
                        }
                    }
                }

                switch (MouseState)
                {
                    case TMouseState.Leave:
                    case TMouseState.Normal:
                        if (Focused)
                        {
                            if (IsShowBorder)
                            {
                                using (var focus = AssemblyHelper.GetImage("QQ.Button.focus.png"))
                                {
                                    DrawHelper.RendererBackground(g, TextRect, focus, true);
                                }
                            }
                            else
                            {
                                DrawHelper.RendererBackground(g, TextRect, NormalImage, true);
                            }
                        }
                        else
                        {
                            DrawHelper.RendererBackground(g, TextRect, NormalImage, true);
                        }
                        break;
                    case TMouseState.Up:
                    case TMouseState.Move:
                        DrawHelper.RendererBackground(g, TextRect, MoveImage, true);
                        break;
                    case TMouseState.Down:
                        DrawHelper.RendererBackground(g, TextRect, DownImage, true);
                        break;
                }

                //if (this.Image != null)
                //{
                //int y = (this.Height - this.Image.Height) / 2;
                //int x = 10;

                //if (!string.IsNullOrEmpty(this.Text))
                //    x = (this.AllRect.Width - (TextHelper.GetStringLen(this.Text) * 6 + this.Image.Width)) / 2;
                //else
                //    x = (this.Width - this.Image.Width) / 2;//将图片绘制在按钮居中位置
                //Rectangle rect = new Rectangle(x, y, this.Image.Width, this.Image.Height);
                //Rectangle imgRect = new Rectangle(0, 0, this.Image.Width, this.Image.Height);
                ////绘制图片
                //g.DrawImage(this.Image, rect, imgRect, GraphicsUnit.Pixel);
                ////绘制文字
                //Rectangle textRect = new Rectangle(this.TextRect.X + this.Image.Width, this.TextRect.Y, this.TextRect.Width, this.TextRect.Height);
                //TextRenderer.DrawText(g, this.Text, this.Font, textRect, this.ForeColor, _textAlign);
                //}
                //else
                //{
                //绘制按钮上的文字
                TextRenderer.DrawText(g, Text, Font, TextRect, ForeColor, _textAlign);
                //}
            }
            else
            {
                using (var gray = AssemblyHelper.GetImage("QQ.Button.gray.png"))
                {
                    DrawHelper.RendererBackground(g, TextRect, gray, true);
                }
                TextRenderer.DrawText(g, Text, Font, TextRect, Color.Gray, _textAlign);
            }
        }

        /// <summary>
        ///     引发 System.Windows.Forms.Form.MouseEnter 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            MouseState = TMouseState.Move;
        }

        /// <summary>
        ///     引发 System.Windows.Forms.Form.MouseLeave 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            MouseState = TMouseState.Leave;
        }

        /// <summary>
        ///     引发 System.Windows.Forms.Form.MouseDown 事件。
        /// </summary>
        /// <param name="mevent">包含事件数据的 System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            MouseState = TMouseState.Down;
        }

        /// <summary>
        ///     引发 System.Windows.Forms.Form.MouseUp 事件。
        /// </summary>
        /// <param name="mevent">包含事件数据的 System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            MouseState = TMouseState.Up;
        }

        #endregion
    }
}