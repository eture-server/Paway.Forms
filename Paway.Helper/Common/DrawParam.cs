﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Paway.Helper
{
    /// <summary>
    /// 一些默认参数
    /// </summary>
    public abstract class DrawParam
    {
        /// <summary>
        /// 字符垂直对齐
        /// </summary>
        public static StringFormat VerticalString
        {
            get
            {
                StringFormat format = new StringFormat { LineAlignment = StringAlignment.Center };
                return format;
            }
        }
        /// <summary>
        /// 字符水平垂直对齐
        /// </summary>
        public static StringFormat LevelString
        {
            get
            {
                StringFormat format = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center,
                };
                return format;
            }
        }
        /// <summary>
        /// 文本垂直结尾省略
        /// </summary>
        public static TextFormatFlags VerticalEndEllipsisText
        {
            get
            {
                return TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis;
            }
        }
        /// <summary>
        /// 文本靠右垂直
        /// </summary>
        public static TextFormatFlags LeftText
        {
            get
            {
                return TextFormatFlags.Left | TextFormatFlags.VerticalCenter;
            }
        }
        /// <summary>
        /// 文本靠右垂直
        /// </summary>
        public static TextFormatFlags RightText
        {
            get
            {
                return TextFormatFlags.Right | TextFormatFlags.VerticalCenter;
            }
        }
        /// <summary>
        /// 文本水平垂直
        /// </summary>
        public static TextFormatFlags LevelText
        {
            get
            {
                return TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
            }
        }
    }
}
