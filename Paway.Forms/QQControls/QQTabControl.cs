using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Paway.Helper;
using Paway.Resource;
using Paway.Win32;

namespace Paway.Forms
{
    /// <summary>
    /// </summary>
    [ToolboxBitmap(typeof(TabControl))]
    public class QQTabControl : TabControl
    {
        #region 构造函数

        /// <summary>
        /// </summary>
        public QQTabControl()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.Selectable |
                ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();
            SizeMode = TabSizeMode.Fixed;
            ItemSize = new Size(80, 32);
        }

        #endregion

        #region 变量

        private readonly Image _titleBackground = AssemblyHelper.GetImage("QQ.TabControl.main_tab_bkg.png");
        private Color _baseColor = Color.White;
        private Color _backColor = Color.Transparent;
        private Color _borderColor = Color.White;
        private Color _pageColor = Color.White;

        /// <summary>
        ///     是否获取了焦点
        /// </summary>
        private bool _isFocus;

        /// <summary>
        ///     选项卡箭头区域
        /// </summary>
        private Rectangle _btnArrowRect = Rectangle.Empty;

        #endregion

        #region 属性

        /// <summary>
        /// </summary>
        [DefaultValue(typeof(Color), "White")]
        [Category("自定义属性")]
        public Color BaseColor
        {
            get { return _baseColor; }
            set
            {
                _baseColor = value;
                Invalidate(true);
            }
        }

        /// <summary>
        /// </summary>
        [DefaultValue(typeof(Color), "Transparent")]
        [Category("自定义属性")]
        public override Color BackColor
        {
            get { return _backColor; }
            set
            {
                _backColor = value;
                Invalidate(true);
            }
        }

