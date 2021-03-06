﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;
using Paway.Forms.Properties;
using Paway.Helper;
using System.Linq;
using System.Collections.Generic;

namespace Paway.Forms
{
    /// <summary>
    /// TComboBox+边框
    /// </summary>
    public class TComboBox : TControl
    {
        #region 变量
        private readonly Image moveImage = Resources.QQ_TextBox_move;
        private TComboBoxBase tComboBox1;
        private readonly Image _borderImage = Resources.QQ_TextBox_normal;
        private TMouseState _mouseState = TMouseState.Normal;

        #endregion

        #region 属性
        /// <summary>
        /// 编辑控件
        /// </summary>
        [Category("Properties")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TComboBoxBase Edit { get { return tComboBox1; } }

        /// <summary>
        /// 绘制
        /// </summary>
        [DefaultValue(TMouseState.Normal)]
        private TMouseState MouseState
        {
            set
            {
                if (_mouseState != value)
                {
                    _mouseState = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 获取或设置控件显示的文字的字体
        /// </summary>
        [Description("获取或设置控件显示的文字的字体")]
        [DefaultValue(typeof(Font), "微软雅黑, 11pt")]
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                if (Edit == null) return;
                Edit.Font = value;
                if (value != null)
                {
                    var hight = TextRenderer.MeasureText("Hello", value).Height;
                    Edit.ItemHeight = hight;
                }
                Invalidate();
            }
        }

        /// <summary>
        /// 此组件的前景色，用于显示文本
        /// </summary>
        [Description("此组件的前景色，用于显示文本。"), Category("外观")]
        [DefaultValue(typeof(Color), "Black")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                if (Edit == null) return;
                if (value == Color.Empty)
                {
                    value = Color.Black;
                }
                base.ForeColor = value;
                Edit.ForeColor = value;
            }
        }

        #endregion

        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        public TComboBox()
        {
            InitializeComponent();
            Edit.SizeChanged += TComboBox1_SizeChanged;
            InitMove();
        }
        private void TComboBox1_SizeChanged(object sender, EventArgs e)
        {
            Height = Edit.Height + 2;
            Edit.Width = Width - 2;
        }
        private void InitializeComponent()
        {
            this.tComboBox1 = new Paway.Forms.TComboBoxBase();
            this.SuspendLayout();
            // 
            // tComboBox1
            // 
            this.tComboBox1.IntegralHeight = false;
            this.tComboBox1.Location = new System.Drawing.Point(1, 1);
            this.tComboBox1.Name = "tComboBox1";
            this.tComboBox1.Size = new System.Drawing.Size(121, 25);
            this.tComboBox1.TabIndex = 0;
            // 
            // TComboBox
            // 
            this.Controls.Add(this.tComboBox1);
            this.Name = "TComboBox";
            this.Size = new System.Drawing.Size(123, 27);
            this.ResumeLayout(false);

        }
        /// <summary>
        /// 返回包含 System.ComponentModel.Component 的名称的 System.String（如果有）
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Value: {0},{1}", this.Edit.SelectedValue, base.ToString());
        }

        #endregion

        #region 重绘
        /// <summary>
        /// 边框图片
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            switch (_mouseState)
            {
                case TMouseState.Move:
                    DrawHelper.RendererBackground(g, ClientRectangle, moveImage, true);
                    break;
            }
        }

        #endregion

        #region 加载枚举列表
        /// <summary>
        /// 加载枚举
        /// </summary>
        public void Init<T>(T value, Func<T, bool> action = null) where T : Enum
        {
            this.Edit.Items.Clear();
            foreach (var field in typeof(T).GetFields(TConfig.Flags))
            {
                var item = (T)field.GetRawConstantValue();
                if (action?.Invoke(item) == true) continue;
                this.Edit.Items.Add(field.Description());
                if (item.Equals(value)) this.Edit.SelectedIndex = this.Edit.Items.Count - 1;
            }
            if (this.Edit.SelectedIndex < 0 && this.Edit.Items.Count > 0) this.Edit.SelectedIndex = 0;
        }

        #endregion

        #region 鼠标移动时的背影事件
        /// <summary>
        /// 背影事件
        /// </summary>
        private void InitMove()
        {
            MouseMove += TComboBox2_MouseMove;
            MouseLeave += TComboBox2_MouseLeave;
            Edit.MouseMove += TComboBox2_MouseMove;
            Edit.MouseLeave += TComboBox2_MouseLeave;
            Edit.DropDownClosed += TComboBox2_MouseLeave;
        }
        private void TComboBox2_MouseLeave(object sender, EventArgs e)
        {
            MouseState = TMouseState.Leave;
        }
        private void TComboBox2_MouseMove(object sender, MouseEventArgs e)
        {
            MouseState = TMouseState.Move;
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
            if (moveImage != null) moveImage.Dispose();
            if (tComboBox1 != null)
            {
                tComboBox1.Dispose();
                tComboBox1 = null;
            }
            if (_borderImage != null) _borderImage.Dispose();
            base.Dispose(disposing);
        }

        #endregion
    }
}