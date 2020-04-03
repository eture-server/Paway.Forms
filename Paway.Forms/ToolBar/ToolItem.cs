using Paway.Helper;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    /// 表示 ToolBar 控件中的单个项。
    /// </summary>
    public class ToolItem : IDisposable
    {
        #region 字段与属性
        /// <summary>
        /// 描述信息的鼠标状态
        /// </summary>
        internal TMouseState MouseDescState { get; set; }

        /// <summary>
        /// Item 当前的鼠标状态
        /// </summary>
        [Browsable(false), Description("Item 当前的鼠标状态")]
        [DefaultValue(TMouseState.Normal)]
        internal TMouseState MouseState { get; set; }

        /// <summary>
        /// 拥有者
        /// </summary>
        [Browsable(false), Description("拥有者")]
        [DefaultValue(typeof(ToolBar), "Tinn")]
        public ToolBar Owner { get; internal set; }

        private Image _image;
        /// <summary>
        /// Item 显示的图片
        /// </summary>
        [DefaultValue(null)]
        [Description("Item 显示的图片")]
        public Image Image
        {
            get { return _image; }
            set
            {
                _image = value;
                TRefresh();
            }
        }
        /// <summary>
        /// 鼠标按下时的图片
        /// </summary>
        private Image _imageDown;
        /// <summary>
        /// 鼠标按下时的图片
        /// </summary>
        [DefaultValue(null)]
        [Description("鼠标按下时的图片")]
        public Image ImageDown
        {
            get { return _imageDown; }
            set
            {
                _imageDown = value;
                TRefresh();
            }
        }
        /// <summary>
        /// 鼠标划过时的图片
        /// </summary>
        private Image _imageMove;
        /// <summary>
        /// 鼠标划过时的图片
        /// </summary>
        [DefaultValue(null)]
        [Description("鼠标划过时的图片")]
        public Image ImageMove
        {
            get { return _imageMove; }
            set
            {
                _imageMove = value;
                TRefresh();
            }
        }

        /// <summary>
        /// 当前 Item 图片显示的 Rectangle
        /// </summary>
        [Browsable(false)]
        public Rectangle ImageRect { get; internal set; }

        /// <summary>
        /// 当前 Item 在 ToolBar 中的 Rectangle
        /// </summary>
        [Browsable(false)]
        public Rectangle Rectangle { get; internal set; }

        /// <summary>
        /// 获取或设置包含有关控件的数据的对象。
        /// </summary>
        [Description("获取或设置包含有关控件的数据的对象")]
        [DefaultValue(null)]
        [TypeConverter(typeof(StringConverter))]
        public object Tag { get; set; }

        /// <summary>
        /// 首行文字
        /// </summary>
        [Browsable(false), Description("首行文字")]
        [DefaultValue(null)]
        public string First { get; private set; }

        /// <summary>
        /// 自动显示快捷键
        /// </summary>
        internal string FirstKeys()
        {
            if (Keys == Keys.None) return First;
            var key = Keys;
            if ((key & Keys.Control) == Keys.Control) key -= Keys.Control;
            if ((key & Keys.Shift) == Keys.Shift) key -= Keys.Shift;
            if ((key & Keys.Alt) == Keys.Alt) key -= Keys.Alt;
            return $"{First}({key})";
        }
        /// <summary>
        /// 自动项提示
        /// </summary>
        internal string TextHit()
        {
            var hit = Hit;
            if (hit.IsNullOrEmpty()) hit = Text;
            if (Keys == Keys.None) return hit;
            var desc = string.Empty;
            var key = Keys;
            if ((key & Keys.Control) == Keys.Control)
            {
                desc += "Ctrl+";
                key -= Keys.Control;
            }
            if ((key & Keys.Shift) == Keys.Shift)
            {
                desc += Keys.Shift + "+";
                key -= Keys.Shift;
            }
            if ((key & Keys.Alt) == Keys.Alt)
            {
                desc += Keys.Alt + "+";
                key -= Keys.Alt;
            }
            desc += key;
            return hit.Replace(First, $"{First}({desc})");
        }

        /// <summary>
        /// 其它行文字
        /// </summary>
        [Browsable(false), Description("其它行文字")]
        [DefaultValue(null)]
        public string Sencond { get; private set; }

        private string _text = string.Empty;
        /// <summary>
        /// Item 上显示的文字信息
        /// </summary>
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [DefaultValue("")]
        public string Text
        {
            get { return _text; }
            set
            {
                if (_text == value) return;
                _text = value;
                First = null;
                Sencond = null;
                if (string.IsNullOrEmpty(value)) return;
                var text = _text.Split(new[] { "\r\n", "&&" }, StringSplitOptions.RemoveEmptyEntries);
                if (text.Length > 0)
                {
                    First = text[0];
                }
                for (var i = 1; i < text.Length; i++)
                {
                    Sencond = string.Format("{0}{1}\r\n", Sencond, text[i]);
                }
                if (Sencond != null)
                {
                    Sencond = Sencond.TrimEnd('\r', '\n');
                }
                TRefresh();
            }
        }

        private string _desc;
        /// <summary>
        /// Item 上显示的描述信息
        /// </summary>
        [Description("Item 上显示的描述信息")]
        [DefaultValue(null)]
        public string Desc
        {
            get { return _desc; }
            set
            {
                if (_desc != value)
                {
                    _desc = value;
                    TRefresh();
                }
            }
        }
        /// <summary>
        /// Item 上显示的描述信息边框
        /// </summary>
        [Browsable(false)]
        public Rectangle RectDesc { get; internal set; }

        private string _headDesc;
        /// <summary>
        /// Item 上显示的头部描述信息
        /// </summary>
        [Description("Item 上显示的头部描述信息")]
        [DefaultValue(null)]
        public string HeadDesc
        {
            get { return _headDesc; }
            set
            {
                if (_headDesc != value)
                {
                    _headDesc = value;
                    TRefresh();
                }
            }
        }

        /// <summary>
        /// Item 上显示的头部描述信息边框
        /// </summary>
        [Browsable(false)]
        public Rectangle RectHeadDesc { get; internal set; }

        private string _endDesc;
        /// <summary>
        /// Item 上显示的尾部描述信息
        /// </summary>
        [Description("Item 上显示的尾部描述信息")]
        [DefaultValue(null)]
        public string EndDesc
        {
            get { return _endDesc; }
            set
            {
                if (_endDesc != value)
                {
                    _endDesc = value;
                    TRefresh();
                }
            }
        }

        /// <summary>
        /// Item 上显示的尾部描述信息边框
        /// </summary>
        [Browsable(false)]
        public Rectangle RectEndDesc { get; internal set; }

        /// <summary>
        /// Item 上鼠标悬停显示信息
        /// </summary>
        [Description("Item 上显示的尾部描述信息")]
        [DefaultValue(null)]
        public string Hit { get; set; }

        private TProperties _color;
        /// <summary>
        /// 优先应用于项的背景色
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TProperties TColor
        {
            get
            {
                if (_color == null)
                {
                    _color = new TProperties();
                    _color.ValueChange += delegate { TRefresh(); };
                }
                return _color;
            }
        }

        private bool _enable = true;
        /// <summary>
        /// Item 当前启用状态
        /// </summary>
        [Description("Item 当前启用状态")]
        [DefaultValue(true)]
        public bool Enable
        {
            get { return _enable; }
            set
            {
                if (_enable != value)
                {
                    _enable = value;
                    TRefresh();
                }
            }
        }

        private bool _visible = true;
        /// <summary>
        /// Item 当前显示状态
        /// </summary>
        [Description("Item 当前显示状态")]
        [DefaultValue(true)]
        public bool Visible
        {
            get { return _visible; }
            set
            {
                if (_visible != value)
                {
                    _visible = value;
                    this.Owner?.TRefresh();
                }
            }
        }

        /// <summary>
        /// Item 当前选中状态
        /// </summary>
        [Browsable(false), Description("Item 当前选中状态")]
        [DefaultValue(false)]
        public bool Selete
        {
            get { return MouseState == TMouseState.Down; }
            set
            {
                MouseState = value ? TMouseState.Down : TMouseState.Normal;
                TRefresh();
            }
        }

        private Padding _tRadiu = new Padding(0);
        /// <summary>
        /// 圆角大小
        /// </summary>
        [Description("圆角大小")]
        [DefaultValue(typeof(Padding), "0,0,0,0")]
        public Padding TRadiu
        {
            get { return _tRadiu; }
            set
            {
                if (_tRadiu != value)
                {
                    _tRadiu = value;
                    TRefresh();
                }
            }
        }

        private bool _iHeard;
        /// <summary>
        /// 头文字
        /// </summary>
        [Description("头文字")]
        [DefaultValue(false)]
        public bool IHeard
        {
            get { return _iHeard; }
            set
            {
                if (_iHeard != value)
                {
                    _iHeard = value;
                    TRefresh();
                }
            }
        }

        /// <summary>
        /// 获取或设置项内的空白
        /// </summary>
        private Padding _textPading = new Padding(0);
        /// <summary>
        /// 获取或设置项内的空白
        /// </summary>
        [Description("获取或设置项内的空白")]
        [DefaultValue(typeof(Padding), "0,0,0,0")]
        public Padding TextPading
        {
            get { return _textPading; }
            set
            {
                if (_textPading != value)
                {
                    _textPading = value;
                    TRefresh();
                }
            }
        }

        /// <summary>
        /// 首行文字属性更新标记
        /// </summary>
        internal bool ITextFirst;
        private TProperties _textFirst;
        /// <summary>
        /// 首行文字属性
        /// </summary>
        [Description("首行文字属性")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TProperties TextFirst
        {
            get
            {
                if (_textFirst == null)
                {
                    _textFirst = new TProperties();
                    _textFirst.ValueChange += delegate (bool result)
                    {
                        ITextFirst = result;
                        TRefresh();
                    };
                }
                return _textFirst;
            }
        }

        /// <summary>
        /// 快捷键
        /// </summary>
        [DefaultValue(Keys.None)]
        [Description("快捷键")]
        public Keys Keys { get; set; }

        #endregion

        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        public ToolItem() { }

        /// <summary>
        /// 构造
        /// </summary>
        public ToolItem(string text, Image image = null)
        {
            Text = text;
            _image = image;
        }
        /// <summary>
        /// 构造
        /// </summary>
        public ToolItem(string text, Shortcut keys, Image image = null) : this(text, image)
        {
            this.Keys = (Keys)keys;
        }

        /// <summary>
        /// 刷新项
        /// </summary>
        private void TRefresh()
        {
            this.Owner?.TRefresh(this);
        }
        /// <summary>
        /// </summary>
        public override string ToString()
        {
            var desc = Text;
            if (IHeard) desc += ",IHeard";
            if (_desc != null) desc += ",desc:" + _desc;
            if (_headDesc != null) desc += ",headDesc:" + _headDesc;
            if (_endDesc != null) desc += ",_endDesc:" + _endDesc;
            return desc;
        }

        /// <summary>
        /// 当前显示图片
        /// </summary>
        /// <returns></returns>
        internal Image AutoImage()
        {
            switch (MouseState)
            {
                case TMouseState.Move:
                case TMouseState.Up:
                    if (ImageMove != null) return ImageMove;
                    break;
                case TMouseState.Down:
                    if (ImageDown != null) return ImageDown;
                    break;
            }
            return Image;
        }

        #endregion

        #region IDisposable
        /// <summary>
        /// 标识此对象已释放
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// 参数为true表示释放所有资源，只能由使用者调用
        /// 参数为false表示释放非托管资源，只能由垃圾回收器自动调用
        /// 如果子类有自己的非托管资源，可以重载这个函数，添加自己的非托管资源的释放
        /// 但是要记住，重载此函数必须保证调用基类的版本，以保证基类的资源正常释放
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            // 如果资源未释放
            // 这个判断主要用了防止对象被多次释放
            if (!disposed)
            {
                // 标识此对象已释放
                disposed = true;
                if (disposing)
                {
                    // TODO: 释放托管资源(托管的对象)。
                    _image = null;
                    Tag = null;
                    Owner = null;
                }

                // TODO: 释放未托管资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。
                // 未托管资源有：　
                // ApplicationContext, Brush, Component, ComponentDesigner, Container, Context, Cursor, 
                // FileStream, Font, Icon, Image, Matrix, Object, OdbcDataReader, OleDBDataReader, Pen, 
                // Regex, Socket, StreamWriter, Timer, Tooltip, 文件句柄, GDI资源, 数据库连接等等资源。
                if (_color != null)
                {
                    _color.Dispose();
                    _color = null;
                }
            }
        }

        /// <summary>
        /// 析构，释放非托管资源
        /// </summary>
        ~ToolItem()
        {
            // TODO: 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
            // 注意，不能在析构函数中释放托管资源，
            // 因为析构函数是有垃圾回收器调用的，可能在析构函数调用之前，类包含的托管资源已经被回收了，从而导致无法预知的结果。
            Dispose(false);
        }

        /// <summary>
        /// 释放资源
        /// 由类的使用者，在外部显示调用，释放类资源
        /// </summary>
        public void Dispose()
        {
            // 释放托管和非托管资源
            Dispose(true);
            // 将对象从垃圾回收器链表中移除，
            // 从而在垃圾回收器工作时，只释放托管资源，而不执行此对象的析构函数
            // TODO: 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}