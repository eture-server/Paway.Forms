using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Paway.Forms.Metro
{
    /// <summary>
    ///     方向按钮
    /// </summary>
    public class DirectionButton : Control
    {
        #region 构d造函数

        /// <summary>
        ///     用默认设置初始化 Paway.Forms.DirectionButton 类的新实例。
        /// </summary>
        public DirectionButton()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();
        }

        #endregion

        #region 方法

        /// <summary>
        ///     绘制三角型
        /// </summary>
        /// <param name="g">画板，Graphics 对象</param>
        private void DrawTriangle(Graphics g)
        {
            //获取控件中心点和三角型所绘画的位置
            var tWidth = 11;
            var tHeight = 6;
            var hCenter = Width/2;
            var vCenter = Height/2 - 1;
            var x = hCenter - tWidth/2;
            var y = vCenter - tHeight/2;
            //构建三角型的路径
            var pointArray = new Point[3];
            switch (_direction)
            {
                case TLocation.Up:
                    pointArray[0] = new Point(hCenter, vCenter);
                    pointArray[1] = new Point(x + tWidth, vCenter + tHeight);
                    pointArray[2] = new Point(x, vCenter + tHeight);
                    break;
                case TLocation.Down:
                    pointArray[0] = new Point(x, vCenter);
                    pointArray[1] = new Point(x + tWidth, vCenter);
                    pointArray[2] = new Point(hCenter, vCenter + tHeight);
                    break;
                case TLocation.Left:
                    break;
                case TLocation.Right:
                    break;
            }
            using (var path = new GraphicsPath())
            {
                path.AddLines(pointArray);
                path.CloseFigure();
                //using (Brush brush = new SolidBrush(this._foreColor))
                //{
                //    g.FillPath(brush, path);
                //}
            }

            //Point point1 = new Point(0, 0);
            //Point point2 = new Point(11, 0);
            //Point point3 = new Point(5, 8);
            //Point[] pointArray = { point1, point2, point3 };

            //g.FillPolygon(Brushes.White, pointArray);
        }

        #endregion

        #region 变量

        /// <summary>
        ///     上下按钮的默认大小
        /// </summary>
        private readonly Size _defaultSize = new Size(80, 20);

        private TMouseState _mouseState = TMouseState.Normal;

        /// <summary>
        ///     箭头所指向的方向
        /// </summary>
        private TLocation _direction = TLocation.Up;

        #endregion

        #region 属性

        /// <summary>
        ///     控件的默认大小
        /// </summary>
        [Description("控件的默认大小"), DefaultValue(typeof (Size), "80, 20")]
        protected override Size DefaultSize
        {
            get { return _defaultSize; }
        }

        /// <summary>
        ///     针对于控件的鼠标状态
        /// </summary>
        [Description("针对于控件的鼠标状态"), DefaultValue(typeof (TMouseState), "Normal")]
        protected TMouseState MouseState
        {
            get { return _mouseState; }
            set
            {
                _mouseState = value;
                Invalidate();
            }
        }

        /// <summary>
        ///     指定箭头所指向的方向
        /// </summary>
        [Browsable(true), Description("指定箭头所指向的方向"), DefaultValue(typeof (TLocation), "Up")]
        public TLocation Direction
        {
            get { return _direction; }
            set
            {
                _direction = value;
                Invalidate();
            }
        }

        #endregion

        #region Override Methods

        /// <summary>
        ///     引发 System.Windows.Forms.Control.Paint 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.PaintEventArgs。</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawTriangle(e.Graphics);
        }

        /// <summary>
        ///     引发 System.Windows.Forms.Control.MouseMove 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            MouseState = TMouseState.Move;
        }

        /// <summary>
        ///     引发 System.Windows.Forms.Control.MouseDown 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            MouseState = TMouseState.Down;
        }

        /// <summary>
        ///     引发 System.Windows.Forms.Control.MouseUp 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            MouseState = TMouseState.Up;
        }

        /// <summary>
        ///     引发 System.Windows.Forms.Control.MouseLeave 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            MouseState = TMouseState.Leave;
        }

        #endregion
    }
}