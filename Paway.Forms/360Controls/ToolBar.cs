using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Paway.Helper;
using Paway.Resource;
using System.Threading.Tasks;

namespace Paway.Forms
{
    /// <summary>
    ///     工具栏
    /// </summary>
    [DefaultProperty("Items")]
    [DefaultEvent("SelectedItemChanged")]
    public class ToolBar : TControl
    {
        #region 构造函数

        /// <summary>
        ///     初始化 Paway.Forms.ToolBar 新的实例。
        /// </summary>
        public ToolBar()
        {
            InitializeComponent();
            Progress();
            InitChange();
            CustomScroll();
            toolTop = new ToolTip();
            this.MouseEnter += ToolBar_MouseEnter;
            _vScroll.MouseEnter += ToolBar_MouseEnter;
            _hScroll.MouseEnter += ToolBar_MouseEnter;
            this.MouseLeave += ToolBar_MouseLeave;
            _vScroll.MouseLeave += ToolBar_MouseLeave;
            _hScroll.MouseLeave += ToolBar_MouseLeave;
        }
        private void ToolBar_MouseLeave(object sender, EventArgs e)
        {
            iMouseStatu = false;
            AutoMouseStatu();
        }
        private void ToolBar_MouseEnter(object sender, EventArgs e)
        {
            iMouseStatu = true;
            AutoMouseStatu();
        }

        #endregion

        #region Private Method

        /// <summary>
        ///     更新图片区域
        /// </summary>
        private void UpdateImageSize()
        {
            if (_iImageShow)
            {
                _imageSizeShow = _imageSize;
                switch (_tLocation)
                {
                    case TILocation.Up:
                        var width = (_itemSize.Width - _imageSize.Width - _textPading.Left - _textPading.Right) / 2;
                        if (width < 0)
                        {
                            _imageSizeShow.Width = _itemSize.Width - _textPading.Left - _textPading.Right;
                            _imageSizeShow.Height =
                                (_imageSizeShow.Width * _imageSize.Height * 1.0 / _imageSize.Width).ToInt();
                        }
                        break;
                    case TILocation.Left:
                        var height = (_itemSize.Height - _imageSize.Height - _textPading.Top - _textPading.Bottom) / 2;
                        if (height < 0)
                        {
                            _imageSizeShow.Height = _itemSize.Height - _textPading.Top - _textPading.Bottom;
                            _imageSizeShow.Width =
                                (_imageSizeShow.Height * _imageSize.Width * 1.0 / _imageSize.Height).ToInt();
                        }
                        break;
                }
            }
        }

        #endregion

        #region 资源图片

        /// <summary>
        ///     默认时的按钮图片
        /// </summary>
        private Image _normalImage;
        private readonly Image _normalImage2 = AssemblyHelper.GetImage("_360.ToolBar.toolbar_normal.png");
        /// <summary>
        ///     默认图片
        /// </summary>
        [DefaultValue(null)]
        [Description("默认时的按钮图片")]
        public virtual Image NormalImage
        {
            get { return _normalImage; }
            set
            {
                _normalImage = value;
                Invalidate(ClientRectangle);
            }
        }

        /// <summary>
        ///     鼠标按下时的图片
        /// </summary>
        private Image _downImage;
        private readonly Image _downImage2 = AssemblyHelper.GetImage("_360.ToolBar.toolbar_hover.png");
        /// <summary>
        ///     鼠标按下时的图片
        /// </summary>
        [DefaultValue(null)]
        [Description("鼠标按下时的图片")]
        public virtual Image DownImage
        {
            get { return _downImage; }
            set
            {
                _downImage = value;
                Invalidate(ClientRectangle);
            }
        }

        /// <summary>
        ///     鼠标划过时的图片
        /// </summary>
        private Image _moveImage;
        private readonly Image _moveImage2 = AssemblyHelper.GetImage("_360.ToolBar.toolbar_pushed.png");
        /// <summary>
        ///     鼠标划过时的图片
        /// </summary>
        [DefaultValue(null)]
        [Description("鼠标划过时的图片")]
        public virtual Image MoveImage
        {
            get { return _moveImage; }
            set
            {
                _moveImage = value;
                Invalidate(ClientRectangle);
            }
        }

        /// <summary>
        ///     多选状态下选中时附加的图片
        /// </summary>
        private Image _selectImage;
        /// <summary>
        ///     多选状态下选中时附加的图片
        /// </summary>
        private readonly Image _selectImage2 = AssemblyHelper.GetImage("Controls.accept_16.png");
        /// <summary>
        ///     多选状态下选中时附加的图片
        /// </summary>
        [DefaultValue(null)]
        [Description("多选状态下选中时附加的图片")]
        public virtual Image SelectImage
        {
            get { return _selectImage; }
            set
            {
                _selectImage = value;
                Invalidate(ClientRectangle);
            }
        }

        private readonly Image main_tabbtn_downImage = AssemblyHelper.GetImage("QQ.TabControl.main_tabbtn_down.png");
        private readonly Image main_tabbtn_highlightImage = AssemblyHelper.GetImage("QQ.TabControl.main_tabbtn_highlight.png");

        #endregion

        #region 私有变量

        /// <summary>
        ///     鼠标弹按下
        /// </summary>
        private bool _iDown;

        /// <summary>
        ///     右键弹出
        /// </summary>
        private bool _iFocus;

        /// <summary>
        ///     选项卡箭头区域
        /// </summary>
        private Rectangle _btnArrowRect = Rectangle.Empty;

        /// <summary>
        ///     类于右键菜单长度
        /// </summary>
        private readonly int _rightLen = 19;

        /// <summary>
        ///     悬停窗口
        /// </summary>
        private readonly ToolTip toolTop;

        /// <summary>
        ///     按下抬起项是否相同中用的过度项
        /// </summary>
        private ToolItem _tempItem;

        #endregion

        #region 公共属性

        #region Int

        /// <summary>
        ///     圆角大小
        /// </summary>
        private int _tRadiu;

        /// <summary>
        ///     圆角大小
        /// </summary>
        [Description("圆角大小"), DefaultValue(0)]
        public int TRadiu
        {
            get { return _tRadiu; }
            set
            {
                _tRadiu = value;
                Invalidate(ClientRectangle);
            }
        }

        /// <summary>
        ///     滚动条宽度
        /// </summary>
        private int _tScrollHeight = 5;

