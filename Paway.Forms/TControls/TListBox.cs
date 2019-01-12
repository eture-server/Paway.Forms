using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Paway.Helper;

namespace Paway.Forms
{
    /// <summary>
    ///     重绘ListBox
    ///     项被选中后的自定义高亮颜色
    /// </summary>
    public class TListBox : ListBox
    {
        #region 构造

        /// <summary>
        ///     重绘ListBox
        /// </summary>
        public TListBox()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
            ItemHeight = 30;
            DrawMode = DrawMode.OwnerDrawFixed;
            BorderStyle = BorderStyle.None;
        }

        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置 System.Windows.Forms.ListBox 中项的高度
        /// </summary>
        [Description("获取或设置 System.Windows.Forms.ListBox 中项的高度")]
        [DefaultValue(30)]
        [Localizable(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public override int ItemHeight
        {
            get { return base.ItemHeight; }
            set { base.ItemHeight = value; }
        }

        /// <summary>
        /// 获取或设置控件的绘图模式
        /// </summary>
        [Description("获取或设置控件的绘图模式")]
        [DefaultValue(DrawMode.OwnerDrawFixed)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public override DrawMode DrawMode
        {
            get { return base.DrawMode; }
            set { base.DrawMode = value; }
        }

        /// <summary>
        /// 获取或设置在 System.Windows.Forms.ListBox 四周绘制的边框的类型
        /// </summary>
        [DefaultValue(BorderStyle.None)]
        [Description("获取或设置在 System.Windows.Forms.ListBox 四周绘制的边框的类型")]
        public new BorderStyle BorderStyle
        {
            get { return base.BorderStyle; }
            set { base.BorderStyle = value; }
        }

        /// <summary>
        ///     项被选中后的背景颜色
        /// </summary>
        private Color _colorSelect = Color.PaleGreen;

        /// <summary>
        ///     项被选中后的背景颜色
        /// </summary>
        [Browsable(true), Category("控件的重绘设置"), Description("项被选中后的背景颜色")]
        [DefaultValue(typeof(Color), "PaleGreen")]
        public Color ColorSelect
        {
            get { return _colorSelect; }
            set
            {
                _colorSelect = value;
                Invalidate();
            }
        }

        /// <summary>
        ///     项被选中后的字体颜色
        /// </summary>
        private Color _colorFore = Color.White;

        /// <summary>
        ///     项被选中后的字体颜色
        /// </summary>
        [Browsable(true), Category("控件的重绘设置"), Description("项被选中后的字体颜色")]
        [DefaultValue(typeof(Color), "White")]
        public Color ColorFore
        {
            get { return _colorFore; }
            set
            {
                _colorFore = value;
                Invalidate();
            }
        }

        /// <summary>
        ///     当鼠标指针移到该项上时的高亮背景颜色
        /// </summary>
        private Color _colorHot = Color.MintCream;

        /// <summary>
        ///     当鼠标指针移到该项上时的高亮背景颜色
        /// </summary>
        [Browsable(true), Category("控件的重绘设置"), Description("当鼠标指针移到该项上时的高亮背景颜色")]
        [DefaultValue(typeof(Color), "MintCream")]
        public Color ColorHot
        {
            get { return _colorHot; }
            set
            {
                _colorHot = value;
                Invalidate();
            }
        }

        /// <summary>
        ///     当鼠标指针移到该项上时的字体颜色
        /// </summary>
        private Color _colorHotFore = Color.White;

        /// <summary>
        ///     当鼠标指针移到该项上时的字体颜色
        /// </summary>
        [Browsable(true), Category("控件的重绘设置"), Description("当鼠标指针移到该项上时的字体颜色")]
        [DefaultValue(typeof(Color), "White")]
        public Color ColorHotFore
        {
            get { return _colorHotFore; }
            set
            {
                _colorHotFore = value;
                Invalidate();
            }
        }

        private StringAlignment _alignment = StringAlignment.Center;

        /// <summary>
        ///     获取或设置垂直面上的文本对齐信息。
        /// </summary>
        [Description("获取或设置垂直面上的文本对齐信息。")]
        [DefaultValue(StringAlignment.Center)]
        public StringAlignment AlignmentVertical
        {
            get { return _alignment; }
            set { _alignment = value; }
        }

        private StringAlignment _lineAlignment = StringAlignment.Center;

        /// <summary>
        ///     获取或设置水平面上的行对齐信息。
        /// </summary>
        [Browsable(true), Description("获取或设置水平面上的行对齐信息。")]
        [DefaultValue(StringAlignment.Center)]
        public StringAlignment AlignmentLine
        {
            get { return _lineAlignment; }
            set { _lineAlignment = value; }
        }

        private int lastIndex = -1;

        #endregion

        #region 事件

        /// <summary>
        ///     鼠标移出控件的可见区域时触发
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);
            //如果当前控件为空
            if (Items.Count <= 0)
                return;

            e.DrawBackground();
            Brush brush = new SolidBrush(ForeColor);
            //文本布局
            var format = new StringFormat
            {
                Alignment = _alignment,
                LineAlignment = _lineAlignment
            };
            //判断当前项是否被选取中
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                //如果当前项被选中
                //用指定的画刷填充列表项范围所形成的矩形
                e.Graphics.FillRectangle(new SolidBrush(BackColor), e.Bounds);
                e.Graphics.FillRectangle(new SolidBrush(_colorSelect),
                    new Rectangle(e.Bounds.X + 1, e.Bounds.Y + 1, e.Bounds.Width - 2, e.Bounds.Height - 2));
                e.Graphics.DrawPath(new Pen(new SolidBrush(_colorSelect)),
                    DrawHelper.CreateRoundPath(
                        new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1), 3));
                brush = new SolidBrush(_colorFore);
            }
            //绘制当前项中的文本
            e.Graphics.DrawString(Items[e.Index].ToString(), Font, brush, e.Bounds, format);
            //绘制聚焦框
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     鼠标移过重绘
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            var index = IndexFromPoint(e.Location);
            if (lastIndex == index) return;
            C_DrawItem(_colorHotFore, _colorHot, index);
            C_DrawItem(ForeColor, BackColor, lastIndex);
            lastIndex = index;
        }

        private void C_DrawItem(Color foreColor, Color backColor, int index)
        {
            if (index == -1 || SelectedIndex == index) return;
            var g = CreateGraphics();
            var rect = GetItemRectangle(index);
            var format = new StringFormat
            {
                Alignment = _alignment,
                LineAlignment = _lineAlignment
            };
            g.FillRectangle(new SolidBrush(backColor), rect);
            g.DrawString(Items[index].ToString(), Font, new SolidBrush(foreColor), rect, format);
            g.Dispose();
        }

        #endregion
    }
}