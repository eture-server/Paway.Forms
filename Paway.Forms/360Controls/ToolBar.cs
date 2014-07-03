﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using Paway.Resource;
using Paway.Helper;
using System.Runtime.InteropServices;
using System.Drawing.Text;

namespace Paway.Forms
{
    /// <summary>
    /// 工具栏
    /// </summary>
    [DefaultProperty("Items")]
    [DefaultEvent("SelectedItemChanged")]
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
        /// 鼠标弹按下
        /// </summary>
        private bool _iDown = false;
        /// <summary>
        /// 右键弹出
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

        #endregion

        #region 构造函数
        /// <summary>
        /// 初始化 Paway.Forms.ToolBar 新的实例。
        /// </summary>
        public ToolBar()
        {
            this.SetStyle(
               ControlStyles.ResizeRedraw |
               ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.Opaque, false);
            this.UpdateStyles();
            InitializeComponent();
            Progress();
            CustomScroll();
        }

        #endregion

        #region 属性
        private TProperties _text;
        /// <summary>
        /// 文字
        /// </summary>
        [DefaultValue(typeof(TProperties), "TextFirst")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TProperties TextFirst
        {
            get
            {
                if (_text == null)
                    _text = new TProperties();
                return _text;
            }
        }
        private TProperties _textSencond;
        /// <summary>
        /// 文字
        /// </summary>
        [DefaultValue(typeof(TProperties), "TextSencond")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TProperties TextSencond
        {
            get
            {
                if (_textSencond == null)
                    _textSencond = new TProperties();
                return _textSencond;
            }
        }
        private TProperties _desc;
        /// <summary>
        /// 正文描述
        /// </summary>
        [DefaultValue(typeof(TProperties), "Desc")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TProperties TDesc
        {
            get
            {
                if (_desc == null)
                    _desc = new TProperties();
                return _desc;
            }
        }
        private TProperties _headDesc;
        /// <summary>
        /// 头部描述
        /// </summary>
        [DefaultValue(typeof(TProperties), "HeadDesc")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TProperties THeadDesc
        {
            get
            {
                if (_headDesc == null)
                    _headDesc = new TProperties();
                return _headDesc;
            }
        }
        private TProperties _endDesc;
        /// <summary>
        /// 尾部描述
        /// </summary>
        [DefaultValue(typeof(TProperties), "EndDesc")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TProperties TEndDesc
        {
            get
            {
                if (_endDesc == null)
                    _endDesc = new TProperties();
                return _endDesc;
            }
        }
        private TProperties _backGround;
        /// <summary>
        /// 背景
        /// </summary>
        [DefaultValue(typeof(TProperties), "BackGround")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TProperties TBackGround
        {
            get
            {
                if (_backGround == null)
                    _backGround = new TProperties();
                return _backGround;
            }
        }
        private Color _backColor;
        /// <summary>
        /// 绘制背景时判定颜色透明度
        /// </summary>
        private Color backColor
        {
            get
            {
                if (_backColor.A > Trans)
                {
                    _backColor = Color.FromArgb(Trans, _backColor.R, _backColor.G, _backColor.B);
                }
                return _backColor;
            }
            set { _backColor = value; }
        }

        private bool _tAdd;
        /// <summary>
        /// 补充整行\列
        /// </summary>
        [Description("补充整行\\列"), DefaultValue(false)]
        public bool TAdd
        {
            get { return _tAdd; }
            set
            {
                _tAdd = value;
                this.Invalidate(this.ClientRectangle);
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
                TPaint(null);
                UpdateScroll();
                BodyBounds.X = 0; BodyBounds.Y = 0;
                this.Invalidate(this.ClientRectangle);
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
                this.Invalidate(this.ClientRectangle);
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
            set { this._iCheckEvent = value; }
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
                this.Invalidate(this.ClientRectangle);
            }
        }
        /// <summary>
        /// 多选开关
        /// </summary>
        private bool _iMultiple = false;
        /// <summary>
        /// 多选开关
        /// </summary>
        [Description("多选开关"), DefaultValue(false)]
        public bool IMultiple
        {
            get { return this._iMultiple; }
            set
            {
                this._iMultiple = value;
                this.Invalidate(this.ClientRectangle);
            }
        }
        /// <summary>
        /// 工具栏中的项
        /// </summary>
        private ToolItemCollection _items;
        /// <summary>
        /// 工具栏中的项
        /// </summary>
        [Description("工具栏中的项"), EditorBrowsable(EditorBrowsableState.Always), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ToolItemCollection Items
        {
            get
            {
                if (this._items == null)
                {
                    this._items = new ToolItemCollection(this);
                    this._items.ListChanged += _items_ListChanged;
                }
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
                this.Invalidate(this.ClientRectangle);
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
                if (value.Height == 0)
                {
                    this._iImageShow = false;
                }
                this.Invalidate(this.ClientRectangle);
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
                this.Invalidate(this.ClientRectangle);
            }
        }
        private ToolItem _tempItem = null;
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
        /// <summary>
        /// 列数量
        /// </summary>
        private int LastItemCount;

        #endregion

        #region 事件
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
        /// <summary>
        /// 当单击项编辑时事件的 Key
        /// </summary>
        private static readonly object EventEditClick = new object();

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
                    handler(item, e);
            }
        }
        /// <summary>
        /// 当编辑项时激发。
        /// </summary>
        /// <param name="item"></param>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        public virtual void OnEditClick(ToolItem item, EventArgs e)
        {
            if (!item.Enable) return;
            EventHandler handler = base.Events[EventEditClick] as EventHandler;
            if (handler != null)
                handler(item, e);
        }

        #endregion

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
        /// <summary>
        /// 当编辑项时事件发生
        /// </summary>
        public event EventHandler EditClick
        {
            add { base.Events.AddHandler(EventEditClick, value); }
            remove { base.Events.RemoveHandler(EventEditClick, value); }
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
            TPaint(e.Graphics);
        }
        /// <summary>
        /// g == null 时 计算宽高
        /// </summary>
        /// <param name="g"></param>
        private void TPaint(Graphics g)
        {
            if (g != null)
            {
                g.TranslateTransform(BodyBounds.X, BodyBounds.Y);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                backColor = TBackGround.ColorSpace;
                g.FillRectangle(new SolidBrush(backColor), new Rectangle(-1, -1, this.Width + 1, this.Height + 1));
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
            if (_tAdd)
            {
                //填充空Item
                for (int i = 0; i < count; i++)
                {
                    DrawItem(g, new ToolItem(), ref xPos, ref yPos, i == count - 1);
                }
            }
        }
        #region 绘制方法
        private void DrawItem(Graphics g, ToolItem item, ref int xPos, ref int yPos, bool iLast)
        {
            // 当前 Item 所在的矩型区域
            item.Rectangle = new Rectangle(xPos, yPos, this._itemSize.Width, this._itemSize.Height);
            if (g != null)
            {
                DrawBackground(g, item);
                DrawImage(g, item);
                DrawText(g, item);
            }
            switch (_tDirection)
            {
                case TDirection.Level:
                    if (xPos + item.Rectangle.Width * 2 + this._itemSpace + this.Padding.Right > this.Width)
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
                    if (yPos + item.Rectangle.Height * 2 + this._itemSpace + this.Padding.Bottom > this.Height)
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
                item.IMouseState = TMouseState.Normal;
            }
            switch (item.MouseState)
            {
                case TMouseState.Normal:
                case TMouseState.Leave:
                    if (!item.Enable)
                    {
                        backColor = Color.Gray;
                    }
                    else
                    {
                        backColor = item.TColor.ColorNormal == Color.Empty ? TBackGround.ColorNormal : item.TColor.ColorNormal;
                    }
                    g.FillRectangle(new SolidBrush(backColor), item.Rectangle);
                    break;
                case TMouseState.Move:
                case TMouseState.Up:
                    DrawMoveBack(g, item);
                    break;
                case TMouseState.Down:
                    backColor = item.TColor.ColorDown == Color.Empty ? TBackGround.ColorDown : item.TColor.ColorDown;
                    if (backColor == Color.Empty)
                    {
                        g.DrawImage(this._pushedImage, item.Rectangle);
                    }
                    else
                    {
                        g.FillRectangle(new SolidBrush(backColor), item.Rectangle);
                    }
                    if (_iMultiple)
                    {
                        g.DrawImage(this._selectImage, new Rectangle(item.Rectangle.Right - this._selectImage.Width, item.Rectangle.Bottom - this._selectImage.Height, this._selectImage.Width, this._selectImage.Height));
                    }
                    IsContextMenu(g, item);
                    break;
            }
        }
        /// <summary>
        /// 绘制鼠标移入时的背景
        /// </summary>
        private void DrawMoveBack(Graphics g, ToolItem item)
        {
            backColor = item.TColor.ColorMove == Color.Empty ? TBackGround.ColorMove : item.TColor.ColorMove;
            if (backColor == Color.Empty)
            {
                g.DrawImage(this._hoverImage, item.Rectangle);
            }
            else
            {
                g.FillRectangle(new SolidBrush(backColor), item.Rectangle);
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
        private void DrawText(Graphics g, ToolItem item)
        {
            Rectangle textRect = Rectangle.Empty;
            if (!string.IsNullOrEmpty(item.Text))
            {
                int pad = 2;
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
                            textRect.X = item.Rectangle.X + pad + _imageSize.Width;
                            textRect.Height = item.Rectangle.Height;
                            textRect.Width = item.Rectangle.Width + item.Rectangle.X - textRect.X;
                            break;
                    }
                }
                if (item.ContextMenuStrip != null)
                {
                    textRect.Width -= _rightLen;
                }
                textRect.Width -= pad * 2;
                textRect.Height -= pad * 2;
                string[] text = item.Text.Split(new string[] { "\r\n", "&" }, StringSplitOptions.RemoveEmptyEntries);
                if (text.Length > 0)
                {
                    int fHight = GetFont(item.MouseState, TextFirst).GetHeight(g).ToInt();
                    int sHight = GetFont(item.MouseState, TextSencond).GetHeight(g).ToInt();
                    int height = textRect.Height - fHight;
                    height -= (text.Length - 1) * sHight;
                    height -= (text.Length - 1) * 6;
                    height /= 2;

                    Rectangle rect = new Rectangle()
                    {
                        X = textRect.X,
                        Y = textRect.Y + height,
                        Width = textRect.Width,
                        Height = fHight,
                    };
                    DrawOtherDesc(g, item.Enable, item.MouseState, TextFirst, text[0], rect);
                    for (int i = 1; i < text.Length; i++)
                    {
                        rect = new Rectangle()
                        {
                            X = textRect.X + (fHight - sHight) / 2,
                            Y = textRect.Y + height + fHight + 6 * i + sHight * (i - 1),
                            Width = textRect.Width,
                            Height = sHight,
                        };
                        DrawOtherDesc(g, item.Enable, item.MouseState, TextSencond, text[i], rect);
                    }
                }
            }
            if (!string.IsNullOrEmpty(item.HeadDesc))
            {
                int dHeight = GetFont(item.MouseState, THeadDesc).GetHeight(g).ToInt() + 6;
                Rectangle rect = new Rectangle()
                {
                    X = textRect.X,
                    Y = textRect.Y,
                    Width = textRect.Width,
                    Height = dHeight,
                };
                DrawOtherDesc(g, item.Enable, item.MouseState, THeadDesc, item.HeadDesc, rect);
            }
            if (!string.IsNullOrEmpty(item.EndDesc))
            {
                int dHeight = GetFont(item.MouseState, TEndDesc).GetHeight(g).ToInt() + 6;
                Rectangle rect = new Rectangle()
                {
                    X = textRect.X,
                    Y = textRect.Y + textRect.Height - dHeight,
                    Width = textRect.Width,
                    Height = dHeight,
                };
                DrawOtherDesc(g, item.Enable, item.MouseState, TEndDesc, item.EndDesc, rect);
            }
            DrawDesc(g, item, textRect);
        }
        private Font GetFont(TMouseState state, TProperties pro)
        {
            switch (state)
            {
                case TMouseState.Normal:
                case TMouseState.Leave:
                    return pro.FontNormal;
                case TMouseState.Move:
                case TMouseState.Up:
                    return pro.FontMove;
                case TMouseState.Down:
                    return pro.FontDown;
            }
            return this.Font;
        }
        /// <summary>
        /// 绘制正文描述
        /// </summary>
        /// <param name="item"></param>
        /// <param name="rect"></param>
        /// <param name="g"></param>
        private void DrawDesc(Graphics g, ToolItem item, Rectangle rect)
        {
            if (string.IsNullOrEmpty(item.Desc)) return;
            SizeF size = g.MeasureString(item.Desc, GetFont(item.MouseState, TDesc));
            item.RectDesc = new Rectangle(rect.X + rect.Width - size.Width.ToInt() + (item.ContextMenuStrip == null ? 0 : 4),
                rect.Y + (rect.Height - size.Height.ToInt()) / 2, size.Width.ToInt() + 2, size.Height.ToInt());
            DrawOtherDesc(g, item.Enable, item.IMouseState, TDesc, item.Desc, item.RectDesc);
        }
        /// <summary>
        /// 绘制其它描述
        /// </summary>
        private void DrawOtherDesc(Graphics g, bool enable, TMouseState state, TProperties desc, string text, Rectangle rect)
        {
            if (string.IsNullOrEmpty(text)) return;

            Color color = this.ForeColor;
            Font font = desc.FontNormal;
            StringFormat format = new StringFormat()
            {
                Alignment = desc.StringVertical,
                LineAlignment = desc.StringHorizontal
            };
            if (!enable)
            {
                g.DrawString(text, font, new SolidBrush(color), rect, format);
                return;
            }
            switch (state)
            {
                case TMouseState.Normal:
                case TMouseState.Leave:
                    color = desc.ColorNormal;
                    font = desc.FontNormal;
                    SizeF size = g.MeasureString(text, font);
                    break;
                case TMouseState.Move:
                case TMouseState.Up:
                    color = desc.ColorMove;
                    font = desc.FontMove;
                    break;
                case TMouseState.Down:
                    color = desc.ColorDown;
                    font = desc.FontDown;
                    break;
            }
            if (color == Color.Empty) color = this.ForeColor;
            g.DrawString(text, font, new SolidBrush(color), rect, format);
        }
        /// <summary>
        /// 判断右键菜单
        /// </summary>
        /// <returns>返回是否有焦点 true时不触发down事件</returns>
        private void IsContextMenu(Graphics g, ToolItem item)
        {
            Point point = this.PointToClient(MousePosition);
            point.X -= BodyBounds.X;
            point.Y -= BodyBounds.Y;
            Image btnArrowImage = null;
            Point contextMenuLocation = this.PointToScreen(new Point(this._btnArrowRect.Left + BodyBounds.X, this._btnArrowRect.Top + BodyBounds.Y + this._btnArrowRect.Height + 2));
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
                if (item.Rectangle.Contains(point))
                {
                    if (_iDown && (this._btnArrowRect.Contains(point) || item.RectDesc.Contains(point)))
                    {
                        btnArrowImage = AssemblyHelper.GetImage("QQ.TabControl.main_tabbtn_down.png");
                    }
                    else
                    {
                        btnArrowImage = AssemblyHelper.GetImage("QQ.TabControl.main_tabbtn_highlight.png");
                    }
                    if (this._iFocus && (this._btnArrowRect.Contains(point) || item.RectDesc.Contains(point)))
                    {
                        contextMenuStrip.Tag = item;
                        contextMenuStrip.Show(contextMenuLocation);
                    }
                    this._btnArrowRect = new Rectangle(item.Rectangle.X + item.Rectangle.Width - btnArrowImage.Width,
                        item.Rectangle.Y + (item.Rectangle.Height - btnArrowImage.Height) / 2, btnArrowImage.Width, btnArrowImage.Height);
                }
            }
            if (btnArrowImage != null)
            {
                //当鼠标进入当前选中的的选项卡时，显示下拉按钮
                g.DrawImage(btnArrowImage, this._btnArrowRect);
            }
        }
        /// <summary>
        /// 右键菜单关闭刷新项
        /// </summary>
        void contextMenuStrip_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            this._iFocus = false;
            ToolItem item = (sender as ContextMenuStrip).Tag as ToolItem;
            Point cursorPoint = this.PointToClient(MousePosition);
            if (!this.ClientRectangle.Contains(cursorPoint))
            {
                if (item.MouseState != TMouseState.Down)
                {
                    InvaItem(item, TMouseState.Normal);
                }
                InvaRectDesc(item, TMouseState.Normal);
            }
            InvaOther(item.Rectangle);
        }

        #endregion

        /// <summary>
        /// 重绘正文描述
        /// </summary>
        /// <param name="item"></param>
        /// <param name="state"></param>
        private void InvaRectDesc(ToolItem item, TMouseState state)
        {
            if (item.IMouseState != state)
            {
                item.IMouseState = state;
                this.Invalidate(new Rectangle(item.RectDesc.X + BodyBounds.X, item.RectDesc.Y + BodyBounds.Y, item.RectDesc.Width, item.RectDesc.Height));
                this.Invalidate(new Rectangle(_btnArrowRect.X + BodyBounds.X, _btnArrowRect.Y + BodyBounds.Y, _btnArrowRect.Width, _btnArrowRect.Height));
            }
        }
        /// <summary>
        /// 重绘Item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="state"></param>
        private void InvaItem(ToolItem item, TMouseState state)
        {
            if (item.MouseState != state)
            {
                item.MouseState = state;
                this.Invalidate(new Rectangle(item.Rectangle.X + BodyBounds.X, item.Rectangle.Y + BodyBounds.Y, item.Rectangle.Width, item.Rectangle.Height));
            }
        }
        /// <summary>
        /// 重绘Item
        /// </summary>
        /// <param name="rect"></param>
        private void InvaOther(Rectangle rect)
        {
            this.Invalidate(new Rectangle(rect.X + BodyBounds.X, rect.Y + BodyBounds.Y, rect.Width, rect.Height));
        }
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
                point.X -= BodyBounds.X;
                point.Y -= BodyBounds.Y;
                foreach (ToolItem item in this.Items)
                {
                    if (item.Rectangle.Contains(point) && (item.RectDesc.Contains(point) || this._btnArrowRect.Contains(point)))
                    {
                        if (item.IMouseState != TMouseState.Down)
                        {
                            InvaRectDesc(item, TMouseState.Move);
                        }
                    }
                    else
                    {
                        InvaRectDesc(item, TMouseState.Normal);
                    }
                    if (!_iCheckEvent && item.MouseState == TMouseState.Down)
                    {
                        continue;
                    }
                    else if (item.Rectangle.Contains(point))
                    {
                        InvaItem(item, TMouseState.Move);
                    }
                    else
                    {
                        InvaItem(item, TMouseState.Normal);
                    }
                }
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
                _iDown = false;
                foreach (ToolItem item in this.Items)
                {
                    InvaRectDesc(item, TMouseState.Normal);
                    if ((_iCheckEvent && !_iMultiple) || item.MouseState != TMouseState.Down)
                    {
                        InvaItem(item, TMouseState.Normal);
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
            if (_iFocus) return;

            if (!this.DesignMode)
            {
                Point point = e.Location;
                point.X -= BodyBounds.X;
                point.Y -= BodyBounds.Y;
                for (int i = 0; i < this.Items.Count; i++)
                {
                    ToolItem item = this.Items[i];
                    OnMouseDown(point, item);
                }
            }
        }
        private void OnMouseDown(Point point, ToolItem item)
        {
            if (item.Rectangle.Contains(point))
            {
                if (item != this._tempItem)
                {
                    this._tempItem = item;
                }
                //事件
                if (item != this._selectedItem)
                {
                    if (!item.RectDesc.Contains(point) && _tEvent == TEvent.Down)
                    {
                        this._selectedItem = item;
                        this._selectedIndex = this.Items.GetIndexOfRange(item);
                        this.OnSelectedItemChanged(item, EventArgs.Empty);
                        this.OnSelectedIndexChanged(item, EventArgs.Empty);
                    }
                }
                //事件
                if (_tEvent == TEvent.Down)
                {
                    if (item.RectDesc.Contains(point) || this._btnArrowRect.Contains(point))
                    {
                        if (item.RectDesc.Contains(point))
                        {
                            this.OnEditClick(item, EventArgs.Empty);
                        }
                        if (item.ContextMenuStrip != null)
                        {
                            this._iFocus = true;
                            InvaOther(this._btnArrowRect);
                        }
                    }
                    else
                    {
                        this.OnItemClick(item, EventArgs.Empty);
                    }
                }
                if (item.RectDesc.Contains(point) || this._btnArrowRect.Contains(point))
                {
                    _iDown = true;
                    InvaRectDesc(item, TMouseState.Down);
                }
                else
                {
                    InvaRectDesc(item, TMouseState.Normal);
                    if (_iMultiple)
                    {
                        _selectedItem = null;
                        if (item.MouseState != TMouseState.Down)
                        {
                            InvaItem(item, TMouseState.Down);
                        }
                        else
                        {
                            InvaItem(item, TMouseState.Normal);
                        }
                    }
                    else
                    {
                        InvaItem(item, TMouseState.Down);
                    }
                }
            }
            else if (!_iMultiple && Contain(point) && !ContainDesc(point))
            {
                InvaItem(item, TMouseState.Normal);
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

            if (!this.DesignMode)
            {
                Point point = e.Location;
                point.X -= BodyBounds.X;
                point.Y -= BodyBounds.Y;
                for (int i = 0; i < this.Items.Count; i++)
                {
                    ToolItem item = this.Items[i];
                    OnMouseUp(point, item);
                }
            }
        }
        private void OnMouseUp(Point point, ToolItem item)
        {
            if (item.Rectangle.Contains(point))
            {
                _iDown = false;
                //事件
                if (item != this._selectedItem && item == _tempItem)
                {
                    if (!item.RectDesc.Contains(point) && _tEvent == TEvent.Up)
                    {
                        this._selectedItem = item;
                        this._selectedIndex = this.Items.GetIndexOfRange(item);
                        this.OnSelectedItemChanged(item, EventArgs.Empty);
                        this.OnSelectedIndexChanged(item, EventArgs.Empty);
                    }
                }
                //事件
                if (_tEvent == TEvent.Up)
                {
                    if (item.RectDesc.Contains(point) || this._btnArrowRect.Contains(point))
                    {
                        if (item.RectDesc.Contains(point))
                        {
                            this.OnEditClick(item, EventArgs.Empty);
                        }
                        if (item.ContextMenuStrip != null)
                        {
                            this._iFocus = true;
                            InvaOther(this._btnArrowRect);
                        }
                    }
                    else
                    {
                        this.OnItemClick(item, EventArgs.Empty);
                    }
                }
                if (item.RectDesc.Contains(point) || this._btnArrowRect.Contains(point))
                {
                    InvaRectDesc(item, TMouseState.Move);
                }
                else
                {
                    InvaRectDesc(item, TMouseState.Normal);
                }
            }
        }
        /// <summary>
        /// 引发 System.Windows.Forms.Form.MouseEnter 事件。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (TScroll && this.ParentForm.ContainsFocus)
            {
                this.Focus();
            }
        }
        /// <summary>
        /// 数据源更新-  BindingList提供
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _items_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (this.ParentForm != null)
            {
                if (LastItemCount != Items.Count)
                {
                    TRefresh();
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
        /// 坐标点是否包含在项中
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool ContainDesc(Point point)
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (this.Items[i].RectDesc.Contains(point))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 选中第一项
        /// </summary>
        public void TClickFirst()
        {
            TClickItem(0);
        }
        /// <summary>
        /// 选中项
        /// </summary>
        public void TClickItem(ToolItem item)
        {
            int index = this.Items.GetIndexOfRange(item);
            TClickItem(index);
        }
        /// <summary>
        /// 选中第index项
        /// </summary>
        public void TClickItem(int index)
        {
            if (this._items.Count == 0) return;
            if (this._items.Count <= index) return;
            if (!_iMultiple)
            {
                _selectedItem = null;
                for (int i = 0; i < _items.Count; i++)
                {
                    InvaItem(_items[i], TMouseState.Normal);
                }
            }
            if (index >= 0)
            {
                this._selectedItem = this._items[index];
                if (!_iCheckEvent)
                {
                    InvaItem(_selectedItem, TMouseState.Down);
                    OnSelectedItemChanged(_items[index], EventArgs.Empty);
                }
                else
                {
                    OnItemClick(_items[index], EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// 刷新控件到原点
        /// </summary>
        public void TStart()
        {
            FixScroll(0);
        }
        /// <summary>
        /// 刷新控件到指定位置
        /// </summary>
        public void TStart(int value)
        {
            FixScroll(value);
        }
        /// <summary>
        /// 自适应高度/宽度
        /// </summary>
        public void TAutoHeight()
        {
            TAutoHeight(0);
        }
        /// <summary>
        /// 自适应高度/宽度
        /// </summary>
        /// <param name="count">最小行/列</param>
        public void TAutoHeight(int count)
        {
            if (this.CountLine < count) this.CountLine = count;
            switch (TDirection)
            {
                case TDirection.Level:
                    this.Height = GetHeight();
                    break;
                case TDirection.Vertical:
                    this.Width = GetWidth();
                    break;
            }
        }
        /// <summary>
        /// 刷新项
        /// </summary>
        /// <param name="item"></param>
        public void TRefresh(ToolItem item)
        {
            int index = this.Items.GetIndexOfRange(item);
            TRefresh(index);
        }
        /// <summary>
        /// 刷新项
        /// </summary>
        /// <param name="index"></param>
        public void TRefresh(int index)
        {
            if (index < 0 || index > Items.Count - 1) return;
            InvaOther(this.Items[index].Rectangle);
        }
        /// <summary>
        /// 刷新控件
        /// </summary>
        public void TRefresh()
        {
            this.TPaint(null);
            UpdateScroll(true, Items.Count > LastItemCount);
            LastItemCount = Items.Count;
            this.Invalidate(this.ClientRectangle);
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
        /// 初始化动态方法
        /// </summary>
        private void Progress()
        {
            timer.Interval = 30;
            this.timer.Tick += timer_Tick;
        }
        /// <summary>
        /// 动态显示项的图像
        /// </summary>
        /// <param name="text">项文本</param>
        /// <param name="newText">项新文本</param>
        public void ProgressItem(string text, string newText = null)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Text == text)
                {
                    ProgressItem(i, newText);
                    break;
                }
            }
        }
        /// <summary>
        /// 动态显示项的图像
        /// </summary>
        /// <param name="index">项索引</param>
        /// <param name="newText">项新文本</param>
        public void ProgressItem(int index, string newText = null)
        {
            this.pIndex = index;
            if (!string.IsNullOrEmpty(newText)) this._items[pIndex].Text = newText;
            image = this._items[pIndex].Image;
            if (pictureBox1.Image != null)
            {
                this._items[pIndex].Image = pictureBox1.Image;
            }
            timer.Enabled = true;
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
            InvaOther(this._items[pIndex].Rectangle);
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

        #region 滚动条
        /// <summary>  
        /// 垂直滚动条  
        /// </summary>  
        private VScrollBar _vScroll;
        /// <summary>  
        /// 水平滚动条  
        /// </summary>  
        private HScrollBar _hScroll;
        private bool _scroll = true;
        /// <summary>
        /// 是否显示滚动条
        /// </summary>
        [Description("是否显示滚动条"), DefaultValue(true)]
        public bool IScroll
        {
            get { return _scroll; }
            set
            {
                _scroll = value;
                if (value)
                {
                    UpdateScroll();
                }
                else
                {
                    _vScroll.Visible = false;
                    _hScroll.Visible = false;
                }
            }
        }
        /// <summary>
        /// 滚动条是否显示
        /// </summary>
        [Browsable(false), Description("滚动条是否显示"), DefaultValue(false)]
        public bool TScroll
        {
            get
            {
                switch (TDirection)
                {
                    case Forms.TDirection.Level:
                        return _vScroll.Visible;
                    case Forms.TDirection.Vertical:
                        return _hScroll.Visible;
                }
                return false;
            }
        }
        /// <summary>
        /// 控件宽度
        /// </summary>
        public new int Width
        {
            get { return _vScroll.Visible ? base.Width - _vScroll.Width : base.Width; }
            set { base.Width = value; }
        }
        /// <summary>
        /// 控件高度
        /// </summary>
        public new int Height
        {
            get { return _hScroll.Visible ? base.Height - _hScroll.Height : base.Height; }
            set { base.Height = value; }
        }
        /// <summary>
        /// 控件显示区域
        /// </summary>
        private Rectangle BodyBounds = Rectangle.Empty;
        /// <summary>
        /// 初始化滚动条
        /// </summary>
        private void CustomScroll()
        {
            _hScroll = new HScrollBar();
            _hScroll.Scroll += _hScroll_Scroll;
            _hScroll.Dock = DockStyle.Bottom;
            Controls.Add(_hScroll);

            _vScroll = new VScrollBar();
            _vScroll.Scroll += _vScroll_Scroll;
            _vScroll.Dock = DockStyle.Right;
            Controls.Add(_vScroll);

            _hScroll.Visible = false;
            _vScroll.Visible = false;
        }

        /// <summary>
        /// 重写滚轮滚动
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (!_scroll) return;
            if (_vScroll.Visible)
            {
                int value = _vScroll.Value - e.Delta * this.ItemSize.Height / 2 / 120;
                FixScroll(value);
            }
            else if (_hScroll.Visible)
            {
                int value = _hScroll.Value - e.Delta * this.ItemSize.Width / 2 / 120;
                FixScroll(value);
            }
            base.OnMouseWheel(e);
        }
        private void FixScroll(int value)
        {
            if (!_scroll) return;
            switch (TDirection)
            {
                case TDirection.Level:
                    if (value < 0) value = 0;
                    if (value > _vScroll.Maximum) value = _vScroll.Maximum;
                    _vScroll.Value = value;
                    BodyBounds.Y = -value;
                    break;
                case TDirection.Vertical:
                    if (value < 0) value = 0;
                    if (value > _hScroll.Maximum) value = _hScroll.Maximum;
                    _hScroll.Value = value;
                    BodyBounds.X = -value;
                    break;
            }
            this.Invalidate(this.ClientRectangle);
        }
        /// <summary>
        /// 大小改变时
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            BodyBounds.Width = this.Width;
            BodyBounds.Height = this.Height;
            TPaint(null);
            UpdateScroll();
        }
        /// <summary>
        /// 更新滚动条状态
        /// </summary>
        private void UpdateScroll()
        {
            UpdateScroll(false, false);
        }
        private void UpdateScroll(bool fix, bool toLast)
        {
            if (!_scroll) return;
            if (CountColumn == 0 || CountLine == 0)
            {
                TPaint(null);
            }
            _vScroll.Visible = false;
            _hScroll.Visible = false;
            switch (TDirection)
            {
                case TDirection.Level:
                    int height = GetHeight();
                    if (this.Height < height)
                    {
                        _vScroll.Visible = true;
                        int max = GetHeight() - this.Height;
                        if (_vScroll.Value > max)
                        {
                            FixScroll(max);
                        }
                        _vScroll.Maximum = max;
                        if (!fix) _vScroll.Value = 0;
                        if (toLast)
                        {
                            FixScroll(_vScroll.Maximum);
                        }
                    }
                    else if (_vScroll.Value > 0)
                    {
                        FixScroll(0);
                    }
                    break;
                case TDirection.Vertical:
                    int width = GetWidth();
                    if (this.Width < width)
                    {
                        _hScroll.Visible = true;
                        int max = GetWidth() - this.Width;
                        if (_hScroll.Value > max)
                        {
                            FixScroll(max);
                        }
                        _hScroll.Maximum = max;
                        if (!fix) _hScroll.Value = 0;
                        if (toLast)
                        {
                            FixScroll(_hScroll.Maximum);
                        }
                    }
                    else if (_hScroll.Value > 0)
                    {
                        FixScroll(0);
                    }
                    break;
            }
        }
        /// <summary>
        /// 水平滚动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _hScroll_Scroll(object sender, ScrollEventArgs e)
        {
            int diff = e.NewValue - e.OldValue;
            if (e.NewValue == 0)
            {
                diff = 0;
                BodyBounds.X = 0;
            }
            BodyBounds.Offset(-diff, 0);
            this.Invalidate(this.ClientRectangle);
        }
        /// <summary>
        /// 垂直滚动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _vScroll_Scroll(object sender, ScrollEventArgs e)
        {
            int diff = e.NewValue - e.OldValue;
            if (e.NewValue == 0)
            {
                diff = 0;
                BodyBounds.Y = 0;
            }
            BodyBounds.Offset(0, -diff);
            this.Invalidate(this.ClientRectangle);
        }

        /// <summary>
        /// 需要的高度
        /// </summary>
        /// <returns></returns>
        private int GetHeight()
        {
            int height = this.Padding.Top + this.Padding.Bottom;
            height += this.CountLine * this.ItemSize.Height;
            height += (this.CountLine - 1) * this.ItemSpace;
            return height;
        }
        /// <summary>
        /// 需要的宽度
        /// </summary>
        /// <returns></returns>
        private int GetWidth()
        {
            int width = this.Padding.Left + this.Padding.Right;
            width += this.CountColumn * this.ItemSize.Width;
            width += (this.CountColumn - 1) * this.ItemSpace;
            return width;
        }

        #endregion
    }
}
