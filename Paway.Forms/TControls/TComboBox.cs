using Paway.Helper;
using Paway.Resource;
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
    /// TComboBox+边框
    /// </summary>
    public class TComboBox : TControl
    {
        #region 属性
        private TComboBoxBase tComboBox1;
        /// <summary>
        /// 编辑控件
        /// </summary>
        [Category("Properties")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TComboBoxBase Edit { get { return this.tComboBox1; } }

        private Image _borderImage = AssemblyHelper.GetImage("QQ.TextBox.normal.png");
        private TMouseState _mouseState = TMouseState.Normal;
        /// <summary>
        /// 绘制
        /// </summary>
        [DefaultValue(typeof(TMouseState), "Normal")]
        protected virtual TMouseState MouseState
        {
            get { return this._mouseState; }
            set
            {
                if (this._mouseState != value)
                {
                    this._mouseState = value;
                    base.Invalidate();
                }
            }
        }
        /// <summary>
        /// 获取或设置控件显示的文字的字体
        /// </summary>
        [Description("获取或设置控件显示的文字的字体"), DefaultValue(typeof(Font), "微软雅黑, 9pt")]
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                if (tComboBox1 == null) return;
                if (value == null)
                {
                    value = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)1);
                }
                base.Font = value;
                tComboBox1.Font = value;
                int hight = TextRenderer.MeasureText("你好", value).Height;
                tComboBox1.ItemHeight = hight;
                this.Invalidate();
            }
        }
        /// <summary>
        /// 此组件的前景色，用于显示文本
        /// </summary>
        [Description("此组件的前景色，用于显示文本。"), Category("外观"), DefaultValue(typeof(Color), "Black")]
        public override Color ForeColor
        {
            get { return this.tComboBox1.ForeColor; }
            set
            {
                if (tComboBox1 == null) return;
                if (value == Color.Empty)
                {
                    value = Color.Black;
                }
                tComboBox1.ForeColor = value;
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
            InitMethod.Init(this);
            this.tComboBox1.SizeChanged += tComboBox1_SizeChanged;
        }
        void tComboBox1_SizeChanged(object sender, EventArgs e)
        {
            this.Height = this.tComboBox1.Height + 2;
            this.tComboBox1.Width = this.Width - 2;
        }

        #endregion

        #region override
        /// <summary>
        /// 边框图片
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            switch (this._mouseState)
            {
                case TMouseState.Move:
                    using (Image hotLine = AssemblyHelper.GetImage("QQ.TextBox.move.png"))
                    {
                        DrawHelper.RendererBackground(g, this.ClientRectangle, hotLine, true);
                    }
                    break;
            }
        }

        #endregion

        #region 鼠标移动时的背影事件
        /// <summary>
        /// 背影事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.MouseMove += TComboBox2_MouseMove;
            this.tComboBox1.MouseMove += TComboBox2_MouseMove;
            this.MouseLeave += TComboBox2_MouseLeave;
            this.tComboBox1.MouseLeave += TComboBox2_MouseLeave;
            this.Edit.DropDownClosed += TComboBox2_MouseLeave;
        }

        void TComboBox2_MouseLeave(object sender, EventArgs e)
        {
            this.MouseState = TMouseState.Leave;
        }

        void TComboBox2_MouseMove(object sender, MouseEventArgs e)
        {
            this.MouseState = TMouseState.Move;
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
    }
}
