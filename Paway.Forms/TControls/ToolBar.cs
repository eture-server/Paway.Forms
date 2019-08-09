using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Paway.Helper;
using System.Threading.Tasks;
using System.Reflection;
using Paway.Forms.Properties;
using System.Diagnostics;

namespace Paway.Forms
{
    /// <summary>
    /// 工具栏
    /// </summary>
    [DefaultProperty("Items")]
    [DefaultEvent("ItemClick")]
    public class ToolBar : TControl
    {
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
            _toolTop = new ToolTip();
            //InitHide();
        }

        #endregion

        #region 私有变量

        #region 资源图片
        /// <summary>
        /// 默认时的按钮图片
        /// </summary>
        private Image _normalImage;
        private readonly Image _normalImage2 = Resources.toolBar_toolbar_normal;
        /// <summary>
        /// 默认图片
        /// </summary>
        [DefaultValue(null)]
        [Description("默认时的按钮图片")]
        public Image NormalImage
        {
            get { return _normalImage; }
            set
            {
                _normalImage = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 鼠标按下时的图片
        /// </summary>
        private Image _downImage;
        private readonly Image _downImage2 = Resources.toolBar_toolbar_hover;
        /// <summary>
        /// 鼠标按下时的图片
        /// </summary>
        [DefaultValue(null)]
        [Description("鼠标按下时的图片")]
        public Image DownImage
        {
            get { return _downImage; }
            set
            {
                _downImage = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 鼠标划过时的图片
        /// </summary>
        private Image _moveImage;
        private readonly Image _moveImage2 = Resources.toolBar_toolbar_pushed;
        /// <summary>
        /// 鼠标划过时的图片
        /// </summary>
        [DefaultValue(null)]
        [Description("鼠标划过时的图片")]
        public Image MoveImage
        {
            get { return _moveImage; }
            set
            {
                _moveImage = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 多选状态下选中时附加的图片
        /// </summary>
        private Image _selectImage;
        /// <summary>
        /// 多选状态下选中时附加的图片
        /// </summary>
        private readonly Image _selectImage2 = Resources.Controls_accept_16;

        #endregion

        /// <summary>
        /// 上一项是否IHeard
        /// </summary>
        private bool iLastHeard;

        /// <summary>
        /// 悬停窗口
        /// </summary>
        private ToolTip _toolTop;

        /// <summary>
        /// 按下抬起项是否相同中用的过度项
        /// </summary>
        private ToolItem _tempItem;
        /// <summary>
        /// 挂起状态
        /// </summary>
        private bool iSuspend;

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
        [Description("圆角大小")]
        [DefaultValue(0)]
        public int TRadiu
        {
            get { return _tRadiu; }
            set
            {
                _tRadiu = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 滚动条宽度
        /// </summary>
        private int _tScrollHeight = 3;
        /// <summary>
        /// 滚动条宽度
        /// </summary>
        [Description("滚动条宽度")]
        [DefaultValue(3)]
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
                    _panelScroll.Height = value;
                }
                if (_vScroll != null)
                {
                    _vScroll.Width = value;
                    _panelScroll.Width = value;
                }
            }
        }

        /// <summary>
        /// 头文字总长度(占用总长度/宽度)
        /// </summary>
        [Browsable(false), Description("头文字总长度(占用总长度/宽度)")]
        [DefaultValue(0)]
        public int THeardLength { get; private set; }

        /// <summary>
        /// 项文本间的间隔
        /// </summary>
        private int _textSpace = 0;
        /// <summary>
        /// 项文本间的间隔
        /// </summary>
        [Description("项文本间的间隔")]
        [DefaultValue(0)]
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
        /// 多行列排列时的行数
        /// </summary>
        [Browsable(false), Description("多行列排列时的行数")]
        [DefaultValue(1)]
        public int TCountLine { get; private set; }

        /// <summary>
        /// 多行列排列时的列数
        /// </summary>
        [Browsable(false), Description("多行列排列时的列数")]
        [DefaultValue(1)]
        public int TCountColumn { get; private set; }

        #endregion

        #region bool
        private bool _iShowTop = true;
        /// <summary>
        /// 显示简短说明
        /// </summary>
        [Description("显示简短说明")]
        [DefaultValue(true)]
        public bool IShowTop { get { return _iShowTop; } set { _iShowTop = value; } }

        /// <summary>
        /// 普通项，不响应鼠标绘制，响应事件
        /// </summary>
        [Description("普通项，不响应鼠标绘制，响应事件")]
        [DefaultValue(false)]
        public bool INormal { get; set; }

        /// <summary>
        /// 补充整行\列
        /// </summary>
        private bool _iAdd;
        /// <summary>
        /// 补充整行\列
        /// </summary>
        [Description("补充整行\\列")]
        [DefaultValue(false)]
        public bool IAdd
        {
            get { return _iAdd; }
            set
            {
                _iAdd = value;
                Invalidate();
            }
        }

        private bool _iClickEvent;
        /// <summary>
        /// 单击事件开关
        /// 单击松开后取消选中状态，只有鼠标移入状态
        /// </summary>
        [Description("单击事件开关")]
        [DefaultValue(false)]
        public bool IClickEvent
        {
            get { return _iClickEvent; }
            set
            {
                if (_iMultiple) return;
                _iClickEvent = value;
            }
        }

        /// <summary>
        /// 图片显示开关
        /// </summary>
        private bool _iImageShow;
        /// <summary>
        /// 图片显示开关
        /// </summary>
        [Description("图片显示开关")]
        [DefaultValue(false)]
        public bool IImageShow
        {
            get { return _iImageShow; }
            set
            {
                _iImageShow = value;
                UpdateImageSize();
                Invalidate();
            }
        }

        /// <summary>
        /// 多选开关
        /// </summary>
        private bool _iMultiple;
        /// <summary>
        /// 多选开关
        /// </summary>
        [Description("多选开关")]
        [DefaultValue(false)]
        public bool IMultiple
        {
            get { return _iMultiple; }
            set
            {
                if (value) _iClickEvent = false;
                _iMultiple = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 自动项宽度
        /// </summary>
        private bool _iAutoWidth;
        /// <summary>
        /// 自动项宽度
        /// 当ItemSize.Width=0时,宽度自动
        /// 当ItemSize.Width>0时,宽度范围限制在(1/2-1)个Width
        /// </summary>
        [Description("自动项宽度")]
        [DefaultValue(false)]
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
                if (_items == null)
                {
                    _items = new ToolItemCollection();
                    _items.ListChanged += Items_ListChanged;
                }
                return _items;
            }
        }

        /// <summary>
        /// 项的大小
        /// </summary>
        private Size _itemSize = new Size(78, 82);
        /// <summary>
        /// 项的大小
        /// </summary>
        [Description("项的大小")]
        [DefaultValue(typeof(Size), "78,82")]
        public Size ItemSize
        {
            get { return _itemSize; }
            set
            {
                _itemSize = value;
                _vScroll.Visible = false;
                _hScroll.Visible = false;
                _vScroll2.Visible = false;
                _panelScroll.Visible = false;
                TPaint();
                UpdateImageSize();
                UpdateScroll();
                Invalidate();
            }
        }

        /// <summary>
        /// 项与项之间的间隔
        /// </summary>
        private int _itemSpace = 1;
        /// <summary>
        /// 项与项之间的间隔
        /// </summary>
        [Description("项与项之间的间隔")]
        [DefaultValue(1)]
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
        /// 项图片的大小
        /// </summary>
        private Size _imageSize = new Size(24, 24);
        /// <summary>
        /// 项图片显示区域大小
        /// </summary>
        private Size _imageSizeShow = Size.Empty;
        /// <summary>
        /// 项图片的大小
        /// </summary>
        [Description("项图片的大小")]
        [DefaultValue(typeof(Size), "24,24")]
        public Size ImageSize
        {
            get { return _imageSize; }
            set
            {
                _imageSize = value;
                UpdateImageSize();
                Invalidate();
            }
        }

        /// <summary>
        /// 当前选中项
        /// </summary>
        private ToolItem _selectedItem;
        /// <summary>
        /// 当前选中项
        /// </summary>
        [Browsable(false), Description("当前选中项")]
        public ToolItem SelectedItem
        {
            get { return _selectedItem; }
        }

        /// <summary>
        /// 当前移入项
        /// </summary>
        [Browsable(false), Description("当前移入项")]
        [DefaultValue(null)]
        public ToolItem MoveItem { get; set; }

        /// <summary>
        /// 选中项的索引
        /// </summary>
        private int _selectedIndex;
        /// <summary>
        /// 选中项的索引
        /// </summary>
        [Browsable(false), Description("选中项的索引")]
        [DefaultValue(0)]
        public int SelectedIndex
        {
            get { return _selectedIndex; }
        }

        #endregion

        #region 字体、颜色属性
        private TProperties _change;
        /// <summary>
        /// 变色项颜色
        /// </summary>
        [Description("变色项颜色")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TProperties TChange
        {
            get
            {
                if (_change == null)
                {
                    _change = new TProperties(MethodBase.GetCurrentMethod());
                    _change.ValueChange += delegate { Invalidate(); };
                }
                return _change;
            }
        }

        private TProperties _text;
        /// <summary>
        /// 文字
        /// </summary>
        [Description("首行文字属性")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TProperties TextFirst
        {
            get
            {
                if (_text == null)
                {
                    _text = new TProperties();
                    _text.ValueChange += delegate { Invalidate(); };
                }
                return _text;
            }
        }

        private TProperties _textSencond;
        /// <summary>
        /// 文字
        /// </summary>
        [Description("从行文字属性")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TProperties TextSencond
        {
            get
            {
                if (_textSencond == null)
                {
                    _textSencond = new TProperties();
                    _textSencond.ValueChange += delegate { Invalidate(); };
                }
                return _textSencond;
            }
        }

        private TProperties _desc;
        /// <summary>
        /// 正文描述
        /// </summary>
        [Description("正文描述属性")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TProperties TDesc
        {
            get
            {
                if (_desc == null)
                {
                    _desc = new TProperties();
                    _desc.ValueChange += delegate { Invalidate(); };
                }
                return _desc;
            }
        }

        private TProperties _headDesc;
        /// <summary>
        /// 头部描述
        /// </summary>
        [Description("头部描述属性")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TProperties THeadDesc
        {
            get
            {
                if (_headDesc == null)
                {
                    _headDesc = new TProperties();
                    _headDesc.ValueChange += delegate { Invalidate(); };
                }
                return _headDesc;
            }
        }

        private TProperties _endDesc;
        /// <summary>
        /// 尾部描述
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TProperties TEndDesc
        {
            get
            {
                if (_endDesc == null)
                {
                    _endDesc = new TProperties();
                    _endDesc.ValueChange += delegate { Invalidate(); };
                }
                return _endDesc;
            }
        }

        private TProperties _backGround;
        /// <summary>
        /// 背景
        /// </summary>
        [Description("背景颜色属性")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TProperties TBackGround
        {
            get
            {
                if (_backGround == null)
                {
                    _backGround = new TProperties();
                    _backGround.ValueChange += delegate { Invalidate(); };
                }
                return _backGround;
            }
        }

        private TProperties _lineColor;
        /// <summary>
        /// 边框线颜色属性
        /// </summary>
        [Description("边框线颜色属性")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TProperties TLineColor
        {
            get
            {
                if (_lineColor == null)
                {
                    _lineColor = new TProperties(MethodBase.GetCurrentMethod());
                    _lineColor.ValueChange += delegate { Invalidate(); };
                }
                return _lineColor;
            }
        }

        private Padding _linePading = new Padding(1);
        /// <summary>
        /// 设置边框线,圆角时仅使用Left值
        /// </summary>
        [Description("设置边框线,圆角时仅使用Left值")]
        [DefaultValue(typeof(Padding), "1,1,1,1")]
        public Padding TLinePading
        {
            get { return _linePading; }
            set
            {
                _linePading = value;
                TRefresh();
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
        [Description("获取或设置项内的空白")]
        [DefaultValue(typeof(Padding), "2,2,2,2")]
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
        /// Item项显示方向
        /// </summary>
        private TDirection _tDirection = TDirection.Level;
        /// <summary>
        /// Item项显示方向
        /// </summary>
        [Description("Item项显示方向")]
        [DefaultValue(typeof(TDirection), "Level")]
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
        /// 图片显示位置
        /// </summary>
        private TLocation _tLocation = TLocation.Left;
        /// <summary>
        /// 图片显示位置
        /// </summary>
        [Description("图片显示位置")]
        [DefaultValue(typeof(TLocation), "Left")]
        public TLocation TLocation
        {
            get { return _tLocation; }
            set
            {
                _tLocation = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 获取或设置控件内的空白。
        /// </summary>
        [Description("获取或设置控件内的空白")]
        [DefaultValue(typeof(Padding), "0, 0, 0, 0")]
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

        #endregion

        #region 事件定义
        /// <summary>
        /// 当选中项的发生改变时
        /// </summary>
        public event Action<ToolItem, EventArgs> SelectedItemChanged;
        /// <summary>
        /// 当选择的 Item 发生改变时激发。
        /// </summary>
        /// <param name="item"></param>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        private void OnSelectedItemChanged(ToolItem item, EventArgs e)
        {
            if (!item.Enable) return;
            SelectedItemChanged?.Invoke(item, e);
        }

        /// <summary>
        /// 当单击项时事件发生
        /// </summary>
        public event Action<ToolItem, EventArgs> ItemClick;
        /// <summary>
        /// 当单击项后激发。
        /// </summary>
        /// <param name="item"></param>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        private void OnItemClick(ToolItem item, EventArgs e)
        {
            if (!item.Enable) return;
            ItemClick?.Invoke(item, e);
        }

        /// <summary>
        /// 当编辑项时事件发生
        /// </summary>
        public event Action<ToolItem, EventArgs> EditClick;
        /// <summary>
        /// 当编辑项时激发。
        /// </summary>
        /// <param name="item"></param>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        private bool OnEditClick(ToolItem item, EventArgs e)
        {
            if (item.Enable && EditClick != null)
            {
                EditClick.Invoke(item, e);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 双击事件
        /// </summary>
        public event Action<ToolItem> ItemDoubleClick;
        /// <summary>
        /// 双击事件
        /// </summary>
        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            if (ItemDoubleClick != null)
            {
                MouseEventArgs me = (MouseEventArgs)e;
                var point = Replace(me.Location);
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Items[i].Rectangle.Contains(point))
                    {
                        ItemDoubleClick.Invoke(Items[i]);
                        return;
                    }
                }
            }
        }

        #endregion

        #region 自定义绘制
        /// <summary>
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdateScroll();
        }

        /// <summary>
        /// 引发 System.Windows.Forms.Form.Paint 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.PaintEventArgs。</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.TranslateTransform(Offset.X, Offset.Y);
            g.PixelOffsetMode = PixelOffsetMode.HighQuality; //与AntiAlias作用相反

            ////并行绘制(GDI是同一个，需要lock，占用更多CPU，效率降低)
            //Parallel.For(0, Items.Count, (i) =>
            //{
            //    DrawItem(g, Items[i]);
            //});
            for (var i = 0; i < Items.Count; i++)
            {
                DrawItem(g, Items[i]);
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
            var xPos = Padding.Left;
            var yPos = Padding.Top;
            TCountColumn = 0;
            TCountLine = 0;
            THeardLength = 0;
            var lastWidth = 0;
            for (var i = 0; i < Items.Count; i++)
            {
                CalcItem(Items[i], ref xPos, ref yPos, ref lastWidth, i == 0);
            }
        }

        private void AddItem(Graphics g)
        {
            var count = Items.Count - 1;
            var xPos = Items[count].Rectangle.X;
            var yPos = Items[count].Rectangle.Y;
            var lastWidth = 0;
            CalcItem(Items[count], ref xPos, ref yPos, ref lastWidth, false, true);
            count = 0;
            //多行/列补充Item
            if (TCountColumn > 1 && TCountLine > 1)
            {
                count = TCountColumn > TCountLine ? TCountColumn : TCountLine;
                if (Items.Count % count == 0) count = 0;
                else count -= Items.Count % count;
            }
            //填充空Item
            for (var i = 0; i < count; i++)
            {
                var temp = new ToolItem();
                CalcItem(temp, ref xPos, ref yPos, ref lastWidth);
                DrawItem(g, temp);
            }
        }

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
                    case TLocation.Up:
                        var width = (_itemSize.Width - _imageSize.Width - _textPading.Left - _textPading.Right) / 2;
                        if (width < 0)
                        {
                            _imageSizeShow.Width = _itemSize.Width - _textPading.Left - _textPading.Right;
                            _imageSizeShow.Height =
                                (_imageSizeShow.Width * _imageSize.Height * 1.0 / _imageSize.Width).ToInt();
                        }
                        break;
                    case TLocation.Left:
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
                            using (var solidBrush = new SolidBrush(color))
                            {
                                g.DrawString(item.Text, item.TColor.FontNormal, solidBrush, temp, item.TColor.StringFormat);
                            }
                            break;
                    }
                }
            }
        }

        private void CalcItem(ToolItem item, ref int xPos, ref int yPos, ref int lastWidth, bool iFirst = false, bool iCalc = false)
        {
            // 当前 Item 所在的矩型区域
            item.Rectangle = new Rectangle(xPos, yPos, _itemSize.Width, _itemSize.Height);
            SizeF size1 = TextRenderer.MeasureText(item.First, TextFirst.FontNormal, item.Rectangle.Size);
            SizeF size2 = TextRenderer.MeasureText(item.Sencond, TextSencond.FontNormal, item.Rectangle.Size);
            int width = Math.Max(size1.Width, size2.Width).ToInt() + _textPading.Left + _textPading.Right;
            if (IImageShow && item.Image != null) width += ImageSize.Width + _textPading.Left + _textPading.Right;
            if (_iAutoWidth)
            {
                if (_itemSize.Width > 0)
                {
                    if (width > _itemSize.Width) width = _itemSize.Width;
                    if (width < _itemSize.Width / 2) width = _itemSize.Width / 2;
                }
                item.Rectangle = new Rectangle(item.Rectangle.X, item.Rectangle.Y, width, item.Rectangle.Height);
            }
            width = Math.Max(size1.Width, size2.Width).ToInt();
            int height = Math.Max(size1.Height, size2.Height).ToInt();
            switch (_tDirection)
            {
                case TDirection.Level:
                    var isNew = xPos + item.Rectangle.Width + Padding.Right > Width;
                    if (item.IHeard || (isNew && xPos != Padding.Left))
                    {
                        xPos = Padding.Left;
                        if (item.IHeard)
                        {
                            if (!iFirst && !iLastHeard)
                            {
                                yPos += _itemSize.Height + _itemSpace;
                            }
                            item.Rectangle = new Rectangle(xPos, yPos, Width, height);
                            if (!iCalc)
                            {
                                THeardLength += height + _itemSpace;
                            }
                            yPos += item.Rectangle.Height + _itemSpace;
                        }
                        else
                        {
                            TCountLine++;
                            yPos += _itemSize.Height + _itemSpace;
                            item.Rectangle = new Rectangle(xPos, yPos, item.Rectangle.Width, item.Rectangle.Height);
                            xPos += item.Rectangle.Width + _itemSpace;
                        }
                    }
                    else
                    {
                        if (!iCalc && xPos == Padding.Left) TCountLine++;
                        if (TCountLine == 1) TCountColumn++;
                        item.Rectangle = new Rectangle(xPos, yPos, item.Rectangle.Width, item.Rectangle.Height);
                        xPos += item.Rectangle.Width + _itemSpace;
                    }
                    break;
                case TDirection.Vertical:
                    isNew = yPos + item.Rectangle.Height + Padding.Bottom > Height;
                    if (item.IHeard || (isNew && yPos != Padding.Top))
                    {
                        yPos = Padding.Top;
                        if (item.IHeard)
                        {
                            if (!iFirst && !iLastHeard)
                            {
                                xPos += _itemSize.Width + _itemSpace;
                            }
                            item.Rectangle = new Rectangle(xPos, yPos, width, Height);
                            if (!iCalc)
                            {
                                THeardLength += width + _itemSpace;
                            }
                            xPos += item.Rectangle.Width + _itemSpace;
                        }
                        else
                        {
                            TCountColumn++;
                            if (TCountLine == 1) xPos += lastWidth + _itemSpace;
                            else xPos += _itemSize.Width + _itemSpace;
                            item.Rectangle = new Rectangle(xPos, yPos, item.Rectangle.Width, item.Rectangle.Height);
                            yPos += item.Rectangle.Height + _itemSpace;
                            lastWidth = item.Rectangle.Width;
                        }
                    }
                    else
                    {
                        if (!iCalc && yPos == Padding.Top) TCountColumn++;
                        if (TCountColumn == 1) TCountLine++;
                        item.Rectangle = new Rectangle(xPos, yPos, item.Rectangle.Width, item.Rectangle.Height);
                        yPos += item.Rectangle.Height + _itemSpace;
                        lastWidth = item.Rectangle.Width;
                    }
                    break;
            }
            iLastHeard = item.IHeard;
        }

        private void DrawItem(Graphics g, ToolItem item)
        {
            var temp = RectangleF.Intersect(g.VisibleClipBounds, item.Rectangle);
            if (temp == RectangleF.Empty) return;
            if (item.IHeard)
            {
                DrawHeard(g, item);
            }
            else
            {
                DrawBackground(g, item);
                DrawImage(g, item);
                DrawText(g, item);
                if (!item.Enable)
                {
                    item.MouseState = TMouseState.Normal;
                    item.TMouseState = TMouseState.Normal;
                }
            }
        }

        /// <summary>
        /// 绘制背景
        /// </summary>
        private void DrawBackground(Graphics g, ToolItem item)
        {
            switch (item.MouseState)
            {
                case TMouseState.Normal:
                case TMouseState.Leave:
                    var color = Color.Gray;
                    if (item.Enable)
                    {
                        color = TranColor(item.TColor.ColorNormal == Color.Empty ? TBackGround.ColorNormal : item.TColor.ColorNormal);
                    }
                    if (color == Color.Empty)
                    {
                        var temp = _normalImage ?? _normalImage2;
                        if (temp != null) g.DrawImage(temp, item.Rectangle);
                    }
                    //else
                    {
                        DrawBackground(g, color, TranColor(TLineColor.ColorNormal), item);
                    }
                    break;
                case TMouseState.Move:
                case TMouseState.Up:
                    DrawMoveBack(g, item);
                    break;
                case TMouseState.Down:
                    color = TranColor(item.TColor.ColorDown == Color.Empty ? TBackGround.ColorDown : item.TColor.ColorDown);
                    if (color == Color.Empty)
                    {
                        var temp = _downImage ?? _downImage2;
                        if (temp != null) g.DrawImage(temp, item.Rectangle);
                    }
                    //else
                    {
                        DrawBackground(g, color, TranColor(TLineColor.ColorDown), item);
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
        /// 填充Item内部颜色
        /// </summary>
        private void DrawBackground(Graphics g, Color color, Color colorLine, ToolItem item)
        {
            var radiu = item.TRadiu > _tRadiu ? item.TRadiu : _tRadiu;
            if (radiu > 0)
            {
                var rect = item.Rectangle;
                if (color != Color.Empty)
                {
                    using (var path = DrawHelper.CreateRoundPath(rect, radiu))
                    using (var solidBrush = new SolidBrush(color))
                    {
                        g.FillPath(solidBrush, path);
                    }
                }
                if (colorLine != Color.Empty && _linePading.Left > 0)
                {
                    var temp = colorLine;
                    if (temp == Color.Transparent) temp = BitmapHelper.RGBAddLight(color == Color.Empty ? Color.LightGray : color, -15);
                    else temp = TranColor(colorLine);
                    if (radiu == this.ItemSize.Width && radiu == this.ItemSize.Height)
                    {
                        //g.PixelOffsetMode = PixelOffsetMode.Default;
                        using (var pen = new Pen(Color.FromArgb(Trans, temp.R, temp.G, temp.B), _linePading.Left))
                        {
                            g.DrawArc(pen,
                                new RectangleF(new PointF(rect.X + _linePading.Left / 2f, rect.Y + _linePading.Left / 2f),
                                new Size(rect.Width - _linePading.Left, rect.Height - _linePading.Left)), 45, 360f);
                        }
                        //g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    }
                    else
                    {
                        using (var path = DrawHelper.CreateRoundPath(rect, radiu, _linePading.Left))
                        using (var pen = new Pen(Color.FromArgb(Trans, temp.R, temp.G, temp.B), _linePading.Left))
                        {
                            g.DrawPath(pen, path);
                        }
                    }
                }
            }
            else
            {
                if (color != Color.Empty)
                {
                    using (var solidBrush = new SolidBrush(color))
                    {
                        g.FillRectangle(solidBrush, item.Rectangle);
                    }
                }
                if (colorLine != Color.Empty)
                {
                    var rect = item.Rectangle;
                    var temp = colorLine;
                    if (temp == Color.Transparent) temp = BitmapHelper.RGBAddLight(color == Color.Empty ? Color.LightGray : color, -15);
                    else temp = TranColor(colorLine);
                    if (_linePading.All == 1)
                    {
                        g.PixelOffsetMode = PixelOffsetMode.Default;
                        rect = new Rectangle(rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
                        using (var pen = new Pen(Color.FromArgb(Trans, temp.R, temp.G, temp.B), _linePading.All))
                        {
                            g.DrawRectangle(pen, rect);
                        }
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        return;
                    }
                    if (_linePading.Left > 0)
                    {
                        var line = _linePading.Left / 2f;
                        using (var pen = new Pen(Color.FromArgb(Trans, temp.R, temp.G, temp.B), _linePading.Left))
                        {
                            g.DrawLine(pen, rect.X + line, rect.Y + _linePading.Top, rect.X + line, rect.Bottom - _linePading.Bottom);
                        }
                    }
                    if (_linePading.Top > 0)
                    {
                        var line = _linePading.Top / 2f;
                        using (var pen = new Pen(Color.FromArgb(Trans, temp.R, temp.G, temp.B), _linePading.Top))
                        {
                            g.DrawLine(pen, rect.X, rect.Y + line, rect.Right, rect.Y + line);
                        }
                    }
                    if (_linePading.Right > 0)
                    {
                        var line = _linePading.Right / 2f;
                        using (var pen = new Pen(Color.FromArgb(Trans, temp.R, temp.G, temp.B), _linePading.Right))
                        {
                            g.DrawLine(pen, rect.Right - line, rect.Y + _linePading.Top, rect.Right - line, rect.Bottom - _linePading.Bottom);
                        }
                    }
                    if (_linePading.Bottom > 0)
                    {
                        var line = _linePading.Bottom / 2f;
                        using (var pen = new Pen(Color.FromArgb(Trans, temp.R, temp.G, temp.B), _linePading.Bottom))
                        {
                            g.DrawLine(pen, rect.X, rect.Bottom - line, rect.Right, rect.Bottom - line);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 绘制鼠标移入时的背景
        /// </summary>
        private void DrawMoveBack(Graphics g, ToolItem item)
        {
            var color = TranColor(item.TColor.ColorMove == Color.Empty ? TBackGround.ColorMove : item.TColor.ColorMove);
            if (color == Color.Empty)
            {
                var temp = _moveImage ?? _moveImage2;
                if (temp != null) g.DrawImage(temp, item.Rectangle);
            }
            //else
            {
                DrawBackground(g, color, TranColor(TLineColor.ColorMove), item);
            }
        }

        /// <summary>
        /// 绘制图片
        /// </summary>
        private void DrawImage(Graphics g, ToolItem item)
        {
            if (_iImageShow && item.Image != null)
            {
                var imageRect = Rectangle.Empty;
                switch (_tLocation)
                {
                    case TLocation.Up:
                        var width = (_itemSize.Width - _imageSizeShow.Width - _textPading.Left - _textPading.Right) / 2;
                        imageRect.X = item.Rectangle.X + _textPading.Left + width;
                        imageRect.Y = item.Rectangle.Y + _textPading.Top;
                        break;
                    case TLocation.Left:
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
        /// 绘制文字
        /// </summary>
        private void DrawText(Graphics g, ToolItem item)
        {
            Rectangle textRect;
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
                        case TLocation.Up:
                            textRect.Y += _imageSizeShow.Height + _textPading.Top;
                            textRect.Height = item.Rectangle.Height - _imageSizeShow.Height - _textPading.Top * 2 -
                                              _textPading.Bottom;
                            break;
                        case TLocation.Left:
                            textRect.X += _imageSizeShow.Width + _textPading.Left;
                            textRect.Width = item.Rectangle.Width - _imageSizeShow.Width - _textPading.Left * 2 -
                                             _textPading.Right;
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(item.HeadDesc))
                {
                    var headHeight = HeightFont(item.MouseState, THeadDesc);
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
                if (!string.IsNullOrEmpty(item.EndDesc))
                {
                    var endHeight = HeightFont(item.MouseState, TEndDesc);
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
                    var size = TextRenderer.MeasureText(item.Desc, GetFont(item.TMouseState, TDesc));
                    textRect.Width -= size.Width;
                }
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
                                X = textRect.X,
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
                    return pro.HeightNormal;
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
        /// 绘制正文描述
        /// </summary>
        /// <param name="item"></param>
        /// <param name="rect"></param>
        /// <param name="g"></param>
        private void DrawDesc(Graphics g, ToolItem item, Rectangle rect)
        {
            if (string.IsNullOrEmpty(item.Desc)) return;
            var size = TextRenderer.MeasureText(item.Desc, GetFont(item.TMouseState, TDesc));
            item.RectDesc = new Rectangle(rect.X + rect.Width, rect.Y + (rect.Height - size.Height) / 2, size.Width, size.Height);
            DrawOtherDesc(g, item, TDesc, item.Desc, item.RectDesc, item.TMouseState);
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

            var color = ForeColor;
            var font = desc.FontNormal;

            if (!item.Enable)
            {
                var temp = new Rectangle(rect.X + Offset.X, rect.Y + Offset.Y, rect.Width, rect.Height);
                TextRenderer.DrawText(g, text, font, temp, color, desc.TextFormat);
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
            {
                var temp = new Rectangle(rect.X + Offset.X, rect.Y + Offset.Y, rect.Width, rect.Height);
                TextRenderer.DrawText(g, text, font, temp, color, desc.TextFormat);
            }
        }

        /// <summary>
        /// 重绘正文描述
        /// </summary>
        /// <param name="item"></param>
        /// <param name="state"></param>
        private void InvaRectDesc(ToolItem item, TMouseState state)
        {
            if (item.TMouseState != state)
            {
                item.TMouseState = state;
                Invalidate(item);
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
                Invalidate(item);
            }
        }

        /// <summary>
        /// 重绘Item
        /// </summary>
        /// <param name="item"></param>
        private void Invalidate(ToolItem item)
        {
            var rect = new Rectangle(item.Rectangle.X + Offset.X, item.Rectangle.Y + Offset.Y, item.Rectangle.Width, item.Rectangle.Height);
            Invalidate(rect);
        }

        /// <summary>
        /// 引发 System.Windows.Forms.Form.MouseMove 事件。
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
                    if (item.RectDesc.Contains(point))
                    {
                        if (item.TMouseState != TMouseState.Down)
                        {
                            InvaRectDesc(item, TMouseState.Move);
                        }
                    }
                    if (_iClickEvent || item.MouseState != TMouseState.Down)
                    {
                        flag = false;
                        if (item.MouseState != TMouseState.Move && item.MouseState != TMouseState.Down)
                        {
                            InvalidateItem(item, TMouseState.Move);
                            if (_iShowTop)
                            {
                                var hit = item.Hit;
                                if (hit.IsNullOrEmpty()) hit = item.Sencond ?? item.First;
                                ShowTooTip(hit);
                            }
                        }
                    }
                }
                else
                {
                    InvaRectDesc(item, TMouseState.Normal);
                    if (_iClickEvent || item.MouseState != TMouseState.Down)
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
        /// 引发 System.Windows.Forms.Form.MouseLeave 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (INormal || DesignMode) return;

            MoveItem = null;
            foreach (var item in Items)
            {
                InvaRectDesc(item, TMouseState.Normal);
                if ((_iClickEvent && !_iMultiple) || item.MouseState != TMouseState.Down)
                {
                    InvalidateItem(item, TMouseState.Normal);
                }
            }
            ClearTemp();
        }
        private void ClearTemp()
        {
            if (_tempItem != null && _tempItem != _selectedItem)
            {
                InvalidateItem(_tempItem, TMouseState.Normal);
                _tempItem = null;
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
                    OnMouseDown(point, item);
                }
                else if (!INormal && !_iMultiple && contain && item != _selectedItem)
                {
                    InvalidateItem(item, TMouseState.Normal);
                }
            }
        }

        private void OnMouseDown(Point point, ToolItem item)
        {
            if (item != _tempItem)
            {
                _tempItem = item;
            }
            if (INormal) return;
            if (item.RectDesc.Contains(point))
            {
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
            ClearTemp();
        }

        private void OnMouseUp(Point point, ToolItem item, EventArgs e)
        {
            if (!INormal)
            {
                if (item.RectDesc.Contains(point))
                {
                    InvaRectDesc(item, TMouseState.Move);
                }
                else
                {
                    InvaRectDesc(item, TMouseState.Normal);
                }
                if (!_iMultiple && _iClickEvent && item.MouseState == TMouseState.Down)
                {
                    InvalidateItem(item, TMouseState.Move);
                }
            }
            if (item == _tempItem)
            {
                //事件
                _tempItem = null;
                if (item.RectDesc.Contains(point))
                {
                    var ifocus = OnEditClick(item, e);
                    if (!ifocus)
                    {
                        Invalidate(item);
                    }
                }
                else
                {
                    if (item != _selectedItem)
                    {
                        if (_selectedItem != null) InvalidateItem(_selectedItem, TMouseState.Normal);
                        _selectedItem = item;
                        _selectedIndex = Items.GetIndexOfRange(item);
                        OnSelectedItemChanged(item, e);
                    }
                    OnItemClick(item, e);
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
            if (iScrollHide && ParentForm.ContainsFocus)
            {
                //设置焦点也许是为了滚动条，目前并无影响，暂且去掉
                //Focus();
            }
        }

        /// <summary>
        /// 数据源更新-  BindingList提供
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Items_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                Items[e.NewIndex].Owner = this;
            }
            if (iSuspend || !ILoad) return;
            if (Items.Count > 1 && e.ListChangedType == ListChangedType.ItemAdded && e.NewIndex == Items.Count - 1)
            {
                var index = Items.Count - 2;
                var xPos = Items[index].Rectangle.X;
                var yPos = Items[index].Rectangle.Y;
                var lastWidth = 0;
                CalcItem(Items[index], ref xPos, ref yPos, ref lastWidth, true, true);
                CalcItem(Items[index + 1], ref xPos, ref yPos, ref lastWidth, false);

                UpdateScroll();
                TEnd();
                Invalidate(Items[index + 1]);
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

        #endregion

        #region 对外公共方法
        /// <summary>
        /// 获取所有选中项
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
        /// 坐标点是否包含在项中
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override bool Contain(Point point)
        {
            if (INormal) return false;
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
        /// 坐标点是否包含在项描述中
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
        /// 移除指定First项或Tag项
        /// </summary>
        /// <param name="text"></param>
        public void TRemove(string text)
        {
            for (var i = 0; i < Items.Count; i++)
            {
                if (Items[i].First == text)
                {
                    Items.RemoveAt(i);
                    return;
                }
            }
            for (var i = 0; i < Items.Count; i++)
            {
                if (Items[i].Tag.ToStrs() == text)
                {
                    Items.RemoveAt(i);
                    break;
                }
            }
        }
        /// <summary>
        /// 查找指定First项或Tag项
        /// </summary>
        public ToolItem FindItem(string text)
        {
            for (var i = 0; i < Items.Count; i++)
            {
                if (Items[i].First == text)
                {
                    return Items[i];
                }
            }
            for (var i = 0; i < Items.Count; i++)
            {
                if (Items[i].Tag.ToStrs() == text)
                {
                    return Items[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 单击第一项
        /// </summary>
        public void TClickFirst()
        {
            TClickItem(0);
        }
        /// <summary>
        /// 单击指定First项或Tag项
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
                if (Items[i].Tag.ToStrs() == text)
                {
                    TClickItem(i);
                    break;
                }
            }
        }
        /// <summary>
        /// 单击项
        /// </summary>
        public void TClickItem(ToolItem item)
        {
            var index = Items.GetIndexOfRange(item);
            TClickItem(index);
        }
        /// <summary>
        /// 单击项
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
            if (index > -1)
            {
                _selectedItem = _items[index];
                if (!_iClickEvent)
                {
                    InvalidateItem(_selectedItem, TMouseState.Down);
                }
                OnSelectedItemChanged(_items[index], EventArgs.Empty);
                OnItemClick(_items[index], EventArgs.Empty);
            }
        }

        /// <summary>
        /// 刷新控件到指定序号Item(仅适合单行)
        /// </summary>
        public void TStart(int index = 0)
        {
            if (!IHaveScroll || index <= 0)
            {
                FixScroll(0);
                return;
            }
            switch (TDirection)
            {
                case TDirection.Level:
                    int value = Padding.Top + ItemSize.Height * index + ItemSpace * index;
                    FixScroll(value);
                    break;
                case TDirection.Vertical:
                    value = Padding.Left + ItemSize.Width * index + ItemSpace * index;
                    FixScroll(value);
                    break;
            }
        }
        /// <summary>
        /// 当前滚动条是否在最底部
        /// </summary>
        public bool IEnd()
        {
            if (!IHaveScroll) return true;
            switch (TDirection)
            {
                case TDirection.Level:
                    return _vScroll.Value == _vScroll.Maximum - _vScroll.SmallChange;
                case TDirection.Vertical:
                    return _vScroll2.Value == _vScroll2.Maximum - _vScroll2.SmallChange;
            }
            return true;
        }
        /// <summary>
        /// 刷新控件到尾部
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
        /// 刷新项
        /// </summary>
        /// <param name="item"></param>
        public void TRefresh(ToolItem item)
        {
            var index = Items.GetIndexOfRange(item);
            TRefresh(index);
        }
        /// <summary>
        /// 刷新项
        /// </summary>
        /// <param name="index"></param>
        public void TRefresh(int index)
        {
            if (index < 0 || index > Items.Count - 1) return;
            Invalidate(Items[index]);
        }

        /// <summary>
        /// 刷新控件
        /// </summary>
        public void TRefresh()
        {
            _vScroll.Visible = false;
            _hScroll.Visible = false;
            _vScroll2.Visible = false;
            _panelScroll.Visible = false;
            TPaint();
            UpdateScroll();
            Invalidate();
        }

        #endregion

        #region 延迟隐藏滚动条
        private void InitHide()
        {
            this.MouseEnter += ToolBar_MouseEnter;
            _vScroll.MouseEnter += ToolBar_MouseEnter;
            _hScroll.MouseEnter += ToolBar_MouseEnter;

            this.MouseDown += ToolBar_MouseEnter;
            _vScroll.MouseDown += ToolBar_MouseEnter;
            _hScroll.MouseDown += ToolBar_MouseEnter;

            this.MouseLeave += ToolBar_MouseLeave;
            _vScroll.MouseLeave += ToolBar_MouseLeave;
            _hScroll.MouseLeave += ToolBar_MouseLeave;
        }
        private void ToolBar_MouseLeave(object sender, EventArgs e)
        {
            AutoMouseStatu(false);
        }
        private void ToolBar_MouseEnter(object sender, EventArgs e)
        {
            AutoMouseStatu(true);
        }
        private void AutoMouseStatu(bool iMouseStatu)
        {
            this.iMouseStatu = iMouseStatu;
            if (_panelScroll.Visible == iScrollHide & _iScroll & this.iMouseStatu) return;
            if (iMouseStatu)
            {
                _panelScroll.Visible = iScrollHide & _iScroll & iMouseStatu;
            }
            else
            {
                new Action<int>(AutoMouseStatu2).BeginInvoke(125, null, null);
            }
        }
        private void AutoMouseStatu2(int delay)
        {
            System.Threading.Thread.Sleep(delay);
            if (DesignMode) return;
            this.Invoke(new Action(AutoMouseStatu3));
        }
        private void AutoMouseStatu3()
        {
            _panelScroll.Visible = iScrollHide & _iScroll & iMouseStatu;
        }

        #endregion

        #region 挂起事件
        /// <summary>
        /// 挂起，不引发事件
        /// </summary>
        public void Suspend()
        {
            iSuspend = true;
        }
        /// <summary>
        /// 恢复
        /// </summary>
        public void Resume()
        {
            iSuspend = false;
            TRefresh();
        }

        #endregion

        #region 动态显示项的图片
        private readonly Timer _tDynamic = new Timer();
        private PictureBox _pictureBox1;

        /// <summary>
        /// 动态项原图片
        /// </summary>
        private Image _image;

        /// <summary>
        /// 动态项
        /// </summary>
        private int progressItemIndex;

        /// <summary>
        /// 项的动态图片
        /// </summary>
        [Description("项的动态图片")]
        [DefaultValue(null)]
        public Image TProgressImage
        {
            get { return _pictureBox1.Image; }
            set { _pictureBox1.Image = value; }
        }

        /// <summary>
        /// 初始化动态方法
        /// </summary>
        private void Progress()
        {
            _tDynamic.Interval = 30;
            _tDynamic.Tick += TDynamic_Tick;
        }

        /// <summary>
        /// 动态显示项的图像
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
        /// 动态显示项的图像
        /// </summary>
        /// <param name="index">项索引</param>
        /// <param name="newText">项新文本</param>
        public void TProgressItem(int index, string newText = null)
        {
            progressItemIndex = index;
            if (!string.IsNullOrEmpty(newText)) _items[progressItemIndex].Text = newText;
            _image = _items[progressItemIndex].Image;
            if (_pictureBox1.Image != null)
            {
                _items[progressItemIndex].Image = _pictureBox1.Image;
            }
            _tDynamic.Enabled = true;
        }

        /// <summary>
        /// 停止动态显示
        /// </summary>
        /// <param name="image">图片</param>
        /// <param name="text">项文本</param>
        public void TProgressStop(Image image = null, string text = null)
        {
            _tDynamic.Enabled = false;
            if (!string.IsNullOrEmpty(text)) _items[progressItemIndex].Text = text;
            _items[progressItemIndex].Image = image ?? this._image;
            TDynamic_Tick(this, EventArgs.Empty);
        }

        private void TDynamic_Tick(object sender, EventArgs e)
        {
            Invalidate(_items[progressItemIndex]);
        }

        private void InitializeComponent()
        {
            _pictureBox1 = new PictureBox();
            ((ISupportInitialize)_pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            _pictureBox1.Location = new Point(0, 0);
            _pictureBox1.Name = "pictureBox1";
            _pictureBox1.Size = new Size(1, 1);
            _pictureBox1.TabIndex = 0;
            _pictureBox1.TabStop = false;
            // 
            // ToolBar
            // 
            Controls.Add(_pictureBox1);
            Name = "ToolBar";
            ((ISupportInitialize)_pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        #region 变色项
        private readonly Timer _tChange = new Timer();

        /// <summary>
        /// 动态项
        /// </summary>
        private int index;

        private void InitChange()
        {
            _tChange.Interval = 600;
            _tChange.Tick += TChange_Tick;
        }

        /// <summary>
        /// 开始变色
        /// </summary>
        public void TChangeStart()
        {
            _tChange.Enabled = true;
        }

        /// <summary>
        /// 停止单项变色
        /// </summary>
        public void TChangeStop(ToolItem item)
        {
            item.IChange = false;
            for (var i = 0; i < Items.Count; i++)
            {
                if (Items[i].IChange)
                {
                    return;
                }
            }
            _tChange.Enabled = false;
        }
        /// <summary>
        /// 停止变色
        /// </summary>
        public void TChangeStop()
        {
            _tChange.Enabled = false;
        }

        private void TChange_Tick(object sender, EventArgs e)
        {
            var result = false;
            for (var i = 0; i < Items.Count; i++)
            {
                if (Items[i].IChange)
                {
                    result = true;
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
                    //已自动更新项
                    //Application.DoEvents();
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
        private TPanel _panelScroll;
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
        [Description("是否显示滚动条")]
        [DefaultValue(true)]
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
        /// 获取滚动条是否有显示
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
        public bool IHaveScroll
        {
            get
            {
                return iScrollHide & _iScroll;
            }
        }

        /// <summary>
        /// 获取当前滚动条的值
        /// </summary>
        [Browsable(false)]
        [DefaultValue(0)]
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
        /// 控件显示偏移坐标
        /// </summary>
        private Point Offset = Point.Empty;

        /// <summary>
        /// 初始化滚动条
        /// </summary>
        private void CustomScroll()
        {
            _panelScroll = new TPanel();
            Controls.Add(_panelScroll);

            _hScroll = new HScrollBar();
            _hScroll.Scroll += HScroll_Scroll;
            //_hScroll.Dock = DockStyle.Bottom;
            //_hScroll.Height = _tScrollHeight;
            _panelScroll.Controls.Add(_hScroll);
            {
                _vScroll2 = new VScrollBar()
                {
                    //_vScroll2.Scroll += _hScroll_Scroll;
                    Dock = DockStyle.Right,
                    Width = 0
                };
                Controls.Add(_vScroll2);
            }

            _vScroll = new VScrollBar();
            _vScroll.Scroll += VScroll_Scroll;
            //_vScroll.Dock = DockStyle.Right;
            //_vScroll.Width = _tScrollHeight;
            _panelScroll.Controls.Add(_vScroll);

            _vScroll.Visible = false;
            _hScroll.Visible = false;
            _vScroll2.Visible = false;
            _panelScroll.Visible = false;
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
                    var value = _vScroll.Value - e.Delta * (ItemSize.Height + ItemSpace) / 120;
                    FixScroll(value);
                    break;
                case TDirection.Vertical:
                    value = _vScroll2.Value - e.Delta * (ItemSize.Width + ItemSpace) / 120;
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
                    var max = _vScroll.Maximum - _vScroll.SmallChange;
                    if (value < 0) value = 0;
                    if (value > max) value = max;
                    valid &= _vScroll.Value != value;
                    _vScroll.Value = value;
                    Offset.Y = -value;
                    break;
                case TDirection.Vertical:
                    max = _vScroll2.Maximum - _vScroll2.SmallChange;
                    if (value < 0) value = 0;
                    if (value > max) value = max;
                    _hScroll.Value = value;
                    valid &= _vScroll2.Value != value;
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
        /// 大小改变时
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            _vScroll.Visible = false;
            _hScroll.Visible = false;
            _vScroll2.Visible = false;
            _panelScroll.Visible = false;
            TPaint();
            UpdateScroll();
        }

        /// <summary>
        /// 更新滚动条状态
        /// </summary>
        private void UpdateScroll()
        {
            _vScroll.Visible = false;
            _hScroll.Visible = false;
            _vScroll2.Visible = false;
            _panelScroll.Visible = false;
            iScrollHide = false;
            switch (TDirection)
            {
                case TDirection.Level:
                    var height = GetHeight();
                    if (Height < height)
                    {
                        iScrollHide = true;
                        _vScroll.Visible = _iScroll;
                        _panelScroll.Width = _tScrollHeight;
                        _panelScroll.Dock = DockStyle.Right;
                        _panelScroll.Visible = ILoad & _iScroll;
                        //_panelScroll.Visible = ILoad & DesignMode;
                        _vScroll.Size = new Size(_panelScroll.Width, _panelScroll.Height + 17 * 2 + 1);
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
                        _panelScroll.Height = _tScrollHeight;
                        _panelScroll.Dock = DockStyle.Bottom;
                        _panelScroll.Visible = ILoad & _iScroll;
                        //_panelScroll.Visible = ILoad & DesignMode;
                        _hScroll.Size = new Size(_panelScroll.Width + 17 * 2 + 1, _panelScroll.Height);
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
                        break;
                }
            }
        }

        /// <summary>
        /// 水平滚动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HScroll_Scroll(object sender, ScrollEventArgs e)
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
                Invalidate();
            }
        }

        /// <summary>
        /// 垂直滚动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VScroll_Scroll(object sender, ScrollEventArgs e)
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
                Invalidate();
            }
        }

        /// <summary>
        /// 需要的高度
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
        /// 需要的宽度
        /// </summary>
        /// <returns></returns>
        private int GetWidth()
        {
            if (TCountLine == 1 && Items.Count > 0) return Items[Items.Count - 1].Rectangle.Right;
            var width = Padding.Left + Padding.Right;
            width += TCountColumn * ItemSize.Width;
            width += (TCountColumn - 1) * ItemSpace;
            width += THeardLength;
            return width;
        }

        #endregion

        #region 悬停窗口显示说明
        /// <summary>
        /// 表示一个长方形的小弹出窗口，该窗口在用户将指针悬停在一个控件上时显示有关该控件用途的简短说明。
        /// </summary>
        private void ShowTooTip(string toolTipText)
        {
            _toolTop.Active = true;
            _toolTop.SetToolTip(this, toolTipText);
        }

        /// <summary>
        /// 弹出窗口不活动
        /// </summary>
        private void HideToolTip()
        {
            if (_toolTop.Active)
            {
                _toolTop.Active = false;
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
                _downImage = null;
                _moveImage = null;
                _normalImage = null;
                _selectImage = null;
            }
            if (_backGround != null)
            {
                _backGround.Dispose();
                _backGround = null;
            }
            if (_lineColor != null)
            {
                _lineColor.Dispose();
                _lineColor = null;
            }
            if (_change != null)
            {
                _change.Dispose();
                _change = null;
            }
            if (_desc != null)
            {
                _desc.Dispose();
                _desc = null;
            }
            if (_downImage2 != null)
            {
                _downImage2.Dispose();
            }
            if (_endDesc != null)
            {
                _endDesc.Dispose();
                _endDesc = null;
            }
            if (_headDesc != null)
            {
                _headDesc.Dispose();
                _headDesc = null;
            }
            if (_hScroll != null)
            {
                _hScroll.Dispose();
                _hScroll = null;
            }
            if (_moveImage2 != null)
            {
                _moveImage2.Dispose();
            }
            if (_normalImage2 != null)
            {
                _normalImage2.Dispose();
            }
            if (_selectedItem != null)
            {
                _selectedItem.Dispose();
                _selectedItem = null;
            }
            if (_selectImage2 != null)
            {
                _selectImage2.Dispose();
            }
            if (_text != null)
            {
                _text.Dispose();
                _text = null;
            }
            if (_textSencond != null)
            {
                _textSencond.Dispose();
                _textSencond = null;
            }
            if (_toolTop != null)
            {
                _toolTop.Dispose();
                _toolTop = null;
            }
            if (_vScroll != null)
            {
                _vScroll.Dispose();
                _vScroll = null;
            }
            if (_vScroll2 != null)
            {
                _vScroll2.Dispose();
                _vScroll2 = null;
            }

            _image = null;
            if (_tDynamic != null)
            {
                _tDynamic.Stop();
                _tDynamic.Dispose();
            }
            if (_pictureBox1 != null)
            {
                _pictureBox1.Image = null;
                _pictureBox1.Dispose();
                _pictureBox1 = null;
            }
            if (_tChange != null)
            {
                _tChange.Stop();
                _tChange.Dispose();
            }
            if (_panelScroll != null)
            {
                _panelScroll.Dispose();
                _panelScroll = null;
            }
            if (MoveItem != null)
            {
                MoveItem.Dispose();
                MoveItem = null;
            }
            if (_items != null)
            {
                Suspend();
                _items.Dispose();
                _items = null;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}