using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Paway.Helper;
using Paway.Resource;

namespace Paway.Forms
{
    /// <summary>
    /// </summary>
    public class QQTextBox : TControl
    {
        #region 变量

        private readonly Image _borderImage = AssemblyHelper.GetImage("QQ.TextBox.normal.png");
        private readonly Image _moveImage = AssemblyHelper.GetImage("QQ.TextBox.move.png");
        private Cursor _cursor = Cursors.IBeam;
        private TMouseState _mouseState = TMouseState.Normal;
        private TMouseState _iconMouseState = TMouseState.Normal;
        private bool _iconIsButton;
        private ErrorProvider _error;
        private Image _icon;

        #endregion

        #region 重载属性

        /// <summary>
        ///     BaseText
        /// </summary>
        protected QQTextBoxBase BaseText;

        /// <summary>
        ///     BaseText
        /// </summary>
        [Browsable(false), Category("Properties")]
        public QQTextBoxBase Edit
        {
            get { return BaseText; }
        }

        /// <summary>
        ///     TNum.获取或设置文本框中选定的文本起始点
        /// </summary>
        [Description("获取或设置文本框中选定的文本起始点"), DefaultValue(0)]
        public int SelectionStart
        {
            get { return BaseText.SelectionStart; }
            set { BaseText.SelectionStart = value; }
        }

        /// <summary>
        ///     TNum.获取或设置文本框中选定的字符数
        /// </summary>
        [Description("获取或设置文本框中选定的字符数"), DefaultValue(0)]
        public int SelectionLength
        {
            get { return BaseText.SelectionLength; }
            set { BaseText.SelectionLength = value; }
        }

        /// <summary>
        ///     TNum.获取或设置一个值，该值指示控件中当前选定的文本
        /// </summary>
        [Browsable(false), Description("获取或设置一个值，该值指示控件中当前选定的文本"), DefaultValue("")]
        public string SelectedText
        {
            get { return BaseText.SelectedText; }
            set { BaseText.SelectedText = value; }
        }

        /// <summary>
        ///     获取控件中文本的长度
        /// </summary>
        [Browsable(false), Description("获取控件中文本的长度")]
        public int TextLength
        {
            get { return BaseText.TextLength; }
        }

        /// <summary>
        ///     指定可以在编辑控件中输入的最大字符数
        /// </summary>
        [Description("指定可以在编辑控件中输入的最大字符数。"), Category("行为"), DefaultValue(32767)]
        public virtual int MaxLength
        {
            get { return BaseText.MaxLength; }
            set { BaseText.MaxLength = value; }
        }

        /// <summary>
        ///     控件编辑控件的文本是否能够跨越多行
        /// </summary>
        [Description("控件编辑控件的文本是否能够跨越多行。"), Category("行为"), DefaultValue(false)]
        public virtual bool Multiline
        {
            get { return BaseText.Multiline; }
            set { BaseText.Multiline = value; }
        }

        /// <summary>
        ///     指示将为单行编辑控件的密码输入显示的字符
        /// </summary>
        [Description("指示将为单行编辑控件的密码输入显示的字符。"), Category("行为"), DefaultValue('\0')]
        public char IsPasswordChat
        {
            get { return BaseText.PasswordChar; }
            set { BaseText.PasswordChar = value; }
        }

        /// <summary>
        ///     控制能否更改编辑控件中的文本
        /// </summary>
        [Description("控制能否更改编辑控件中的文本。"), Category("行为"), DefaultValue(false)]
        public virtual bool ReadOnly
        {
            get { return BaseText.ReadOnly; }
            set { BaseText.ReadOnly = value; }
        }

        /// <summary>
        ///     指示编辑控件中的文本是否以默认的密码字符显示
        /// </summary>
        [Description("指示编辑控件中的文本是否以默认的密码字符显示。"), Category("行为"), DefaultValue(false)]
        public virtual bool IsSystemPasswordChar
        {
            get { return BaseText.UseSystemPasswordChar; }
            set { BaseText.UseSystemPasswordChar = value; }
        }

