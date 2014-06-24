using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;

namespace Paway.Forms
{
    /// <summary>
    /// 表示 ToolBar 控件中的单个项。
    /// </summary>
    public class ToolItem
    {
        #region 属性
        /// <summary>
        /// Item 显示的图片
        /// </summary>
        [Description("Item 显示的图片"), DefaultValue(typeof(Image), "null")]
        public Image Image { get; set; }
        /// <summary>
        /// 获取或设置包含有关控件的数据的对象。 
        /// </summary>
        [Description("获取或设置包含有关控件的数据的对象"), DefaultValue(null)]
        [TypeConverter(typeof(StringConverter))]
        public object Tag { get; set; }
        /// <summary>
        /// 首行文字
        /// </summary>
        [Browsable(false), Description("首行文字"), DefaultValue(null)]
        public string First { get; set; }
        /// <summary>
        /// 其它行文字
        /// </summary>
        [Browsable(false), Description("其它行文字"), DefaultValue(null)]
        public string Sencond { get; set; }
        private string _text;
        /// <summary>
        /// Item 上显示的文字信息
        /// </summary>
        [DefaultValue(null)]
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                string[] text = _text.Split(new string[] { "\r\n", "&" }, StringSplitOptions.RemoveEmptyEntries);
                if (text.Length > 0)
                {
                    First = text[0];
                }
                Sencond = null;
                for (int i = 1; i < text.Length; i++)
                {
                    Sencond = string.Format("{0}{1}\r\n", Sencond, text[i]);
                }
                if (Sencond != null)
                {
                    Sencond = Sencond.TrimEnd(new Char[] { '\r', '\n' });
                }
            }
        }
        /// <summary>
        /// Item 上显示的描述信息
        /// </summary>
        [Description("Item 上显示的描述信息"), DefaultValue(null)]
        public string Desc { get; set; }
        /// <summary>
        /// Item 上显示的头部描述信息
        /// </summary>
        [Description("Item 上显示的头部描述信息"), DefaultValue(null)]
        public string HeadDesc { get; set; }
        /// <summary>
        /// Item 上显示的尾部描述信息
        /// </summary>
        [Description("Item 上显示的尾部描述信息"), DefaultValue(null)]
        public string EndDesc { get; set; }
        /// <summary>
        /// Item 当前默认颜色
        /// </summary>
        [Description("Item 当前默认颜色")]
        public Color Color { get; set; }
        /// <summary>
        /// 当前 Item 在 ToolBar 中的 Rectangle
        /// </summary>
        internal Rectangle Rectangle { get; set; }
        /// <summary>
        /// 描述信息边框
        /// </summary>
        internal Rectangle RectDesc { get; set; }
        /// <summary>
        /// 描述信息的鼠标状态
        /// </summary>
        internal TMouseState IMouseState { get; set; }
        /// <summary>
        /// Item 当前的鼠标状态
        /// </summary>
        [Browsable(false), Description("Item 当前的鼠标状态"), DefaultValue(typeof(TMouseState), "Normal")]
        public TMouseState MouseState { get; set; }
        /// <summary>
        /// Item 上的右键菜单
        /// </summary>
        [Description("Item 上的右键菜单"), DefaultValue(typeof(ContextMenuStrip), null)]
        public ContextMenuStrip ContextMenuStrip { get; set; }
        private bool _enable = true;
        /// <summary>
        /// Item 当前启用状态
        /// </summary>
        [Description("Item 当前启用状态"), DefaultValue(true)]
        public bool Enable
        {
            get { return _enable; }
            set { _enable = value; }
        }

        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        public ToolItem() { }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="text"></param>
        public ToolItem(string text) : this(text, null) { }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="text"></param>
        /// <param name="image"></param>
        public ToolItem(string text, Image image)
        {
            this.Text = text;
            this.Image = image;
        }

        #endregion

        #endregion
    }
}