        /// <summary>
        /// </summary>
        [DefaultValue(typeof(Color), "White")]
        [Category("自定义属性")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                Invalidate(true);
            }
        }

        /// <summary>
        /// </summary>
        [DefaultValue(typeof(Color), "White")]
        [Description("所有TabPage的背景颜色")]
        [Category("自定义属性")]
        public Color PageColor
        {
            get { return _pageColor; }
            set
            {
                _pageColor = value;
                if (TabPages.Count > 0)
                {
                    for (var i = 0; i < TabPages.Count; i++)
                    {
                        TabPages[i].BackColor = _pageColor;
                    }
                }
                Invalidate(true);
            }
        }

        private readonly Image main_tab_checkImage = AssemblyHelper.GetImage("QQ.TabControl.main_tab_check.png");
        private readonly Image main_tabbtn_downImage = AssemblyHelper.GetImage("QQ.TabControl.main_tabbtn_down.png");
        private readonly Image main_tabbtn_highlightImage = AssemblyHelper.GetImage("QQ.TabControl.main_tabbtn_highlight.png");
        private readonly Image main_tab_highlightImage = AssemblyHelper.GetImage("QQ.TabControl.main_tab_highlight.png");

        #endregion

        #region Override Methods

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;

            DrawBackground(g);
            DrawTabPages(g);
        }

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!DesignMode)
                Invalidate();
        }

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (!DesignMode)
                Invalidate();
        }

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (!DesignMode)
            {
                if (e.Button == MouseButtons.Left && _btnArrowRect.Contains(e.Location))
                {
                    if (!_isFocus)
                    {
                        _isFocus = true;
                        Invalidate(_btnArrowRect);
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg != (int)WindowsMessage.WM_CONTEXTMENU) //0x007B鼠标右键
            {
                base.WndProc(ref m);
            }
        }

        #endregion

        #region Draw Methods

        /// <summary>
        ///     绘制背景
        /// </summary>
        /// <param name="g"></param>
        private void DrawBackground(Graphics g)
        {
            var width = ClientRectangle.Width;
            var height = ClientRectangle.Height - DisplayRectangle.Height;
            var backColor = Enabled ? _backColor : SystemColors.Control;
            using (var brush = new SolidBrush(backColor))
            {
                g.FillRectangle(brush, ClientRectangle);
            }
            var bgRect = new Rectangle(2, 2, Width - 2, ItemSize.Height);
            DrawImage(g, _titleBackground, bgRect); //绘制背景图
        }

        /// <summary>
        ///     绘图
        /// </summary>
        /// <param name="g"></param>
        /// <param name="image"></param>
        /// <param name="rect"></param>
        private void DrawImage(Graphics g, Image image, Rectangle rect)
        {
            g.DrawImage(image, new Rectangle(rect.X, rect.Y, 5, rect.Height), 0, 0, 5, image.Height,
                GraphicsUnit.Pixel);
            g.DrawImage(image, new Rectangle(rect.X + 5, rect.Y, rect.Width - 10, rect.Height), 5, 0, image.Width - 10,
                image.Height, GraphicsUnit.Pixel);
            g.DrawImage(image, new Rectangle(rect.X + rect.Width - 5, rect.Y, 5, rect.Height), image.Width - 5, 0, 5,
                image.Height, GraphicsUnit.Pixel);
        }

        private void DrawTabPages(Graphics g)
        {
            using (var brush = new SolidBrush(_pageColor))
            {
                var x = 2;
                var y = ItemSize.Height;
                var width = Width - 2;
                var height = Height - ItemSize.Height;
                g.FillRectangle(brush, x, y, width, height);
                g.DrawRectangle(new Pen(_borderColor), x, y, width - 1, height - 1);
            }
            var tabRect = Rectangle.Empty;
            var cursorPoint = PointToClient(MousePosition);
            for (var i = 0; i < TabCount; i++)
            {
                var page = TabPages[i];
                tabRect = GetTabRect(i);
                var baseColor = Color.Yellow;
                Image baseTabHeaderImage = null;
                Image btnArrowImage = null;

                if (SelectedIndex == i) //是否选中
                {
                    baseTabHeaderImage = main_tab_checkImage;
                    if (TabPages[i] is QQTabPage)
                    {
                        var contextMenuLocation =
                            PointToScreen(new Point(_btnArrowRect.Left, _btnArrowRect.Top + _btnArrowRect.Height + 5));
                        var contextMenuStrip = (TabPages[i] as QQTabPage).ContextMenuShow;
                        if (contextMenuStrip != null)
                        {
                            contextMenuStrip.Closed -= contextMenuStrip_Closed;
                            contextMenuStrip.Closed += contextMenuStrip_Closed;
                            if (contextMenuLocation.X + contextMenuStrip.Width >
                                Screen.PrimaryScreen.WorkingArea.Width - 20)
                            {
                                contextMenuLocation.X = Screen.PrimaryScreen.WorkingArea.Width - contextMenuStrip.Width -
                                                        50;
                            }
                            if (tabRect.Contains(cursorPoint))
                            {
                                if (_isFocus)
                                {
                                    btnArrowImage = main_tabbtn_downImage;
                                    contextMenuStrip.Show(contextMenuLocation);
                                }
                                else
                                {
                                    btnArrowImage = main_tabbtn_highlightImage;
                                }
                                _btnArrowRect = new Rectangle(tabRect.X + tabRect.Width - btnArrowImage.Width, tabRect.Y,
                                    btnArrowImage.Width, btnArrowImage.Height);
                            }
                            else if (_isFocus)
                            {
                                btnArrowImage = main_tabbtn_downImage;
                                contextMenuStrip.Show(contextMenuLocation);
                            }
                        }
                    }
                }
                else if (tabRect.Contains(cursorPoint)) //鼠标滑过
                {
                    baseTabHeaderImage = main_tab_highlightImage;
                }
                if (baseTabHeaderImage != null)
                {
                    if (SelectedIndex == i)
                    {
                        if (SelectedIndex == TabCount - 1)
                            tabRect.Inflate(2, 0);
                        else
                            tabRect.Inflate(1, 0);
                    }
                    DrawImage(g, baseTabHeaderImage, tabRect);
                    if (btnArrowImage != null)
                    {
                        //当鼠标进入当前选中的的选项卡时，显示下拉按钮
                        g.DrawImage(btnArrowImage, _btnArrowRect);
                    }
                }
                TextRenderer.DrawText(g, page.Text, page.Font, tabRect, page.ForeColor);
            }
        }

        private void contextMenuStrip_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            _isFocus = false;
            Invalidate(_btnArrowRect);
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
                if (_titleBackground != null)
                    _titleBackground.Dispose();
                if (main_tab_checkImage != null)
                    main_tab_checkImage.Dispose();
                if (main_tabbtn_downImage != null)
                    main_tabbtn_downImage.Dispose();
                if (main_tabbtn_highlightImage != null)
                    main_tabbtn_highlightImage.Dispose();
                if (main_tab_highlightImage != null)
                    main_tab_highlightImage.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}