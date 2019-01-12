using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Paway.Helper;
using Paway.Resource;

namespace Paway.Forms
{
    /// <summary>
    ///     通知窗口
    /// </summary>
    public class NotifyForm : QQForm
    {
        #region 构造函数

        /// <summary>
        ///     初始化 Paway.Forms.NotifyForm 新的实例
        /// </summary>
        public NotifyForm()
        {
            ShowInTaskbar = false;
            IShadow = false;
        }

        #endregion

        #region 变量

        /// <summary>
        ///     控制窗口动画效果
        /// </summary>
        private Timer Timer;

        /// <summary>
        ///     当前类的实例
        /// </summary>
        private NotifyForm Notify;

        /// <summary>
        ///     显示窗口后停留的时间，以“秒”为单位
        /// </summary>
        private int ShowInterval = 5;

        /// <summary>
        ///     计算时间
        /// </summary>
        private int Interval;

        #endregion

        #region 资源图片

        /// <summary>
        ///     背景图片
        /// </summary>
        private readonly Image _backImage = AssemblyHelper.GetImage("QQ.SkinMgr.all_inside02_bkg.png");

        /// <summary>
        ///     分割线
        /// </summary>
        private readonly Image _splitImage = AssemblyHelper.GetImage("QQ.FormFrame.ContactFilter_splitter.png");

        #endregion

        #region 属性

        /// <summary>
        ///     窗口大小
        /// </summary>
        private readonly Size SIZE = new Size(250, 178);

        /// <summary>
        ///     设置窗口大小的最大值
        /// </summary>
        [Description("设置窗口大小的最大值")]
        [DefaultValue(typeof(Size), "250, 178")]
        public override Size MaximumSize
        {
            get { return SIZE; }
        }

        /// <summary>
        ///     设置窗口大小的最小值
        /// </summary>
        [Description("设置窗口大小的最小值")]
        [DefaultValue(typeof(Size), "250, 178")]
        public override Size MinimumSize
        {
            get { return SIZE; }
        }

        /// <summary>
        ///     提示文字信息
        /// </summary>
        private string _notifyText = string.Empty;

        /// <summary>
        ///     提示文字信息
        /// </summary>
        [Description("提示文字信息")]
        [DefaultValue(null)]
        public string NotifyText
        {
            get { return _notifyText; }
            set
            {
                if (_notifyText != value)
                {
                    _notifyText = value;
                    Invalidate(NotifyTextRect);
                }
            }
        }

        /// <summary>
        ///     提示信息的文字颜色
        /// </summary>
        private Color _notifyForeColor = Color.Black;

        /// <summary>
        ///     提示信息的文字颜色
        /// </summary>
        [Description("提示信息的文字颜色")]
        [DefaultValue(typeof(Color), "Black")]
        public Color NotifyForeColor
        {
            get { return _notifyForeColor; }
            set
            {
                if (_notifyForeColor != value)
                {
                    _notifyForeColor = value;
                    Invalidate(NotifyTextRect);
                }
            }
        }

        /// <summary>
        ///     提示信息的字体
        /// </summary>
        private Font _notifyFont = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 1);

        /// <summary>
        ///     提示信息的字体
        /// </summary>
        [Description("提示信息的字体")]
        [DefaultValue(typeof(Font), "微软雅黑, 9pt")]
        public Font NotifyFont
        {
            get { return _notifyFont; }
            set
            {
                if (_notifyFont != value)
                {
                    _notifyFont = value;
                    Invalidate(NotifyTextRect);
                }
            }
        }

        /// <summary>
        ///     提示信息显示的矩型区域
        /// </summary>
        private Rectangle NotifyTextRect
        {
            get
            {
                var x = 10;
                var y = TitleBarRect.Height;
                var width = Width - x * 2;
                var height = Height - y - ControlRect.Height;
                return new Rectangle(x, y, width, height);
            }
        }

        /// <summary>
        ///     控制按钮的区域
        /// </summary>
        private Rectangle ControlRect
        {
            get
            {
                var x = 10;
                var height = 30;
                var y = Height - height;
                var width = Width - x * 2;
                return new Rectangle(x, y, width, height);
            }
        }

        #endregion

        #region 方法

        /// <summary>
        ///     将以动画的形式显示窗口，默认存在时间为 5 秒
        /// </summary>
        public void AnimalShow()
        {
            AnimalShow(null, null, true);
        }

        /// <summary>
        ///     将以动画的形式显示，默认存在时间为 5 秒
        /// </summary>
        /// <param name="caption">窗口标题</param>
        public void AnimalShow(string caption)
        {
            AnimalShow(caption, null, true);
        }

        /// <summary>
        ///     将以动画的形式显示，默认存在时间为 5 秒
        /// </summary>
        /// <param name="caption">窗口标题</param>
        /// <param name="text">窗口内容</param>
        public void AnimalShow(string caption, string text)
        {
            AnimalShow(caption, text, true);
        }

