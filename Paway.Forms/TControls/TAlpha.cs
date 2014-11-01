using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Paway.Win32;

namespace Paway.Forms
{
    /// <summary>
    /// 控件假透明
    /// </summary>
    public class TAlpha : MControl
    {
        #region 属性
        private int _alpha = 100;
        /// <summary>
        /// 透明度
        /// </summary>
        public int Alpha
        {
            get { return _alpha; }
            set
            {
                if (_alpha != value)
                {
                    _alpha = value;
                    this.Invalidate();
                }
            }
        }
        private Color _bkcolor = Color.Blue;
        private Label label1;
        /// <summary>
        /// 背景颜色
        /// </summary>
        public Color bkColor
        {
            get { return _bkcolor; }
            set
            {
                _bkcolor = value;
                //this.Invalidate();
                //this.Update();
            }
        }
        private Color _bdcolor = Color.Red;
        /// <summary>
        /// 边框颜色
        /// </summary>
        public Color bdColor
        {
            get
            {
                return _bdcolor;
            }
            set
            {
                _bdcolor = value;
                this.Invalidate();
                this.Update();
            }
        }
        #endregion

        #region 构造函数
        private Timer sTimer;
        private int intervel;
        private DockStyle dock;
        private Size size;
        private Point point;
        private Button button1;
        private Size step;
        /// <summary>
        /// 控件假透明
        /// </summary>
        public TAlpha()
        {
            //base.SetStyle(ControlStyles.OptimizedDoubleBuffer |
            //    ControlStyles.DoubleBuffer |
            //    ControlStyles.AllPaintingInWmPaint |
            //    ControlStyles.SupportsTransparentBackColor |
            //    ControlStyles.ResizeRedraw |
            //    ControlStyles.UserPaint, true);

            this.SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.DoubleBuffer, false);
            //SetStyle(ControlStyles.Opaque, true);

            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //SetStyle(ControlStyles.Opaque, true);

            //this.SetStyle(
            //    ControlStyles.OptimizedDoubleBuffer |
            //    ControlStyles.DoubleBuffer, false);
            this.UpdateStyles();
            InitializeComponent();
            sTimer = new Timer();
            sTimer.Interval = 10;
            sTimer.Tick += sTimer_Tick;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //MStart();
        }
        public void MStart()
        {
            this.dock = this.Dock;
            if (this.Parent != null && this.dock == DockStyle.Fill)
            {
                this.Size = this.Parent.Size;
            }
            this.size = this.Size;
            this.Dock = DockStyle.None;
            this.Size = this.size;
            {
            }
            Alpha = 255;
            intervel = 10;
            sTimer.Start();
        }

        void sTimer_Tick(object sender, EventArgs e)
        {
            if (this.IsDisposed) return;
            NativeMethods.LockWindowUpdate(this.Handle);
            {
                if (this.Alpha > intervel)
                {
                    this.Alpha -= intervel;
                }
                else
                {
                    this.Alpha = 0;
                    sTimer.Stop();
                }
            }
            NativeMethods.LockWindowUpdate(IntPtr.Zero);
            if (!sTimer.Enabled)
            {
                this.Size = this.size;
                this.Dock = dock;
            }
        }

        #endregion

        #region 重载函数
        #region 开启 WS_EX_TRANSPARENT,使控件支持透明
        /// <summary>
        /// 开启 WS_EX_TRANSPARENT,使控件支持透明
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;  // 开启 WS_EX_TRANSPARENT,使控件支持透明
                return cp;
            }
        }
        #endregion

        #region 不绘制背景
        /// <summary>
        /// 不绘制背景
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
            //不绘制背景
        }
        #endregion

        #region 绘制图形
        /// <summary>
        /// 绘制背景
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            Bitmap bmp = new Bitmap(this.Width, this.Height);
            Graphics bufg = Graphics.FromImage(bmp);
            bufg.DrawRectangle(new Pen(Color.FromArgb(this._alpha, this._bdcolor)), new Rectangle(0, 0, this.Size.Width, this.Size.Height));
            bufg.FillRectangle(new SolidBrush(Color.FromArgb(this._alpha, this._bkcolor)), this.Bounds);
            g.DrawImage(bmp, 0, 0);
            bmp.Save(@"d:\1.png");
            bufg.Dispose();
            bmp.Dispose();
        }

        #endregion

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("宋体", 22F);
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(132, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 70);
            this.label1.TabIndex = 0;
            this.label1.Text = "二手返修存在严重";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(77, 100);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // TAlpha
            // 
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Name = "TAlpha";
            this.Size = new System.Drawing.Size(274, 216);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
