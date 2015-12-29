using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Paway.Forms
{
    internal class TabStripMenuGlyph
    {
        #region 方法

        /// <summary>
        ///     绘制菜单图型
        /// </summary>
        /// <param name="g"></param>
        public void DrawGlyph(Graphics g)
        {
            if (_mouseState == TMouseState.Move)
            {
                var fill = _renderer.ColorTable.ButtonSelectedHighlight;
                using (Brush brush = new SolidBrush(fill))
                {
                    g.FillRectangle(brush, _bounds);
                }
                var borderRect = _bounds;
                borderRect.Width--;
                borderRect.Height--;

                g.DrawRectangle(SystemPens.Highlight, borderRect);
            }
            var bak = g.SmoothingMode;

            using (var pen = new Pen(Color.Black))
            {
                pen.Width = 2;
                g.DrawLine(pen,
                    new Point(_bounds.Left + _bounds.Width / 3 - 2, _bounds.Height / 2 - 1),
                    new Point(_bounds.Right - _bounds.Width / 3, _bounds.Height / 2 - 1));
            }
            g.FillPolygon(Brushes.Black, new[]
            {
                new Point(_bounds.Left + _bounds.Width/3 - 2, _bounds.Height/2 + 2),
                new Point(_bounds.Right - _bounds.Width/3, _bounds.Height/2 + 2),
                new Point(_bounds.Left + _bounds.Width/2 - 1, _bounds.Bottom - 4)
            });
            g.SmoothingMode = bak;
        }

        #endregion

        #region 变量

        private Rectangle _bounds = Rectangle.Empty;

        private TMouseState _mouseState = TMouseState.Normal;

        private readonly ToolStripProfessionalRenderer _renderer;

        #endregion

        #region 构造函数

        internal TabStripMenuGlyph()
        {
        }

        internal TabStripMenuGlyph(ToolStripProfessionalRenderer renderer)
        {
            _renderer = renderer;
        }

        internal TabStripMenuGlyph(Rectangle bounds, TMouseState mouseState)
        {
            _bounds = bounds;
            _mouseState = mouseState;
        }

        #endregion

        #region 属性

        /// <summary>
        ///     当前鼠标状态
        /// </summary>
        [DefaultValue(typeof(TMouseState), "Normal")]
        public TMouseState MouseState
        {
            get { return _mouseState; }
            set { _mouseState = value; }
        }

        /// <summary>
        ///     菜单按钮的区域
        /// </summary>
        [DefaultValue(typeof(Rectangle), "Empty")]
        public Rectangle Bounds
        {
            get { return _bounds; }
            set { _bounds = value; }
        }

        #endregion
    }
}