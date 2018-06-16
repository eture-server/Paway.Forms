using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Paway.Helper;

namespace Paway.Forms
{
    /// <summary>
    ///     重绘DrawCombobox
    ///     当鼠标指针移到该项上时的高亮度颜色
    /// </summary>
    [DefaultProperty("Items")]
    public class TComboBoxBase : ComboBox
    {
        /// <summary>
        /// </summary>
        public TComboBoxBase()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
            DrawMode = DrawMode.OwnerDrawFixed;
            DrawItem += DrawCombobox_DrawItem;
            ItemHeight = 17;
            DropDownHeight = 200;
            DropDownStyle = ComboBoxStyle.DropDownList;
            DrawMode = DrawMode.OwnerDrawFixed;
            FormattingEnabled = true;
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        }

        #region 方法
        private void DrawCombobox_DrawItem(object sender, DrawItemEventArgs e)
        {
            //如果当前控件为空
            if (e.Index < 0)
                return;

            e.DrawBackground();
            //获取表示所绘制项的边界的矩形
            var rect = e.Bounds;
            //定义要绘制到控件中的图标图像
            //定义字体对象
            var font = new Font("微软雅黑", Font.Size);
            Brush brush = new SolidBrush(ForeColor);
            //获得当前Item的文本
            //绑定字段
            var obj = Items[e.Index];
            var type = obj.GetType();
            object str = null;
            if (type == typeof(DataRowView))
            {
                var dr = obj as DataRowView;
                str = dr[DisplayMember];
            }
            else if (type != typeof(string) && !type.IsValueType)
            {
                var properties = TypeDescriptor.GetProperties(type);
                if (properties.Count > 1)
                {
                    for (var i = 0; i < properties.Count; i++)
                    {
                        if (properties[i].Name == DisplayMember)
                        {
                            str = properties[i].GetValue(obj);
                            break;
                        }
                    }
                }
            }
            else
            {
                str = Items[e.Index];
            }
            //选中项ComboBoxEdit
            if ((e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit)
            {
                e.Graphics.FillRectangle(new SolidBrush(BackColor), rect);
            }
            //Selected
            else if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                //在当前项图形表面上划一个矩形
                e.Graphics.FillRectangle(new SolidBrush(_colorSelect), rect);
                brush = new SolidBrush(_colorFore);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(BackColor), rect);
            }
            //在当前项图形表面上划上图标
            //g.DrawImage(ico, new Point(rect.Left, rect.Top));
            //在当前项图形表面上划上当前Item的文本
            //g.DrawString(tempString, font, new SolidBrush(Color.Black), rect.Left + ico.Size.Width, rect.Top);
            e.Graphics.DrawString(str?.ToString(), font, brush, rect, DrawHelper.StringVertical);
            //将绘制聚焦框
            e.DrawFocusRectangle();
        }

        #endregion

        #region 属性
        private Color _colorSelect = Color.PaleTurquoise;
        /// <summary>
        ///     当鼠标指针移到该项上时的高亮度颜色
        /// </summary>
        [Browsable(true), Category("控件的重绘设置"), Description("当鼠标指针移到该项上时的高亮度颜色")]
        [DefaultValue(typeof(Color), "PaleTurquoise")]
        public Color ColorSelect
        {
            get { return _colorSelect; }
            set
            {
                _colorSelect = value;
                Invalidate();
            }
        }

        private Color _colorFore = Color.Black;
        /// <summary>
        ///     项被选中后的字体颜色
        /// </summary>
        [Browsable(true), Category("控件的重绘设置"), Description("项被选中后的字体颜色")]
        [DefaultValue(typeof(Color), "Black")]
        public Color ColorFore
        {
            get { return _colorFore; }
            set
            {
                _colorFore = value;
                Invalidate();
            }
        }

        #endregion

        #region 重载属性默认值
        /// <summary>
        ///     获取或设置 System.Windows.Forms.ComboBox 下拉部分的高度（以像素为单位）。
        /// </summary>
        [Description("获取或设置 System.Windows.Forms.ComboBox 下拉部分的高度（以像素为单位）"), DefaultValue(200)]
        public new int DropDownHeight
        {
            get { return base.DropDownHeight; }
            set { base.DropDownHeight = value; }
        }

        /// <summary>
        ///     获取或设置组合框中的某项的高度
        /// </summary>
        [Description("获取或设置组合框中的某项的高度"), DefaultValue(17)]
        public new int ItemHeight
        {
            get { return base.ItemHeight; }
            set { base.ItemHeight = value; }
        }

        /// <summary>
        ///     获取或设置指定组合框样式的值。
        /// </summary>
        [Description("获取或设置指定组合框样式的值")]
        [DefaultValue(typeof(ComboBoxStyle), "DropDownList")]
        public new ComboBoxStyle DropDownStyle
        {
            get { return base.DropDownStyle; }
            set { base.DropDownStyle = value; }
        }

        /// <summary>
        ///     获取或设置一个值，该值指示是由您的代码还是由操作系统来处理列表中的元素的绘制。。
        /// </summary>
        [Description("获取或设置一个值，该值指示是由您的代码还是由操作系统来处理列表中的元素的绘制")]
        [DefaultValue(typeof(DrawMode), "OwnerDrawFixed")]
        public new DrawMode DrawMode
        {
            get { return base.DrawMode; }
            set { base.DrawMode = value; }
        }

        /// <summary>
        ///     获取或设置一个值，该值指示是否将格式设置应用于 System.Windows.Forms.ListControl 的
        ///     System.Windows.Forms.ListControl.DisplayMember 属性。
        /// </summary>
        [Description("获取或设置一个值，该值指示是否将格式设置应用于 System.Windows.Forms.ListControl 的System.Windows.Forms.ListControl.DisplayMember 属性")]
        [DefaultValue(true)]
        public new bool FormattingEnabled
        {
            get { return base.FormattingEnabled; }
            set { base.FormattingEnabled = value; }
        }

        /// <summary>
        ///     获取或设置控件绑定到的容器的边缘并确定控件如何随其父级一起调整大小
        /// </summary>
        [Description("获取或设置控件绑定到的容器的边缘并确定控件如何随其父级一起调整大小")]
        [DefaultValue(typeof(AnchorStyles), "Top, Bottom, Left, Right")]
        public override AnchorStyles Anchor
        {
            get { return base.Anchor; }
            set { base.Anchor = value; }
        }

        #endregion
    }
}