        /// <summary>
        ///     将以动画的形式显示，默认存在时间为 5 秒
        /// </summary>
        /// <param name="caption">窗口标题</param>
        /// <param name="text">窗口内容</param>
        /// <param name="timer">窗口是否计时消失</param>
        public void AnimalShow(string caption, string text, bool timer)
        {
            if (Notify == null)
                Notify = new NotifyForm();
            if (Timer == null)
                Timer = new Timer();
            if (!string.IsNullOrEmpty(caption)) Notify.TextShow = caption;
            if (!string.IsNullOrEmpty(text)) Notify.NotifyText = text;
            Timer.Interval = 100;
            Timer.Tick += Timer_Tick;
            Timer.Enabled = timer;
            Notify.Show();
        }

        /// <summary>
        ///     将以动画的形式显示，默认存在时间为 5 秒
        /// </summary>
        /// <param name="caption">窗口标题</param>
        /// <param name="text">窗口内容</param>
        /// <param name="interval">窗口存在的时间h</param>
        public void AnimalShow(string caption, string text, int interval)
        {
            ShowInterval = interval;
            AnimalShow(caption, text, true);
        }

        /// <summary>
        ///     执行窗口动画操作
        /// </summary>
        protected void Timer_Tick(object sender, EventArgs e)
        {
            if (Notify != null)
            {
                // 动态改变窗口位置
                var pos = Notify.Height / 100;
                var top = Screen.PrimaryScreen.WorkingArea.Height - Notify.Height;
                if (Notify.Top > top + pos * 10)
                {
                    for (var i = 0; i < 35; i++)
                    {
                        Notify.Top -= pos;
                    }
                    return;
                }
                Notify.Top = top;
                if (ShowInterval > Interval)
                {
                    var count = 1;
                    if (ShowInterval > 30)
                    {
                        count = ShowInterval / 5;
                    }
                    Timer.Interval = count * 1000 < 0 ? int.MaxValue : count * 1000;
                    Interval += count;
                    return;
                }
                Timer.Interval = 100;
                if (Notify != null)
                {
                    if (Notify.Opacity > 0) // 动画降低窗口透明度
                    {
                        Notify.Opacity -= 0.1;
                    }
                    else // 释放窗口资源
                    {
                        Interval = 0;
                        Timer.Enabled = false;
                        Timer.Dispose();
                        Timer = null;
                        Notify.Dispose();
                        Notify = null;
                    }
                }
            }
        }

        #endregion

        #region Override Methods

        /// <summary>
        ///     在窗口加载时，初始化部分数据
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!DesignMode)
            {
                var xPos = Screen.PrimaryScreen.WorkingArea.Width - Width;
                var yPos = Screen.PrimaryScreen.WorkingArea.Height;
                Location = new Point(xPos, yPos);

                TopMost = true;
                TopLevel = true;
                ShowIcon = false;
                IResize = false;
                IDrawBorder = true;
                SysButton = TSysButton.Close;
                BackColor = Color.FromArgb(0, 122, 204);
            }
        }

        /// <summary>
        ///     绘制描述
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_notifyText != string.Empty)
            {
                var g = e.Graphics;
                var sf = new StringFormat()
                {
                    FormatFlags = StringFormatFlags.LineLimit,
                    Trimming = StringTrimming.EllipsisCharacter
                };
                using (Brush brush = new SolidBrush(NotifyForeColor))
                {
                    g.DrawString(NotifyText, NotifyFont, brush, NotifyTextRect, sf);
                }
            }
        }

        /// <summary>
        ///     填充背景
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            var g = e.Graphics;
            var rect = ClientRectangle;
            // 左上角
            DrawHelper.DrawImage(g, _backImage, 0, 0, 5, 5, 5, 5, 10, 10);
            // 左边
            DrawHelper.DrawImage(g, _backImage, 0, 5, 5, Height - 10, 5, 10, 10, _backImage.Height - 20);
            // 左下角
            DrawHelper.DrawImage(g, _backImage, 0, Height - 5, 5, 5, 5, _backImage.Height - 20, 5, 5);

            // 右上角
            DrawHelper.DrawImage(g, _backImage, Width - 5, 0, 5, 5, _backImage.Width - 10, 5, 5, 5);
            // 右边
            DrawHelper.DrawImage(g, _backImage, Width - 5, 5, 5, Height - 10, _backImage.Width - 10, 10, 5,
                _backImage.Height - 20);
            // 右下角
            DrawHelper.DrawImage(g, _backImage, Width - 5, Height - 5, 5, 5, _backImage.Width - 10,
                _backImage.Height - 10, 5, 5);

            // 上边
            DrawHelper.DrawImage(g, _backImage, 5, 0, Width - 10, 5, 10, 5, _backImage.Width - 20, 5);
            // 下边
            DrawHelper.DrawImage(g, _backImage, 5, Height - 5, Width - 10, 5, 10, _backImage.Height - 10,
                _backImage.Width - 20, 5);
            // 填充
            DrawHelper.DrawImage(g, _backImage, 5, 5, Width - 10, Height - 10, 10, 10, _backImage.Width - 20,
                _backImage.Height - 20);

            // 划分线
            g.DrawImage(
                _splitImage,
                new Rectangle(ControlRect.X, ControlRect.Y, _splitImage.Width, _splitImage.Height),
                0,
                0,
                _splitImage.Width,
                _splitImage.Height,
                GraphicsUnit.Pixel);
        }

        #endregion
    }
}