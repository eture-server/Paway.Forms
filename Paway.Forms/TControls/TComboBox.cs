using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Paway.Helper;
using Paway.Resource;

namespace Paway.Forms
{
    /// <summary>
    ///     TComboBox+边框
    /// </summary>
    public class TComboBox : TControl
    {
        #region override
        private readonly Image moveImage = AssemblyHelper.GetImage("QQ.TextBox.move.png");
        /// <summary>
        ///     边框图片
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

        #region 设计器
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
            this.tComboBox1.Size = new System.Drawing.Size(121, 23);
            this.tComboBox1.TabIndex = 0;
            // 
            // TComboBox
            // 
            this.Controls.Add(this.tComboBox1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.Name = "TComboBox";
            this.Size = new System.Drawing.Size(123, 25);
            this.ResumeLayout(false);
        }

        #endregion

        #region 属性
        private TComboBoxBase tComboBox1;
        /// <summary>
        ///     编辑控件
        /// </summary>
        [Category("Properties")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TComboBoxBase Edit { get { return tComboBox1; } }

        private readonly Image _borderImage = AssemblyHelper.GetImage("QQ.TextBox.normal.png");
        private TMouseState _mouseState = TMouseState.Normal;

        /// <summary>
        ///     绘制
        /// </summary>
        [DefaultValue(TMouseState.Normal)]
        protected virtual TMouseState MouseState
        {
            get { return _mouseState; }
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
        ///     获取或设置控件显示的文字的字体
        /// </summary>
        [Description("获取或设置控件显示的文字的字体")]
        [DefaultValue(typeof(Font), "微软雅黑, 9pt")]
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                if (Edit == null) return;
                if (value == null)
                {
                    value = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 1);
                }
                base.Font = value;
                Edit.Font = value;
                var hight = TextRenderer.MeasureText("你好", value).Height;
                Edit.ItemHeight = hight;
                Invalidate();
            }
        }

        /// <summary>
        ///     此组件的前景色，用于显示文本
        /// </summary>
        [Description("此组件的前景色，用于显示文本。"), Category("外观")]
        [DefaultValue(typeof(Color), "WindowText")]
        public override Color ForeColor
        {
            get { return Edit.ForeColor; }
            set
            {
                if (Edit == null) return;
                if (value == Color.Empty)
                {
                    value = Color.Black;
                }
                Edit.ForeColor = value;
            }
        }

        #endregion

        #region 构造
        /// <summary>
        ///     构造
        /// </summary>
        public TComboBox()
        {
            InitializeComponent();
            Edit.SizeChanged += TComboBox1_SizeChanged;
        }

        private void TComboBox1_SizeChanged(object sender, EventArgs e)
        {
            Height = Edit.Height + 2;
            Edit.Width = Width - 2;
        }

        #endregion

        #region 鼠标移动时的背影事件
        /// <summary>
        ///     背影事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            MouseMove += TComboBox2_MouseMove;
            Edit.MouseMove += TComboBox2_MouseMove;
            MouseLeave += TComboBox2_MouseLeave;
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
                if (moveImage != null)
                    moveImage.Dispose();
                if (tComboBox1 != null)
                    tComboBox1.Dispose();
                if (_borderImage != null)
                    _borderImage.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}