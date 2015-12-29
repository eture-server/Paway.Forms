using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Paway.Forms
{
    internal class TabStripCloseButton
    {
        #region 方法

        public void DrawCross(Graphics g)
        {
            if (_mouseState == TMouseState.Move)
            {
                var fill = _renderer.ColorTable.ButtonSelectedHighlight;
                using (Brush brush = new SolidBrush(fill))
                {
                    g.FillRectangle(brush, _bounds);
                    var borderRect = _bounds;

                    borderRect.Width--;
                    borderRect.Height--;

                    g.DrawRectangle(SystemPens.Highlight, borderRect);
                }
            }
            using (var pen = new Pen(Color.Black))
            {
                g.DrawLine(
                    pen,
                    _bounds.Left + 3,
                    _bounds.Top + 3,
                    _bounds.Right - 5,
                    _bounds.Bottom - 4);
                g.DrawLine(
                    pen,
                    _bounds.Right - 5,
                    _bounds.Top + 3,
                    _bounds.Left + 3,
                    _bounds.Bottom - 4);
            }
        }

        #endregion

        #region 变量

        private Rectangle _bounds = Rectangle.Empty;
        private TMouseState _mouseState = TMouseState.Normal;
        private readonly ToolStripProfessionalRenderer _renderer;

        #endregion

        #region 构造函数

        public TabStripCloseButton()
        {
        }

        public TabStripCloseButton(ToolStripProfessionalRenderer renderer)
        {
            _renderer = renderer;
        }

        public TabStripCloseButton(Rectangle bounds, TMouseState mouseState)
        {
            _bounds = bounds;
            _mouseState = mouseState;
        }

        #endregion

        #region 属性

        [DefaultValue(typeof(Rectangle), "Empty")]
        public Rectangle Bounds
        {
            get { return _bounds; }
            set { _bounds = value; }
        }

        [DefaultValue(typeof(TMouseState), "Normal")]
        public TMouseState MouseState
        {
            get { return _mouseState; }
            set { _mouseState = value; }
        }

        #endregion
    }
}