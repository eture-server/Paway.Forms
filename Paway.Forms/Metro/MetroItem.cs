using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Paway.Forms.Metro
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class MetroItem
    {
        #region 变量
        private string _fileName = "腾讯QQ";
        private string _filePath = "E:\\";
        private Image _icon = null;
        private bool _isSystem = false;
        private string _classID = "";
        private TItemType _itemType = TItemType.None;

        private TMouseState _mouseState = TMouseState.Normal;
        private Size _size = Size.Empty;
        private Point _location = Point.Empty;
        private Rectangle _rectangle = Rectangle.Empty;

        #endregion

        #region 属性

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get { return this._fileName; }
            set { this._fileName = value; }
        }
        /// <summary>
        /// 文件地址
        /// </summary>
        public string FilePath
        {
            get { return this._filePath; }
            set { this._filePath = value; }
        }
        /// <summary>
        /// 文件图标
        /// </summary>
        public Image Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }
        /// <summary>
        /// 是否为系统程序
        /// </summary>
        public bool IsSystem
        {
            get { return _isSystem; }
            set { _isSystem = value; }
        }
        /// <summary>
        /// 系统程序的ClassID
        /// </summary>
        public string ClassID
        {
            get { return _classID; }
            set { _classID = value; }
        }
        /// <summary>
        /// item的类型
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TItemType ItemType
        {
            get { return this._itemType; }
            set { this._itemType = value; }
        }

        #region Interval 属性

        internal TMouseState MouseState
        {
            get { return this._mouseState; }
            set { this._mouseState = value; }
        }

        internal Size Size
        {
            get { return this._size; }
            set { this._size = value; }
        }

        internal Point Location
        {
            get { return this._location; }
            set { this._location = value; }
        }

        internal Rectangle Rectangle
        {
            get
            {
                return new Rectangle(this.Location, this.Size);
            }
        }

        #endregion

        #endregion

        #region 方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="itemRect"></param>
        /// <param name="renderer"></param>
        internal virtual void OnPaint(Graphics g, Rectangle itemRect, MetroRenderer renderer)
        {
            int height = itemRect.Height - 30;
            int width = itemRect.Width - 30;
            Rectangle iconRect = new Rectangle(15, itemRect.Y + 15, width, height);
            if (this.Icon != null)//绘制图标
            {
                g.DrawImage(this.Icon, iconRect, 0, 0, this.Icon.Width, this.Icon.Height, GraphicsUnit.Pixel);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="itemRect"></param>
        /// <param name="renderer"></param>
        internal virtual void OnPaintBackground(Graphics g, Rectangle itemRect, MetroRenderer renderer)
        {
            Color color = renderer.BackColor;
            //绘制背景颜色
            switch (this._mouseState)
            {
                case TMouseState.Normal:
                case TMouseState.Leave:
                    color = renderer.BackColor;
                    break;
                case TMouseState.Move:
                case TMouseState.Up:
                    color = renderer.EnterColor;
                    break;
                case TMouseState.Down:
                    color = renderer.DownColor;
                    break;
            }
            //填充背景色
            using (Brush brush = new SolidBrush(color))
            {
                g.FillRectangle(brush, itemRect);
            }
        }
        #endregion
    }
}