        /// <summary>
        ///     指示多行编辑控件是否自动换行
        /// </summary>
        [Description("指示多行编辑控件是否自动换行。"), Category("行为"), DefaultValue(true)]
        public virtual bool WordWrap
        {
            get { return BaseText.WordWrap; }
            set { BaseText.WordWrap = value; }
        }

        /// <summary>
        ///     多行编辑中的文本行，作为字符串值的数组
        /// </summary>
        [Browsable(false), Description("多行编辑中的文本行，作为字符串值的数组。"), Category("外观")]
        public virtual string[] Lines
        {
            get { return BaseText.Lines; }
            set { BaseText.Lines = value; }
        }

        /// <summary>
        ///     指示对于多行编辑控件，将为此控件显示哪些滚动条
        /// </summary>
        [Description("指示对于多行编辑控件，将为此控件显示哪些滚动条。"), Category("外观")]
        [DefaultValue(typeof(ScrollBars), "None")]
        public virtual ScrollBars ScrollBars
        {
            get { return BaseText.ScrollBars; }
            set { BaseText.ScrollBars = value; }
        }

        /// <summary>
        ///     指示应该如何对齐编辑控件的文本
        /// </summary>
        [Description("指示应该如何对齐编辑控件的文本。"), Category("外观")]
        [DefaultValue(typeof(HorizontalAlignment), "Left")]
        public virtual HorizontalAlignment TextAlign
        {
            get { return BaseText.TextAlign; }
            set { BaseText.TextAlign = value; }
        }

        /// <summary>
        ///     获取或设置 System.Windows.Forms.TextBox 中的当前文本
        /// </summary>
        [Description("获取或设置 System.Windows.Forms.TextBox 中的当前文本"), Browsable(true)]
        public override string Text
        {
            get { return BaseText.Text; }
            set { BaseText.Text = value; }
        }

        /// <summary>
        /// </summary>
        [DefaultValue(typeof(Cursor), "IBeam")]
        public override Cursor Cursor
        {
            get { return _cursor; }
            set { _cursor = value; }
        }

        /// <summary>
        /// </summary>
        [DefaultValue(typeof(Size), "20,24")]
        public override Size MinimumSize
        {
            get { return new Size(20, 24); }
            set { base.MinimumSize = value; }
        }

