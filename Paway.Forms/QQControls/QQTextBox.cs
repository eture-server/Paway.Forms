using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Paway.Resource;
using Paway.Helper;
using System.Reflection;

namespace Paway.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class QQTextBox : UserControl
    {
        #region 变量
        /// <summary>
        /// BaseText
        /// </summary>
        protected QQTextBoxBase BaseText;

        /// <summary>
        /// BaseText
        /// </summary>
        [Browsable(false), Category("Properties")]
        public QQTextBoxBase Edit { get { return BaseText; } }

        /// <summary>
        /// TNum.获取或设置文本框中选定的文本起始点
        /// </summary>
        [Description("获取或设置文本框中选定的文本起始点"), DefaultValue(0)]
        public int SelectionStart
        {
            get { return BaseText.SelectionStart; }
            set { BaseText.SelectionStart = value; }
        }
        /// <summary>
        /// TNum.获取或设置文本框中选定的字符数
        /// </summary>
        [Description("获取或设置文本框中选定的字符数"), DefaultValue(0)]
        public int SelectionLength
        {
            get { return BaseText.SelectionLength; }
            set { BaseText.SelectionLength = value; }
        }
        /// <summary>
        /// TNum.获取或设置一个值，该值指示控件中当前选定的文本
        /// </summary>
        [Description("获取或设置一个值，该值指示控件中当前选定的文本")]
        public string SelectedText
        {
            get { return BaseText.SelectedText; }
            set { BaseText.SelectedText = value; }
        }
        /// <summary>
        /// 获取控件中文本的长度
        /// </summary>
        [Description("获取控件中文本的长度")]
        public int TextLength
        {
            get { return BaseText.TextLength; }
        }

        private Image _borderImage = AssemblyHelper.GetImage("QQ.TextBox.normal.png");
        private Cursor _cursor = Cursors.IBeam;
        private TMouseState _mouseState = TMouseState.Normal;
        private TMouseState _iconMouseState = TMouseState.Normal;
        private bool _iconIsButton;
        private ErrorProvider error;
        private IContainer components;
        private Image _icon;
        #endregion

        #region 构造函数
        /// <summary>
        /// 
        /// </summary>
        public QQTextBox()
        {
            this.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.DoubleBuffer, true);
            InitializeComponent();
            this.InitEvents();
            this.BackColor = Color.Transparent;
            this.UpdateStyles();
        }

        /// <summary>
        /// 控件背景
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.Parent != null)
            {
                BaseText.BackColor = _isTrans ? this.Parent.BackColor : Color.White;
            }
            this.KeyDown += QQTextBox_KeyDown;
        }

        #endregion

        #region 扩展
        void QQTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && Control.ModifierKeys == Keys.Control)
            {
                this.BaseText.SelectAll();
            }
        }

        #endregion

        #region 自定义事件 && 激发事件的方法
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler IconClick;
        /// <summary>
        /// 
        /// </summary>
        private void OnIconClick()
        {
            if (this.IconClick != null)
                this.IconClick(this, EventArgs.Empty);
        }
        /// <summary>
        /// 在 Text 属性值更改时发生
        /// </summary>
        public new event EventHandler TextChanged;
        private void OnTextChang()
        {
            if (TextChanged != null)
            {
                TextChanged(this, EventArgs.Empty);
            }
        }
        #endregion

        #region 属性
        /// <summary>
        /// 是否通过正则表达式
        /// </summary>
        [Browsable(false), Description("是否通过正则表达式")]
        public bool IsError
        {
            get
            {
                BaseText_LostFocus(this, EventArgs.Empty);
                return !string.IsNullOrEmpty(error.GetError(this));
            }
        }
        private string _regex;
        /// <summary>
        /// 自定义正则表达式
        /// </summary>
        [Browsable(true), Description("自定义正则表达式"), DefaultValue(null)]
        public string Regex
        {
            get { return _regex; }
            set { _regex = value; }
        }
        private RegexType _regexType;
        /// <summary>
        /// 在控件失去焦点时使用正则表达示验证字符
        /// </summary>
        [Description("在控件失去焦点时使用正则表达示验证字符"), DefaultValue(typeof(RegexType), "None")]
        public RegexType RegexType
        {
            get { return _regexType; }
            set
            {
                _regexType = value;
                if (value == RegexType.Ip)
                {
                    BaseText.Text = "0.0.0.0";
                }
            }
        }
        private bool _isTrans = false;
        /// <summary>
        /// 背景是否透明
        /// </summary>
        [Description("背景是否透明"), DefaultValue(false)]
        public bool IsTrans
        {
            get { return _isTrans; }
            set
            {
                _isTrans = value;
                if (this.Parent != null)
                {
                    BaseText.BackColor = value ? this.Parent.BackColor : Color.White;
                }
                this.Invalidate();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Description("指定可以在编辑控件中输入的最大字符数。"), Category("行为"), DefaultValue(32767)]
        public virtual int MaxLength
        {
            get { return this.BaseText.MaxLength; }
            set { this.BaseText.MaxLength = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Description("控件编辑控件的文本是否能够跨越多行。"), Category("行为"), DefaultValue(false)]
        public virtual bool Multiline
        {
            get { return this.BaseText.Multiline; }
            set { this.BaseText.Multiline = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Description("指示将为单行编辑控件的密码输入显示的字符。"), Category("行为"), DefaultValue(false)]
        public char IsPasswordChat
        {
            get { return this.BaseText.PasswordChar; }
            set { this.BaseText.PasswordChar = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Description("控制能否更改编辑控件中的文本。"), Category("行为"), DefaultValue(false)]
        public virtual bool ReadOnly
        {
            get { return this.BaseText.ReadOnly; }
            set { this.BaseText.ReadOnly = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Description("指示编辑控件中的文本是否以默认的密码字符显示。"), Category("行为"), DefaultValue(false)]
        public virtual bool IsSystemPasswordChar
        {
            get { return this.BaseText.UseSystemPasswordChar; }
            set { this.BaseText.UseSystemPasswordChar = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Description("指示多行编辑控件是否自动换行。"), Category("行为"), DefaultValue(true)]
        public virtual bool WordWrap
        {
            get { return this.BaseText.WordWrap; }
            set { this.BaseText.WordWrap = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Description("用于显示控件中文本的字体。"), Category("外观")]
        new public virtual Font Font
        {
            get { return this.BaseText.Font; }
            set { this.BaseText.Font = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Description("此组件的前景色，用于显示文本。"), Category("外观")]
        new public virtual Color ForeColor
        {
            get { return this.BaseText.ForeColor; }
            set { this.BaseText.ForeColor = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Description("多行编辑中的文本行，作为字符串值的数组。"), Category("外观")]
        public virtual string[] Lines
        {
            get { return this.BaseText.Lines; }
            set { this.BaseText.Lines = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Description("指示对于多行编辑控件，将为此控件显示哪些滚动条。"), Category("外观")]
        [DefaultValue(typeof(ScrollBars), "None")]
        public virtual ScrollBars ScrollBars
        {
            get { return this.BaseText.ScrollBars; }
            set { this.BaseText.ScrollBars = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Description("指示应该如何对齐编辑控件的文本。"), Category("外观")]
        [DefaultValue(typeof(HorizontalAlignment), "Left")]
        public virtual HorizontalAlignment TextAlign
        {
            get { return this.BaseText.TextAlign; }
            set { this.BaseText.TextAlign = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Description("文本框的图标"), Category("自定义属性")]
        public virtual Image Icon
        {
            get { return this._icon; }
            set
            {
                this._icon = value;
                base.Invalidate(this.IconRect);
                this.PositionTextBox();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Description("文本框的图标是否是按钮"), Category("自定义属性"), DefaultValue(false)]
        public virtual bool IconIsButton
        {
            get { return this._iconIsButton; }
            set { this._iconIsButton = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Description("水印文字"), Category("自定义属性")]
        public virtual string WaterText
        {
            get { return this.BaseText.WaterText; }
            set { this.BaseText.WaterText = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Description("水印颜色"), Category("自定义属性")]
        [DefaultValue(typeof(Color), "DarkGray")]
        public virtual Color WaterColor
        {
            get { return this.BaseText.WaterColor; }
            set { this.BaseText.WaterColor = value; }
        }
        /// <summary>
        /// 获取或设置 System.Windows.Forms.TextBox 中的当前文本
        /// </summary>
        [Description("获取或设置 System.Windows.Forms.TextBox 中的当前文本"), Browsable(true)]
        public override string Text
        {
            get { return this.BaseText.Text; }
            set { this.BaseText.Text = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(typeof(Cursor), "IBeam")]
        public override Cursor Cursor
        {
            get { return this._cursor; }
            set { this._cursor = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(typeof(Size), "20, 24")]
        public override Size MinimumSize
        {
            get { return new Size(20, 24); }
            set { base.MinimumSize = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(typeof(TMouseState), "Normal")]
        protected virtual TMouseState MouseState
        {
            get { return this._mouseState; }
            set
            {
                this._mouseState = value;
                base.Invalidate();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(typeof(TMouseState), "Normal")]
        protected virtual TMouseState IconMouseState
        {
            get { return this._iconMouseState; }
            set
            {
                this._iconMouseState = value;
                base.Invalidate();
            }
        }
        /// <summary>
        /// 图标的绘制区域
        /// </summary>
        protected virtual Rectangle IconRect
        {
            get { return new Rectangle(3, 3, 20, 20); }
        }
        /// <summary>
        /// 获取或设置控件的背景色
        /// </summary>
        [Description("获取或设置控件的背景色"), DefaultValue(typeof(Color), "Transparent")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 加载事件
        /// </summary>
        private void InitEvents()
        {
            this.BaseText.MouseMove += new MouseEventHandler(BaseText_MouseMove);
            this.BaseText.MouseLeave += new EventHandler(BaseText_MouseLeave);
            this.BaseText.KeyDown += new KeyEventHandler(BaseText_KeyDown);
            this.BaseText.KeyPress += BaseText_KeyPress;
            this.BaseText.KeyUp += new KeyEventHandler(BaseText_KeyUp);
            this.BaseText.TextChanged += BaseText_TextChanged;
            this.BaseText.LostFocus += BaseText_LostFocus;
        }

        /// <summary>
        /// 设计界面
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QQTextBox));
            this.BaseText = new Paway.Forms.QQTextBoxBase();
            this.error = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.error)).BeginInit();
            this.SuspendLayout();
            // 
            // BaseText
            // 
            this.BaseText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BaseText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.BaseText.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BaseText.Location = new System.Drawing.Point(3, 4);
            this.BaseText.Margin = new System.Windows.Forms.Padding(0);
            this.BaseText.Name = "BaseText";
            this.BaseText.Size = new System.Drawing.Size(172, 16);
            this.BaseText.TabIndex = 0;
            this.BaseText.WaterText = "";
            // 
            // error
            // 
            this.error.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.error.ContainerControl = this;
            this.error.Icon = ((System.Drawing.Icon)(resources.GetObject("error.Icon")));
            // 
            // QQTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.BaseText);
            this.Name = "QQTextBox";
            this.Size = new System.Drawing.Size(178, 24);
            ((System.ComponentModel.ISupportInitialize)(this.error)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        /// <summary>
        /// 偏移文本框
        /// </summary>
        protected virtual void PositionTextBox()
        {
            if (this._icon != null)
            {
                int position = 23;
                this.BaseText.Width -= position;
                this.BaseText.Location = new Point(
                    this.BaseText.Location.X + position,
                    this.BaseText.Location.Y);
            }
        }

        #endregion

        #region 事件
        /// <summary>
        /// 触发 Text 属性值更改时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BaseText_TextChanged(object sender, EventArgs e)
        {
            if (TextChanged != null)
            {
                TextChanged(sender, e);
            }
        }
        /// <summary>
        /// 鼠标移开子TextBox
        /// </summary>
        public void TMouseLeave()
        {
            BaseText_MouseLeave(this, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BaseText_MouseLeave(object sender, EventArgs e)
        {
            this.MouseState = TMouseState.Leave;
        }
        /// <summary>
        /// 鼠标进入子TextBox
        /// </summary>
        public void TMouseEnter()
        {
            BaseText_MouseMove(this, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BaseText_MouseMove(object sender, MouseEventArgs e)
        {
            this.MouseState = TMouseState.Move;
        }

        private void BaseText_KeyUp(object sender, KeyEventArgs e)
        {
            this.OnKeyUp(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseText_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.OnKeyPress(e);
        }
        /// <summary>
        /// 失去焦点验证正则
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BaseText_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(BaseText.Text))
            {
                switch (_regexType)
                {
                    case Helper.RegexType.Ip:
                        error.SetError(this, "请输入Ip");
                        break;
                    case Helper.RegexType.Password:
                        error.SetError(this, "请输入密码");
                        break;
                    case Helper.RegexType.PosInt:
                        error.SetError(this, "请输入一个正整数");
                        break;
                    case Helper.RegexType.Normal:
                    case Helper.RegexType.Custom:
                        error.SetError(this, "请输入字符");
                        break;
                }
                return;
            }
            switch (_regexType)
            {
                case RegexType.Ip:
                case RegexType.Normal:
                case RegexType.Password:
                case RegexType.PosInt:
                    int index = StringHelper.RegexChecked(BaseText.Text, _regexType);
                    if (index != BaseText.Text.Length)
                    {
                        error.SetError(this, string.Format("不可以输入字符:{0}", BaseText.Text[index]));
                    }
                    else
                    {
                        error.SetError(this, null);
                    }
                    break;
                case RegexType.Custom:
                    index = StringHelper.RegexChecked(BaseText.Text, _regex);
                    if (index != BaseText.Text.Length)
                    {
                        error.SetError(this, string.Format("不可以输入字符:{0}", BaseText.Text[index]));
                    }
                    else
                    {
                        error.SetError(this, null);
                    }
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseText_KeyDown(object sender, KeyEventArgs e)
        {
            this.OnKeyDown(e);
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// 当文本框的大小发生改变时，将文本框的类型换成多行文本
        /// </summary>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (this.Height > 26)
                this.BaseText.Multiline = true;
            else
                this.BaseText.Multiline = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_isTrans) return;
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            switch (this._mouseState)
            {
                case TMouseState.Move:
                    using (Image hotLine = AssemblyHelper.GetImage("QQ.TextBox.move.png"))
                    {
                        DrawHelper.RendererBackground(g, this.ClientRectangle, hotLine, true);
                    }
                    break;
                default:
                    DrawHelper.RendererBackground(g, this.ClientRectangle, this._borderImage, true);
                    break;
            }
            if (this._icon != null)
            {
                Rectangle iconRect = this.IconRect;
                if (this._iconMouseState == TMouseState.Down)
                {
                    iconRect.X += 1;
                    iconRect.Y += 1;
                }
                g.DrawImage(this._icon, iconRect, 0, 0, this._icon.Width, this._icon.Height, GraphicsUnit.Pixel);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            this.MouseState = TMouseState.Move;
            if (this._icon != null && this.IconRect.Contains(e.Location))
            {
                if (this._iconIsButton)
                    this.Cursor = Cursors.Hand;
                else
                    this.Cursor = Cursors.Default;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (this._icon != null && this._iconIsButton)
            {
                if (e.Button == MouseButtons.Left && this.IconRect.Contains(e.Location))
                {
                    this.IconMouseState = TMouseState.Down;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (this._icon != null && this._iconIsButton)
            {
                this.IconMouseState = TMouseState.Up;
                if (e.Button == MouseButtons.Left && this.IconRect.Contains(e.Location))
                    this.OnIconClick();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.MouseState = TMouseState.Leave;
            this.Cursor = Cursors.Default;
        }
        #endregion
    }
}
