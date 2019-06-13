using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    ///     自定义圆形进度条
    /// </summary>
    public class TProgressBar : TControl
    {
        #region 变量

        /// <summary>
        ///     SizeChanged前的大小
        /// </summary>
        private int last;

        /// <summary>
        ///     防止重复绘制
        /// </summary>
        private bool iChange = true;

        #endregion

        #region 属性

        private int value;

        /// <summary>
        ///     当前进度值
        /// </summary>
        [Description("当前进度值")]
        [DefaultValue(0)]
        public int Value
        {
            get { return value; }
            set
            {
                this.value = value;
                Invalidate();
            }
        }

        private int tWidth = 2;

        /// <summary>
        ///     进度条宽度
        /// </summary>
        [Description("进度条宽度")]
        [DefaultValue(2)]
        public int TWidth
        {
            get { return tWidth; }
            set
            {
                tWidth = value;
                Invalidate();
            }
        }

        private Color tColorNormal = Color.LightGray;

        /// <summary>
        ///     进度条空颜色
        /// </summary>
        [Description("进度条空颜色")]
        [DefaultValue(typeof(Color), "LightGray")]
        public Color TColorNormal
        {
            get { return tColorNormal; }
            set
            {
                if (value == Color.Empty)
                    value = Color.LightGray;
                tColorNormal = value;
                Invalidate();
            }
        }

        private Color tColor = Color.DeepSkyBlue;

        /// <summary>
        ///     进度条颜色
        /// </summary>
        [Description("进度条颜色")]
        [DefaultValue(typeof(Color), "DeepSkyBlue")]
        public Color TColor
        {
            get { return tColor; }
            set
            {
                if (value == Color.Empty)
                    value = Color.DeepSkyBlue;
                tColor = value;
                Invalidate();
            }
        }

        private Font tFont = new Font("微软雅黑", 15f, FontStyle.Regular, GraphicsUnit.Point, 1);

        /// <summary>
        ///     进度条描述字体
        /// </summary>
        [Description("进度条描述字体")]
        [DefaultValue(typeof(Font), "微软雅黑, 15pt")]
        public Font TFont
        {
            get { return tFont; }
            set
            {
                tFont = value;
                Invalidate();
            }
        }

        #endregion

        #region 重绘

        /// <summary>
        ///     HandleCreated 事件。
        /// </summary>
        protected override void OnHandleCreated(EventArgs e)
        {
            last = Width;
            base.OnHandleCreated(e);
        }

        /// <summary>
        ///     SizeChanged 事件。
        ///     自动调整宽高，保持一致。
        /// </summary>
        protected override void OnSizeChanged(EventArgs e)
        {
            try
            {
                if (!iChange) return;
                iChange = false;
                if (Width < last || Height < last)
                {
                    Width = Math.Min(Width, Height);
                    Height = Width;
                    last = Width;
                }
                else if (Width > last || Height > last)
                {
                    Width = Math.Max(Width, Height);
                    Height = Width;
                    last = Width;
                }
                base.OnSizeChanged(e);
            }
            finally
            {
                iChange = true;
            }
        }

        /// <summary>
        ///     绘制进度条
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
                DrawLine(e.Graphics, e.ClipRectangle, tColorNormal, 100);
                DrawLine(e.Graphics, e.ClipRectangle, tColor, value);
                //绘制进度描述
                var color = ForeColor;
                if (value != 100)
                    color = tColor;
                var size = e.Graphics.MeasureString(value.ToString(), tFont);
                var point = new Point(Width / 2 - (int)size.Width / 2 - 1, Height / 2 - (int)size.Height / 2 + 2);
                e.Graphics.DrawString(value.ToString(), tFont, new SolidBrush(color), point);
            }
            catch { }
        }

        private void DrawLine(Graphics g, Rectangle rect, Color color, int value)
        {
            using (var p = new Pen(new SolidBrush(color), tWidth))
            {
                //设置起止点线帽  
                //p.StartCap = LineCap.Round;
                //p.EndCap = LineCap.Round;
                //设置连续两段的联接样式  
                p.LineJoin = LineJoin.Round;
                g.DrawArc(p, new Rectangle(new Point(rect.X + tWidth / 2, rect.Y + tWidth / 2),
                        new Size(rect.Width - tWidth, rect.Height - tWidth)), 45, (float)(value * 3.6));
            }
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
                if (tFont != null)
                    tFont.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}