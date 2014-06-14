using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using Paway.Resource;
using Paway.Helper;
using System.Runtime.InteropServices;

namespace Paway.Forms
{
    /// <summary>
    /// 工具栏
    /// </summary>
    [DefaultProperty("Items")]
    [DefaultEvent("SelectedIndexChanged")]
    public class ToolBar : TControl
    {
        #region 资源图片
        private Image _normalImage = AssemblyHelper.GetImage("_360.ToolBar.toolbar_normal.png");
        private Image _pushedImage = AssemblyHelper.GetImage("_360.ToolBar.toolbar_pushed.png");
        private Image _hoverImage = AssemblyHelper.GetImage("_360.ToolBar.toolbar_hover.png");
        /// <summary>
        /// 多选状态下选中时附加的图片
        /// </summary>
        private Image _selectImage = AssemblyHelper.GetImage("Controls.accept_16.png");

        #endregion

        #region 变量
        /// <summary>
        /// 是否获取了焦点
        /// </summary>
        private bool _iFocus = false;
        /// <summary>
        /// 选项卡箭头区域
        /// </summary>
        private Rectangle _btnArrowRect = Rectangle.Empty;
        /// <summary>
        /// 类于右键菜单长度
        /// </summary>
        private int _rightLen = 19;
        /// <summary>
        /// 字体颜色
        /// </summary>
        private Color _colorFore;

        #endregion

        #region 事件对像
        /// <summary>
        /// 当选中项的索引发生改变时事件的 Key
        /// </summary>
        private static readonly object EventSelectedIndexChanged = new object();
        /// <summary>
        /// 当选中项的发生改变时事件的 Key
        /// </summary>
        private static readonly object EventSelectedItemChanged = new object();
        /// <summary>
        /// 当单击项时事件的 Key
        /// </summary>
        private static readonly object EventItemClick = new object();

        #endregion

        #region 构造函数
        /// <summary>
        /// 初始化 Paway.Forms.ToolBar 新的实例。
        /// </summary>
        public ToolBar()
        {
            this.SetStyle(
               ControlStyles.ResizeRedraw |
               ControlStyles.Selectable |
               ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.Opaque, false);
            this.UpdateStyles();
            InitializeComponent();
            timer.Interval = 30;
            this.timer.Tick += timer_Tick;
        }

        #endregion

