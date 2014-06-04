using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    /// 重绘ListBox
    /// 项被选中后的自定义高亮颜色
    /// </summary>
    public class TListBox : ListBox
    {
        #region 构造
        /// <summary>
        /// 重绘ListBox
        /// </summary>
        public TListBox()
        {
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.DoubleBuffer, true);
            this.UpdateStyles();

            this.ItemHeight = 30;
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.BorderStyle = BorderStyle.None;
        }

        #endregion

        #region 属性

        /// <summary>
        /// 项被选中后的背景颜色
        /// </summary>
        private Color _colorSelect = Color.PaleGreen;
        /// <summary>
        /// 项被选中后的背景颜色
        /// </summary>
        [Browsable(true), Category("控件的重绘设置"), Description("项被选中后的背景颜色")]
        [DefaultValue(typeof(Color), "PaleGreen")]
        public Color ColorSelect
        {
            get { return _colorSelect; }
            set
            {
                _colorSelect = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// 项被选中后的字体颜色
        /// </summary>
        private Color _colorFore = Color.White;
        /// <summary>
        /// 项被选中后的字体颜色
        /// </summary>
        [Browsable(true), Category("控件的重绘设置"), Description("项被选中后的字体颜色")]
        [DefaultValue(typeof(Color), "White")]
        public Color ColorFore
        {
            get { return _colorFore; }
            set
            {
                _colorFore = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// 当鼠标指针移到该项上时的高亮背景颜色
        /// </summary>
        private Color _colorHot = Color.MintCream;
        /// <summary>
        /// 当鼠标指针移到该项上时的高亮背景颜色
        /// </summary>
        [Browsable(true), Category("控件的重绘设置"), Description("当鼠标指针移到该项上时的高亮背景颜色")]
        [DefaultValue(typeof(Color), "MintCream")]
        public Color ColorHot
        {
            get { return _colorHot; }
            set
            {
                _colorHot = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// 当鼠标指针移到该项上时的字体颜色
        /// </summary>
        private Color _colorHotFore = Color.White;
        /// <summary>
        /// 当鼠标指针移到该项上时的字体颜色
        /// </summary>
        [Browsable(true), Category("控件的重绘设置"), Description("当鼠标指针移到该项上时的字体颜色")]
        [DefaultValue(typeof(Color), "White")]
        public Color ColorHotFore
        {
            get { return _colorHotFore; }
            set
            {
                _colorHotFore = value;
                this.Invalidate();
            }
        }
        private StringAlignment _alignment = StringAlignment.Center;
        /// <summary>
        /// 获取或设置垂直面上的文本对齐信息。
        /// </summary>
        [Description("获取或设置垂直面上的文本对齐信息。"), DefaultValue(typeof(StringAlignment), "Center")]
        public StringAlignment AlignmentVertical
        {
            get { return _alignment; }
            set { _alignment = value; }
        }
        private StringAlignment _lineAlignment = StringAlignment.Center;
        /// <summary>
        /// 获取或设置水平面上的行对齐信息。
        /// </summary>
        [Browsable(true), Description("获取或设置水平面上的行对齐信息。"), DefaultValue(typeof(StringAlignment), "Center")]
        public StringAlignment AlignmentLine
        {
            get { return _lineAlignment; }
            set { _lineAlignment = value; }
        }
        private int lastIndex = -1;

        #endregion

        #region 事件
        /// <summary>
        /// 鼠标移出控件的可见区域时触发
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);
            //如果当前控件为空
            if (this.Items.Count <= 0)
                return;

            e.DrawBackground();
            Brush brush = new SolidBrush(this.ForeColor);
            //文本布局
            StringFormat format = new StringFormat
            {
                Alignment = _alignment,
                LineAlignment = _lineAlignment,
            };
            //判断当前项是否被选取中
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                //如果当前项被选中
                //用指定的画刷填充列表项范围所形成的矩形
                e.Graphics.FillRectangle(new SolidBrush(this.BackColor), e.Bounds);
                e.Graphics.FillRectangle(new SolidBrush(_colorSelect),
                    new Rectangle(e.Bounds.X + 1, e.Bounds.Y + 1, e.Bounds.Width - 2, e.Bounds.Height - 2));
                e.Graphics.DrawPath(new Pen(new SolidBrush(_colorSelect)),
                    DrawHelper.CreateRoundPath(new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1), 3));
                brush = new SolidBrush(this._colorFore);
            }
            //绘制当前项中的文本
            e.Graphics.DrawString(this.Items[e.Index].ToString(), this.Font, brush, e.Bounds, format);
            //绘制聚焦框
            e.DrawFocusRectangle();
        }
        /// <summary>
        /// 鼠标移过重绘
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int index = IndexFromPoint(e.Location);
            if (lastIndex == index) return;
            C_DrawItem(_colorHotFore, _colorHot, index);
            C_DrawItem(this.ForeColor, this.BackColor, lastIndex);
            lastIndex = index;
        }
        private void C_DrawItem(Color foreColor, Color backColor, int index)
        {
            if (index == -1 || this.SelectedIndex == index) return;
            Graphics g = this.CreateGraphics();
            Rectangle rect = GetItemRectangle(index);
            StringFormat format = new StringFormat
            {
                Alignment = _alignment,
                LineAlignment = _lineAlignment,
            };
            g.FillRectangle(new SolidBrush(backColor), rect);
            g.DrawString(this.Items[index].ToString(), this.Font, new SolidBrush(foreColor), rect, format);
        }

        #endregion
    }
}
