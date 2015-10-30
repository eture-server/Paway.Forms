using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Paway.Forms.TControls
{
    /// <summary>
    /// 自定义圆形进度条
    /// </summary>
    public class TProgressBar : TControl
    {
        #region 变量
        /// <summary>
        /// SizeChanged前的大小
        /// </summary>
        private int last;
        /// <summary>
        /// 防止重复绘制
        /// </summary>
        private bool iChange = true;

        #endregion

        #region 属性
        private int value = 0;
        /// <summary>
        /// 当前进度值
        /// </summary>
        [Description("当前进度值"), DefaultValue(0)]
        public int Value
        {
            get { return this.value; }
            set
            {
                this.value = value;
                this.Invalidate();
            }
        }

        private int tWidth = 2;
        /// <summary>
        /// 进度条宽度
        /// </summary>
        [Description("进度条宽度"), DefaultValue(2)]
        public int TWidth
        {
            get { return this.tWidth; }
            set
            {
                this.tWidth = value;
                this.Invalidate();
            }
        }

        private Color tColorNormal = Color.LightGray;
        /// <summary>
        /// 进度条空颜色
        /// </summary>
        [Description("进度条空颜色"), DefaultValue(typeof(Color), "LightGray")]
        public Color TColorNormal
        {
            get { return tColorNormal; }
            set
            {
                if (value == Color.Empty)
                    value = Color.LightGray;
                tColorNormal = value;
                this.Invalidate();
            }
        }
        private Color tColor = Color.DeepSkyBlue;
        /// <summary>
        /// 进度条颜色
        /// </summary>
        [Description("进度条颜色"), DefaultValue(typeof(Color), "DeepSkyBlue")]
        public Color TColor
        {
            get { return tColor; }
            set
            {
                if (value == Color.Empty)
                    value = Color.DeepSkyBlue;
                tColor = value;
                this.Invalidate();
            }
        }

        private Font tFont = new Font("微软雅黑", 15f, FontStyle.Regular, GraphicsUnit.Point, (byte)1);
        /// <summary>
        /// 进度条描述字体
        /// </summary>
        [Description("进度条描述字体"), DefaultValue(typeof(Font), "微软雅黑, 15pt")]
        public Font TFont
        {
            get { return tFont; }
            set
            {
                tFont = value;
                this.Invalidate();
            }
        }

        #endregion

        #region 重绘
        /// <summary>
        /// HandleCreated 事件。
        /// </summary>
        protected override void OnHandleCreated(EventArgs e)
        {
            this.last = this.Width;
            base.OnHandleCreated(e);
        }
        /// <summary>
        /// SizeChanged 事件。
        /// 自动调整宽高，保持一致。
        /// </summary>
        protected override void OnSizeChanged(EventArgs e)
        {
            try
            {
                if (!iChange) return;
                iChange = false;
                if (this.Width < this.last || this.Height < this.last)
                {
                    this.Width = Math.Min(this.Width, this.Height);
                    this.Height = this.Width;
                    this.last = this.Width;
                }
                else if (this.Width > this.last || this.Height > this.last)
                {
                    this.Width = Math.Max(this.Width, this.Height);
                    this.Height = this.Width;
                    this.last = this.Width;
                }
                base.OnSizeChanged(e);
            }
            finally
            {
                iChange = true;
            }
        }
        /// <summary>
        /// 绘制进度条
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
                DrawLine(e.Graphics, e.ClipRectangle, this.tColorNormal, 100);
                DrawLine(e.Graphics, e.ClipRectangle, this.tColor, this.value);
                //绘制进度描述
                Color color = this.ForeColor;
                if (this.value != 100)
                    color = this.tColor;
                SizeF size = e.Graphics.MeasureString(this.value.ToString(), this.tFont);
                Point point = new Point(this.Width / 2 - (int)size.Width / 2 - 1, this.Height / 2 - (int)size.Height / 2 + 2);
                e.Graphics.DrawString(this.value.ToString(), this.tFont, new SolidBrush(color), point);
            }
            catch { }
        }
        private void DrawLine(Graphics g, Rectangle rect, Color color, int value)
        {
            color = Color.FromArgb(Trans, color.R, color.G, color.B);
            using (Pen p = new Pen(new SolidBrush(color), tWidth))
            {
                //设置起止点线帽  
                //p.StartCap = LineCap.Round;
                //p.EndCap = LineCap.Round;
                //设置连续两段的联接样式  
                p.LineJoin = LineJoin.Round;
                g.DrawArc(p, new Rectangle(new Point(rect.X + tWidth / 2, rect.Y + tWidth / 2), new Size(rect.Width - 1 - tWidth, rect.Height - 1 - tWidth)), 45, (float)((float)value * 3.6));
            }
        }

        #endregion
    }
}
