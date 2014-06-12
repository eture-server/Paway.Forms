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
        public Image Image { get; set; }
        /// <summary>
        /// 获取或设置包含有关控件的数据的对象。 
        /// </summary>
        public object Tag { get; set; }
        /// <summary>
        /// Item 上显示的文字信息
        /// </summary>
        [DefaultValue("toolItem")]
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string Text { get; set; }
        /// <summary>
        /// Item 上显示的头部描述信息
        /// </summary>
        public string HeadDesc { get; set; }
        /// <summary>
        /// Item 上显示的尾部描述信息
        /// </summary>
        public string EndDesc { get; set; }
        /// <summary>
        /// Item 当前默认颜色
        /// </summary>
        public Color Color { get; set; }
        /// <summary>
        /// 当前 Item 在 ToolBar 中的 Rectangle
        /// </summary>
        public Rectangle Rectangle { get; set; }
        /// <summary>
        /// Item 当前的鼠标状态
        /// </summary>
        internal TMouseState MouseState { get; set; }
        /// <summary>
        /// Item 上的右键菜单
        /// </summary>
        [DefaultValue("Item 上的右键菜单")]
        public ContextMenuStrip ContextMenuStrip { get; set; }

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
