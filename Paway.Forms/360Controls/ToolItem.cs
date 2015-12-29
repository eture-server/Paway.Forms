﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    ///     表示 ToolBar 控件中的单个项。
    /// </summary>
    public class ToolItem
    {
        #region 属性

        /// <summary>
        ///     拥有者
        /// </summary>
        [Browsable(false), Description("拥有者"), DefaultValue(typeof(ToolBar), "Tinn")]
        public ToolBar Owner { get; internal set; }

        /// <summary>
        ///     Item 显示的图片
        /// </summary>
        [Description("Item 显示的图片"), DefaultValue(typeof(Image), "null")]
        public Image Image { get; set; }

        /// <summary>
        ///     当前 Item 图片显示的 Rectangle
        /// </summary>
        [Browsable(false)]
        public Rectangle ImageRect { get; internal set; }

        /// <summary>
        ///     获取或设置包含有关控件的数据的对象。
        /// </summary>
        [Description("获取或设置包含有关控件的数据的对象"), DefaultValue(null)]
        [TypeConverter(typeof(StringConverter))]
        public object Tag { get; set; }

        /// <summary>
        ///     分组文字
        /// </summary>
        [Description("分组文字"), DefaultValue(null)]
        public string Group { get; private set; }

        /// <summary>
        ///     首行文字
        /// </summary>
        [Browsable(false), Description("首行文字"), DefaultValue(null)]
        public string First { get; private set; }

        /// <summary>
        ///     其它行文字
        /// </summary>
        [Browsable(false), Description("其它行文字"), DefaultValue(null)]
        public string Sencond { get; private set; }

        private string _text = string.Empty;

        /// <summary>
        ///     Item 上显示的文字信息
        /// </summary>
        [Editor(
            "System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
            typeof(UITypeEditor))]
        [DefaultValue(typeof(string), "")]
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                First = null;
                Sencond = null;
                if (string.IsNullOrEmpty(value)) return;
                var text = _text.Split(new[] { "\r\n", "&" }, StringSplitOptions.RemoveEmptyEntries);
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
            }
        }

        /// <summary>
        ///     Item 上显示的描述信息
        /// </summary>
        [Description("Item 上显示的描述信息"), DefaultValue(null)]
        public string Desc { get; set; }

        /// <summary>
        ///     Item 上显示的头部描述信息
        /// </summary>
        [Description("Item 上显示的头部描述信息"), DefaultValue(null)]
        public string HeadDesc { get; set; }

        /// <summary>
        ///     Item 上显示的尾部描述信息
        /// </summary>
        [Description("Item 上显示的尾部描述信息"), DefaultValue(null)]
        public string EndDesc { get; set; }

        private TProperties _color;

        /// <summary>
        ///     优先应用于项的背景色
        /// </summary>
        [DefaultValue(typeof(TProperties), "TColor")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TProperties TColor
        {
            get
            {
                if (_color == null)
                    _color = new TProperties();
                return _color;
            }
        }

        /// <summary>
        ///     当前 Item 在 ToolBar 中的 Rectangle
        /// </summary>
        [Browsable(false)]
        public Rectangle Rectangle { get; internal set; }

        /// <summary>
        ///     描述信息边框
        /// </summary>
        internal Rectangle RectDesc { get; set; }

        /// <summary>
        ///     描述信息的鼠标状态
        /// </summary>
        internal TMouseState IMouseState { get; set; }

        /// <summary>
        ///     Item 当前的鼠标状态
        /// </summary>
        [Browsable(false), Description("Item 当前的鼠标状态"), DefaultValue(typeof(TMouseState), "Normal")]
        public TMouseState MouseState { get; set; }

        /// <summary>
        ///     Item 上的右键菜单
        /// </summary>
        [Description("Item 上的右键菜单"), DefaultValue(typeof(ContextMenuStrip), null)]
        public ContextMenuStrip ContextMenuStrip { get; set; }

        private bool _enable = true;

        /// <summary>
        ///     Item 当前启用状态
        /// </summary>
        [Description("Item 当前启用状态"), DefaultValue(true)]
        public bool Enable
        {
            get { return _enable; }
            set { _enable = value; }
        }

        /// <summary>
        ///     Item 当前选中状态
        /// </summary>
        [Browsable(false), Description("Item 当前选中状态"), DefaultValue(false)]
        public bool Selete
        {
            get { return MouseState == TMouseState.Down; }
            set { MouseState = value ? TMouseState.Down : TMouseState.Normal; }
        }

        /// <summary>
        ///     文本内容
        /// </summary>
        [Description("文本内容"), DefaultValue(false)]
        public bool IText { get; set; }

        /// <summary>
        ///     圆角大小
        /// </summary>
        [Description("圆角大小"), DefaultValue(0)]
        public int TRadiu { get; set; }

        /// <summary>
        ///     头文字
        /// </summary>
        [Description("头文字"), DefaultValue(false)]
        public bool IHeard { get; set; }

        /// <summary>
        ///     变色项
        /// </summary>
        [Description("变色项"), DefaultValue(false)]
        public bool IChange { get; set; }

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
            Image = image;
        }

        #endregion

        #endregion
    }
}