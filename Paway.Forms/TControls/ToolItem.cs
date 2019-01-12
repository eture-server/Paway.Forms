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
    ///     表示 ToolBar 控件中的单个项。
    /// </summary>
    public class ToolItem : IDisposable
    {
        #region 属性
        /// <summary>
        ///     拥有者
        /// </summary>
        [Browsable(false), Description("拥有者")]
        [DefaultValue(typeof(ToolBar), "Tinn")]
        public ToolBar Owner { get; internal set; }

        private Image _image;
        /// <summary>
        ///     Item 显示的图片
        /// </summary>
        [Description("Item 显示的图片")]
        [DefaultValue(typeof(Image), "null")]
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
        ///     当前 Item 图片显示的 Rectangle
        /// </summary>
        [Browsable(false)]
        public Rectangle ImageRect { get; internal set; }

        /// <summary>
        ///     获取或设置包含有关控件的数据的对象。
        /// </summary>
        [Description("获取或设置包含有关控件的数据的对象")]
        [DefaultValue(null)]
        [TypeConverter(typeof(StringConverter))]
        public object Tag { get; set; }

        /// <summary>
        ///     首行文字
        /// </summary>
        [Browsable(false), Description("首行文字")]
        [DefaultValue(null)]
        public string First { get; private set; }

        /// <summary>
        ///     其它行文字
        /// </summary>
        [Browsable(false), Description("其它行文字")]
        [DefaultValue(null)]
        public string Sencond { get; private set; }

        private string _text = string.Empty;
        /// <summary>
        ///     Item 上显示的文字信息
        /// </summary>
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [DefaultValue("")]
        public string Text
        {
            get { return _text; }
            set
            {
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
        ///     Item 上显示的描述信息
        /// </summary>
        [Description("Item 上显示的描述信息")]
        [DefaultValue(null)]
        public string Desc
        {
            get { return _desc; }
            set
            {
                _desc = value;
                TRefresh();
            }
        }
        /// <summary>
        ///     Item 上显示的描述信息边框
        /// </summary>
        [Browsable(false)]
        public Rectangle RectDesc { get; internal set; }

        private string _headDesc;
        /// <summary>
        ///     Item 上显示的头部描述信息
        /// </summary>
        [Description("Item 上显示的头部描述信息")]
        [DefaultValue(null)]
        public string HeadDesc
        {
            get { return _headDesc; }
            set
            {
                _headDesc = value;
                TRefresh();
            }
        }

        /// <summary>
        ///     Item 上显示的头部描述信息边框
        /// </summary>
        [Browsable(false)]
        public Rectangle RectHeadDesc { get; internal set; }

        private string _endDesc;
        /// <summary>
        ///     Item 上显示的尾部描述信息
        /// </summary>
        [Description("Item 上显示的尾部描述信息")]
        [DefaultValue(null)]
        public string EndDesc
        {
            get { return _endDesc; }
            set
            {
                _endDesc = value;
                TRefresh();
            }
        }

        /// <summary>
        ///     Item 上显示的尾部描述信息边框
        /// </summary>
        [Browsable(false)]
        public Rectangle RectEndDesc { get; internal set; }

        /// <summary>
        ///     Item 上鼠标悬停显示信息
        /// </summary>
        [Description("Item 上显示的尾部描述信息")]
        [DefaultValue(null)]
        public string Hit { get; set; }

        private TProperties _color;
        /// <summary>
        ///     优先应用于项的背景色
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TProperties TColor
        {
            get
            {
                if (_color == null)
                {
                    _color = new TProperties(MethodBase.GetCurrentMethod());
                    _color.ValueChange += delegate { TRefresh(); };
                }
                return _color;
            }
        }

        /// <summary>
        ///     当前 Item 在 ToolBar 中的 Rectangle
        /// </summary>
        [Browsable(false)]
        public Rectangle Rectangle { get; internal set; }

        /// <summary>
        ///     描述信息的鼠标状态
        /// </summary>
        internal TMouseState TMouseState { get; set; }

        /// <summary>
        ///     Item 当前的鼠标状态
        /// </summary>
        [Browsable(false), Description("Item 当前的鼠标状态")]
        [DefaultValue(TMouseState.Normal)]
        internal TMouseState MouseState { get; set; }

        private bool _enable = true;
        /// <summary>
        ///     Item 当前启用状态
        /// </summary>
        [Description("Item 当前启用状态")]
        [DefaultValue(true)]
        public bool Enable
        {
            get { return _enable; }
            set
            {
                _enable = value;
                TRefresh();
            }
        }

        /// <summary>
        ///     Item 当前选中状态
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

        private int _tRadiu;
        /// <summary>
        ///     圆角大小
        /// </summary>
        [Description("圆角大小")]
        [DefaultValue(0)]
        public int TRadiu
        {
            get { return _tRadiu; }
            set
            {
                _tRadiu = value;
                TRefresh();
            }
        }

        private bool _iHeard;
        /// <summary>
        ///     头文字
        /// </summary>
        [Description("头文字")]
        [DefaultValue(false)]
        public bool IHeard
        {
            get { return _iHeard; }
            set
            {
                _iHeard = value;
                TRefresh();
            }
        }

        private bool _iChange;
        /// <summary>
        ///     变色项
        /// </summary>
        [Description("变色项")]
        [DefaultValue(false)]
        public bool IChange
        {
            get { return _iChange; }
            set
            {
                _iChange = value;
                TRefresh();
            }
        }

        #region 构造

        /// <summary>
        ///     构造
        /// </summary>
        public ToolItem()
        {
        }

        /// <summary>
        ///     构造
        /// </summary>
        /// <param name="text"></param>
        public ToolItem(string text)
            : this(text, null)
        {
        }

        /// <summary>
        ///     构造
        /// </summary>
        /// <param name="text"></param>
        /// <param name="image"></param>
        public ToolItem(string text, Image image)
        {
            Text = text;
            _image = image;
        }

        /// <summary>
        /// 刷新项
        /// </summary>
        private void TRefresh()
        {
            if (this.Owner != null)
                this.Owner.TRefresh(this);
        }

        #endregion

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用
        /// <summary>
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    if (_color != null)
                        _color.Dispose();
                    _image = null;
                    Tag = null;
                    Owner = null;
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~TProperties() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        /// <summary>
        /// 释放资源
        /// </summary>
        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}