        /// <summary>
        ///     获取或设置控件显示的文字的字体
        /// </summary>
        [Description("获取或设置控件显示的文字的字体"), DefaultValue(typeof(Font), "微软雅黑, 9pt")]
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                if (BaseText == null) return;
                if (value == null)
                {
                    value = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 1);
                }
                base.Font = value;
                BaseText.Font = value;
                UpdateHeight();
                Invalidate();
            }
        }

        /// <summary>
        ///     自动更新高度
        /// </summary>
        private void UpdateHeight()
        {
            if (!Multiline)
            {
                var hight = TextRenderer.MeasureText("你好", Font).Height;
                Height = hight + 8;
            }
        }

        /// <summary>
        ///     此组件的前景色，用于显示文本
        /// </summary>
        [Description("此组件的前景色，用于显示文本。"), Category("外观"), DefaultValue(typeof(Color), "Black")]
        public override Color ForeColor
        {
            get { return BaseText.ForeColor; }
            set
            {
                if (value == Color.Empty)
                {
                    value = Color.Black;
                }
                if (BaseText == null) return;
                BaseText.ForeColor = value;
            }
        }

        #endregion

        #region 重载属性默认值

        /// <summary>
        ///     获取或设置控件的高度和宽度
        /// </summary>
        [Description("获取或设置控件的高度和宽度"), DefaultValue(typeof(Size), "166,24")]
        public new Size Size
        {
            get { return base.Size; }
            set
            {
                if (value == Size.Empty)
                {
                    value = new Size(166, 24);
                }
                base.Size = value;
                var width = TextRenderer.MeasureText("你好", Font).Width;
                BaseText.Size = new Size(value.Width - width / 4, value.Height - 8);
                UpdateHeight();
                _iconRect.Y = (Height - _iconRect.Height) / 2;
                PositionTextBox();
                Invalidate();
            }
        }

        /// <summary>
        ///     获取或设置控件内的空白。
        /// </summary>
        [Description("获取或设置控件内的空白"), DefaultValue(typeof(Padding), "0,0,0,3")]
        public new Padding Padding
        {
            get { return base.Padding; }
            set { base.Padding = value; }
        }

        #endregion

        #region 新属性

        /// <summary>
        ///     是否通过正则表达式
        /// </summary>
        [Browsable(false), Description("是否通过正则表达式")]
        public bool IError
        {
            get
            {
                BaseText_LostFocus(this, EventArgs.Empty);
                bool result = !string.IsNullOrEmpty(_error.GetError(this));
                if (result) this.Focus();
                return result;
            }
        }

        private string _regex;

        /// <summary>
        ///     自定义正则表达式
        /// </summary>
        [Browsable(true), Description("自定义正则表达式"), DefaultValue(null)]
        public virtual string Regex
        {
            get { return _regex; }
            set { _regex = value; }
        }

        private RegexType _regexType;
        private IContainer components;

        /// <summary>
        ///     在控件失去焦点时使用正则表达示验证字符
        /// </summary>
        [Description("在控件失去焦点时使用正则表达示验证字符"), DefaultValue(typeof(RegexType), "None")]
        public virtual RegexType RegexType
        {
            get { return _regexType; }
            set
            {
                _regexType = value;
                if (!string.IsNullOrEmpty(BaseText.Text)) return;
                switch (value)
                {
                    case RegexType.Ip:
                        BaseText.Text = "0.0.0.0";
                        break;
                    case RegexType.PosInt:
                        BaseText.Text = "0";
                        break;
                    case RegexType.Number:
                        BaseText.Text = "0.0";
                        break;
                }
            }
        }

        /// <summary>
        ///     最小输入字符数
        /// </summary>
        [Description("最小输入字符数"), DefaultValue(0)]
        public int RLength { get; set; }

        private bool _isBorder = true;
        /// <summary>
        ///     是否显示边框
        /// </summary>
        [Description("是否显示边框"), DefaultValue(true)]
        public bool IBorder
        {
            get { return _isBorder; }
            set
            {
                _isBorder = value;
                Invalidate();
            }
        }

        private bool _isTrans;
        /// <summary>
        ///     背景是否透明
        /// </summary>
        [Description("背景是否透明"), DefaultValue(false)]
        public bool ITrans
        {
            get { return _isTrans; }
            set
            {
                _isTrans = value;
                if (Parent != null)
                    BaseText.BackColor = value ? Parent.BackColor : BackColor;
                BackColor = value ? Color.Transparent : BackColor;
                Invalidate();
            }
        }

        /// <summary>
        ///     文本框的图标
        /// </summary>
        [Description("文本框的图标"), Category("自定义属性"), DefaultValue(null)]
        public virtual Image Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                Invalidate(IconRect);
                PositionTextBox();
            }
        }

        /// <summary>
        ///     文本框的图标是否是按钮
        /// </summary>
        [Description("文本框的图标是否是按钮"), Category("自定义属性"), DefaultValue(false)]
        public virtual bool IconIsButton
        {
            get { return _iconIsButton; }
            set { _iconIsButton = value; }
        }

        /// <summary>
        /// </summary>
        [Editor(
            "System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
            typeof(UITypeEditor))]
        [Description("水印文字"), Category("自定义属性"), DefaultValue(null)]
        public virtual string WaterText
        {
            get { return BaseText.WaterText; }
            set { BaseText.WaterText = value; }
        }

        /// <summary>
        /// </summary>
        [Description("水印颜色"), Category("自定义属性"), DefaultValue(typeof(Color), "DarkGray")]
        public virtual Color WaterColor
        {
            get { return BaseText.WaterColor; }
            set { BaseText.WaterColor = value; }
        }

        /// <summary>
        /// </summary>
        [DefaultValue(typeof(TMouseState), "Normal")]
        protected virtual TMouseState MouseState
        {
            get { return _mouseState; }
            set
            {
                if (_mouseState == value) return;
                _mouseState = value;
                Invalidate();
            }
        }

        /// <summary>
        /// </summary>
        [DefaultValue(typeof(TMouseState), "Normal")]
        protected virtual TMouseState IconMouseState
        {
            get { return _iconMouseState; }
            set
            {
                if (_iconMouseState == value) return;
                _iconMouseState = value;
                Invalidate();
            }
        }

        private Rectangle _iconRect = new Rectangle(3, 3, 20, 20);

        /// <summary>
        ///     图标的绘制区域
        /// </summary>
        protected virtual Rectangle IconRect
        {
            get { return _iconRect; }
            set { _iconRect = value; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// </summary>
        public QQTextBox()
        {
            InitializeComponent();
            InitEvents();
            Padding = new Padding(0, 0, 0, 3);
            TConfig.Init(this);
        }

        /// <summary>
        ///     设计界面
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QQTextBox));
            this._error = new System.Windows.Forms.ErrorProvider(this.components);
            this.BaseText = new Paway.Forms.QQTextBoxBase();
            ((System.ComponentModel.ISupportInitialize)(this._error)).BeginInit();
            this.SuspendLayout();
            // 
            // error
            // 
            this._error.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this._error.ContainerControl = this;
            this._error.Icon = ((System.Drawing.Icon)(resources.GetObject("error.Icon")));
            // 
            // BaseText
            // 
            this.BaseText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BaseText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.BaseText.Location = new System.Drawing.Point(3, 4);
            this.BaseText.Name = "BaseText";
            this.BaseText.Size = new System.Drawing.Size(100, 16);
            this.BaseText.TabIndex = 0;
            // 
            // QQTextBox
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.BaseText);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.Name = "QQTextBox";
            this.Size = new System.Drawing.Size(166, 24);
            ((System.ComponentModel.ISupportInitialize)(this._error)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region 外部方法

        /// <summary>
        /// </summary>
        public event EventHandler IconClick;
        /// <summary>
        /// 处理窗口消息事件
        /// </summary>
        public event EventHandler<CmdKeyEventArgs> CmdKeyEvent;

        /// <summary>
        /// </summary>
        private void OnIconClick()
        {
            if (IconClick != null)
                IconClick(this, EventArgs.Empty);
        }

        /// <summary>
        ///     在 Text 属性值更改时发生
        /// </summary>
        public new event EventHandler TextChanged;

        private void OnTextChang()
        {
            if (TextChanged != null)
            {
                TextChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     偏移文本框
        /// </summary>
        protected virtual void PositionTextBox()
        {
            if (_icon != null)
            {
                var position = 23;
                BaseText.Width -= position;
                BaseText.Location = new Point(
                    position + 3,
                    BaseText.Location.Y);
            }
        }

        /// <summary>
        ///     鼠标进入子TextBox
        /// </summary>
        public void TMouseEnter()
        {
            BaseText_MouseMove(this, null);
        }

        /// <summary>
        ///     鼠标移开子TextBox
        /// </summary>
        public void TMouseLeave()
        {
            BaseText_MouseLeave(this, null);
        }

        #endregion

        #region 事件方法

        /// <summary>
        ///     加载事件
        /// </summary>
        private void InitEvents()
        {
            BaseText.MouseMove += BaseText_MouseMove;
            BaseText.MouseLeave += BaseText_MouseLeave;
            BaseText.KeyDown += BaseText_KeyDown;
            BaseText.KeyPress += BaseText_KeyPress;
            BaseText.KeyUp += BaseText_KeyUp;
            BaseText.TextChanged += BaseText_TextChanged;
            BaseText.LostFocus += BaseText_LostFocus;
            BaseText.GotFocus += BaseText_GotFocus;
            BaseText.MouseEnter += BaseText_MouseEnter;
            KeyDown += QQTextBox_KeyDown;
        }

        private void BaseText_MouseMove(object sender, MouseEventArgs e)
        {
            MouseState = TMouseState.Move;
        }

        private void BaseText_MouseLeave(object sender, EventArgs e)
        {
            MouseState = TMouseState.Leave;
            if (string.IsNullOrEmpty(BaseText.Text))
            {
                //this.ParentForm.SelectNextControl(this.ParentForm, true, false, false, true);
            }
        }

        private void BaseText_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e);
        }

        private void BaseText_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnKeyPress(e);
        }

        private void BaseText_KeyUp(object sender, KeyEventArgs e)
        {
            OnKeyUp(e);
        }

        /// <summary>
        ///     触发 Text 属性值更改时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseText_TextChanged(object sender, EventArgs e)
        {
            if (TextChanged != null)
            {
                TextChanged(sender, e);
            }
        }

        /// <summary>
        ///     获得焦点时清空错误提示
        /// </summary>
        private void BaseText_GotFocus(object sender, EventArgs e)
        {
            Reset();
        }

        /// <summary>
        ///     失去焦点验证正则
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseText_LostFocus(object sender, EventArgs e)
        {
            string result = null;
            if (BaseText.TextLength < RLength)
            {
                Reset(string.Format("请输入不少于{0}位字符", RLength));
                return;
            }
            if (string.IsNullOrEmpty(BaseText.Text)) return;
            switch (_regexType)
            {
                case RegexType.Ip:
                case RegexType.Normal:
                case RegexType.Password:
                case RegexType.PosInt:
                case RegexType.Number:
                    result = StringHelper.RegexChecked(BaseText.Text, _regexType);
                    break;
                case RegexType.Custom:
                    result = StringHelper.RegexChecked(BaseText.Text, _regex);
                    break;
            }
            if (!string.IsNullOrEmpty(result))
            {
                Reset(string.Format("不可以输入字符:{0}", result));
            }
            else
            {
                Reset();
            }
        }

        /// <summary>
        ///     清除错误描述
        /// </summary>
        public void Reset()
        {
            _error.SetError(this, null);
        }

        /// <summary>
        ///     设置错误描述
        /// </summary>
        public void Reset(string er)
        {
            _error.SetError(this, er);
        }

        /// <summary>
        ///     鼠标进入控件激活
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseText_MouseEnter(object sender, EventArgs e)
        {
            if (ParentForm.ContainsFocus)
            {
                //this.Focus();
            }
        }

        private void QQTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && ModifierKeys == Keys.Control)
            {
                BaseText.SelectAll();
            }
        }
        /// <summary>
        /// 监听键盘事件
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (CmdKeyEvent != null)
            {
                CmdKeyEventArgs eve = new CmdKeyEventArgs(msg, keyData);
                CmdKeyEvent(this, eve);
                if (eve.Result)
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        #region Override Methods

        /// <summary>
        ///     控件背景
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ITrans = ITrans;
        }

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            switch (_mouseState)
            {
                case TMouseState.Move:
                    if (!_isTrans)
                    {
                        DrawHelper.RendererBackground(g, ClientRectangle, _moveImage, true);
                    }
                    break;
                default:
                    if (!_isTrans && _isBorder)
                    {
                        DrawHelper.RendererBackground(g, ClientRectangle, _borderImage, true);
                    }
                    break;
            }
            if (_icon != null)
            {
                var iconRect = IconRect;
                if (_iconMouseState == TMouseState.Down)
                {
                    iconRect.X += 1;
                    iconRect.Y += 1;
                }
                g.DrawImage(_icon, iconRect, 0, 0, _icon.Width, _icon.Height, GraphicsUnit.Pixel);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            MouseState = TMouseState.Move;
            if (_icon != null && IconRect.Contains(e.Location))
            {
                if (_iconIsButton)
                    Cursor = Cursors.Hand;
                else
                    Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (_icon != null && _iconIsButton)
            {
                if (e.Button == MouseButtons.Left && IconRect.Contains(e.Location))
                {
                    IconMouseState = TMouseState.Down;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (_icon != null && _iconIsButton)
            {
                IconMouseState = TMouseState.Up;
                if (e.Button == MouseButtons.Left && IconRect.Contains(e.Location))
                    OnIconClick();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            MouseState = TMouseState.Leave;
            Cursor = Cursors.Default;
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
                if (_borderImage != null)
                    _borderImage.Dispose();
                if (_moveImage != null)
                    _moveImage.Dispose();
                if (_error != null)
                    _error.Dispose();
                _icon = null;
                if (BaseText != null)
                    BaseText.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}