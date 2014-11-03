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
    public class TAlpha : Panel
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
                    //Application.DoEvents();
                    //if (this.IsDisposed) return;
                    ////NativeMethods.LockWindowUpdate(this.Handle);
                    //this.BackColor = Color.FromArgb(Alpha, 255, 0, 0);
                    //NativeMethods.LockWindowUpdate(IntPtr.Zero);
                }
            }
        }
        private Color _bkcolor = Color.Blue;
        /// <summary>
        /// 背景颜色
        /// </summary>
        public Color BkColor { get; set; }

        #endregion

        #region 构造函数
        /// <summary>
        /// 控件假透明
        /// </summary>
        //public TAlpha()
        //{
        //    //this.SetStyle(
        //    //    ControlStyles.OptimizedDoubleBuffer |
        //    //    ControlStyles.DoubleBuffer, false);
        //    //this.UpdateStyles();
        //}

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
            //this.SetStyle(
            //ControlStyles.UserPaint |
            ////ControlStyles.AllPaintingInWmPaint |
            //ControlStyles.ResizeRedraw |
            //ControlStyles.Selectable |
            //ControlStyles.SupportsTransparentBackColor, true);

            //this.SetStyle(
            //    ControlStyles.OptimizedDoubleBuffer |
            //    ControlStyles.DoubleBuffer, false);
            //SetStyle(ControlStyles.Opaque, true);

            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //SetStyle(ControlStyles.Opaque, true);

            //this.SetStyle(
            //    ControlStyles.OptimizedDoubleBuffer |
            //    ControlStyles.DoubleBuffer, false);
            this.UpdateStyles();
            sTimer = new Timer();
            sTimer.Interval = 10;
            sTimer.Tick += sTimer_Tick;
            //MStart();
        }
        public void MStart()
        {
            this.Dock = DockStyle.Fill;
            Alpha = 255;
            intervel = 10;
            sTimer.Start();
        }

        void sTimer_Tick(object sender, EventArgs e)
        {
            if (this.IsDisposed) return;
            //NativeMethods.LockWindowUpdate(this.Handle);
            {
                if (this.Alpha > intervel)
                {
                    this.Alpha -= intervel;
                    this.Update();
                }
                else
                {
                    this.Alpha = 0;
                    sTimer.Stop();
                    this.Parent.Controls.Remove(this);
                }
            }
            //NativeMethods.LockWindowUpdate(IntPtr.Zero);
        }

        #endregion

        /// <summary>
        /// 开启 WS_EX_TRANSPARENT,使控件支持透明
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }

        /// <summary>
        /// 不绘制背景
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
        }

        /// <summary>
        /// 绘制背景图形
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            Bitmap bmp = new Bitmap(this.Width, this.Height);
            Graphics bufg = Graphics.FromImage(bmp);
            bufg.FillRectangle(new SolidBrush(Color.FromArgb(this._alpha, this._bkcolor)), new Rectangle(Point.Empty, this.Size));
            g.DrawImage(bmp, 0, 0);
            //bufg.Dispose();
            //bmp.Dispose();
        }
    }
}