        #region 属性
        private StringAlignment _alignment = StringAlignment.Center;
        /// <summary>
        /// Text文本水平位置对齐方式
        /// </summary>
        [Description("文本水平位置对齐方式"), DefaultValue(typeof(StringAlignment), "Center")]
        public StringAlignment TTextAlignment
        {
            get { return _alignment; }
            set
            {
                _alignment = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// 第二行字体
        /// </summary>
        private Font _fontSecond = new System.Drawing.Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)1);
        /// <summary>
        /// 第二行字体
        /// </summary>
        [Description("第二行字体"), DefaultValue(typeof(Font), "宋体, 9pt")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Font FontSecond
        {
            get { return _fontSecond; }
            set { _fontSecond = value; }
        }
        /// <summary>
        /// 尾部描述文字字体
        /// </summary>
        private Font _fontEndDesc = new System.Drawing.Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)1);
        /// <summary>
        /// 尾部描述文字字体
        /// </summary>
        [Description("尾部描述文字字体"), DefaultValue(typeof(Font), "宋体, 9pt")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Font FontEndDesc
        {
            get { return _fontEndDesc; }
            set { _fontEndDesc = value; }
        }
        /// <summary>
        /// 头部描述文字字体
        /// </summary>
        private Font _fontHeadDesc = new System.Drawing.Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)1);
        /// <summary>
        /// 头部描述文字字体
        /// </summary>
        [Description("头部描述文字字体"), DefaultValue(typeof(Font), "宋体, 9pt")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Font FontHeadDesc
        {
            get { return _fontHeadDesc; }
            set { _fontHeadDesc = value; }
        }

        /// <summary>
        /// 默认的颜色
        /// </summary>
        private Color _normalSpace = Color.Transparent;
        /// <summary>
        /// 默认的颜色
        /// </summary>
        [Description("默认的颜色"), DefaultValue(typeof(Color), "Transparent")]
        public Color ColorNormal
        {
            get { return this._normalSpace; }
            set
            {
                _normalSpace = value;
                if (value.A > Trans)
                {
                    _normalSpace = Color.FromArgb(Trans, value.R, value.G, value.B);
                }
                Invalidate(true);
            }
        }
        /// <summary>
        /// 项间隔的颜色
        /// </summary>
        private Color _colorSpace = Color.Transparent;
        /// <summary>
        /// 项间隔的颜色
        /// </summary>
        [Description("项间隔的颜色"), DefaultValue(typeof(Color), "Transparent")]
        public Color ColorSpace
        {
            get { return this._colorSpace; }
            set
            {
                _colorSpace = value;
                if (value.A > Trans)
                {
                    _colorSpace = Color.FromArgb(Trans, value.R, value.G, value.B);
                }
                Invalidate(true);
            }
        }

        /// <summary>
        /// 事件触发点
        /// </summary>
        private TEvent _tEvent = TEvent.Down;
        /// <summary>
        /// 事件触发点
        /// </summary>
        [Description("事件触发点"), DefaultValue(typeof(TEvent), "Down")]
        public TEvent TEvent
        {
            get { return this._tEvent; }
            set
            {
                this._tEvent = value;
            }
        }
        /// <summary>
        /// Item项显示方向
        /// </summary>
        private TDirection _tDirection = TDirection.Level;
        /// <summary>
        /// Item项显示方向
        /// </summary>
        [Description("Item项显示方向"), DefaultValue(typeof(TDirection), "Level")]
        public TDirection TDirection
        {
            get { return this._tDirection; }
            set
            {
                this._tDirection = value;
                base.Invalidate(true);
            }
        }
        /// <summary>
        /// 图片显示位置
        /// </summary>
        private TILocation _tLocation = TILocation.Up;
        /// <summary>
        /// 图片显示位置
        /// </summary>
        [Description("图片显示位置"), DefaultValue(typeof(TILocation), "Up")]
        public TILocation TLocation
        {
            get { return this._tLocation; }
            set
            {
                this._tLocation = value;
                base.Invalidate(true);
            }
        }

        /// <summary>
        /// 是否将颜色应用到文字
        /// </summary>
        private bool _iColorText;
        /// <summary>
        /// 是否将颜色应用到文字
        /// </summary>
        [Description("是否将颜色应用到文字"), DefaultValue(false)]
        public bool IColorText
        {
            get { return _iColorText; }
            set
            {
                _iColorText = value;
                Invalidate(true);
            }
        }
        /// <summary>
        /// 单击事件开关
        /// 单击松开后取消选中状态，只有鼠标移入状态
        /// </summary>
        private bool _iCheckEvent = false;
        /// <summary>
        /// 单击事件开关
        /// 单击松开后取消选中状态，只有鼠标移入状态
        /// </summary>
        [Description("单击事件开关"), DefaultValue(false)]
        public bool ICheckEvent
        {
            get { return this._iCheckEvent; }
            set
            {
                this._iCheckEvent = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// 图片显示开关
        /// </summary>
        private bool _iImageShow = true;
        /// <summary>
        /// 图片显示开关
        /// </summary>
        [Description("图片显示开关"), DefaultValue(true)]
        public bool IImageShow
        {
            get { return this._iImageShow; }
            set
            {
                this._iImageShow = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// 是否允许多选
        /// </summary>
        private bool _isMultiple = false;
        /// <summary>
        /// 是否允许多选
        /// </summary>
        [Description("是否允许多选"), DefaultValue(false)]
        public bool IsMultiple
        {
            get { return this._isMultiple; }
            set
            {
                this._isMultiple = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// 工具栏中的项
        /// </summary>
        private ToolItemCollection _items = null;
        /// <summary>
        /// 工具栏中的项
        /// </summary>
        [Description("工具栏中的项"), EditorBrowsable(EditorBrowsableState.Always), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ToolItemCollection Items
        {
            get
            {
                if (this._items == null)
                    this._items = new ToolItemCollection(this);
                return this._items;
            }
        }
        /// <summary>
        /// 每一项的大小
        /// </summary>
        private Size _itemSize = new Size(74, 82);
        /// <summary>
        /// Item 的大小
        /// </summary>
        [Description("Item 的大小"), DefaultValue(typeof(Size), "74,82")]
        public Size ItemSize
        {
            get { return this._itemSize; }
            set
            {
                this._itemSize = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// 每一项图片显示的大小
        /// </summary>
        private Size _imageSize = new Size(48, 48);
        /// <summary>
        /// Item Image 的大小
        /// </summary>
        [Description("Item Image的大小"), DefaultValue(typeof(Size), "48,48")]
        public Size ImageSize
        {
            get { return this._imageSize; }
            set
            {
                this._imageSize = value;
                switch (_tLocation)
                {
                    case TILocation.Up:
                        this._itemSize = new Size(this._itemSize.Width, 34 + value.Height);
                        break;
                    case TILocation.Left:
                        this._itemSize = new Size(58 + _rightLen + value.Width, this._itemSize.Height);
                        break;
                }
                if (value.Height == 0)
                {
                    this._iImageShow = false;
                }
                this.Invalidate();
            }
        }
        /// <summary>
        /// 项与项之间的间隔
        /// </summary>
        private int _itemSpace = 1;
        /// <summary>
        /// 项与项之间的间隔
        /// </summary>
        [Description("项与项之间的间隔"), DefaultValue(1)]
        public int ItemSpace
        {
            get { return this._itemSpace; }
            set
            {
                this._itemSpace = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// 当前选中项
        /// </summary>
        private ToolItem _selectedItem = null;
        /// <summary>
        /// 当前选中的 Item
        /// </summary>
        [Browsable(false), Description("当前选中的 Item")]
        public ToolItem SelectedItem
        {
            get { return this._selectedItem; }
        }
        /// <summary>
        /// 选中项的索引
        /// </summary>
        private int _selectedIndex = 0;
        /// <summary>
        /// 选中 Item 的索引
        /// </summary>
        [Browsable(false), Description("选中 Item 的索引"), DefaultValue(0)]
        public int SelectedIndex
        {
            get { return this._selectedIndex; }
        }

        /// <summary>
        /// 重写父类的默认大小
        /// </summary>
        protected override Size DefaultSize
        {
            get { return new Size(300, 82); }
        }

        /// <summary>
        /// 多行列排列时的行数
        /// </summary>
        [Browsable(false), Description("多行列排列时的行数"), DefaultValue(1)]
        public int CountLine { get; private set; }
        /// <summary>
        /// 多行列排列时的列数
        /// </summary>
        [Browsable(false), Description("多行列排列时的列数"), DefaultValue(1)]
        public int CountColumn { get; private set; }

        #endregion

        #region 事件
        /// <summary>
        /// 当选中项的索引发生改变时
        /// </summary>
        public event EventHandler SelectedIndexChanged
        {
            add { base.Events.AddHandler(EventSelectedIndexChanged, value); }
            remove { base.Events.RemoveHandler(EventSelectedIndexChanged, value); }
        }
        /// <summary>
        /// 当选中项的发生改变时
        /// </summary>
        public event EventHandler SelectedItemChanged
        {
            add { base.Events.AddHandler(EventSelectedItemChanged, value); }
            remove { base.Events.RemoveHandler(EventSelectedItemChanged, value); }
        }
        /// <summary>
        /// 当单击项时事件发生
        /// </summary>
        public event EventHandler ItemClick
        {
            add { base.Events.AddHandler(EventItemClick, value); }
            remove { base.Events.RemoveHandler(EventItemClick, value); }
        }

        #endregion

        #region Override Methods
        /// <summary>
        /// 引发 System.Windows.Forms.Form.Paint 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.PaintEventArgs。</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            if (!_iColorText)
            {
                g.FillRectangle(new SolidBrush(this.ColorSpace), new Rectangle(-1, -1, this.Width + 1, this.Height + 1));
            }
            int xPos = this.Padding.Left;
            int yPos = this.Padding.Top;
            this.CountColumn = 1;
            this.CountLine = 1;
            for (int i = 0; i < this.Items.Count; i++)
            {
                DrawItem(g, this.Items[i], ref xPos, ref yPos, i == this.Items.Count - 1);
            }
            int count = 0;
            //多行/列补充Item
            if (this.CountColumn != 1 && this.CountLine != 1)
            {
                count = this.CountColumn > this.CountLine ? this.CountColumn : this.CountLine;
                if (this.Items.Count % count == 0) count = 0;
                else count = count - this.Items.Count % count;
            }
            //填充空Item
            for (int i = 0; i < count; i++)
            {
                DrawItem(g, new ToolItem(), ref xPos, ref yPos, i == count - 1);
            }
        }
        #region 绘制方法
        private void DrawItem(Graphics g, ToolItem item, ref int xPos, ref int yPos, bool iLast)
        {
            // 当前 Item 所在的矩型区域
            item.Rectangle = new Rectangle(xPos, yPos, this._itemSize.Width, this._itemSize.Height);
            _colorFore = this.ForeColor;
            DrawBackground(g, item);
            DrawImage(g, item);
            DrawText(g, item, _colorFore);
            switch (_tDirection)
            {
                case TDirection.Level:
                    if (xPos + item.Rectangle.Width * 2 + this._itemSpace > this.Width)
                    {
                        xPos = this.Padding.Left;
                        if (!iLast)
                        {
                            yPos += item.Rectangle.Height + this._itemSpace;
                            this.CountLine++;
                        }
                    }
                    else
                    {
                        xPos += item.Rectangle.Width + this._itemSpace;
                        if (this.CountLine == 1 && !iLast) this.CountColumn++;
                    }
                    break;
                case TDirection.Vertical:
                    if (yPos + item.Rectangle.Height * 2 + this._itemSpace > this.Height)
                    {
                        yPos = this.Padding.Top;
                        if (!iLast)
                        {
                            xPos += item.Rectangle.Width + this._itemSpace;
                            this.CountColumn++;
                        }
                    }
                    else
                    {
                        yPos += item.Rectangle.Height + this._itemSpace;
                        if (this.CountColumn == 1 && !iLast) this.CountLine++;
                    }
                    break;
            }
        }
        /// <summary>
        /// 绘制背景
        /// </summary>
        private void DrawBackground(Graphics g, ToolItem item)
        {
            if (!item.Enable)
            {
                item.MouseState = TMouseState.Normal;
            }
            switch (item.MouseState)
            {
                case TMouseState.Normal:
                case TMouseState.Leave:
                    Color color = item.Color == Color.Empty ? (_iColorText ? this.ForeColor : _normalSpace) : item.Color;
                    if (color.A > Trans)
                    {
                        color = Color.FromArgb(Trans, color.R, color.G, color.B);
                    }
                    if (_iColorText)
                    {
                        _colorFore = color;
                    }
                    else
                    {
                        g.FillRectangle(new SolidBrush(color), item.Rectangle);
                    }
                    break;
                case TMouseState.Move:
                case TMouseState.Up:
                    DrawMoveBack(g, item);
                    break;
                case TMouseState.Down:
                    if (IsContextMenu(g, item))
                    {
                        DrawMoveBack(g, item);
                    }
                    else
                    {
                        if (_iColorText)
                        {
                            _colorFore = this.ColorDownBack;
                        }
                        else if (ColorDownBack == Color.Transparent)
                        {
                            g.DrawImage(this._pushedImage, item.Rectangle);
                        }
                        else
                        {
                            g.FillRectangle(new SolidBrush(this.ColorDownBack), item.Rectangle);
                        }
                        IsContextMenu(g, item);
                    }
                    if (_isMultiple)
                    {
                        g.DrawImage(this._selectImage, new Rectangle(item.Rectangle.Right - this._selectImage.Width, item.Rectangle.Bottom - this._selectImage.Height, this._selectImage.Width, this._selectImage.Height));
                    }
                    break;
            }
        }
        /// <summary>
        /// 绘制鼠标移入时的背景
        /// </summary>
        private void DrawMoveBack(Graphics g, ToolItem item)
        {
            if (_iColorText)
            {
                _colorFore = this.ColorMoveBack;
            }
            else if (ColorMoveBack == Color.Transparent)
            {
                g.DrawImage(this._hoverImage, item.Rectangle);
            }
            else
            {
                g.FillRectangle(new SolidBrush(this.ColorMoveBack), item.Rectangle);
            }
            IsContextMenu(g, item);
        }
        /// <summary>
        /// 绘制图片
        /// </summary>
        private void DrawImage(Graphics g, ToolItem item)
        {
            if (_iImageShow && item.Image != null)
            {
                Rectangle imageRect = new Rectangle();
                switch (_tLocation)
                {
                    case TILocation.Up:
                        imageRect.X = item.Rectangle.X + (item.Rectangle.Width - this._imageSize.Width) / 2;
                        imageRect.Y = 6;
                        break;
                    case TILocation.Left:
                        imageRect.X = item.Rectangle.X + 2;
                        imageRect.Y = item.Rectangle.Y + (item.Rectangle.Height - this._imageSize.Height) / 2;
                        break;
                }
                imageRect.Size = this._imageSize;
                g.DrawImage(item.Image, imageRect);
            }
        }
        /// <summary>
        /// 绘制文字
        /// </summary>
        private void DrawText(Graphics g, ToolItem item, Color color)
        {
            if (!item.Enable)
            {
                _colorFore = this.ForeColor;
            }
            Rectangle textRect = Rectangle.Empty;
            if (!string.IsNullOrEmpty(item.Text))
            {
                StringFormat format = new StringFormat()
                {
                    Alignment = _alignment,
                    LineAlignment = StringAlignment.Center
                };
                int pad = 3;
                textRect = new Rectangle
                {
                    X = item.Rectangle.X + pad,
                    Y = item.Rectangle.Y + pad,
                    Width = item.Rectangle.Width,
                };
                if (!_iImageShow)
                {
                    textRect.Height = item.Rectangle.Height;
                }
                else
                {
                    switch (_tLocation)
                    {
                        case TILocation.Up:
                            textRect.Y = item.Rectangle.Height / 5 * 3;
                            textRect.Height = item.Rectangle.Height / 5 * 2;
                            break;
                        case TILocation.Left:
                            textRect.X = item.Rectangle.X + 2 * 2 + _imageSize.Width;
                            textRect.Height = item.Rectangle.Height;
                            textRect.Width = item.Rectangle.Width + item.Rectangle.X - textRect.X - _rightLen;
                            break;
                    }
                }
                textRect.Width -= pad * 2;
                textRect.Height -= pad * 2;
                string[] text = item.Text.Split(new string[] { "\r\n", "&" }, StringSplitOptions.RemoveEmptyEntries);
                if (text.Length > 0)
                {
                    int tHight = Font.GetHeight(g).ToInt();
                    int sHight = _fontSecond.GetHeight(g).ToInt();
                    int height = textRect.Height - tHight;
                    height -= (text.Length - 1) * sHight;
                    height -= (text.Length - 1) * 6;
                    height /= 2;

                    Rectangle rect = new Rectangle()
                    {
                        X = textRect.X,
                        Y = textRect.Y + height,
                        Width = textRect.Width,
                        Height = tHight,
                    };
                    g.DrawString(text[0], this.Font, new SolidBrush(color), rect, format);
                    for (int i = 1; i < text.Length; i++)
                    {
                        rect = new Rectangle()
                        {
                            X = textRect.X + (tHight - sHight) / 2,
                            Y = textRect.Y + height + (tHight + 6) * i,
                            Width = textRect.Width,
                            Height = sHight,
                        };
                        g.DrawString(text[i], _fontSecond, new SolidBrush(color), rect, format);
                    }
                }
            }
            if (!string.IsNullOrEmpty(item.Desc))
            {
                TextRenderer.DrawText(g, item.Desc, Font, textRect, color, DrawParam.TextRight);
            }
            if (!string.IsNullOrEmpty(item.HeadDesc))
            {
                int dHeight = _fontHeadDesc.GetHeight(g).ToInt() + 6;
                Rectangle descRect = new Rectangle()
                {
                    X = textRect.X,
                    Y = textRect.Y,
                    Width = textRect.Width,
                    Height = dHeight,
                };
                TextRenderer.DrawText(g, item.HeadDesc, _fontHeadDesc, descRect, this.ForeColor, DrawParam.TextLeft);
            }
            if (!string.IsNullOrEmpty(item.EndDesc))
            {
                int dHeight = _fontEndDesc.GetHeight(g).ToInt() + 6;
                Rectangle descRect = new Rectangle()
                {
                    X = textRect.X,
                    Y = textRect.Y + textRect.Height - dHeight,
                    Width = textRect.Width,
                    Height = dHeight,
                };
                TextRenderer.DrawText(g, item.EndDesc, _fontEndDesc, descRect, this.ForeColor, DrawParam.TextRight);
            }
        }
        /// <summary>
        /// 判断右键菜单
        /// </summary>
        /// <returns>返回是否有焦点 true时不触发down事件</returns>
        private bool IsContextMenu(Graphics g, ToolItem item)
        {
            Point cursorPoint = this.PointToClient(MousePosition);
            Image btnArrowImage = null;
            Point contextMenuLocation = this.PointToScreen(new Point(this._btnArrowRect.Left, this._btnArrowRect.Top + this._btnArrowRect.Height + 2));
            ContextMenuStrip contextMenuStrip = item.ContextMenuStrip;
            if (contextMenuStrip != null)
            {
                contextMenuStrip.Tag = item;
                contextMenuStrip.Closed -= contextMenuStrip_Closed;
                contextMenuStrip.Closed += contextMenuStrip_Closed;
                if (contextMenuLocation.X + contextMenuStrip.Width > Screen.PrimaryScreen.WorkingArea.Width - 20)
                {
                    contextMenuLocation.X = Screen.PrimaryScreen.WorkingArea.Width - contextMenuStrip.Width - 50;
                }
                if (item.Rectangle.Contains(cursorPoint))
                {
                    if (this._iFocus)
                    {
                        btnArrowImage = AssemblyHelper.GetImage("QQ.TabControl.main_tabbtn_down.png");
                        contextMenuStrip.Show(contextMenuLocation);
                    }
                    else
                    {
                        btnArrowImage = AssemblyHelper.GetImage("QQ.TabControl.main_tabbtn_highlight.png");
                    }
                    this._btnArrowRect = new Rectangle(item.Rectangle.X + item.Rectangle.Width - btnArrowImage.Width, item.Rectangle.Y, btnArrowImage.Width, btnArrowImage.Height);
                }
            }
            if (btnArrowImage != null)
            {
                //当鼠标进入当前选中的的选项卡时，显示下拉按钮
                g.DrawImage(btnArrowImage, this._btnArrowRect);
            }
            return _iFocus;
        }
        /// <summary>
        /// 右键菜单关闭刷新项
        /// </summary>
        void contextMenuStrip_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            this._iFocus = false;
            ToolItem item = (sender as ContextMenuStrip).Tag as ToolItem;
            item.MouseState = TMouseState.Leave;
            this.Invalidate(item.Rectangle);
        }

        #endregion

        /// <summary>
        /// 引发 System.Windows.Forms.Form.MouseMove 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!this.DesignMode)
            {
                Point point = e.Location;
                foreach (ToolItem item in this.Items)
                {
                    if (!_iCheckEvent && item.MouseState == TMouseState.Down)
                    {
                        continue;
                    }
                    else if (item.Rectangle.Contains(point))
                    {
                        item.MouseState = TMouseState.Move;
                        this.Invalidate(item.Rectangle);
                    }
                    else
                    {
                        item.MouseState = TMouseState.Leave;
                        this.Invalidate(item.Rectangle);
                    }
                }
                this.Invalidate();
            }
        }
        /// <summary>
        /// 引发 System.Windows.Forms.Form.MouseLeave 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_iFocus) return;

            if (!this.DesignMode)
            {
                foreach (ToolItem item in this.Items)
                {
                    if ((_iCheckEvent && !_isMultiple) || item.MouseState != TMouseState.Down)
                    {
                        item.MouseState = TMouseState.Leave;
                        this.Invalidate(item.Rectangle);
                    }
                }
            }
        }
        /// <summary>
        /// 引发 System.Windows.Forms.Form.MouseDown 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left) return;
            if (!this.DesignMode)
            {
                Point point = e.Location;
                if (this._btnArrowRect.Contains(point))
                {
                    this._iFocus = true;
                    base.Invalidate(this._btnArrowRect);
                }
                bool iIn = Contain(point);
                for (int i = 0; i < this.Items.Count; i++)
                {
                    ToolItem item = this.Items[i];
                    if (item.Rectangle.Contains(point))
                    {
                        if (item != this.SelectedItem)
                        {
                            this._selectedItem = item;
                            if (_tEvent == TEvent.Down)
                            {
                                this._selectedIndex = this.Items.GetIndexOfRange(item);
                                this.OnSelectedItemChanged(item, EventArgs.Empty);
                                this.OnSelectedIndexChanged(item, EventArgs.Empty);
                            }
                        }
                        if (_tEvent == TEvent.Down)
                        {
                            this.OnItemClick(item, EventArgs.Empty);
                        }
                        if (_isMultiple)
                        {
                            if (item.MouseState != TMouseState.Down)
                            {
                                item.MouseState = TMouseState.Down;
                            }
                            else
                            {
                                item.MouseState = TMouseState.Normal;
                            }
                        }
                        else
                        {
                            item.MouseState = TMouseState.Down;
                        }
                    }
                    else if (!_isMultiple && iIn)
                    {
                        item.MouseState = TMouseState.Normal;
                        this.Invalidate(item.Rectangle);
                    }
                }
                this.Invalidate();
            }
        }
        /// <summary>
        /// 引发 System.Windows.Forms.Form.MouseUp 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button != MouseButtons.Left) return;
            if (_iFocus) return;

            if (!this.DesignMode)
            {
                Point point = e.Location;
                for (int i = 0; i < this.Items.Count; i++)
                {
                    ToolItem item = this.Items[i];
                    if (item.Rectangle.Contains(point))
                    {
                        if (item == this.SelectedItem)
                        {
                            if (_tEvent == TEvent.Up)
                            {
                                this._selectedIndex = this.Items.GetIndexOfRange(item);
                                this.OnSelectedItemChanged(item, EventArgs.Empty);
                                this.OnSelectedIndexChanged(item, EventArgs.Empty);
                                this.OnItemClick(item, EventArgs.Empty);
                            }
                        }
                        if (item != this.SelectedItem)
                        {
                            item.MouseState = TMouseState.Up;
                            this.Invalidate(item.Rectangle);
                        }
                    }
                }
            }
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// 获取所有选中项
        /// </summary>
        /// <returns></returns>
        public List<ToolItem> GetAllItems()
        {
            List<ToolItem> iList = new List<ToolItem>();
            for (int i = 0; i < this._items.Count; i++)
            {
                if (_items[i].MouseState == TMouseState.Down)
                {
                    iList.Add(_items[i]);
                }
            }
            return iList;
        }
        /// <summary>
        /// 坐标点是否包含在项中
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override bool Contain(Point point)
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (this.Items[i].Rectangle.Contains(point))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 选中第一项
        /// </summary>
        public void TFirstItem()
        {
            TClickItem(0);
        }
        /// <summary>
        /// 选中第index项
        /// </summary>
        public void TClickItem(int index)
        {
            if (this._items.Count == 0) return;
            if (this._items.Count <= index) return;
            if (!_isMultiple)
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    if (i == index) continue;
                    _items[i].MouseState = TMouseState.Normal;
                }
            }
            this._selectedItem = this._items[index];
            if (!_iCheckEvent)
            {
                this._selectedItem.MouseState = TMouseState.Down;
                OnSelectedItemChanged(_items[index], EventArgs.Empty);
            }
            else
            {
                OnItemClick(_items[index], EventArgs.Empty);
            }
            this.Invalidate();
        }
        #endregion

        #region 激发事件的方法
        /// <summary>
        /// 当选择的 Item 发生改变时激发。
        /// </summary>
        /// <param name="item"></param>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        public virtual void OnSelectedItemChanged(ToolItem item, EventArgs e)
        {
            if (!item.Enable) return;
            EventHandler handler = base.Events[EventSelectedItemChanged] as EventHandler;
            if (handler != null)
                handler(this, e);
        }
        /// <summary>
        /// 当选择的 Item 索引发生改变时激发。
        /// </summary>
        /// <param name="item"></param>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        public virtual void OnSelectedIndexChanged(ToolItem item, EventArgs e)
        {
            if (!item.Enable) return;
            EventHandler handler = base.Events[EventSelectedIndexChanged] as EventHandler;
            if (handler != null)
                handler(this, e);
        }
        /// <summary>
        /// 当单击项时激发。
        /// </summary>
        /// <param name="item"></param>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        public virtual void OnItemClick(ToolItem item, EventArgs e)
        {
            if (!item.Enable) return;
            if (_iCheckEvent)
            {
                EventHandler handler = base.Events[EventItemClick] as EventHandler;
                if (handler != null)
                    handler(this, e);
            }
        }

        #endregion

        #region 扩展方法 - 动态显示项的图像
        private Timer timer = new Timer();
        private PictureBox pictureBox1;
        private Image image;
        /// <summary>
        /// 动态图片列
        /// </summary>
        private int pIndex;
        /// <summary>
        /// 动态显示的图片
        /// </summary>
        [Description("动态显示的图片"), DefaultValue(null)]
        public Image ProgressImage
        {
            get { return pictureBox1.Image; }
            set { pictureBox1.Image = value; }
        }
        /// <summary>
        /// 动态显示项的图像
        /// </summary>
        /// <param name="text">项文本</param>
        /// <param name="newText">项新文本</param>
        public void ProgressStart(string text, string newText = null)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Text == text)
                {
                    this.pIndex = i;
                    break;
                }
            }
            ProgressStart(this.pIndex, newText);
        }
        /// <summary>
        /// 动态显示项的图像
        /// </summary>
        /// <param name="index">项索引</param>
        /// <param name="text">项文本</param>
        public void ProgressStart(int index, string text = null)
        {
            timer.Enabled = true;
            this.pIndex = index;
            if (!string.IsNullOrEmpty(text)) this._items[pIndex].Text = text;
            image = this._items[pIndex].Image;
            this._items[pIndex].Image = pictureBox1.Image;
        }
        /// <summary>
        /// 停止动态显示
        /// </summary>
        /// <param name="image">图片</param>
        /// <param name="text">项文本</param>
        public void ProgressStop(Image image = null, string text = null)
        {
            timer.Enabled = false;
            if (!string.IsNullOrEmpty(text)) this._items[pIndex].Text = text;
            this._items[pIndex].Image = image ?? this.image;
            timer_Tick(this, EventArgs.Empty);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.Invalidate(this._items[pIndex].Rectangle);
        }
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1, 1);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // ToolBar
            // 
            this.Controls.Add(this.pictureBox1);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
    }
}