        /// <summary>
        ///     滚动条宽度
        /// </summary>
        [Description("滚动条宽度"), DefaultValue(5)]
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
                    panelScroll.Height = value;
                }
                if (_vScroll != null)
                {
                    _vScroll.Width = value;
                    panelScroll.Width = value;
                }
            }
        }

        /// <summary>
        ///     头文字总长度(占用总长度/宽度)
        /// </summary>
        [Browsable(false), Description("头文字总长度(占用总长度/宽度)"), DefaultValue(0)]
        public int THeardLength { get; private set; }

        /// <summary>
        ///     项文本间的间隔
        /// </summary>
        private int _textSpace = 6;

        /// <summary>
        ///     项文本间的间隔
        /// </summary>
        [Description("项文本间的间隔"), DefaultValue(6)]
        public int TextSpace
        {
            get { return _textSpace; }
            set
            {
                _textSpace = value;
                TRefresh();
            }
        }

        /// <summary>
        ///     多行列排列时的行数
        /// </summary>
        [Browsable(false), Description("多行列排列时的行数"), DefaultValue(1)]
        public int TCountLine { get; private set; }

        /// <summary>
        ///     多行列排列时的列数
        /// </summary>
        [Browsable(false), Description("多行列排列时的列数"), DefaultValue(1)]
        public int TCountColumn { get; private set; }

        #endregion

        #region bool
        private bool _iShowTop = true;
        /// <summary>
        ///     显示简短说明
        /// </summary>
        [Description("显示简短说明"), DefaultValue(true)]
        public bool IShowTop { get { return _iShowTop; } set { _iShowTop = value; } }

        private bool _iText;
        /// <summary>
        ///     显示为文本内容
        /// </summary>
        [Description("显示为文本内容"), DefaultValue(false)]
        public bool IText
        {
            get { return _iText; }
            set
            {
                _iText = value;
                Invalidate(ClientRectangle);
            }
        }

        /// <summary>
        ///     普通项，不响应鼠标绘制
        /// </summary>
        [Description("普通项，不响应鼠标绘制"), DefaultValue(false)]
        public bool INormal { get; set; }
        private bool _iItemLine;
        /// <summary>
        ///     绘制边框线开关
        /// </summary>
        [Description("绘制边框线开关"), DefaultValue(false)]
        public bool IItemLine
        {
            get { return _iItemLine; }
            set
            {
                _iItemLine = value;
                Invalidate(ClientRectangle);
            }
        }

        /// <summary>
        ///     补充整行\列
        /// </summary>
        private bool _iAdd;
        /// <summary>
        ///     补充整行\列
        /// </summary>
        [Description("补充整行\\列"), DefaultValue(false)]
        public bool IAdd
        {
            get { return _iAdd; }
            set
            {
                _iAdd = value;
                Invalidate(ClientRectangle);
            }
        }

        /// <summary>
        ///     单击事件开关
        ///     单击松开后取消选中状态，只有鼠标移入状态
        /// </summary>
        [Description("单击事件开关"), DefaultValue(false)]
        public bool ICheckEvent { get; set; }

        /// <summary>
        ///     图片显示开关
        /// </summary>
        private bool _iImageShow;
        /// <summary>
        ///     图片显示开关
        /// </summary>
        [Description("图片显示开关"), DefaultValue(false)]
        public bool IImageShow
        {
            get { return _iImageShow; }
            set
            {
                _iImageShow = value;
                UpdateImageSize();
                Invalidate(ClientRectangle);
            }
        }

        /// <summary>
        ///     多选开关
        /// </summary>
        private bool _iMultiple;
        /// <summary>
        ///     多选开关
        /// </summary>
        [Description("多选开关"), DefaultValue(false)]
        public bool IMultiple
        {
            get { return _iMultiple; }
            set
            {
                _iMultiple = value;
                Invalidate(ClientRectangle);
            }
        }

        /// <summary>
        ///     自动项宽度
        /// </summary>
        private bool _iAutoWidth;
        /// <summary>
        ///     自动项宽度
        /// </summary>
        [Description("自动项宽度"), DefaultValue(false)]
        public bool IAutoWidth
        {
            get { return _iAutoWidth; }
            set
            {
                _iAutoWidth = value;
                TRefresh();
            }
        }

        #endregion

        #region 其它

        /// <summary>
        ///     获取或设置项内的空白
        /// </summary>
        private Padding _textPading = new Padding(2);

        /// <summary>
        ///     获取或设置项内的空白
        /// </summary>
        [Description("获取或设置项内的空白"), DefaultValue(typeof(Padding), "2,2,2,2")]
        public Padding TextPading
        {
            get { return _textPading; }
            set
            {
                _textPading = value;
                TRefresh();
            }
        }

        /// <summary>
        ///     事件触发点
        /// </summary>
        private TEvent _tEvent = TEvent.Up;

        /// <summary>
        ///     事件触发点
        /// </summary>
        [Description("事件触发点"), DefaultValue(typeof(TEvent), "Up")]
        public TEvent TEvent
        {
            get { return _tEvent; }
            set { _tEvent = value; }
        }

        /// <summary>
        ///     Item项显示方向
        /// </summary>
        private TDirection _tDirection = TDirection.Level;

        /// <summary>
        ///     Item项显示方向
        /// </summary>
        [Description("Item项显示方向"), DefaultValue(typeof(TDirection), "Level")]
        public TDirection TDirection
        {
            get { return _tDirection; }
            set
            {
                _tDirection = value;
                TRefresh();
            }
        }

        /// <summary>
        ///     图片显示位置
        /// </summary>
        private TILocation _tLocation = TILocation.Up;

        /// <summary>
        ///     图片显示位置
        /// </summary>
        [Description("图片显示位置"), DefaultValue(typeof(TILocation), "Up")]
        public TILocation TLocation
        {
            get { return _tLocation; }
            set
            {
                _tLocation = value;
                Invalidate(ClientRectangle);
            }
        }

        #endregion

        #region 数据项

        /// <summary>
        ///     工具栏中的项列表
        /// </summary>
        private ToolItemCollection _items;

        /// <summary>
        ///     工具栏中的项列表
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("工具栏中的项列表"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ToolItemCollection Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new ToolItemCollection(this);
                    _items.ListChanged += _items_ListChanged;
                }
                return _items;
            }
        }

        /// <summary>
        ///     项的大小
        /// </summary>
        private Size _itemSize = new Size(78, 82);

        /// <summary>
        ///     项的大小
        /// </summary>
        [Description("项的大小"), DefaultValue(typeof(Size), "78,82")]
        public Size ItemSize
        {
            get { return _itemSize; }
            set
            {
                _itemSize = value;
                _vScroll.Visible = false;
                _hScroll.Visible = false;
                _vScroll2.Visible = false;
                panelScroll.Visible = false;
                TPaint();
                UpdateImageSize();
                UpdateScroll();
                Invalidate(ClientRectangle);
            }
        }

        /// <summary>
        ///     项与项之间的间隔
        /// </summary>
        private int _itemSpace = 1;

        /// <summary>
        ///     项与项之间的间隔
        /// </summary>
        [Description("项与项之间的间隔"), DefaultValue(1)]
        public int ItemSpace
        {
            get { return _itemSpace; }
            set
            {
                _itemSpace = value;
                TRefresh();
            }
        }

        /// <summary>
        ///     项图片的大小
        /// </summary>
        private Size _imageSize = new Size(48, 48);

        /// <summary>
        ///     项图片显示区域大小
        /// </summary>
        private Size _imageSizeShow = Size.Empty;

        /// <summary>
        ///     项图片的大小
        /// </summary>
        [Description("项图片的大小"), DefaultValue(typeof(Size), "48,48")]
        public Size ImageSize
        {
            get { return _imageSize; }
            set
            {
                _imageSize = value;
                UpdateImageSize();
                Invalidate(ClientRectangle);
            }
        }

        /// <summary>
        ///     当前选中项
        /// </summary>
        private ToolItem _selectedItem;

        /// <summary>
        ///     当前选中项
        /// </summary>
        [Browsable(false), Description("当前选中项")]
        public ToolItem SelectedItem
        {
            get { return _selectedItem; }
        }

        /// <summary>
        ///     当前移入项
        /// </summary>
        [Browsable(false), Description("当前移入项"), DefaultValue(null)]
        public ToolItem MoveItem { get; set; }

        /// <summary>
        ///     选中项的索引
        /// </summary>
        private int _selectedIndex;

        /// <summary>
        ///     选中项的索引
        /// </summary>
        [Browsable(false), Description("选中项的索引"), DefaultValue(0)]
        public int SelectedIndex
        {
            get { return _selectedIndex; }
        }

        #endregion

        #region 字体、颜色属性

        private TProperties _change;

        /// <summary>
        ///     变色项颜色
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
                    _change.ValueChange += delegate { Invalidate(ClientRectangle); };
                }
                return _change;
            }
        }

        private TProperties _text;

        /// <summary>
        ///     文字
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
                    _text.ValueChange += delegate { Invalidate(ClientRectangle); };
                }
                return _text;
            }
        }

        private TProperties _textSencond;

        /// <summary>
        ///     文字
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
                    _textSencond.ValueChange += delegate { Invalidate(ClientRectangle); };
                }
                return _textSencond;
            }
        }

        private TProperties _desc;

        /// <summary>
        ///     正文描述
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
                    _desc.ValueChange += delegate { Invalidate(ClientRectangle); };
                }
                return _desc;
            }
        }

        private TProperties _headDesc;

        /// <summary>
        ///     头部描述
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
                    _headDesc.ValueChange += delegate { Invalidate(ClientRectangle); };
                }
                return _headDesc;
            }
        }

        private TProperties _endDesc;

        /// <summary>
        ///     尾部描述
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
                    _endDesc.ValueChange += delegate { Invalidate(ClientRectangle); };
                }
                return _endDesc;
            }
        }

        private TProperties _backGround;

        /// <summary>
        ///     背景
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
                    _backGround.ValueChange += delegate { Invalidate(ClientRectangle); };
                }
                return _backGround;
            }
        }

        #endregion

        /// <summary>
        ///     重写父类的默认大小
        /// </summary>
        protected override Size DefaultSize
        {
            get { return new Size(300, 82); }
        }

        /// <summary>
        ///     获取或设置控件内的空白。
        /// </summary>
        [Description("获取或设置控件内的空白"), DefaultValue(typeof(Padding), "0, 0, 0, 0")]
        public new Padding Padding
        {
            get { return base.Padding; }
            set
            {
                base.Padding = value;
                TRefresh();
            }
        }

        #endregion

        #region 事件定义

        #region 事件对像

        /// <summary>
        ///     当选中项的发生改变时事件的 Key
        /// </summary>
        private static readonly object EventSelectedItemChanged = new object();

        /// <summary>
        ///     当单击项时事件的 Key
        /// </summary>
        private static readonly object EventItemClick = new object();

        /// <summary>
        ///     当单击项编辑时事件的 Key
        /// </summary>
        private static readonly object EventEditClick = new object();

        /// <summary>
        ///     当项菜单弹出时事件
        /// </summary>
        private static readonly object EventOpening = new object();

        #endregion

        #region 激发事件的方法

        /// <summary>
        ///     当选择的 Item 发生改变时激发。
        /// </summary>
        /// <param name="item"></param>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        public virtual void OnSelectedItemChanged(ToolItem item, EventArgs e)
        {
            if (!item.Enable) return;
            var handler = Events[EventSelectedItemChanged] as EventHandler;
            if (handler != null)
            {
                item.Owner = this;
                handler(item, e);
            }
        }

        /// <summary>
        ///     当单击项时激发。
        /// </summary>
        /// <param name="item"></param>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        public virtual void OnItemClick(ToolItem item, EventArgs e)
        {
            if (!item.Enable) return;
            if (ICheckEvent)
            {
                var handler = Events[EventItemClick] as EventHandler;
                if (handler != null)
                {
                    item.Owner = this;
                    handler(item, e);
                }
            }
        }

        /// <summary>
        ///     当编辑项时激发。
        /// </summary>
        /// <param name="item"></param>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        public virtual bool OnEditClick(ToolItem item, EventArgs e)
        {
            if (!item.Enable) return false;
            var handler = Events[EventEditClick] as EventHandler;
            if (handler != null)
            {
                item.Owner = this;
                handler(item, e);
                return true;
            }
            return false;
        }

        /// <summary>
        ///     当项菜单弹出时事件发生。
        /// </summary>
        /// <param name="item"></param>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        public virtual bool OnEventOpening(ToolItem item, EventArgs e)
        {
            if (!item.Enable) return false;
            var handler = Events[EventOpening] as EventHandler;
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
        ///     当选中项的发生改变时
        /// </summary>
        public event EventHandler SelectedItemChanged
        {
            add { Events.AddHandler(EventSelectedItemChanged, value); }
            remove { Events.RemoveHandler(EventSelectedItemChanged, value); }
        }

        /// <summary>
        ///     当单击项时事件发生
        /// </summary>
        public event EventHandler ItemClick
        {
            add { Events.AddHandler(EventItemClick, value); }
            remove { Events.RemoveHandler(EventItemClick, value); }
        }

        /// <summary>
        ///     当编辑项时事件发生
        /// </summary>
        public event EventHandler EditClick
        {
            add { Events.AddHandler(EventEditClick, value); }
            remove { Events.RemoveHandler(EventEditClick, value); }
        }

        /// <summary>
        ///     当编辑项时事件发生
        /// </summary>
        public event EventHandler MenuOpening
        {
            add { Events.AddHandler(EventOpening, value); }
            remove { Events.RemoveHandler(EventOpening, value); }
        }

        #endregion

        #region Override Methods
        /// <summary>
        ///     引发 System.Windows.Forms.Form.Paint 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.PaintEventArgs。</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.TranslateTransform(Offset.X, Offset.Y);
            g.PixelOffsetMode = PixelOffsetMode.Half; //与AntiAlias作用相反
            var temp = g.VisibleClipBounds;
            //修理毛边
            temp = new RectangleF(temp.X - 1, temp.Y - 1, temp.Width + 2, temp.Height + 2);
            TranColor = TBackGround.ColorSpace;
            g.FillRectangle(new SolidBrush(TranColor), temp);

            ////多线程处理(GDI是同一个，占用更多CPU，绘制效率有提升吗)
            //Parallel.For(0, Items.Count, (i) =>
            //{
            //    DrawItem(g, Items[i]);
            //});
            //for (var i = 0; i < Items.Count; i++)
            //{
            //    DrawItemAysc(g, Items[i]);
            //}
            for (var i = 0; i < Items.Count; i++)
            {
                DrawItem(g, Items[i]);
                DrawItemAysc(g, Items[i]);
            }
            if (_iAdd)
            {
                AddItem(g);
            }
        }

        /// <summary>
        ///     计算宽高
        /// </summary>
        private void TPaint()
        {
            var xPos = Padding.Left;
            var yPos = Padding.Top;
            TCountColumn = 1;
            TCountLine = 1;
            THeardLength = 0;
            isLastNew = true;
            for (var i = 0; i < Items.Count; i++)
            {
                Calctem(Items[i], ref xPos, ref yPos, i == Items.Count - 1);
            }
        }

        private void AddItem(Graphics g)
        {
            var count = Items.Count - 1;
            var xPos = Items[count].Rectangle.X;
            var yPos = Items[count].Rectangle.Y;
            Calctem(Items[count], ref xPos, ref yPos, false);
            count = 0;
            //多行/列补充Item
            if (TCountColumn > 1 && TCountLine > 1)
            {
                count = TCountColumn > TCountLine ? TCountColumn : TCountLine;
                if (Items.Count % count == 0) count = 0;
                else count = count - Items.Count % count;
            }
            if (_iAdd)
            {
                //填充空Item
                for (var i = 0; i < count; i++)
                {
                    var temp = new ToolItem();
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
                    var temp = new Rectangle(item.Rectangle.X, item.Rectangle.Y + Offset.Y, item.Rectangle.Width, item.Rectangle.Height);
                    switch (_tDirection)
                    {
                        case TDirection.Level:
                            TextRenderer.DrawText(g, item.Text, item.TColor.FontNormal, temp, item.TColor.ColorNormal, item.TColor.TextFormat);
                            break;
                        case TDirection.Vertical:
                            var color = item.TColor.ColorNormal;
                            if (color == Color.Empty) color = Color.Black;
                            g.DrawString(item.Text, item.TColor.FontNormal, new SolidBrush(color), temp,
                            item.TColor.StringFormat);
                            break;
                    }
                }
            }
        }

        private void Calctem(ToolItem item, ref int xPos, ref int yPos, bool iLast)
        {
            // 当前 Item 所在的矩型区域
            item.Rectangle = new Rectangle(xPos, yPos, _itemSize.Width, _itemSize.Height);
            if (_iAutoWidth)
            {
                Graphics g = this.CreateGraphics();
                if (_iText || item.IText)
                {
                    SizeF size1 = g.MeasureString(item.Text, TextFirst.FontNormal, item.Rectangle.Size);
                    item.Rectangle = new Rectangle(item.Rectangle.X, item.Rectangle.Y, size1.Width.ToInt() + 1 + _textPading.Left + _textPading.Right, item.Rectangle.Height);
                }
                else
                {
                    SizeF size1 = TextRenderer.MeasureText(item.Text, TextFirst.FontNormal, item.Rectangle.Size);
                    item.Rectangle = new Rectangle(item.Rectangle.X, item.Rectangle.Y, size1.Width.ToInt() + _textPading.Left + _textPading.Right, item.Rectangle.Height);
                }
            }
            var size = TextRenderer.MeasureText("你", Font);
            switch (_tDirection)
            {
                case TDirection.Level:
                    var isNew = xPos + item.Rectangle.Width + _itemSize.Width + _itemSpace + Padding.Right > Width;
                    if (item.IHeard || isNew)
                    {
                        xPos = Padding.Left;
                        if (!iLast)
                        {
                            if (item.IHeard)
                            {
                                if (!isLastNew)
                                {
                                    yPos += _itemSize.Height + _itemSpace;
                                    TCountLine++;
                                }
                                item.Rectangle = new Rectangle(xPos, yPos, Width, size.Height);
                                THeardLength += size.Height + _itemSpace * 2;
                            }
                            else
                            {
                                TCountLine++;
                            }
                            yPos += item.Rectangle.Height + _itemSpace;
                        }
                    }
                    else
                    {
                        xPos += item.Rectangle.Width + _itemSpace;
                        if (TCountLine == 1 && !iLast) TCountColumn++;
                    }
                    isLastNew = isNew;
                    break;
                case TDirection.Vertical:
                    isNew = yPos + item.Rectangle.Height * 2 + _itemSpace + Padding.Bottom > Height;
                    if (item.IHeard || isNew)
                    {
                        yPos = Padding.Top;
                        if (!iLast)
                        {
                            if (item.IHeard)
                            {
                                if (!isLastNew)
                                {
                                    xPos += _itemSize.Width + _itemSpace;
                                    TCountColumn++;
                                }
                                item.Rectangle = new Rectangle(xPos, yPos, size.Width, Height);
                                THeardLength += size.Width + _itemSpace * 2;
                            }
                            else
                            {
                                TCountColumn++;
                            }
                            xPos += item.Rectangle.Width + _itemSpace;
                        }
                    }
                    else
                    {
                        yPos += item.Rectangle.Height + _itemSpace;
                        if (TCountColumn == 1 && !iLast) TCountLine++;
                    }
                    isLastNew = isNew;
                    break;
            }
        }

        private void DrawItem(Graphics g, ToolItem item)
        {
            RectangleF temp = RectangleF.Empty;
            temp = RectangleF.Intersect(g.VisibleClipBounds, item.Rectangle);
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
        /// 同步绘制
        /// </summary>
        private void DrawItemAysc(Graphics g, ToolItem item)
        {
            if (item.IHeard) return;
            RectangleF temp = RectangleF.Intersect(g.VisibleClipBounds, item.Rectangle);
            if (temp == RectangleF.Empty) return;

            if (!item.Enable)
            {
                item.MouseState = TMouseState.Normal;
                item.IMouseState = TMouseState.Normal;
            }
            switch (item.MouseState)
            {
                case TMouseState.Move:
                case TMouseState.Up:
                case TMouseState.Down:
                    IsContextMenu(g, item);
                    break;
            }
        }

        /// <summary>
        ///     绘制背景
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
                        TranColor = item.TColor.ColorNormal == Color.Empty
                            ? TBackGround.ColorNormal
                            : item.TColor.ColorNormal;
                    }
                    if (TranColor == Color.Empty)
                    {
                        var temp = _normalImage ?? _normalImage2;
                        g.DrawImage(temp, item.Rectangle);
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
                        var temp = _downImage ?? _downImage2;
                        g.DrawImage(temp, item.Rectangle);
                    }
                    else
                    {
                        DrawBackground(g, TranColor, item);
                    }
                    Image image = _selectImage ?? _selectImage2;
                    if (_iMultiple && image != null)
                    {
                        g.DrawImage(image, new Rectangle(item.Rectangle.Right - image.Width, item.Rectangle.Bottom - image.Height, image.Width, image.Height));
                    }
                    break;
            }
        }

        /// <summary>
        ///     填充Item内部颜色
        /// </summary>
        protected virtual void DrawBackground(Graphics g, Color color, ToolItem item)
        {
            var radiu = item.TRadiu > _tRadiu ? item.TRadiu : _tRadiu;
            if (radiu > 0)
            {
                var path = DrawHelper.CreateRoundPath(item.Rectangle, radiu);
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
                    var rect = item.Rectangle;
                    rect = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height - 1);
                    g.DrawRectangle(new Pen(Color.FromArgb(Trans, 100, 100, 100)), rect);
                }
            }
        }

        /// <summary>
        ///     绘制鼠标移入时的背景
        /// </summary>
        private void DrawMoveBack(Graphics g, ToolItem item)
        {
            TranColor = item.TColor.ColorMove == Color.Empty ? TBackGround.ColorMove : item.TColor.ColorMove;
            if (TranColor == Color.Empty)
            {
                var temp = _moveImage ?? _moveImage2;
                g.DrawImage(temp, item.Rectangle);
            }
            else
            {
                DrawBackground(g, TranColor, item);
            }
        }

        /// <summary>
        ///     绘制图片
        /// </summary>
        private void DrawImage(Graphics g, ToolItem item)
        {
            if (_iImageShow && item.Image != null)
            {
                var imageRect = Rectangle.Empty;
                switch (_tLocation)
                {
                    case TILocation.Up:
                        var width = (_itemSize.Width - _imageSizeShow.Width - _textPading.Left - _textPading.Right) / 2;
                        imageRect.X = item.Rectangle.X + _textPading.Left + width;
                        imageRect.Y = item.Rectangle.Y + _textPading.Top;
                        break;
                    case TILocation.Left:
                        var height = (_itemSize.Height - _imageSizeShow.Height - _textPading.Top - _textPading.Bottom) / 2;
                        imageRect.X = item.Rectangle.X + _textPading.Left;
                        imageRect.Y = item.Rectangle.Y + _textPading.Top + height;
                        break;
                }
                imageRect.Size = _imageSizeShow;
                item.ImageRect = imageRect;
                g.DrawImage(item.Image, imageRect);
            }
        }

        /// <summary>
        ///     绘制文字
        /// </summary>
        private void DrawText(Graphics g, ToolItem item)
        {
            var textRect = Rectangle.Empty;
            if (string.IsNullOrEmpty(item.Text)) item.Text = string.Empty;
            {
                textRect = new Rectangle
                {
                    X = item.Rectangle.X + _textPading.Left,
                    Y = item.Rectangle.Y + _textPading.Top,
                    Width = item.Rectangle.Width - _textPading.Left - _textPading.Right,
                    Height = item.Rectangle.Height - _textPading.Top - _textPading.Bottom
                };
                if (_iImageShow && item.Image != null)
                {
                    switch (_tLocation)
                    {
                        case TILocation.Up:
                            textRect.Y += _imageSizeShow.Height + _textPading.Top;
                            textRect.Height = item.Rectangle.Height - _imageSizeShow.Height - _textPading.Top * 2 -
                                              _textPading.Bottom;
                            break;
                        case TILocation.Left:
                            textRect.X += _imageSizeShow.Width + _textPading.Left;
                            textRect.Width = item.Rectangle.Width - _imageSizeShow.Width - _textPading.Left * 2 -
                                             _textPading.Right;
                            break;
                    }
                }
                if (item.ContextMenuStrip != null)
                {
                    textRect.Width -= _rightLen;
                }
                var headHeight = 0;
                if (!string.IsNullOrEmpty(item.HeadDesc))
                {
                    headHeight = HeightFont(item.MouseState, THeadDesc);
                    var rect = new Rectangle
                    {
                        X = textRect.X,
                        Y = textRect.Y,
                        Width = textRect.Width,
                        Height = headHeight
                    };
                    item.RectHeadDesc = rect;
                    DrawOtherDesc(g, item, THeadDesc, item.HeadDesc, rect);
                }
                var endHeight = 0;
                if (!string.IsNullOrEmpty(item.EndDesc))
                {
                    endHeight = HeightFont(item.MouseState, TEndDesc);
                    var rect = new Rectangle
                    {
                        X = textRect.X,
                        Y = textRect.Y + textRect.Height - endHeight,
                        Width = textRect.Width,
                        Height = endHeight
                    };
                    item.RectEndDesc = rect;
                    DrawOtherDesc(g, item, TEndDesc, item.EndDesc, rect);
                }
                if (!string.IsNullOrEmpty(item.Desc))
                {
                    var size = TextRenderer.MeasureText(item.Desc, GetFont(item.IMouseState, TDesc));
                    textRect.Width -= size.Width;
                }
                if (_iText || item.IText)
                {
                    var rect = new Rectangle(textRect.X, textRect.Y + headHeight, textRect.Width,
                        textRect.Height - headHeight - endHeight);
                    DrawOtherDesc(g, item, TextFirst, item.Text, rect);
                }
                else
                {
                    var text = item.Text.Split(new[] { "\r\n", "&&" }, StringSplitOptions.RemoveEmptyEntries);
                    if (text.Length > 0)
                    {
                        var fHight = HeightFont(item.MouseState, TextFirst);
                        var sHight = HeightFont(item.MouseState, TextSencond);
                        var height = textRect.Height - fHight;
                        height -= (text.Length - 1) * sHight;
                        height -= (text.Length - 1) * _textSpace;
                        height /= 2;

                        var rect = new Rectangle
                        {
                            X = textRect.X,
                            Y = textRect.Y + height,
                            Width = textRect.Width,
                            Height = fHight
                        };
                        DrawOtherDesc(g, item, TextFirst, text[0], rect);
                        for (var i = 1; i < text.Length; i++)
                        {
                            rect = new Rectangle
                            {
                                X = textRect.X + (fHight - sHight) / 2,
                                Y = textRect.Y + height + fHight + _textSpace * i + sHight * (i - 1),
                                Width = textRect.Width,
                                Height = sHight
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
            return Font;
        }

        /// <summary>
        ///     绘制正文描述
        /// </summary>
        /// <param name="item"></param>
        /// <param name="rect"></param>
        /// <param name="g"></param>
        private void DrawDesc(Graphics g, ToolItem item, Rectangle rect)
        {
            if (string.IsNullOrEmpty(item.Desc)) return;
            var size = TextRenderer.MeasureText(item.Desc, GetFont(item.IMouseState, TDesc));
            item.RectDesc = new Rectangle(rect.X + rect.Width + (item.ContextMenuStrip == null ? 0 : 4),
                rect.Y + (rect.Height - size.Height) / 2, size.Width, size.Height);
            DrawOtherDesc(g, item, TDesc, item.Desc, item.RectDesc, item.IMouseState);
        }

        /// <summary>
        ///     绘制其它描述
        /// </summary>
        private void DrawOtherDesc(Graphics g, ToolItem item, TProperties desc, string text, Rectangle rect)
        {
            DrawOtherDesc(g, item, desc, text, rect, item.MouseState);
        }

        /// <summary>
        ///     绘制其它描述
        /// </summary>
        private void DrawOtherDesc(Graphics g, ToolItem item, TProperties desc, string text, Rectangle rect,
            TMouseState state)
        {
            if (string.IsNullOrEmpty(text)) return;

            var color = ForeColor;
            var font = desc.FontNormal;

            if (!item.Enable)
            {
                if (_iText || item.IText)
                {
                    g.DrawString(text, font, new SolidBrush(color), rect, desc.StringFormat);
                }
                else
                {
                    var temp = new Rectangle(rect.X + Offset.X, rect.Y + Offset.Y, rect.Width, rect.Height);
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
            if (color == Color.Empty) color = ForeColor;
            if (_iText || item.IText)
            {
                g.DrawString(text, font, new SolidBrush(color), rect, desc.StringFormat);
            }
            else
            {
                var temp = new Rectangle(rect.X + Offset.X, rect.Y + Offset.Y, rect.Width, rect.Height);
                TextRenderer.DrawText(g, text, font, temp, color, desc.TextFormat);
            }
        }

        /// <summary>
        ///     判断右键菜单
        /// </summary>
        /// <returns>返回是否有焦点 true时不触发down事件</returns>
        private void IsContextMenu(Graphics g, ToolItem item)
        {
            var point = PointToClient(MousePosition);
            point.X -= Offset.X;
            point.Y -= Offset.Y;
            Image btnArrowImage = null;
            var x = _btnArrowRect.Left;
            if (item.RectDesc.Width != 0)
            {
                x = item.RectDesc.Left;
            }
            var contextMenuLocation =
                PointToScreen(new Point(x + Offset.X, _btnArrowRect.Top + Offset.Y + _btnArrowRect.Height + 2));
            var contextMenuStrip = item.ContextMenuStrip;
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
                    if (_iDown && (_btnArrowRect.Contains(point) || item.RectDesc.Contains(point)))
                    {
                        btnArrowImage = main_tabbtn_downImage;
                    }
                    else
                    {
                        btnArrowImage = main_tabbtn_highlightImage;
                    }
                    if (_iFocus && (_btnArrowRect.Contains(point) || item.RectDesc.Contains(point)))
                    {
                        contextMenuStrip.Tag = item;
                        contextMenuStrip.Show(contextMenuLocation);
                    }
                    _btnArrowRect = new Rectangle(item.Rectangle.X + item.Rectangle.Width - btnArrowImage.Width,
                        item.Rectangle.Y + (item.Rectangle.Height - btnArrowImage.Height) / 2, btnArrowImage.Width,
                        btnArrowImage.Height);
                }
            }
            if (btnArrowImage != null)
            {
                //当鼠标进入当前选中的的选项卡时，显示下拉按钮
                g.DrawImage(btnArrowImage, _btnArrowRect);
            }
        }

        /// <summary>
        ///     当项菜单弹出时事件发生
        /// </summary>
        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            var item = (sender as ContextMenuStrip).Tag as ToolItem;
            OnEventOpening(item, e);
        }

        /// <summary>
        ///     右键菜单关闭刷新项
        /// </summary>
        private void contextMenuStrip_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            _iFocus = false;
            var item = (sender as ContextMenuStrip).Tag as ToolItem;
            var cursorPoint = PointToClient(MousePosition);
            if (!ClientRectangle.Contains(cursorPoint))
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
        ///     重绘正文描述
        /// </summary>
        /// <param name="item"></param>
        /// <param name="state"></param>
        private void InvaRectDesc(ToolItem item, TMouseState state)
        {
            if (item.IMouseState != state)
            {
                item.IMouseState = state;
                Invalidate(new Rectangle(item.Rectangle.X + Offset.X, item.Rectangle.Y + Offset.Y, item.Rectangle.Width,
                    item.Rectangle.Height));
            }
        }

        /// <summary>
        ///     重绘Item
        /// </summary>
        private void InvalidateItem(ToolItem item, TMouseState state)
        {
            if (item.MouseState != state)
            {
                item.MouseState = state;
                Invalidate(new Rectangle(item.Rectangle.X + Offset.X, item.Rectangle.Y + Offset.Y, item.Rectangle.Width,
                    item.Rectangle.Height));
            }
        }

        /// <summary>
        ///     重绘Item
        /// </summary>
        /// <param name="item"></param>
        private void InvalidateItem(ToolItem item)
        {
            Invalidate(new Rectangle(item.Rectangle.X + Offset.X, item.Rectangle.Y + Offset.Y, item.Rectangle.Width,
                item.Rectangle.Height));
        }

        /// <summary>
        ///     引发 System.Windows.Forms.Form.MouseMove 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (INormal) return;
            if (DesignMode) return;

            var point = e.Location;
            point.X -= Offset.X;
            point.Y -= Offset.Y;
            var flag = true;
            foreach (var item in Items)
            {
                if (item.Rectangle.Contains(point))
                {
                    MoveItem = item;
                    if (item.RectDesc.Contains(point) || _btnArrowRect.Contains(point))
                    {
                        if (item.IMouseState != TMouseState.Down)
                        {
                            InvaRectDesc(item, TMouseState.Move);
                        }
                    }
                    if (ICheckEvent || item.MouseState != TMouseState.Down)
                    {
                        flag = false;
                        if (item.MouseState != TMouseState.Move && item.MouseState != TMouseState.Down)
                        {
                            InvalidateItem(item, TMouseState.Move);
                            if (_iShowTop)
                            {
                                ShowTooTip(item.Hit ?? item.Sencond ?? item.First);
                            }
                        }
                    }
                }
                else
                {
                    InvaRectDesc(item, TMouseState.Normal);
                    if (ICheckEvent || item.MouseState != TMouseState.Down)
                    {
                        InvalidateItem(item, TMouseState.Normal);
                    }
                }
            }
            if (flag)
            {
                HideToolTip();
            }
        }

        /// <summary>
        ///     引发 System.Windows.Forms.Form.MouseLeave 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_iFocus || INormal || DesignMode) return;

            _iDown = false;
            MoveItem = null;
            foreach (var item in Items)
            {
                InvaRectDesc(item, TMouseState.Normal);
                if ((ICheckEvent && !_iMultiple) || item.MouseState != TMouseState.Down)
                {
                    InvalidateItem(item, TMouseState.Normal);
                }
            }
        }

        /// <summary>
        ///     引发 System.Windows.Forms.Form.MouseDown 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left || _iFocus) return;
            if (DesignMode) return;

            var point = e.Location;
            point.X -= Offset.X;
            point.Y -= Offset.Y;
            var contain = Contain(point) && !ContainDesc(point);

            for (var i = 0; i < Items.Count; i++)
            {
                var item = Items[i];
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
            if (item != _tempItem)
            {
                _tempItem = item;
            }
            //事件
            if (item != _selectedItem)
            {
                if (!item.RectDesc.Contains(point) && _tEvent == TEvent.Down)
                {
                    _selectedItem = item;
                    _selectedIndex = Items.GetIndexOfRange(item);
                    OnSelectedItemChanged(item, e);
                }
            }
            //事件
            if (_tEvent == TEvent.Down)
            {
                if (item.RectDesc.Contains(point) || _btnArrowRect.Contains(point))
                {
                    var ifocus = false;
                    if (item.RectDesc.Contains(point))
                    {
                        ifocus = OnEditClick(item, e);
                    }
                    if (!ifocus && item.ContextMenuStrip != null)
                    {
                        _iFocus = true;
                        InvalidateItem(item);
                    }
                }
                else
                {
                    OnItemClick(item, e);
                }
            }
            if (INormal) return;
            if (item.RectDesc.Contains(point) || _btnArrowRect.Contains(point))
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
        ///     引发 System.Windows.Forms.Form.MouseUp 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (DesignMode) return;
            if (e.Button != MouseButtons.Left) return;

            var point = e.Location;
            point.X -= Offset.X;
            point.Y -= Offset.Y;
            for (var i = 0; i < Items.Count; i++)
            {
                var item = Items[i];
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
                if (item != _selectedItem && !item.RectDesc.Contains(point))
                {
                    _selectedItem = item;
                    _selectedIndex = Items.GetIndexOfRange(item);
                    OnSelectedItemChanged(item, e);
                }
                if (item.RectDesc.Contains(point) || _btnArrowRect.Contains(point))
                {
                    var ifocus = false;
                    if (item.RectDesc.Contains(point))
                    {
                        ifocus = OnEditClick(item, e);
                    }
                    if (!ifocus && item.ContextMenuStrip != null)
                    {
                        _iFocus = true;
                        InvalidateItem(item);
                    }
                }
                else
                {
                    OnItemClick(item, e);
                }
            }
            if (INormal) return;
            if (item.RectDesc.Contains(point) || _btnArrowRect.Contains(point))
            {
                InvaRectDesc(item, TMouseState.Move);
            }
            else
            {
                InvaRectDesc(item, TMouseState.Normal);
            }
            if (!_iMultiple && ICheckEvent && item.MouseState == TMouseState.Down)
            {
                InvalidateItem(item, TMouseState.Move);
            }
        }

        /// <summary>
        ///     引发 System.Windows.Forms.Form.MouseEnter 事件。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (iScrollHide && ParentForm.ContainsFocus)
            {
                Focus();
            }
        }

        /// <summary>
        ///     数据源更新-  BindingList提供
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _items_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (!ILoad) return;

            if (Items.Count > 1 && e.ListChangedType == ListChangedType.ItemAdded && e.NewIndex == Items.Count - 1)
            {
                var last = 0;
                switch (TDirection)
                {
                    case TDirection.Level:
                        last = TCountLine;
                        break;
                    case TDirection.Vertical:
                        last = TCountColumn;
                        break;
                }
                var count = Items.Count - 2;
                var xPos = Items[count].Rectangle.X;
                var yPos = Items[count].Rectangle.Y;
                Calctem(Items[count], ref xPos, ref yPos, false);
                Calctem(Items[count + 1], ref xPos, ref yPos, true);

                var valid = false;
                switch (TDirection)
                {
                    case TDirection.Level:
                        valid = last != TCountLine;
                        break;
                    case TDirection.Vertical:
                        valid = last != TCountColumn;
                        break;
                }
                UpdateScroll(true, valid);
                InvalidateItem(Items[count + 1]);
            }
            else
            {
                if (_selectedItem != null && e.ListChangedType == ListChangedType.ItemDeleted)
                {
                    var index = Items.GetIndexOfRange(_selectedItem);
                    if (index == -1)
                    {
                        _selectedItem = null;
                    }
                }
                TRefresh();
            }
        }

        /// <summary>
        ///     返回包含 System.ComponentModel.Component 的名称的 System.String（如果有）
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} - {1}", this.Name, TConfig.Name);
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     获取所有选中项
        /// </summary>
        /// <returns></returns>
        public List<ToolItem> TSelectedItems()
        {
            var list = new List<ToolItem>();
            for (var i = 0; i < _items.Count; i++)
            {
                if (_items[i].MouseState == TMouseState.Down)
                {
                    list.Add(_items[i]);
                }
            }
            return list;
        }

        /// <summary>
        ///     坐标点是否包含在项中
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override bool Contain(Point point)
        {
            for (var i = 0; i < Items.Count; i++)
            {
                if (Items[i].Rectangle.Contains(point))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///     坐标点是否包含在项描述中
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool ContainDesc(Point point)
        {
            for (var i = 0; i < Items.Count; i++)
            {
                if (Items[i].RectDesc.Contains(point))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///     移除项
        /// </summary>
        public void TRemove(string text)
        {
            for (var i = 0; i < Items.Count; i++)
            {
                if (Items[i].First == text)
                {
                    Items.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        ///     选中第一项
        /// </summary>
        public void TClickFirst()
        {
            TClickItem(0);
        }

        /// <summary>
        ///     单击项
        /// </summary>
        public void TClickItem(string text)
        {
            for (var i = 0; i < Items.Count; i++)
            {
                if (Items[i].First == text)
                {
                    TClickItem(i);
                    return;
                }
            }
            for (var i = 0; i < Items.Count; i++)
            {
                if (Items[i].Tag.ToString2() == text)
                {
                    TClickItem(i);
                    break;
                }
            }
        }
        /// <summary>
        ///     单击项
        /// </summary>
        public void TClickItem(ToolItem item)
        {
            var index = Items.GetIndexOfRange(item);
            TClickItem(index);
        }
        /// <summary>
        ///     单击项
        /// </summary>
        public void TClickItem(int index)
        {
            if (_items.Count == 0) return;
            if (_items.Count <= index) return;
            if (!_iMultiple)
            {
                _selectedItem = null;
                for (var i = 0; i < _items.Count; i++)
                {
                    InvalidateItem(_items[i], TMouseState.Normal);
                }
            }
            if (index >= 0)
            {
                _selectedItem = _items[index];
                if (!ICheckEvent)
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
        ///     刷新控件到原点
        /// </summary>
        public void TStart()
        {
            FixScroll(0);
        }
        /// <summary>
        ///     刷新控件到指定位置
        /// </summary>
        public void TStart(int value)
        {
            FixScroll(IHaveScroll ? value : 0);
        }
        /// <summary>
        ///     刷新控件到尾部
        /// </summary>
        public void TEnd()
        {
            if (!IHaveScroll) return;
            switch (TDirection)
            {
                case TDirection.Level:
                    FixScroll(_vScroll.Maximum);
                    break;
                case TDirection.Vertical:
                    FixScroll(_vScroll2.Maximum);
                    break;
            }
        }

        /// <summary>
        ///     自适应高度/宽度
        /// </summary>
        public void TAutoHeight()
        {
            TAutoHeight(0);
        }

        /// <summary>
        ///     自适应高度/宽度
        /// </summary>
        /// <param name="count">最小行/列</param>
        public void TAutoHeight(int count)
        {
            if (TCountLine < count) TCountLine = count;
            switch (TDirection)
            {
                case TDirection.Level:
                    Height = GetHeight();
                    break;
                case TDirection.Vertical:
                    Width = GetWidth();
                    break;
            }
        }

        /// <summary>
        ///     刷新项
        /// </summary>
        /// <param name="item"></param>
        public void TRefresh(ToolItem item)
        {
            var index = Items.GetIndexOfRange(item);
            TRefresh(index);
        }

        /// <summary>
        ///     刷新项
        /// </summary>
        /// <param name="index"></param>
        public void TRefresh(int index)
        {
            if (index < 0 || index > Items.Count - 1) return;
            InvalidateItem(Items[index]);
        }

        /// <summary>
        ///     刷新控件
        /// </summary>
        public void TRefresh()
        {
            _vScroll.Visible = false;
            _hScroll.Visible = false;
            _vScroll2.Visible = false;
            panelScroll.Visible = false;
            TPaint();
            UpdateScroll();
            Invalidate(ClientRectangle);
        }
        private void AutoMouseStatu()
        {
            if (panelScroll.Visible == iScrollHide & _iScroll & iMouseStatu) return;
            new Action(AutoMouseStatu2).BeginInvoke(null, null);
        }
        private void AutoMouseStatu2()
        {
            System.Threading.Thread.Sleep(50);
            if (DesignMode) return;
            this.Invoke(new Action(AutoMouseStatu3));
        }
        private void AutoMouseStatu3()
        {
            panelScroll.Visible = iScrollHide & _iScroll & iMouseStatu;
        }

        #endregion

        #region 动态显示项的图片
        private Timer tDynamic = new Timer();
        private PictureBox pictureBox1;

        /// <summary>
        ///     动态项原图片
        /// </summary>
        private Image image;

        /// <summary>
        ///     动态项
        /// </summary>
        private int progressItemIndex;

        /// <summary>
        ///     项的动态图片
        /// </summary>
        [Description("项的动态图片"), DefaultValue(null)]
        public Image TProgressImage
        {
            get { return pictureBox1.Image; }
            set { pictureBox1.Image = value; }
        }

        /// <summary>
        ///     初始化动态方法
        /// </summary>
        private void Progress()
        {
            tDynamic.Interval = 30;
            tDynamic.Tick += tDynamic_Tick;
        }

        /// <summary>
        ///     动态显示项的图像
        /// </summary>
        /// <param name="text">项文本</param>
        /// <param name="newText">项新文本</param>
        public void TProgressItem(string text, string newText = null)
        {
            for (var i = 0; i < _items.Count; i++)
            {
                if (_items[i].Text == text)
                {
                    TProgressItem(i, newText);
                    break;
                }
            }
        }

        /// <summary>
        ///     动态显示项的图像
        /// </summary>
        /// <param name="index">项索引</param>
        /// <param name="newText">项新文本</param>
        public void TProgressItem(int index, string newText = null)
        {
            progressItemIndex = index;
            if (!string.IsNullOrEmpty(newText)) _items[progressItemIndex].Text = newText;
            image = _items[progressItemIndex].Image;
            if (pictureBox1.Image != null)
            {
                _items[progressItemIndex].Image = pictureBox1.Image;
            }
            tDynamic.Enabled = true;
        }

        /// <summary>
        ///     停止动态显示
        /// </summary>
        /// <param name="image">图片</param>
        /// <param name="text">项文本</param>
        public void TProgressStop(Image image = null, string text = null)
        {
            tDynamic.Enabled = false;
            if (!string.IsNullOrEmpty(text)) _items[progressItemIndex].Text = text;
            _items[progressItemIndex].Image = image ?? this.image;
            tDynamic_Tick(this, EventArgs.Empty);
        }

        private void tDynamic_Tick(object sender, EventArgs e)
        {
            InvalidateItem(_items[progressItemIndex]);
        }

        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            ((ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1, 1);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // ToolBar
            // 
            Controls.Add(pictureBox1);
            Name = "ToolBar";
            ((ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        #region 变色项
        private Timer tChange = new Timer();

        /// <summary>
        ///     动态项
        /// </summary>
        private int index;

        private void InitChange()
        {
            tChange.Interval = 600;
            tChange.Tick += tChange_Tick;
        }

        /// <summary>
        ///     开始变色
        /// </summary>
        public void TChangeStart()
        {
            tChange.Enabled = true;
        }

        /// <summary>
        ///     停止变色
        /// </summary>
        public void TChangeStop()
        {
            tChange.Enabled = false;
        }

        private void tChange_Tick(object sender, EventArgs e)
        {
            var result = false;
            for (var i = 0; i < Items.Count; i++)
            {
                if (Items[i].IChange)
                {
                    result = true;
                    var color = Items[i].TColor.ColorNormal;
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
                    InvalidateItem(Items[i]);
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
        /// 滚动条外框
        /// </summary>
        private TControl panelScroll;
        /// <summary>
        ///     垂直滚动条
        /// </summary>
        private VScrollBar _vScroll;

        /// <summary>
        ///     水平滚动条
        /// </summary>
        private HScrollBar _hScroll;

        /// <summary>
        ///     隐藏的垂直滚动条，与 水平滚动条 联动，尝试解决平板横向无法滑动。
        /// </summary>
        private VScrollBar _vScroll2;

        /// <summary>
        ///     隐藏滚动条，实际有滚动效果，自动设置
        /// </summary>
        private bool iScrollHide;

        /// <summary>
        ///     是否显示滚动条
        /// </summary>
        private bool _iScroll = true;
        /// <summary>
        ///     是否显示滚动条
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
        /// 当前鼠标进入状态
        /// </summary>
        private bool iMouseStatu;

        /// <summary>
        ///     获取滚动条是否有显示
        /// </summary>
        [Browsable(false), DefaultValue(false)]
        public bool IHaveScroll
        {
            get
            {
                switch (TDirection)
                {
                    case TDirection.Level:
                        return _vScroll.Visible;
                    case TDirection.Vertical:
                        return _hScroll.Visible;
                }
                return false;
            }
        }

        /// <summary>
        ///     获取当前滚动条的值
        /// </summary>
        [Browsable(false), DefaultValue(0)]
        public int IScrollValue
        {
            get
            {
                switch (TDirection)
                {
                    case TDirection.Level:
                        return _vScroll.Value;
                    case TDirection.Vertical:
                        return _hScroll.Value;
                }
                return 0;
            }
        }

        /// <summary>
        ///     控件显示偏移坐标
        /// </summary>
        private Point Offset = Point.Empty;

        /// <summary>
        ///     初始化滚动条
        /// </summary>
        private void CustomScroll()
        {
            panelScroll = new TControl();
            Controls.Add(panelScroll);

            _hScroll = new HScrollBar();
            _hScroll.Scroll += _hScroll_Scroll;
            //_hScroll.Dock = DockStyle.Bottom;
            //_hScroll.Height = _tScrollHeight;
            panelScroll.Controls.Add(_hScroll);
            {
                _vScroll2 = new VScrollBar();
                //_vScroll2.Scroll += _hScroll_Scroll;
                _vScroll2.Dock = DockStyle.Right;
                _vScroll2.Width = 0;
                Controls.Add(_vScroll2);
            }

            _vScroll = new VScrollBar();
            _vScroll.Scroll += _vScroll_Scroll;
            //_vScroll.Dock = DockStyle.Right;
            //_vScroll.Width = _tScrollHeight;
            panelScroll.Controls.Add(_vScroll);

            _vScroll.Visible = false;
            _hScroll.Visible = false;
            _vScroll2.Visible = false;
            panelScroll.Visible = false;
        }

        /// <summary>
        ///     将界面坐标转为控件坐标
        /// </summary>
        public Point Replace(Point point)
        {
            return new Point(point.X - Offset.X, point.Y - Offset.Y);
        }

        /// <summary>
        ///     重写滚轮滚动
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (!iScrollHide) return;
            switch (TDirection)
            {
                case TDirection.Level:
                    var value = _vScroll.Value - e.Delta * ItemSize.Height / 120;
                    FixScroll(value);
                    break;
                case TDirection.Vertical:
                    value = _vScroll2.Value - e.Delta * ItemSize.Width / 120;
                    FixScroll(value);
                    break;
            }
            base.OnMouseWheel(e);
        }

        /// <summary>
        ///     滚动到指定值位置
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valid">是否需要重绘，默认重绘</param>
        private void FixScroll(int value, bool valid = true)
        {
            switch (TDirection)
            {
                case TDirection.Level:
                    var max = _vScroll.Maximum - _vScroll.SmallChange;
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
                Invalidate(ClientRectangle);
            }
        }

        /// <summary>
        ///     大小改变时
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            _vScroll.Visible = false;
            _hScroll.Visible = false;
            _vScroll2.Visible = false;
            panelScroll.Visible = false;
            TPaint();
            UpdateScroll();
        }

        /// <summary>
        ///     更新滚动条状态
        /// </summary>
        private void UpdateScroll(bool toLast = false, bool toValid = false)
        {
            var valid = _vScroll.Visible || _hScroll.Visible;
            _vScroll.Visible = false;
            _hScroll.Visible = false;
            _vScroll2.Visible = false;
            panelScroll.Visible = false;
            iScrollHide = false;
            switch (TDirection)
            {
                case TDirection.Level:
                    var height = GetHeight();
                    if (Height < height)
                    {
                        iScrollHide = true;
                        _vScroll.Visible = _iScroll;
                        panelScroll.Width = _tScrollHeight;
                        panelScroll.Dock = DockStyle.Right;
                        panelScroll.Visible = _iScroll;
                        _vScroll.Size = new Size(panelScroll.Width, panelScroll.Height + 17 * 2 + 1);
                        _vScroll.Location = new Point(0, -17);
                    }
                    else if (_vScroll.Value >= 0)
                    {
                        FixScroll(0, false);
                    }
                    break;
                case TDirection.Vertical:
                    var width = GetWidth();
                    if (Width < width)
                    {
                        iScrollHide = true;
                        _hScroll.Visible = _iScroll;
                        _vScroll2.Visible = _iScroll;
                        panelScroll.Height = _tScrollHeight;
                        panelScroll.Dock = DockStyle.Bottom;
                        panelScroll.Visible = _iScroll;
                        _hScroll.Size = new Size(panelScroll.Width + 17 * 2 + 1, panelScroll.Height);
                        _hScroll.Location = new Point(-17, 0);
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
                    TPaint();
                }
                switch (TDirection)
                {
                    case TDirection.Level:
                        var max = GetHeight() - Height;
                        if (_vScroll.Value > max)
                        {
                            FixScroll(max, false);
                        }
                        _vScroll.Maximum = GetHeight();
                        _vScroll.LargeChange = Height / 2;
                        _vScroll.SmallChange = _vScroll.LargeChange;
                        _vScroll.Maximum = max + _vScroll.LargeChange;

                        valid = !valid | toLast & toValid;
                        if (toLast)
                            FixScroll(_vScroll.Maximum, valid);
                        break;
                    case TDirection.Vertical:
                        max = GetWidth() - Width;
                        if (_vScroll2.Value > max)
                        {
                            FixScroll(max, false);
                        }
                        _vScroll2.Maximum = GetWidth();
                        _vScroll2.LargeChange = Width / 2;
                        _vScroll2.SmallChange = _vScroll2.LargeChange;
                        _vScroll2.Maximum = max + _vScroll2.LargeChange;
                        {
                            _hScroll.LargeChange = _vScroll2.LargeChange;
                            _hScroll.SmallChange = _vScroll2.SmallChange;
                            _hScroll.Maximum = _vScroll2.Maximum;
                        }
                        valid = !valid | toLast & toValid;
                        if (toLast)
                            FixScroll(_vScroll2.Maximum, valid);
                        break;
                }
            }
            panelScroll.Visible = DesignMode;
        }

        /// <summary>
        ///     水平滚动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _hScroll_Scroll(object sender, ScrollEventArgs e)
        {
            var diff = e.NewValue - e.OldValue;
            if (e.NewValue == 0)
            {
                diff = 0;
                Offset.X = 0;
            }
            Offset.Offset(-diff, 0);
            _vScroll2.Value = _hScroll.Value;
            if (diff != 0 || e.NewValue == 0)
            {
                Invalidate(ClientRectangle);
            }
        }

        /// <summary>
        ///     垂直滚动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _vScroll_Scroll(object sender, ScrollEventArgs e)
        {
            var diff = e.NewValue - e.OldValue;
            if (e.NewValue == 0)
            {
                diff = 0;
                Offset.Y = 0;
            }
            Offset.Offset(0, -diff);
            if (diff != 0 || e.NewValue == 0)
            {
                Invalidate(ClientRectangle);
            }
        }

        /// <summary>
        ///     需要的高度
        /// </summary>
        /// <returns></returns>
        private int GetHeight()
        {
            var height = Padding.Top + Padding.Bottom;
            height += TCountLine * ItemSize.Height;
            height += (TCountLine - 1) * ItemSpace;
            height += THeardLength;
            return height;
        }

        /// <summary>
        ///     需要的宽度
        /// </summary>
        /// <returns></returns>
        private int GetWidth()
        {
            var width = Padding.Left + Padding.Right;
            width += TCountColumn * ItemSize.Width;
            width += (TCountColumn - 1) * ItemSpace;
            width += THeardLength;
            return width;
        }

        #endregion

        #region 悬停窗口显示说明
        /// <summary>
        ///     表示一个长方形的小弹出窗口，该窗口在用户将指针悬停在一个控件上时显示有关该控件用途的简短说明。
        /// </summary>
        protected void ShowTooTip(string toolTipText)
        {
            toolTop.Active = true;
            toolTop.SetToolTip(this, toolTipText);
        }

        /// <summary>
        ///     弹出窗口不活动
        /// </summary>
        protected void HideToolTip()
        {
            if (toolTop.Active)
            {
                toolTop.Active = false;
            }
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
                if (_change != null)
                    _change.Dispose();
                if (_text != null)
                    _text.Dispose();
                if (_textSencond != null)
                    _textSencond.Dispose();
                if (_desc != null)
                    _desc.Dispose();
                if (_headDesc != null)
                    _headDesc.Dispose();
                if (_endDesc != null)
                    _endDesc.Dispose();
                if (_backGround != null)
                    _backGround.Dispose();
                _normalImage = null;
                _normalImage2.Dispose();
                _downImage = null;
                _moveImage = null;
                _selectImage = null;
                if (toolTop != null)
                    toolTop.Dispose();
                if (_tempItem != null)
                    _tempItem.Dispose();
                if (_selectedItem != null)
                    _selectedItem.Dispose();
                if (MoveItem != null)
                    MoveItem.Dispose();
                if (_vScroll != null)
                    _vScroll.Dispose();
                if (_hScroll != null)
                    _hScroll.Dispose();
                if (_vScroll2 != null)
                    _vScroll2.Dispose();
                if (_items != null)
                    _items.Dispose();
                tChange.Stop();
                tChange = null;
                tDynamic.Stop();
                tDynamic = null;
                image = null;
                pictureBox1.Image = null;
                pictureBox1.Dispose();
                pictureBox1 = null;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}