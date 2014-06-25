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
    /// 自定义基控件
    /// </summary>
    [Designer("System.Windows.Forms.Design.ParentControlDesigner,   System.Design ")]
    public class TControl : UserControl, IControl
    {
        #region 变量
        private List<Locate> tList;
        private Size normal = Size.Empty;
        /// <summary>
        /// 指定窗体窗口如何显示
        /// </summary>
        protected FormWindowState _windowState = FormWindowState.Normal;
        /// <summary>
        /// 星星图
        /// </summary>
        private Image star = AssemblyHelper.GetImage("Controls.t.png");

        #endregion

        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        public TControl()
        {
            this.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.DoubleBuffer |
                ControlStyles.SupportsTransparentBackColor, true);
            this.UpdateStyles();
        }

        #endregion

        #region 属性
        private Color _color;
        /// <summary>
        /// 获取或设置控件的背景色
        /// </summary>
        [Description("获取或设置控件的背景色"), DefaultValue(typeof(Color), "Transparent")]
        public override Color BackColor
        {
            get
            {
                if (_color == Color.Empty || _color == SystemColors.Control)
                    _color = Color.Transparent;
                if (_color.R == SystemColors.Control.R && _color.G == SystemColors.Control.G && _color.B == SystemColors.Control.B)
                    _color = Color.Transparent;
                return _color;
            }
            set
            {
                _color = value;
                if (value.A > _trans)
                {
                    _color = Color.FromArgb(_trans, value.R, value.G, value.B);
                }
            }
        }

        private int _trans = 255;
        /// <summary>
        /// 控件透明度
        /// </summary>
        [Description("透明度"), DefaultValue(255)]
        public int Trans
        {
            get { return _trans; }
            set
            {
                if (value < 0 || value > 255)
                {
                    value = 255;
                }
                _trans = value;
            }
        }

        #endregion

        #region 接口
        /// <summary>
        /// 坐标点是否包含在项中
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool Contain(Point p) { return false; }

        #endregion

        #region 在窗体上固定控件位置
        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ToLocate();
        }
        /// <summary>
        /// 在窗体上固定控件位置
        /// </summary>
        protected void AddLocate(Control control)
        {
            AddLocate(control, StringAlignment.Center, StringAlignment.Center);
        }
        /// <summary>
        /// 在窗体上固定控件位置
        /// </summary>
        protected void AddLocate(Control control, StringAlignment lLocation)
        {
            AddLocate(control, lLocation, lLocation);
        }
        /// <summary>
        /// 在窗体上固定控件位置
        /// </summary>
        protected void AddLocate(Control control, StringAlignment xLocation, StringAlignment yLocation)
        {
            if (normal == Size.Empty)
            {
                normal = this.Size;
            }
            if (tList == null)
            {
                tList = new List<Locate>();
            }
            tList.Add(new Locate(control, control.Location, xLocation, yLocation));
        }
        private void ToLocate()
        {
            if (tList == null) return;
            for (int i = 0; i < tList.Count; i++)
            {
                int left = 0;
                int top = 0;
                switch (tList[i].XLocation)
                {
                    case StringAlignment.Near:
                        left = 0;
                        break;
                    case StringAlignment.Center:
                        left = tList[i].Control.Width / 2;
                        break;
                    case StringAlignment.Far:
                        left = tList[i].Control.Width;
                        break;
                }
                switch (tList[i].YLocation)
                {
                    case StringAlignment.Near:
                        top = 0;
                        break;
                    case StringAlignment.Center:
                        top = tList[i].Control.Height / 2;
                        break;
                    case StringAlignment.Far:
                        top = tList[i].Control.Height;
                        break;
                }
                int x = this.Width * (tList[i].Point.X + left) / normal.Width;
                int y = this.Height * (tList[i].Point.Y + top) / normal.Height;
                tList[i].Control.Location = new Point(x - left, y - top);
            }
        }
        #endregion

        #region 动态星星
        private BackgroundWorker timer;
        /// <summary>
        /// 消灭星星
        /// </summary>
        public void Star()
        {
            if (timer == null)
            {
                timer = new BackgroundWorker();
                timer.WorkerSupportsCancellation = true;
                timer.DoWork += timer_DoWork;
            }
            if (!timer.IsBusy)
            {
                timer.RunWorkerAsync();
            }
        }

        void timer_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Random ran = new Random();
            Graphics g = this.CreateGraphics();
            Image star = BitmapHelper.ConvertTo(this.star, BConvertType.Trans, 30);
            while (true)
            {
                if (worker.CancellationPending) break;

                int width = ran.Next(20, 20);
                Rectangle rect = new Rectangle(ran.Next(this.Width - width), ran.Next(this.Height - width), width, width);
                rect = new Rectangle(100, 60, 20, 20);
                Point point = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
                for (int i = 1; i <= width / 2; i++)
                {
                    rect = new Rectangle(point.X - i, point.Y - i, i * 2, i * 2);
                    g.DrawImage(star, rect);
                    System.Threading.Thread.Sleep(100 + i * 10);
                }
                System.Threading.Thread.Sleep(ran.Next(300));
                g.FillRectangle(new SolidBrush(Color.Transparent), rect);
                star = BitmapHelper.ConvertTo(this.star, BConvertType.Trans, 0);
                g.DrawImage(star, rect);
                //System.Threading.Thread.Sleep(100);
                //g.DrawImage(BitmapHelper.ConvertTo(this.star, BConvertType.Trans, 50), rect);
                //System.Threading.Thread.Sleep(100);
                //g.DrawImage(BitmapHelper.ConvertTo(this.star, BConvertType.Trans, 0), rect);
                //System.Threading.Thread.Sleep(100);

                System.Threading.Thread.Sleep(ran.Next(3000));
            }
        }
        /// <summary>
        /// 消灭星星
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (timer != null && timer.IsBusy)
            {
                timer.CancelAsync();
            }
        }

        #endregion
    }
}
