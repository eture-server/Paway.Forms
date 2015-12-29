using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Paway.Resource;

namespace Paway.Forms.Metro
{
    public partial class MetroForm : Form
    {
        #region 构造函数

        /// <summary>
        /// </summary>
        public MetroForm()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.Selectable |
                ControlStyles.ContainerControl, true);
            SetStyle(ControlStyles.Opaque, false);
            UpdateStyles();

            BackColor = Renderer.BackColor;
            InitializeComponent();
        }

        #endregion

        #region 变量

        /// <summary>
        ///     Item宽度（高度）
        /// </summary>
        private readonly int _itemWidth = 80;

        /// <summary>
        /// </summary>
        private int _itemHeight = 80;

        /// <summary>
        /// </summary>
        private MetroRenderer _renderer;

        /// <summary>
        /// </summary>
        private MetroItemCollection _items;

        /// <summary>
        /// </summary>
        private bool _mouseDown;

        /// <summary>
        /// </summary>
        private Rectangle _startRect = Rectangle.Empty;

        /// <summary>
        /// </summary>
        private readonly Image START_IMAGE = AssemblyHelper.GetImage("Icons.start.png");

        /// <summary>
        /// </summary>
        private Size _startSize = new Size(50, 50);

        /// <summary>
        /// </summary>
        private TMouseState _startState = TMouseState.Normal;

        /// <summary>
        /// </summary>
        private static readonly object EventClickStart = new object();

        /// <summary>
        /// </summary>
        private static readonly object EventClickMetroFormItem = new object();

        #endregion

        #region 属性

        /// <summary>
        /// </summary>
        public MetroRenderer Renderer
        {
            get
            {
                if (_renderer == null)
                    _renderer = new MetroRenderer();
                return _renderer;
            }
            set { _renderer = value; }
        }

        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public MetroItemCollection Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new MetroItemCollection(this);
                }
                return _items;
            }
        }

        /// <summary>
        /// </summary>
        [Description("每个项(Item)的高度"), DefaultValue(80)]
        public int ItemHeight
        {
            get { return _itemHeight; }
            set { _itemHeight = value; }
        }

        /// <summary>
        /// </summary>
        protected Rectangle StartRect
        {
            get
            {
                if (START_IMAGE != null && _startRect == Rectangle.Empty)
                {
                    var y = Height - _itemHeight;
                    _startRect = new Rectangle(0, y, Width, _itemHeight);
                }
                return _startRect;
            }
        }

        #endregion

        #region 事件

        /// <summary>
        ///     当单击开始按钮时，激发的事件
        /// </summary>
        public event EventHandler ClickStart
        {
            add { Events.AddHandler(EventClickStart, value); }
            remove { Events.RemoveHandler(EventClickStart, value); }
        }

        /// <summary>
        ///     当单击Item时激情的事件
        /// </summary>
        public event EventHandler ClickMetroFormItem
        {
            add { Events.AddHandler(EventClickMetroFormItem, value); }
            remove { Events.RemoveHandler(EventClickMetroFormItem, value); }
        }

        #endregion

        #region 方法

        /// <summary>
        ///     绘制开始菜单UI
        /// </summary>
        /// <param name="g"></param>
        private void DrawStart(Graphics g)
        {
            var backColor = Renderer.BackColor;
            switch (_startState)
            {
                case TMouseState.Normal:
                case TMouseState.Leave:
                    backColor = Renderer.BackColor;
                    break;
                case TMouseState.Move:
                case TMouseState.Up:
                    backColor = Renderer.EnterColor;
                    break;
                case TMouseState.Down:
                    backColor = Renderer.DownColor;
                    break;
            }
            using (Brush brush = new SolidBrush(backColor))
            {
                g.FillRectangle(brush, StartRect);
            }
            using (var path = new GraphicsPath())
            {
                #region 手工绘制

                //const int TEMP = 4;
                //int x = 15;
                //int y = this.StartRect.Height / 5 - 2;

                //int width = this.Width - (x * 2);
                //Point[] point = new Point[4];
                //point[0] = new Point(x, this.StartRect.Y + y + TEMP);
                //point[1] = new Point(x + width, this.StartRect.Y + y - 2);
                //point[2] = new Point(x + width, this.StartRect.Bottom - y + 2);
                //point[3] = new Point(x, this.StartRect.Bottom - y - TEMP);
                //path.AddLines(point);
                //path.CloseFigure();
                //using (Brush brush = new SolidBrush(this.Renderer.StartColor))
                //{
                //    g.FillPath(brush, path);
                //}
                //using (Pen pen = new Pen(backColor, 3.0f))
                //{
                //    int lineY = this.StartRect.Y + (this.StartRect.Height / 2);
                //    g.DrawLine(pen, x - 1, lineY, width + x + 1, lineY);
                //}
                //using (Pen pen = new Pen(backColor, 3.0f))
                //{
                //    int lineX = width / 5 * 2 + x;
                //    g.DrawLine(
                //        pen,
                //        lineX,
                //        this.StartRect.Y + y,
                //        lineX,
                //        this.StartRect.Bottom - y);
                //}

                #endregion

                var x = (Width - _startSize.Width) / 2;
                var y = StartRect.Y + (StartRect.Height - _startSize.Height) / 2;
                var rect = new Rectangle(new Point(x, y), _startSize);
                g.DrawImage(START_IMAGE, rect, 0, 0, START_IMAGE.Width, START_IMAGE.Height, GraphicsUnit.Pixel);
            }
        }

        /// <summary>
        ///     打开Item
        /// </summary>
        /// <param name="item"></param>
        private void OpenProcess(MetroItem item)
        {
            if (string.IsNullOrEmpty(item.FilePath))
            {
                MessageBox.Show("找不到指定路径");
                return;
            }
            switch (item.ItemType)
            {
                case TItemType.Application:
                    break;
                case TItemType.Directory:
                    break;
                case TItemType.Exe:
                    Process.Start(item.FilePath);
                    break;
                case TItemType.Menu:
                    break;
                case TItemType.None:
                    break;
                case TItemType.System:
                    if (string.IsNullOrEmpty(item.ClassID))
                    {
                        MessageBox.Show("找不到ClassID");
                        return;
                    }
                    Process.Start(item.FilePath, item.ClassID);
                    break;
            }
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (!DesignMode)
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                if (Items.Count > 0)
                {
                    int x = 0, y = 0;
                    var width = Width - 1;
                    //循环绘制每一项
                    foreach (var item in _items)
                    {
                        var itemRect = new Rectangle(x, y, width, _itemHeight - 1);
                        //itemRect.Inflate(1, 1);
                        item.Location = new Point(x, y);
                        item.Size = new Size(width, _itemHeight);
                        item.OnPaintBackground(g, itemRect, Renderer);
                        item.OnPaint(g, itemRect, Renderer);
                        y += _itemHeight;
                    }
                }
                DrawStart(g); //绘制开始菜单
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!DesignMode && !_mouseDown)
            {
                if (Items.Count > 0)
                {
                    foreach (var item in Items)
                    {
                        if (item.Rectangle.Contains(e.Location))
                            item.MouseState = TMouseState.Move;
                        else
                            item.MouseState = TMouseState.Leave;
                        Invalidate(item.Rectangle);
                    }
                }
                if (StartRect.Contains(e.Location))
                {
                    _startState = TMouseState.Move;
                    Invalidate(StartRect);
                }
                else
                {
                    _startState = TMouseState.Leave;
                    Invalidate(StartRect);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (!DesignMode && e.Button == MouseButtons.Left)
            {
                if (Items.Count > 0)
                {
                    foreach (var item in Items)
                    {
                        if (item.Rectangle.Contains(e.Location))
                        {
                            item.MouseState = TMouseState.Down;
                            _mouseDown = true;
                        }
                        Invalidate(item.Rectangle);
                    }
                }
                if (StartRect.Contains(e.Location))
                {
                    _mouseDown = true;
                    _startState = TMouseState.Down;
                    Invalidate(StartRect);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (!DesignMode && e.Button == MouseButtons.Left)
            {
                if (Items.Count > 0)
                {
                    foreach (var item in Items)
                    {
                        if (item.Rectangle.Contains(e.Location))
                        {
                            item.MouseState = TMouseState.Up;
                            OnClickMetroFormItem(item, EventArgs.Empty);
                            OpenProcess(item);
                        }
                        Invalidate(item.Rectangle);
                    }
                }
                if (StartRect.Contains(e.Location))
                {
                    _startState = TMouseState.Up;
                    OnClickStart(this, EventArgs.Empty);
                    Invalidate(StartRect);
                }
                _mouseDown = false;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (!DesignMode && Items.Count > 0)
            {
                foreach (var item in Items)
                {
                    if (item.MouseState != TMouseState.Leave)
                    {
                        item.MouseState = TMouseState.Leave;
                        Invalidate(item.Rectangle);
                    }
                }
                if (_startState != TMouseState.Leave)
                {
                    _startState = TMouseState.Leave;
                    Invalidate(StartRect);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!DesignMode)
            {
                var width = Screen.PrimaryScreen.WorkingArea.Width;
                var height = Screen.PrimaryScreen.WorkingArea.Height;
                Size = new Size(_itemWidth, height);
                Location = new Point(width - _itemWidth, 0);
            }
        }

        #endregion

        #region 激发事件的方法

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnClickStart(object sender, EventArgs e)
        {
            var handler = Events[EventClickStart] as EventHandler;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnClickMetroFormItem(object sender, EventArgs e)
        {
            var handler = Events[EventClickMetroFormItem] as EventHandler;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        #endregion
    }
}