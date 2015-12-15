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
using System.Drawing.Text;
using Paway.Win32;
using System.Threading.Tasks;
using System.Collections.Concurrent;

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
        /// <summary>
        /// 默认时的按钮图片
        /// </summary>
        private Image _normalImage = null;
        private Image _normalImage2 = AssemblyHelper.GetImage("_360.ToolBar.toolbar_normal.png");
        /// <summary>
        /// 默认图片
        /// </summary>
        [DefaultValue((string)null)]
        [Description("默认时的按钮图片")]
        public virtual Image NormalImage
        {
            get { return this._normalImage; }
            set
            {
                this._normalImage = value;
                this.Invalidate(this.ClientRectangle);
            }
        }
        /// <summary>
        /// 鼠标按下时的图片
        /// </summary>
        private Image _downImage = null;
        private Image _downImage2 = AssemblyHelper.GetImage("_360.ToolBar.toolbar_hover.png");
        /// <summary>
        /// 鼠标按下时的图片
        /// </summary>
        [DefaultValue((string)null)]
        [Description("鼠标按下时的图片")]
        public virtual Image DownImage
        {
            get { return this._downImage; }
            set
            {
                this._downImage = value;
                this.Invalidate(this.ClientRectangle);
            }
        }
        /// <summary>
        /// 鼠标划过时的图片
        /// </summary>
        private Image _moveImage = null;
        private Image _moveImage2 = AssemblyHelper.GetImage("_360.ToolBar.toolbar_pushed.png");
        /// <summary>
        /// 鼠标划过时的图片
        /// </summary>
        [DefaultValue((string)null)]
        [Description("鼠标划过时的图片")]
        public virtual Image MoveImage
        {
            get { return this._moveImage; }
            set
            {
                this._moveImage = value;
                this.Invalidate(this.ClientRectangle);
            }
        }
        /// <summary>
        /// 多选状态下选中时附加的图片
        /// </summary>
        private Image _selectImage = AssemblyHelper.GetImage("Controls.accept_16.png");
        /// <summary>
        /// 多选状态下选中时附加的图片
        /// </summary>
        //[DefaultValue((string)null)]
        [Description("多选状态下选中时附加的图片")]
        public virtual Image SelectImage
        {
            get { return this._selectImage; }
            set
            {
                this._selectImage = value;
                this.Invalidate(this.ClientRectangle);
            }
        }

        #endregion

        #region 私有变量
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
        /// <summary>
        /// 悬停窗口
        /// </summary>
        private ToolTip toolTop;
        /// <summary>
        /// 按下抬起项是否相同中用的过度项
        /// </summary>
        private ToolItem _tempItem = null;

        #endregion

        #region 构造函数
        /// <summary>
        /// 初始化 Paway.Forms.ToolBar 新的实例。
        /// </summary>
        public ToolBar()
        {
            InitializeComponent();
            Progress();
            InitChange();
            CustomScroll();
            toolTop = new ToolTip();
        }

        #endregion

        #region 公共属性
        #region Int
        /// <summary>
        /// 圆角大小
        /// </summary>
        private int _tRadiu;
        /// <summary>
        /// 圆角大小
        /// </summary>
        [Description("圆角大小"), DefaultValue(0)]
        public int TRadiu
        {
            get { return _tRadiu; }
            set
            {
                this._tRadiu = value;
                this.Invalidate(this.ClientRectangle);
            }
        }

        /// <summary>
        /// 滚动条宽度
        /// </summary>
        private int _tScrollHeight = 3;
        /// <summary>
        /// 滚动条宽度
        /// </summary>
        [Description("滚动条宽度"), DefaultValue(3)]
        public int TScrollHeight
        {
            get { return _tScrollHeight; }
            set
            {
                if (value < 0) value = 0;
                _tScrollHeight = value;
                if (_hScroll != null)
                {
                    _hScroll.Height = value;
                }
                if (_vScroll != null)
                {
                    _vScroll.Width = value;
                }
                this.TRefresh();
            }
        }

        /// <summary>
        /// 头文字总长度(占用总长度/宽度)
        /// </summary>
        [Browsable(false), Description("头文字总长度(占用总长度/宽度)"), DefaultValue(0)]
        public int THeardLength { get; private set; }

        /// <summary>
        /// 项文本间的间隔
        /// </summary>
        private int _textSpace = 6;
        /// <summary>
        /// 项文本间的间隔
        /// </summary>
        [Description("项文本间的间隔"), DefaultValue(6)]
        public int TextSpace
        {
            get { return this._textSpace; }
            set
            {
                this._textSpace = value;
                this.Invalidate(this.ClientRectangle);
            }
        }

        /// <summary>
        /// 多行列排列时的行数
        /// </summary>
        [Browsable(false), Description("多行列排列时的行数"), DefaultValue(1)]
        public int TCountLine { get; private set; }
        /// <summary>
        /// 多行列排列时的列数
        /// </summary>
        [Browsable(false), Description("多行列排列时的列数"), DefaultValue(1)]
        public int TCountColumn { get; private set; }

        #endregion

        #region bool
        /// <summary>
        /// 显示简短说明
        /// </summary>
        [Description("显示简短说明"), DefaultValue(false)]
        public bool IShowToolTop { get; set; }
        private bool _iText;
        /// <summary>
        /// 显示为文本内容
        /// </summary>
        [Description("显示为文本内容"), DefaultValue(false)]
        public bool IText
        {
            get { return this._iText; }
            set
            {
                this._iText = value;
                this.Invalidate(this.ClientRectangle);
            }
        }
        /// <summary>
        /// 普通项，不响应鼠标绘制
        /// </summary>
        [Description("普通项，不响应鼠标绘制"), DefaultValue(false)]
        public bool INormal { get; set; }
        private bool _iItemLine;
        /// <summary>
        /// 绘制边框线开关
        /// </summary>
        [Description("绘制边框线开关"), DefaultValue(false)]
        public bool IItemLine
        {
            get { return this._iItemLine; }
            set
            {
                this._iItemLine = value;
                this.Invalidate(this.ClientRectangle);
            }
        }

        /// <summary>
        /// 补充整行\列
        /// </summary>
        private bool _iAdd;
        /// <summary>
        /// 补充整行\列
        /// </summary>
        [Description("补充整行\\列"), DefaultValue(false)]
        public bool IAdd
        {
            get { return _iAdd; }
            set
            {
                this._iAdd = value;
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
        private bool _iImageShow = false;
        /// <summary>
        /// 图片显示开关
        /// </summary>
        [Description("图片显示开关"), DefaultValue(false)]
        public bool IImageShow
        {
            get { return this._iImageShow; }
            set
            {
                this._iImageShow = value;
                UpdateImageSize();
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

        #endregion

        #region 其它
        /// <summary>
        /// 获取或设置项内的空白
        /// </summary>
        private Padding _textPading = new Padding(2);
        /// <summary>
        /// 获取或设置项内的空白
        /// </summary>
        [Description("获取或设置项内的空白"), DefaultValue(typeof(Padding), "2,2,2,2")]
        public Padding TextPading
        {
            get { return this._textPading; }
            set
            {
                this._textPading = value;
                UpdateImageSize();
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
            set { this._tEvent = value; }
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
                this.TRefresh();
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

        #endregion

        #region 数据项
        /// <summary>
        /// 工具栏中的项列表
        /// </summary>
        private ToolItemCollection _items;
        /// <summary>
        /// 工具栏中的项列表
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("工具栏中的项列表"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
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
        /// 项的大小
        /// </summary>
        private Size _itemSize = new Size(78, 82);
        /// <summary>
        /// 项的大小
        /// </summary>
        [Description("项的大小"), DefaultValue(typeof(Size), "78,82")]
        public Size ItemSize
        {
            get { return this._itemSize; }
            set
            {
                this._itemSize = value;
                _vScroll.Visible = false;
                _hScroll.Visible = false;
                _vScroll2.Visible = false;
                this.TPaint();
                UpdateImageSize();
                UpdateScroll();
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
                this.TRefresh();
            }
        }
        /// <summary>
        /// 项图片的大小
        /// </summary>
        private Size _imageSize = new Size(48, 48);
        /// <summary>
        /// 项图片显示区域大小
        /// </summary>
        private Size _imageSizeShow = Size.Empty;
        /// <summary>
        /// 项图片的大小
        /// </summary>
        [Description("项图片的大小"), DefaultValue(typeof(Size), "48,48")]
        public Size ImageSize
        {
            get { return this._imageSize; }
            set
            {
                this._imageSize = value;
                UpdateImageSize();
                this.Invalidate(this.ClientRectangle);
            }
        }
        /// <summary>
        /// 当前选中项
        /// </summary>
        private ToolItem _selectedItem = null;
        /// <summary>
        /// 当前选中项
        /// </summary>
        [Browsable(false), Description("当前选中项")]
        public ToolItem SelectedItem
        {
            get { return this._selectedItem; }
        }
        /// <summary>
        /// 当前移入项
        /// </summary>
        [Browsable(false), Description("当前移入项"), DefaultValue((string)null)]
        public ToolItem MoveItem { get; set; }
        /// <summary>
        /// 选中项的索引
        /// </summary>
        private int _selectedIndex = 0;
        /// <summary>
        /// 选中项的索引
        /// </summary>
        [Browsable(false), Description("选中项的索引"), DefaultValue(0)]
        public int SelectedIndex
        {
            get { return this._selectedIndex; }
        }

        #endregion

        #region 字体、颜色属性
        private TProperties _change;
        /// <summary>
        /// 变色项颜色
        /// </summary>
        [Description("变色项颜色")]
        [DefaultValue(typeof(TProperties), "Change")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TProperties TChange
        {
            get
            {
                if (_change == null)
                {
                    _change = new TProperties();
                    _change.ValueChange += delegate { this.Invalidate(this.ClientRectangle); };
                }
                return _change;
            }
        }
        private TProperties _text;
        /// <summary>
        /// 文字
        /// </summary>
        [Description("首行文字属性")]
        [DefaultValue(typeof(TProperties), "TextFirst")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TProperties TextFirst
        {
            get
            {
                if (_text == null)
                {
                    _text = new TProperties();
                    _text.ValueChange += delegate { this.Invalidate(this.ClientRectangle); };
                }
                return _text;
            }
        }
        private TProperties _textSencond;
        /// <summary>
        /// 文字
        /// </summary>
        [Description("从行文字属性")]
        [DefaultValue(typeof(TProperties), "TextSencond")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TProperties TextSencond
        {
            get
            {
                if (_textSencond == null)
                {
                    _textSencond = new TProperties();
                    _textSencond.ValueChange += delegate { this.Invalidate(this.ClientRectangle); };
                }
                return _textSencond;
            }
        }
        private TProperties _desc;
        /// <summary>
        /// 正文描述
        /// </summary>
        [Description("正文描述属性")]
        [DefaultValue(typeof(TProperties), "Desc")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TProperties TDesc
        {
            get
            {
                if (_desc == null)
                {
                    _desc = new TProperties();
                    _desc.ValueChange += delegate { this.Invalidate(this.ClientRectangle); };
                }
                return _desc;
            }
        }
        private TProperties _headDesc;
        /// <summary>
        /// 头部描述
        /// </summary>
        [Description("头部描述属性")]
        [DefaultValue(typeof(TProperties), "HeadDesc")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TProperties THeadDesc
        {
            get
            {
                if (_headDesc == null)
                {
                    _headDesc = new TProperties();
                    _headDesc.ValueChange += delegate { this.Invalidate(this.ClientRectangle); };
                }
                return _headDesc;
            }
        }
        private TProperties _endDesc;
        /// <summary>
        /// 尾部描述
        /// </summary>
        [Description("尾部描述属性")]
        [DefaultValue(typeof(TProperties), "EndDesc")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TProperties TEndDesc
        {
            get
            {
                if (_endDesc == null)
                {
                    _endDesc = new TProperties();
                    _endDesc.ValueChange += delegate { this.Invalidate(this.ClientRectangle); };
                }
                return _endDesc;
            }
        }
        private TProperties _backGround;
        /// <summary>
        /// 背景
        /// </summary>
        [Description("背景颜色属性")]
        [DefaultValue(typeof(TProperties), "BackGround")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TProperties TBackGround
        {
            get
            {
                if (_backGround == null)
                {
                    _backGround = new TProperties();
                    _backGround.ValueChange += delegate { this.Invalidate(this.ClientRectangle); };
                }
                return _backGround;
            }
        }

        #endregion

        /// <summary>
        /// 重写父类的默认大小
        /// </summary>
        protected override Size DefaultSize
        {
            get { return new Size(300, 82); }
        }

        /// <summary>
        /// 获取或设置控件内的空白。
        /// </summary>
        [Description("获取或设置控件内的空白"), DefaultValue(typeof(Padding), "0, 0, 0, 0")]
        public new Padding Padding
        {
            get { return base.Padding; }
            set
            {
                base.Padding = value;
                this.TRefresh();
            }
        }

        #endregion

        #region 事件定义
        #region 事件对像
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
        /// <summary>
        /// 当项菜单弹出时事件
        /// </summary>
        private static readonly object EventOpening = new object();

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
            {
                item.Owner = this;
                handler(item, e);
            }
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
                {
                    item.Owner = this;
                    handler(item, e);
                }
            }
        }
        /// <summary>
        /// 当编辑项时激发。
        /// </summary>
        /// <param name="item"></param>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        public virtual bool OnEditClick(ToolItem item, EventArgs e)
        {
            if (!item.Enable) return false;
            EventHandler handler = base.Events[EventEditClick] as EventHandler;
            if (handler != null)
            {
                item.Owner = this;
                handler(item, e);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 当项菜单弹出时事件发生。
        /// </summary>
        /// <param name="item"></param>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        public virtual bool OnEventOpening(ToolItem item, EventArgs e)
        {
            if (!item.Enable) return false;
            EventHandler handler = base.Events[EventOpening] as EventHandler;
            if (handler != null)
            {
                item.Owner = this;
                handler(item, e);
                return true;
            }
            return false;
        }

        #endregion
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
        /// <summary>
        /// 当编辑项时事件发生
        /// </summary>
        public event EventHandler MenuOpening
        {
            add { base.Events.AddHandler(EventOpening, value); }
            remove { base.Events.RemoveHandler(EventOpening, value); }
        }

        #endregion

        #region Private Method
        /// <summary>
        /// 更新图片区域
        /// </summary>
        private void UpdateImageSize()
        {
            if (_iImageShow)
            {
                _imageSizeShow = _imageSize;
                switch (_tLocation)
                {
                    case TILocation.Up:
                        int width = (_itemSize.Width - _imageSize.Width - _textPading.Left - _textPading.Right) / 2;
                        if (width < 0)
                        {
                            _imageSizeShow.Width = _itemSize.Width - _textPading.Left - _textPading.Right;
                            _imageSizeShow.Height = (_imageSizeShow.Width * _imageSize.Height * 1.0 / _imageSize.Width).ToInt();
                        }
                        break;
                    case TILocation.Left:
                        int height = (_itemSize.Height - _imageSize.Height - _textPading.Top - _textPading.Bottom) / 2;
                        if (height < 0)
                        {
                            _imageSizeShow.Height = _itemSize.Height - _textPading.Top - _textPading.Bottom;
                            _imageSizeShow.Width = (_imageSizeShow.Height * _imageSize.Width * 1.0 / _imageSize.Height).ToInt();
                        }
                        break;
                }
            }
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
            g.TranslateTransform(Offset.X, Offset.Y);
            //g.PixelOffsetMode = PixelOffsetMode.Half; //与AntiAlias作用相反
            g.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF temp = g.VisibleClipBounds;
            //修理毛边
            temp = new RectangleF(temp.X - 1, temp.Y - 1, temp.Width + 2, temp.Height + 2);
            TranColor = TBackGround.ColorSpace;
            g.FillRectangle(new SolidBrush(TranColor), temp);

            //if (this.Items.Count > 0)
            //{
            //    //多线程处理(GDI是同一个，绘制效率没有提升)
            //    Parallel.ForEach(Partitioner.Create(0, this.Items.Count), (H) =>
            //    {
            //        for (int i = H.Item1; i < H.Item2; i++)
            //        {
            //            DrawItem(g, this.Items[i]);
            //        }
            //    });
            //}
            for (int i = 0; i < this.Items.Count; i++)
            {
                DrawItem(g, this.Items[i]);
            }
            if (_iAdd)
            {
                AddItem(g);
            }
        }
        /// <summary>
        /// 计算宽高
        /// </summary>
        private void TPaint()
        {
            int xPos = this.Padding.Left;
            int yPos = this.Padding.Top;
            this.TCountColumn = 1;
            this.TCountLine = 1;
            this.THeardLength = 0;
            this.isLastNew = true;
            for (int i = 0; i < this.Items.Count; i++)
            {
                Calctem(this.Items[i], ref xPos, ref yPos, i == this.Items.Count - 1);
            }
        }
        private void AddItem(Graphics g)
        {
            int count = this.Items.Count - 1;
            int xPos = this.Items[count].Rectangle.X;
            int yPos = this.Items[count].Rectangle.Y;
            Calctem(this.Items[count], ref xPos, ref yPos, false);
            count = 0;
            //多行/列补充Item
            if (this.TCountColumn > 1 && this.TCountLine > 1)
            {
                count = this.TCountColumn > this.TCountLine ? this.TCountColumn : this.TCountLine;
                if (this.Items.Count % count == 0) count = 0;
                else count = count - this.Items.Count % count;
            }
            if (_iAdd)
            {
                //填充空Item
                for (int i = 0; i < count; i++)
                {
                    ToolItem temp = new ToolItem();
                    Calctem(temp, ref xPos, ref yPos, i == count - 1);
                    DrawItem(g, temp);
                }
            }
        }

        #region 绘制方法
        private bool isLastNew;
        private void DrawHeard(Graphics g, ToolItem item)
        {
            if (!string.IsNullOrEmpty(item.Text))
            {
                if (g != null)
                {
                    Rectangle temp = new Rectangle(item.Rectangle.X, item.Rectangle.Y + Offset.Y,
                        item.Rectangle.Width, item.Rectangle.Height);
                    switch (_tDirection)
                    {
                        case TDirection.Level:
                            TextRenderer.DrawText(g, item.Text, item.TColor.FontNormal, temp, item.TColor.ColorNormal, item.TColor.TextFormat);
                            break;
                        case TDirection.Vertical:
                            Color color = item.TColor.ColorNormal;
                            if (color == Color.Empty) color = Color.Black;
                            g.DrawString(item.Text, item.TColor.FontNormal, new SolidBrush(color), temp, item.TColor.StringFormat);
                            break;
                    }
                }
            }
        }
        private void Calctem(ToolItem item, ref int xPos, ref int yPos, bool iLast)
        {
            // 当前 Item 所在的矩型区域
            item.Rectangle = new Rectangle(xPos, yPos, this._itemSize.Width, this._itemSize.Height);
            Size size = TextRenderer.MeasureText("你", this.Font);
            switch (_tDirection)
            {
                case TDirection.Level:
                    bool isNew = xPos + item.Rectangle.Width * 2 + this._itemSpace + this.Padding.Right > this.TWidth;
                    if (item.IHeard || isNew)
                    {
                        xPos = this.Padding.Left;
                        if (!iLast)
                        {
                            if (item.IHeard)
                            {
                                if (!isLastNew)
                                {
                                    yPos += this._itemSize.Height + this._itemSpace;
                                    this.TCountLine++;
                                }
                                item.Rectangle = new Rectangle(xPos, yPos, this.TWidth, size.Height);
                                this.THeardLength += size.Height + this._itemSpace * 2;
                            }
                            else
                            {
                                this.TCountLine++;
                            }
                            yPos += item.Rectangle.Height + this._itemSpace;
                        }
                    }
                    else
                    {
                        xPos += item.Rectangle.Width + this._itemSpace;
                        if (this.TCountLine == 1 && !iLast) this.TCountColumn++;
                    }
                    this.isLastNew = isNew;
                    break;
                case TDirection.Vertical:
                    isNew = yPos + item.Rectangle.Height * 2 + this._itemSpace + this.Padding.Bottom > this.THeight;
                    if (item.IHeard || isNew)
                    {
                        yPos = this.Padding.Top;
                        if (!iLast)
                        {
                            if (item.IHeard)
                            {
                                if (!isLastNew)
                                {
                                    xPos += this._itemSize.Width + this._itemSpace;
                                    this.TCountColumn++;
                                }
                                item.Rectangle = new Rectangle(xPos, yPos, size.Width, this.THeight);
                                this.THeardLength += size.Width + this._itemSpace * 2;
                            }
                            else
                            {
                                this.TCountColumn++;
                            }
                            xPos += item.Rectangle.Width + this._itemSpace;
                        }
                    }
                    else
                    {
                        yPos += item.Rectangle.Height + this._itemSpace;
                        if (this.TCountColumn == 1 && !iLast) this.TCountLine++;
                    }
                    this.isLastNew = isNew;
                    break;
            }
        }
        private void DrawItem(Graphics g, ToolItem item)
        {
            RectangleF temp = RectangleF.Intersect(g.VisibleClipBounds, item.Rectangle);
            if (temp != RectangleF.Empty)
            {
                if (item.IHeard)
                {
                    DrawHeard(g, item);
                }
                else
                {
                    DrawBackground(g, item);
                    DrawImage(g, item);
                    DrawText(g, item);
                }
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
                        TranColor = Color.Gray;
                    }
                    else
                    {
                        TranColor = item.TColor.ColorNormal == Color.Empty ? TBackGround.ColorNormal : item.TColor.ColorNormal;
                    }
                    if (TranColor == Color.Empty)
                    {
                        g.DrawImage(this._normalImage ?? this._normalImage2, item.Rectangle);
                    }
                    else
                    {
                        DrawBackground(g, TranColor, item);
                    }
                    break;
                case TMouseState.Move:
                case TMouseState.Up:
                    DrawMoveBack(g, item);
                    break;
                case TMouseState.Down:
                    TranColor = item.TColor.ColorDown == Color.Empty ? TBackGround.ColorDown : item.TColor.ColorDown;
                    if (TranColor == Color.Empty)
                    {
                        g.DrawImage(this._downImage ?? this._downImage2, item.Rectangle);
                    }
                    else
                    {
                        DrawBackground(g, TranColor, item);
                    }
                    if (_iMultiple && this._selectImage != null)
                    {
                        g.DrawImage(this._selectImage, new Rectangle(item.Rectangle.Right - this._selectImage.Width, item.Rectangle.Bottom - this._selectImage.Height, this._selectImage.Width, this._selectImage.Height));
                    }
                    IsContextMenu(g, item);
                    break;
            }
        }
        /// <summary>
        /// 填充Item内部颜色
        /// </summary>
        protected virtual void DrawBackground(Graphics g, Color color, ToolItem item)
        {
            int radiu = item.TRadiu > _tRadiu ? item.TRadiu : _tRadiu;
            if (radiu > 0)
            {
                GraphicsPath path = DrawHelper.CreateRoundPath(item.Rectangle, radiu);
                g.FillPath(new SolidBrush(color), path);
                if (_iItemLine)
                {
                    g.DrawPath(new Pen(Color.FromArgb(Trans, 100, 100, 100)), path);
                }
            }
            else
            {
                g.FillRectangle(new SolidBrush(color), item.Rectangle);
                if (_iItemLine)
                {
                    Rectangle rect = item.Rectangle;
                    rect = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height - 1);
                    g.DrawRectangle(new Pen(Color.FromArgb(Trans, 100, 100, 100)), rect);
                }
            }
        }
        /// <summary>
        /// 绘制鼠标移入时的背景
        /// </summary>
        private void DrawMoveBack(Graphics g, ToolItem item)
        {
            TranColor = item.TColor.ColorMove == Color.Empty ? TBackGround.ColorMove : item.TColor.ColorMove;
            if (TranColor == Color.Empty)
            {
                g.DrawImage(this._moveImage ?? this._moveImage2, item.Rectangle);
            }
            else
            {
                DrawBackground(g, TranColor, item);
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
                Rectangle imageRect = Rectangle.Empty;
                switch (_tLocation)
                {
                    case TILocation.Up:
                        int width = (_itemSize.Width - _imageSizeShow.Width - _textPading.Left - _textPading.Right) / 2;
                        imageRect.X = item.Rectangle.X + _textPading.Left + width;
                        imageRect.Y = item.Rectangle.Y + _textPading.Top;
                        break;
                    case TILocation.Left:
                        int height = (_itemSize.Height - _imageSizeShow.Height - _textPading.Top - _textPading.Bottom) / 2;
                        imageRect.X = item.Rectangle.X + _textPading.Left;
                        imageRect.Y = item.Rectangle.Y + _textPading.Top + height;
                        break;
                }
                imageRect.Size = this._imageSizeShow;
                item.ImageRect = imageRect;
                g.DrawImage(item.Image, imageRect);
            }
        }
        /// <summary>
        /// 绘制文字
        /// </summary>
        private void DrawText(Graphics g, ToolItem item)
        {
            Rectangle textRect = Rectangle.Empty;
            if (string.IsNullOrEmpty(item.Text)) item.Text = string.Empty;
            {
                textRect = new Rectangle
                {
                    X = item.Rectangle.X + _textPading.Left,
                    Y = item.Rectangle.Y + _textPading.Top,
                    Width = item.Rectangle.Width - _textPading.Left - _textPading.Right,
                    Height = item.Rectangle.Height - _textPading.Top - _textPading.Bottom,
                };
                if (_iImageShow && item.Image != null)
                {
                    switch (_tLocation)
                    {
                        case TILocation.Up:
                            textRect.Y += _imageSizeShow.Height + _textPading.Top;
                            textRect.Height = item.Rectangle.Height - _imageSizeShow.Height - _textPading.Top * 2 - _textPading.Bottom;
                            break;
                        case TILocation.Left:
                            textRect.X += _imageSizeShow.Width + _textPading.Left;
                            textRect.Width = item.Rectangle.Width - _imageSizeShow.Width - _textPading.Left * 2 - _textPading.Right;
                            break;
                    }
                }
                if (item.ContextMenuStrip != null)
                {
                    textRect.Width -= _rightLen;
                }
                int headHeight = 0;
                if (!string.IsNullOrEmpty(item.HeadDesc))
                {
                    headHeight = HeightFont(item.MouseState, THeadDesc);
                    Rectangle rect = new Rectangle()
                    {
                        X = textRect.X,
                        Y = textRect.Y,
                        Width = textRect.Width,
                        Height = headHeight,
                    };
                    DrawOtherDesc(g, item, THeadDesc, item.HeadDesc, rect);
                }
                int endHeight = 0;
                if (!string.IsNullOrEmpty(item.EndDesc))
                {
                    endHeight = HeightFont(item.MouseState, TEndDesc);
                    Rectangle rect = new Rectangle()
                    {
                        X = textRect.X,
                        Y = textRect.Y + textRect.Height - endHeight,
                        Width = textRect.Width,
                        Height = endHeight,
                    };
                    DrawOtherDesc(g, item, TEndDesc, item.EndDesc, rect);
                }
                if (!string.IsNullOrEmpty(item.Desc))
                {
                    Size size = TextRenderer.MeasureText(item.Desc, GetFont(item.IMouseState, TDesc));
                    textRect.Width -= size.Width;
                }
                if (this.IText || item.IText)
                {
                    Rectangle rect = new Rectangle(textRect.X, textRect.Y + headHeight, textRect.Width, textRect.Height - headHeight - endHeight);
                    DrawOtherDesc(g, item, TextFirst, item.Text, rect);
                }
                else
                {
                    string[] text = item.Text.Split(new string[] { "\r\n", "&" }, StringSplitOptions.RemoveEmptyEntries);
                    if (text.Length > 0)
                    {
                        int fHight = HeightFont(item.MouseState, TextFirst);
                        int sHight = HeightFont(item.MouseState, TextSencond);
                        int height = textRect.Height - fHight;
                        height -= (text.Length - 1) * sHight;
                        height -= (text.Length - 1) * _textSpace;
                        height /= 2;

                        Rectangle rect = new Rectangle()
                        {
                            X = textRect.X,
                            Y = textRect.Y + height,
                            Width = textRect.Width,
                            Height = fHight,
                        };
                        DrawOtherDesc(g, item, TextFirst, text[0], rect);
                        for (int i = 1; i < text.Length; i++)
                        {
                            rect = new Rectangle()
                            {
                                X = textRect.X + (fHight - sHight) / 2,
                                Y = textRect.Y + height + fHight + _textSpace * i + sHight * (i - 1),
                                Width = textRect.Width,
                                Height = sHight,
                            };
                            DrawOtherDesc(g, item, TextSencond, text[i], rect);
                        }
                    }
                }
            }
            DrawDesc(g, item, textRect);
        }
        private int HeightFont(TMouseState state, TProperties pro)
        {
            switch (state)
            {
                case TMouseState.Normal:
                case TMouseState.Leave:
                    return pro.HeightDown;
                case TMouseState.Move:
                case TMouseState.Up:
                    return pro.HeightMove;
                case TMouseState.Down:
                    return pro.HeightDown;
            }
            return 0;
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
            Size size = TextRenderer.MeasureText(item.Desc, GetFont(item.IMouseState, TDesc));
            item.RectDesc = new Rectangle(rect.X + rect.Width + (item.ContextMenuStrip == null ? 0 : 4),
                rect.Y + (rect.Height - size.Height) / 2, size.Width, size.Height);
            DrawOtherDesc(g, item, TDesc, item.Desc, item.RectDesc, item.IMouseState);
        }
        /// <summary>
        /// 绘制其它描述
        /// </summary>
        private void DrawOtherDesc(Graphics g, ToolItem item, TProperties desc, string text, Rectangle rect)
        {
            DrawOtherDesc(g, item, desc, text, rect, item.MouseState);
        }
        /// <summary>
        /// 绘制其它描述
        /// </summary>
        private void DrawOtherDesc(Graphics g, ToolItem item, TProperties desc, string text, Rectangle rect, TMouseState state)
        {
            if (string.IsNullOrEmpty(text)) return;

            Color color = this.ForeColor;
            Font font = desc.FontNormal;

            if (!item.Enable)
            {
                if (this.IText || item.IText)
                {
                    g.DrawString(text, font, new SolidBrush(color), rect, desc.StringFormat);
                }
                else
                {
                    Rectangle temp = new Rectangle(rect.X + Offset.X, rect.Y + Offset.Y, rect.Width, rect.Height);
                    TextRenderer.DrawText(g, text, font, temp, color, desc.TextFormat);
                }
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
            if (this.IText || item.IText)
            {
                g.DrawString(text, font, new SolidBrush(color), rect, desc.StringFormat);
            }
            else
            {
                Rectangle temp = new Rectangle(rect.X + Offset.X, rect.Y + Offset.Y, rect.Width, rect.Height);
                TextRenderer.DrawText(g, text, font, temp, color, desc.TextFormat);
            }
        }
        /// <summary>
        /// 判断右键菜单
        /// </summary>
        /// <returns>返回是否有焦点 true时不触发down事件</returns>
        private void IsContextMenu(Graphics g, ToolItem item)
        {
            Point point = this.PointToClient(MousePosition);
            point.X -= Offset.X;
            point.Y -= Offset.Y;
            Image btnArrowImage = null;
            int x = this._btnArrowRect.Left;
            if (item.RectDesc.Width != 0)
            {
                x = item.RectDesc.Left;
            }
            Point contextMenuLocation = this.PointToScreen(new Point(x + Offset.X, this._btnArrowRect.Top + Offset.Y + this._btnArrowRect.Height + 2));
            ContextMenuStrip contextMenuStrip = item.ContextMenuStrip;
            if (contextMenuStrip != null)
            {
                contextMenuStrip.Tag = item;
                contextMenuStrip.Opening -= contextMenuStrip_Opening;
                contextMenuStrip.Opening += contextMenuStrip_Opening;
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
        /// 当项菜单弹出时事件发生
        /// </summary>
        void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ToolItem item = (sender as ContextMenuStrip).Tag as ToolItem;
            OnEventOpening(item, e);
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
                    InvalidateItem(item, TMouseState.Normal);
                }
                InvaRectDesc(item, TMouseState.Normal);
            }
            InvalidateItem(item);
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
                this.Invalidate(new Rectangle(item.Rectangle.X + Offset.X, item.Rectangle.Y + Offset.Y, item.Rectangle.Width, item.Rectangle.Height));
            }
        }
        /// <summary>
        /// 重绘Item
        /// </summary>
        private void InvalidateItem(ToolItem item, TMouseState state)
        {
            if (item.MouseState != state)
            {
                item.MouseState = state;
                this.Invalidate(new Rectangle(item.Rectangle.X + Offset.X, item.Rectangle.Y + Offset.Y, item.Rectangle.Width, item.Rectangle.Height));
            }
        }
        /// <summary>
        /// 重绘Item
        /// </summary>
        /// <param name="item"></param>
        private void InvalidateItem(ToolItem item)
        {
            this.Invalidate(new Rectangle(item.Rectangle.X + Offset.X, item.Rectangle.Y + Offset.Y, item.Rectangle.Width, item.Rectangle.Height));
        }
        /// <summary>
        /// 引发 System.Windows.Forms.Form.MouseMove 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (INormal) return;
            if (this.DesignMode) return;

            Point point = e.Location;
            point.X -= Offset.X;
            point.Y -= Offset.Y;
            bool flag = true;
            foreach (ToolItem item in this.Items)
            {
                if (item.Rectangle.Contains(point))
                {
                    this.MoveItem = item;
                    if (item.RectDesc.Contains(point) || this._btnArrowRect.Contains(point))
                    {
                        if (item.IMouseState != TMouseState.Down)
                        {
                            InvaRectDesc(item, TMouseState.Move);
                        }
                    }
                    if (_iCheckEvent || item.MouseState != TMouseState.Down)
                    {
                        flag = false;
                        if (item.MouseState != TMouseState.Move && item.MouseState != TMouseState.Down)
                        {
                            InvalidateItem(item, TMouseState.Move);
                            if (IShowToolTop)
                            {
                                this.ShowTooTip(item.Sencond ?? item.First);
                            }
                        }
                    }
                }
                else
                {
                    InvaRectDesc(item, TMouseState.Normal);
                    if (_iCheckEvent || item.MouseState != TMouseState.Down)
                    {
                        InvalidateItem(item, TMouseState.Normal);
                    }
                }
            }
            if (flag)
            {
                this.HideToolTip();
            }
        }
        /// <summary>
        /// 引发 System.Windows.Forms.Form.MouseLeave 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_iFocus || INormal) return;
            if (this.DesignMode) return;

            _iDown = false;
            this.MoveItem = null;
            foreach (ToolItem item in this.Items)
            {
                InvaRectDesc(item, TMouseState.Normal);
                if ((_iCheckEvent && !_iMultiple) || item.MouseState != TMouseState.Down)
                {
                    InvalidateItem(item, TMouseState.Normal);
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
            if (e.Button != MouseButtons.Left || _iFocus) return;
            if (this.DesignMode) return;

            Point point = e.Location;
            point.X -= Offset.X;
            point.Y -= Offset.Y;
            bool contain = Contain(point) && !TContainDesc(point);

            for (int i = 0; i < this.Items.Count; i++)
            {
                ToolItem item = this.Items[i];
                if (item.Rectangle.Contains(point))
                {
                    OnMouseDown(point, item, e, contain);
                }
                else if (!INormal && !_iMultiple && contain)
                {
                    InvalidateItem(item, TMouseState.Normal);
                }
            }
        }
        private void OnMouseDown(Point point, ToolItem item, EventArgs e, bool contain)
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
                    this.OnSelectedItemChanged(item, e);
                }
            }
            //事件
            if (_tEvent == TEvent.Down)
            {
                if (item.RectDesc.Contains(point) || this._btnArrowRect.Contains(point))
                {
                    bool ifocus = false;
                    if (item.RectDesc.Contains(point))
                    {
                        ifocus = this.OnEditClick(item, e);
                    }
                    if (!ifocus && item.ContextMenuStrip != null)
                    {
                        this._iFocus = true;
                        InvalidateItem(item);
                    }
                }
                else
                {
                    this.OnItemClick(item, e);
                }
            }
            if (INormal) return;
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
                        InvalidateItem(item, TMouseState.Down);
                    }
                    else
                    {
                        InvalidateItem(item, TMouseState.Normal);
                    }
                }
                else
                {
                    InvalidateItem(item, TMouseState.Down);
                }
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
            if (this.DesignMode) return;

            Point point = e.Location;
            point.X -= Offset.X;
            point.Y -= Offset.Y;
            for (int i = 0; i < this.Items.Count; i++)
            {
                ToolItem item = this.Items[i];
                if (item.Rectangle.Contains(point))
                {
                    OnMouseUp(point, item, e);
                }
            }
        }
        private void OnMouseUp(Point point, ToolItem item, EventArgs e)
        {
            _iDown = false;
            //事件
            if (_tEvent == TEvent.Up && item == _tempItem)
            {
                if (item != this._selectedItem && !item.RectDesc.Contains(point))
                {
                    this._selectedItem = item;
                    this._selectedIndex = this.Items.GetIndexOfRange(item);
                    this.OnSelectedItemChanged(item, e);
                }
                if (item.RectDesc.Contains(point) || this._btnArrowRect.Contains(point))
                {
                    bool ifocus = false;
                    if (item.RectDesc.Contains(point))
                    {
                        ifocus = this.OnEditClick(item, e);
                    }
                    if (!ifocus && item.ContextMenuStrip != null)
                    {
                        this._iFocus = true;
                        InvalidateItem(item);
                    }
                }
                else
                {
                    this.OnItemClick(item, e);
                }
            }
            if (INormal) return;
            if (item.RectDesc.Contains(point) || this._btnArrowRect.Contains(point))
            {
                InvaRectDesc(item, TMouseState.Move);
            }
            else
            {
                InvaRectDesc(item, TMouseState.Normal);
            }
            if (!_iMultiple && _iCheckEvent && item.MouseState == TMouseState.Down)
            {
                InvalidateItem(item, TMouseState.Move);
            }
        }
        /// <summary>
        /// 引发 System.Windows.Forms.Form.MouseEnter 事件。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (iScrollHide && this.ParentForm.ContainsFocus)
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
            if (!ILoad) return;

            if (Items.Count > 1 && e.ListChangedType == ListChangedType.ItemAdded && e.NewIndex == Items.Count - 1)
            {
                int last = 0;
                switch (TDirection)
                {
                    case TDirection.Level:
                        last = TCountLine;
                        break;
                    case Forms.TDirection.Vertical:
                        last = TCountColumn;
                        break;
                }
                int count = this.Items.Count - 2;
                int xPos = this.Items[count].Rectangle.X;
                int yPos = this.Items[count].Rectangle.Y;
                Calctem(this.Items[count], ref xPos, ref yPos, false);
                Calctem(this.Items[count + 1], ref xPos, ref yPos, true);

                bool valid = false;
                switch (TDirection)
                {
                    case TDirection.Level:
                        valid = last != TCountLine;
                        break;
                    case Forms.TDirection.Vertical:
                        valid = last != TCountColumn;
                        break;
                }
                UpdateScroll(true, valid);
                this.InvalidateItem(this.Items[count + 1]);
            }
            else
            {
                if (this._selectedItem != null && e.ListChangedType == ListChangedType.ItemDeleted)
                {
                    int index = this.Items.GetIndexOfRange(this._selectedItem);
                    if (index == -1)
                    {
                        this._selectedItem = null;
                    }
                }
                this.TRefresh();
            }
        }
        /// <summary>
        /// 返回包含 System.ComponentModel.Component 的名称的 System.String（如果有）
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Tinn&a";
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// 获取所有选中项
        /// </summary>
        /// <returns></returns>
        public List<ToolItem> TSelectedItems()
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
        /// 坐标点是否包含在项描述中
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool TContainDesc(Point point)
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
        /// 移除项
        /// </summary>
        public void TRemove(string text)
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (this.Items[i].First == text)
                {
                    this.Items.RemoveAt(i);
                    break;
                }
            }
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
        public void TClickItem(string text)
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (this.Items[i].First == text)
                {
                    TClickItem(i);
                    break;
                }
            }
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
                    InvalidateItem(_items[i], TMouseState.Normal);
                }
            }
            if (index >= 0)
            {
                this._selectedItem = this._items[index];
                if (!_iCheckEvent)
                {
                    InvalidateItem(_selectedItem, TMouseState.Down);
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
            FixScroll(IHaveScroll ? value : 0);
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
            if (this.TCountLine < count) this.TCountLine = count;
            switch (TDirection)
            {
                case TDirection.Level:
                    this.THeight = GetHeight();
                    break;
                case TDirection.Vertical:
                    this.TWidth = GetWidth();
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
            InvalidateItem(this.Items[index]);
        }
        /// <summary>
        /// 刷新控件
        /// </summary>
        public void TRefresh()
        {
            _vScroll.Visible = false;
            _hScroll.Visible = false;
            _vScroll2.Visible = false;
            this.TPaint();
            UpdateScroll();
            this.Invalidate(this.ClientRectangle);
        }
        #endregion

        #region 动态显示项的图片
        private Timer tDynamic = new Timer();
        private PictureBox pictureBox1;
        /// <summary>
        /// 动态项原图片
        /// </summary>
        private Image image;
        /// <summary>
        /// 动态项
        /// </summary>
        private int progressItemIndex;
        /// <summary>
        /// 项的动态图片
        /// </summary>
        [Description("项的动态图片"), DefaultValue(null)]
        public Image TProgressImage
        {
            get { return pictureBox1.Image; }
            set { pictureBox1.Image = value; }
        }
        /// <summary>
        /// 初始化动态方法
        /// </summary>
        private void Progress()
        {
            tDynamic.Interval = 30;
            this.tDynamic.Tick += tDynamic_Tick;
        }
        /// <summary>
        /// 动态显示项的图像
        /// </summary>
        /// <param name="text">项文本</param>
        /// <param name="newText">项新文本</param>
        public void TProgressItem(string text, string newText = null)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Text == text)
                {
                    TProgressItem(i, newText);
                    break;
                }
            }
        }
        /// <summary>
        /// 动态显示项的图像
        /// </summary>
        /// <param name="index">项索引</param>
        /// <param name="newText">项新文本</param>
        public void TProgressItem(int index, string newText = null)
        {
            this.progressItemIndex = index;
            if (!string.IsNullOrEmpty(newText)) this._items[progressItemIndex].Text = newText;
            image = this._items[progressItemIndex].Image;
            if (pictureBox1.Image != null)
            {
                this._items[progressItemIndex].Image = pictureBox1.Image;
            }
            tDynamic.Enabled = true;
        }
        /// <summary>
        /// 停止动态显示
        /// </summary>
        /// <param name="image">图片</param>
        /// <param name="text">项文本</param>
        public void TProgressStop(Image image = null, string text = null)
        {
            tDynamic.Enabled = false;
            if (!string.IsNullOrEmpty(text)) this._items[progressItemIndex].Text = text;
            this._items[progressItemIndex].Image = image ?? this.image;
            tDynamic_Tick(this, EventArgs.Empty);
        }

        private void tDynamic_Tick(object sender, EventArgs e)
        {
            InvalidateItem(this._items[progressItemIndex]);
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
            this.Name = "ToolBar";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        #region 变色项
        private Timer tChange = new Timer();
        /// <summary>
        /// 动态项
        /// </summary>
        private int index;
        private void InitChange()
        {
            tChange.Interval = 600;
            tChange.Tick += tChange_Tick;
        }
        /// <summary>
        /// 开始变色
        /// </summary>
        public void TChangeStart()
        {
            tChange.Enabled = true;
        }
        /// <summary>
        /// 停止变色
        /// </summary>
        public void TChangeStop()
        {
            tChange.Enabled = false;
        }

        private void tChange_Tick(object sender, EventArgs e)
        {
            bool result = false;
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].IChange)
                {
                    result = true;
                    Color color = Items[i].TColor.ColorNormal;
                    switch (index % 3)
                    {
                        case 0:
                            Items[i].TColor.ColorNormal = TChange.ColorNormal;
                            break;
                        case 1:
                            Items[i].TColor.ColorNormal = TChange.ColorMove;
                            break;
                        case 2:
                            Items[i].TColor.ColorNormal = TChange.ColorDown;
                            break;
                    }
                    this.InvalidateItem(Items[i]);
                    Application.DoEvents();
                    Items[i].TColor.ColorNormal = color;
                }
            }
            index++;
            if (!result) TChangeStop();
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
        /// <summary>
        /// 隐藏的垂直滚动条，与 水平滚动条 联动，尝试解决平板横向无法滑动。
        /// </summary>
        private VScrollBar _vScroll2;
        /// <summary>
        /// 隐藏滚动条，实际有滚动效果，自动设置
        /// </summary>
        private bool iScrollHide;
        /// <summary>
        /// 是否显示滚动条
        /// </summary>
        private bool _iScroll = true;
        /// <summary>
        /// 是否显示滚动条
        /// </summary>
        [Description("是否显示滚动条"), DefaultValue(true)]
        public bool IScroll
        {
            get { return _iScroll; }
            set
            {
                _iScroll = value;
                TRefresh();
            }
        }
        /// <summary>
        /// 获取滚动条是否有显示
        /// </summary>
        [Browsable(false), DefaultValue(false)]
        public bool IHaveScroll
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
        /// 获取当前滚动条的值
        /// </summary>
        [Browsable(false), DefaultValue(0)]
        public int IScrollValue
        {
            get
            {
                switch (TDirection)
                {
                    case Forms.TDirection.Level:
                        return _vScroll.Value;
                    case Forms.TDirection.Vertical:
                        return _hScroll.Value;
                }
                return 0;
            }
        }
        /// <summary>
        /// 控件显示区域宽度
        /// </summary>
        private int TWidth
        {
            get { return _vScroll.Visible ? this.Width - _vScroll.Width : this.Width; }
            set { this.Width = value; }
        }
        /// <summary>
        /// 控件显示区域高度
        /// </summary>
        private int THeight
        {
            get { return _hScroll.Visible ? this.Height - _hScroll.Height : this.Height; }
            set { this.Height = value; }
        }
        /// <summary>
        /// 控件显示偏移坐标
        /// </summary>
        private Point Offset = Point.Empty;
        /// <summary>
        /// 初始化滚动条
        /// </summary>
        private void CustomScroll()
        {
            _hScroll = new HScrollBar();
            _hScroll.Scroll += _hScroll_Scroll;
            _hScroll.Dock = DockStyle.Bottom;
            _hScroll.Height = _tScrollHeight;
            Controls.Add(_hScroll);
            {
                _vScroll2 = new VScrollBar();
                //_vScroll2.Scroll += _hScroll_Scroll;
                _vScroll2.Dock = DockStyle.Right;
                _vScroll2.Width = 0;
                Controls.Add(_vScroll2);
            }

            _vScroll = new VScrollBar();
            _vScroll.Scroll += _vScroll_Scroll;
            _vScroll.Dock = DockStyle.Right;
            _vScroll.Width = _tScrollHeight;
            Controls.Add(_vScroll);

            _hScroll.Visible = false;
            _vScroll.Visible = false;
            _vScroll2.Visible = false;
        }

        /// <summary>
        /// 将界面坐标转为控件坐标
        /// </summary>
        public Point Replace(Point point)
        {
            return new Point(point.X - Offset.X, point.Y - Offset.Y);
        }

        /// <summary>
        /// 重写滚轮滚动
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (!iScrollHide) return;
            switch (TDirection)
            {
                case TDirection.Level:
                    int value = _vScroll.Value - e.Delta * this.ItemSize.Height / 120;
                    FixScroll(value);
                    break;
                case TDirection.Vertical:
                    value = _vScroll2.Value - e.Delta * this.ItemSize.Width / 120;
                    FixScroll(value);
                    break;
            }
            base.OnMouseWheel(e);
        }
        /// <summary>
        /// 滚动到指定值位置
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valid">是否需要重绘，默认重绘</param>
        private void FixScroll(int value, bool valid = true)
        {
            switch (TDirection)
            {
                case TDirection.Level:
                    int max = _vScroll.Maximum - _vScroll.SmallChange;
                    if (value < 0) value = 0;
                    if (value > max) value = max;
                    _vScroll.Value = value;
                    Offset.Y = -value;
                    break;
                case TDirection.Vertical:
                    max = _vScroll2.Maximum - _vScroll2.SmallChange;
                    if (value < 0) value = 0;
                    if (value > max) value = max;
                    _hScroll.Value = value;
                    _vScroll2.Value = value;
                    Offset.X = -value;
                    break;
            }
            if (valid)
            {
                this.Invalidate(this.ClientRectangle);
            }
        }
        /// <summary>
        /// 大小改变时
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            _vScroll.Visible = false;
            _hScroll.Visible = false;
            _vScroll2.Visible = false;
            this.TPaint();
            UpdateScroll();
        }
        /// <summary>
        /// 更新滚动条状态
        /// </summary>
        private void UpdateScroll(bool toLast = false, bool toValid = false)
        {
            bool valid = _vScroll.Visible || _hScroll.Visible;
            _vScroll.Visible = false;
            _hScroll.Visible = false;
            _vScroll2.Visible = false;
            iScrollHide = false;
            switch (TDirection)
            {
                case TDirection.Level:
                    int height = GetHeight();
                    if (this.THeight < height)
                    {
                        iScrollHide = true;
                        _vScroll.Visible = _iScroll;
                    }
                    else if (_vScroll.Value >= 0)
                    {
                        FixScroll(0, false);
                    }
                    break;
                case TDirection.Vertical:
                    int width = GetWidth();
                    if (this.TWidth < width)
                    {
                        iScrollHide = true;
                        _hScroll.Visible = _iScroll;
                        _vScroll2.Visible = _iScroll;
                    }
                    else if (_vScroll2.Value >= 0)
                    {
                        FixScroll(0, false);
                    }
                    break;
            }
            if (iScrollHide && _iScroll)
            {
                if (!toLast)
                {
                    this.TPaint();
                }
                switch (TDirection)
                {
                    case TDirection.Level:
                        int max = GetHeight() - this.Height;
                        if (_vScroll.Value > max)
                        {
                            FixScroll(max, false);
                        }
                        _vScroll.Maximum = GetHeight();
                        _vScroll.LargeChange = this.Height / 2;
                        _vScroll.SmallChange = _vScroll.LargeChange;
                        _vScroll.Maximum = max + _vScroll.LargeChange;

                        valid = !valid | toLast & toValid;
                        FixScroll(toLast ? _vScroll.Maximum : 0, valid);
                        break;
                    case TDirection.Vertical:
                        max = GetWidth() - this.Width;
                        if (_vScroll2.Value > max)
                        {
                            FixScroll(max, false);
                        }
                        _vScroll2.Maximum = GetWidth();
                        _vScroll2.LargeChange = this.Width / 2;
                        _vScroll2.SmallChange = _vScroll2.LargeChange;
                        _vScroll2.Maximum = max + _vScroll2.LargeChange;
                        {
                            _hScroll.LargeChange = _vScroll2.LargeChange;
                            _hScroll.SmallChange = _vScroll2.SmallChange;
                            _hScroll.Maximum = _vScroll2.Maximum;
                        }
                        valid = !valid | toLast & toValid;
                        FixScroll(toLast ? _vScroll2.Maximum : 0, valid);
                        break;
                }
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
                Offset.X = 0;
            }
            Offset.Offset(-diff, 0);
            _vScroll2.Value = _hScroll.Value;
            if (diff != 0 || e.NewValue == 0)
            {
                this.Invalidate(this.ClientRectangle);
            }
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
                Offset.Y = 0;
            }
            Offset.Offset(0, -diff);
            if (diff != 0 || e.NewValue == 0)
            {
                this.Invalidate(this.ClientRectangle);
            }
        }

        /// <summary>
        /// 需要的高度
        /// </summary>
        /// <returns></returns>
        private int GetHeight()
        {
            int height = this.Padding.Top + this.Padding.Bottom;
            height += this.TCountLine * this.ItemSize.Height;
            height += (this.TCountLine - 1) * this.ItemSpace;
            height += this.THeardLength;
            return height;
        }
        /// <summary>
        /// 需要的宽度
        /// </summary>
        /// <returns></returns>
        private int GetWidth()
        {
            int width = this.Padding.Left + this.Padding.Right;
            width += this.TCountColumn * this.ItemSize.Width;
            width += (this.TCountColumn - 1) * this.ItemSpace;
            width += this.THeardLength;
            return width;
        }

        #endregion

        #region 悬停窗口显示说明
        /// <summary>
        /// 表示一个长方形的小弹出窗口，该窗口在用户将指针悬停在一个控件上时显示有关该控件用途的简短说明。
        /// </summary>
        protected void ShowTooTip(string toolTipText)
        {
            this.toolTop.Active = true;
            this.toolTop.SetToolTip(this, toolTipText);
        }
        /// <summary>
        /// 弹出窗口不活动
        /// </summary>
        protected void HideToolTip()
        {
            if (this.toolTop.Active)
            {
                this.toolTop.Active = false;
            }
        }

        #endregion
    }
}
