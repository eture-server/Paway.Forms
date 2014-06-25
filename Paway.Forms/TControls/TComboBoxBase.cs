﻿using Paway.Helper;
using Paway.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    /// 重绘DrawCombobox
    /// 当鼠标指针移到该项上时的高亮度颜色
    /// </summary>
    public class TComboBoxBase : ComboBox
    {
        /// <summary>
        /// </summary>
        public TComboBoxBase()
        {
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.DoubleBuffer, true);
            this.UpdateStyles();

            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.DrawItem += DrawCombobox_DrawItem;
            this.ItemHeight = 16;
        }

        #region 属性
        private Color _colorSelect = Color.PaleTurquoise;
        /// <summary>
        /// 当鼠标指针移到该项上时的高亮度颜色
        /// </summary>
        [Browsable(true), Category("控件的重绘设置"), Description("当鼠标指针移到该项上时的高亮度颜色")]
        [DefaultValue(typeof(Color), "PaleTurquoise")]
        public Color ColorSelect
        {
            get { return _colorSelect; }
            set
            {
                _colorSelect = value;
                this.Invalidate();
            }
        }

        private Color _colorFore = Color.Black;
        /// <summary>
        /// 项被选中后的字体颜色
        /// </summary>
        [Browsable(true), Category("控件的重绘设置"), Description("项被选中后的字体颜色")]
        [DefaultValue(typeof(Color), "Black")]
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
        /// 获取或设置组合框中的某项的高度
        /// </summary>
        [Description("获取或设置组合框中的某项的高度"), DefaultValue(16)]
        public new int ItemHeight
        {
            get { return base.ItemHeight; }
            set { base.ItemHeight = value; }
        }

        #endregion

        #region 方法
        void DrawCombobox_DrawItem(object sender, DrawItemEventArgs e)
        {
            //如果当前控件为空
            if (e.Index < 0)
                return;

            e.DrawBackground();
            //获取表示所绘制项的边界的矩形
            System.Drawing.Rectangle rect = e.Bounds;
            //定义要绘制到控件中的图标图像
            //Image ico = System.Drawing.Image.FromFile(@"d:\d.png");
            //定义字体对象
            Font font = new Font("微软雅黑", this.Font.Size);
            Brush brush = new SolidBrush(this.ForeColor);
            //获得当前Item的文本
            //绑定字段
            object obj = this.Items[e.Index];
            var type = obj.GetType();
            string str = null;
            if (obj is DataRowView)
            {
                DataRowView dr = obj as DataRowView;
                str = dr[this.DisplayMember].ToString();
            }
            else
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(type);
                if (properties.Count > 0)
                {
                    for (int i = 0; i < properties.Count; i++)
                    {
                        if (properties[i].Name == this.DisplayMember)
                        {
                            str = properties[i].GetValue(obj).ToString();
                            break;
                        }
                    }
                }
                else
                {
                    str = this.Items[e.Index].ToString();
                }
            }
            //选中项ComboBoxEdit
            if ((e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit)
            {
                e.Graphics.FillRectangle(new SolidBrush(this.BackColor), rect);
            }
            //Selected
            else if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                //在当前项图形表面上划一个矩形
                e.Graphics.FillRectangle(new SolidBrush(_colorSelect), rect);
                brush = new SolidBrush(this._colorFore);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(this.BackColor), rect);
            }
            //在当前项图形表面上划上图标
            //g.DrawImage(ico, new Point(rect.Left, rect.Top));
            //在当前项图形表面上划上当前Item的文本
            //g.DrawString(tempString, font, new SolidBrush(Color.Black), rect.Left + ico.Size.Width, rect.Top);
            e.Graphics.DrawString(str, font, brush, rect, DrawParam.VerticalString);
            //将绘制聚焦框
            e.DrawFocusRectangle();
        }

        #endregion
    }
}
