﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Paway.Forms.Properties;
using Paway.Helper;

namespace Paway.Forms
{
    /// <summary>
    /// CheckBox
    /// </summary>
    [DefaultProperty("Checked")]
    [ToolboxBitmap(typeof(CheckBox))]
    public class QQCheckBox : CheckBox
    {
        #region 变量
        /// <summary>
        /// 当前的属标状态
        /// </summary>
        private TMouseState _mouseState = TMouseState.Normal;
        private readonly Image normalImage = Resources.QQ_CheckBox_normal;
        private readonly Image tick_normalImage = Resources.QQ_CheckBox_tick_normal;
        private readonly Image tick_highlightImage = Resources.QQ_CheckBox_tick_highlight;
        private readonly Image hightlightImage = Resources.QQ_CheckBox_hightlight;
        private readonly Image _tick_normalImage = Resources.QQ_CheckBox__tick_normal;
        private readonly Image _tick_highlightImage = Resources.QQ_CheckBox__tick_highlight;

        #endregion

        #region 属性
        /// <summary>
        /// 文本区域
        /// </summary>
        private Rectangle TextRect
        {
            get { return new Rectangle(17, 0, Width - 17, Height); }
        }

        /// <summary>
        /// 图片显示区域
        /// </summary>
        private Rectangle ImageRect
        {
            get { return new Rectangle(0, (Height - 17) / 2, 17, 17); }
        }

        /// <summary>
        /// 鼠标状态
        /// </summary>
        [Description("鼠标状态")]
        [DefaultValue(TMouseState.Normal)]
        private TMouseState MouseState
        {
            set
            {
                _mouseState = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 获取或设置控件的背景色
        /// </summary>
        [Description("获取或设置控件的背景色")]
        [DefaultValue(typeof(Color), "Transparent")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                if (value == Color.Empty || value == SystemColors.Control)
                {
                    value = Color.Transparent;
                }
                base.BackColor = value;
            }
        }

        /// <summary>
        /// 获取或设置控件的前景色。
        /// </summary>
        [Description("获取或设置控件的前景色")]
        [DefaultValue(typeof(Color), "Black")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                if (value == Color.Empty)
                {
                    value = Color.Black;
                }
                base.ForeColor = value;
            }
        }

        #endregion

        #region 构造函数
        /// <summary>
        /// </summary>
        public QQCheckBox()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.Selectable |
                ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, false);
            UpdateStyles();
            ForeColor = Color.Black;
            BackColor = Color.Transparent;
            this.AutoSize = true;
        }

        #endregion

        #region 重绘
        /// <summary>
        /// 重绘
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;

            var flags = TextFormatFlags.Left |
                        TextFormatFlags.SingleLine |
                        TextFormatFlags.VerticalCenter;
            var color = Enabled ? ForeColor : Color.LightGray;
            TextRenderer.DrawText(e.Graphics, Text, Font, TextRect, color, flags);
            if (!Enabled)
            {
                switch (CheckState)
                {
                    case CheckState.Checked:
                        break;
                    case CheckState.Indeterminate:
                        break;
                    case CheckState.Unchecked:
                        g.DrawImage(normalImage, ImageRect);
                        break;
                }
            }
            else
            {
                switch (_mouseState)
                {
                    case TMouseState.Normal:
                    case TMouseState.Leave:
                        if (Checked)
                        {
                            g.DrawImage(tick_normalImage, ImageRect);
                        }
                        else
                        {
                            g.DrawImage(normalImage, ImageRect);
                        }
                        break;
                    case TMouseState.Down:
                    case TMouseState.Up:
                    case TMouseState.Move:
                        if (Checked)
                        {
                            g.DrawImage(tick_highlightImage, ImageRect);
                        }
                        else
                        {
                            g.DrawImage(hightlightImage, ImageRect);
                        }
                        break;
                }
                if (CheckState == CheckState.Indeterminate)
                {
                    if (_mouseState == TMouseState.Down || _mouseState == TMouseState.Move)
                    {
                        g.DrawImage(_tick_normalImage, ImageRect);
                    }
                    else
                    {
                        g.DrawImage(_tick_highlightImage, ImageRect);
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        protected override void OnMouseEnter(EventArgs eventargs)
        {
            base.OnMouseEnter(eventargs);
            MouseState = TMouseState.Move;
        }

        /// <summary>
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            MouseState = TMouseState.Down;
        }

        /// <summary>
        /// </summary>
        protected override void OnMouseLeave(EventArgs eventargs)
        {
            base.OnMouseLeave(eventargs);
            MouseState = TMouseState.Leave;
        }

        /// <summary>
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            MouseState = TMouseState.Up;
        }

        #endregion

        #region Dispose
        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            if (normalImage != null) normalImage.Dispose();
            if (tick_normalImage != null) tick_normalImage.Dispose();
            if (tick_highlightImage != null) tick_highlightImage.Dispose();
            if (hightlightImage != null) hightlightImage.Dispose();
            if (_tick_normalImage != null) _tick_normalImage.Dispose();
            if (_tick_highlightImage != null) _tick_highlightImage.Dispose();
            base.Dispose(disposing);
        }

        #endregion
    }
}