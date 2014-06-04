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
    public class TComboBox2 : UserControl
    {
        #region 属性
        private TComboBox tComboBox1;
        /// <summary>
        /// 编辑控件
        /// </summary>
        [Category("Properties")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TComboBox Edit { get { return this.tComboBox1; } }

        private Image _borderImage = AssemblyHelper.GetImage("QQ.TextBox.normal.png");
        private EMouseState _mouseState = EMouseState.Normal;
        /// <summary>
        /// 绘制
        /// </summary>
        [DefaultValue(typeof(EMouseState), "Normal")]
        protected virtual EMouseState MouseState
        {
            get { return this._mouseState; }
            set
            {
                this._mouseState = value;
                base.Invalidate();
            }
        }
        /// <summary>
        /// 获取或设置控件的背景色
        /// </summary>
        [Description("获取或设置控件的背景色"), DefaultValue(typeof(Color), "Transparent")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        #endregion

        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        public TComboBox2()
        {
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.DoubleBuffer, true);
            this.UpdateStyles();

            InitializeComponent();
            this.BackColor = Color.Transparent;
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
                case EMouseState.Move:
                    using (Image hotLine = AssemblyHelper.GetImage("QQ.TextBox.move.png"))
                    {
                        DrawHelper.RendererBackground(g, this.ClientRectangle, hotLine, true);
                    }
                    break;
                default:
                    DrawHelper.RendererBackground(g, this.ClientRectangle, this._borderImage, true);
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
            this.Validated += TComboBox2_MouseLeave;
        }

        void TComboBox2_MouseLeave(object sender, EventArgs e)
        {
            this.MouseState = EMouseState.Leave;
        }

        void TComboBox2_MouseMove(object sender, MouseEventArgs e)
        {
            this.MouseState = EMouseState.Move;
        }

        #endregion

        #region 设计器
        private void InitializeComponent()
        {
            this.tComboBox1 = new Paway.Forms.TComboBox();
            this.SuspendLayout();
            // 
            // tComboBox1
            // 
            this.tComboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tComboBox1.ColorFore = System.Drawing.Color.Black;
            this.tComboBox1.ColorSelect = System.Drawing.Color.PaleTurquoise;
            this.tComboBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.tComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tComboBox1.FormattingEnabled = true;
            this.tComboBox1.ItemHeight = 16;
            this.tComboBox1.Location = new System.Drawing.Point(1, 1);
            this.tComboBox1.Name = "tComboBox1";
            this.tComboBox1.Size = new System.Drawing.Size(121, 22);
            this.tComboBox1.TabIndex = 0;
            // 
            // TControl
            // 
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.tComboBox1);
            this.Name = "TControl";
            this.Size = new System.Drawing.Size(123, 24);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
