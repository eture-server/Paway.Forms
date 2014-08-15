using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Paway.Win32;
using System.Diagnostics;
using Paway.Resource;
using Paway.Helper;
using System.ComponentModel;

namespace Paway.Forms
{
    /// <summary>
    /// 通知窗口
    /// </summary>
    public class NotifyForm : QQForm
    {
        #region 变量
        /// <summary>
        /// 控制窗口动画效果
        /// </summary>
        private Timer Timer = null;
        /// <summary>
        /// 当前类的实例
        /// </summary>
        private NotifyForm Notify = null;
        /// <summary>
        /// 显示窗口后停留的时间，以“秒”为单位
        /// </summary>
        private int ShowInterval = 5;
        /// <summary>
        /// 计算时间
        /// </summary>
        private int Interval = 0;

        #endregion

        #region 资源图片
        /// <summary>
        /// 背景图片
        /// </summary>
        private Image _backImage = AssemblyHelper.GetImage("QQ.SkinMgr.all_inside02_bkg.png");
        /// <summary>
        /// 分割线
        /// </summary>
        private Image _splitImage = AssemblyHelper.GetImage("QQ.FormFrame.ContactFilter_splitter.png");

        #endregion

        #region 构造函数
        /// <summary>
        /// 初始化 Paway.Forms.NotifyForm 新的实例
        /// </summary>
        public NotifyForm()
            : base()
        {
            this.ShowInTaskbar = false;
            this.Shadow = false;
        }

        #endregion

        #region 属性
        /// <summary>
        /// 窗口大小
        /// </summary>
        private readonly Size SIZE = new Size(250, 178);
        /// <summary>
        /// 设置窗口大小的最大值
        /// </summary>
        [Description("设置窗口大小的最大值"), DefaultValue(typeof(Size), "250, 178")]
        public override Size MaximumSize
        {
            get { return SIZE; }
        }
        /// <summary>
        /// 设置窗口大小的最小值
        /// </summary>
        [Description("设置窗口大小的最小值"), DefaultValue(typeof(Size), "250, 178")]
        public override Size MinimumSize
        {
            get { return SIZE; }
        }
        /// <summary>
        /// 提示文字信息
        /// </summary>
        private string _notifyText = string.Empty;
        /// <summary>
        /// 提示文字信息
        /// </summary>
        [Description("提示文字信息"), DefaultValue(null)]
        public string NotifyText
        {
            get { return this._notifyText; }
            set
            {
                if (this._notifyText != value)
                {
                    this._notifyText = value;
                    this.Invalidate(this.NotifyTextRect);
                }
            }
        }
        /// <summary>
        /// 提示信息的文字颜色
        /// </summary>
        private Color _notifyForeColor = Color.Black;
        /// <summary>
        /// 提示信息的文字颜色
        /// </summary>
        [Description("提示信息的文字颜色"), DefaultValue(typeof(Color), "Black")]
        public Color NotifyForeColor
        {
            get { return this._notifyForeColor; }
            set
            {
                if (this._notifyForeColor != value)
                {
                    this._notifyForeColor = value;
                    this.Invalidate(this.NotifyTextRect);
                }
            }
        }
        /// <summary>
        /// 提示信息的字体
        /// </summary>
        private Font _notifyFont = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)1);
        /// <summary>
        /// 提示信息的字体
        /// </summary>
        [Description("提示信息的字体"), DefaultValue(typeof(Font), "微软雅黑, 9pt")]
        public Font NotifyFont
        {
            get { return this._notifyFont; }
            set
            {
                if (this._notifyFont != value)
                {
                    this._notifyFont = value;
                    this.Invalidate(this.NotifyTextRect);
                }
            }
        }
        /// <summary>
        /// 提示信息显示的矩型区域
        /// </summary>
        private Rectangle NotifyTextRect
        {
            get
            {
                int x = 10;
                int y = base.TitleBarRect.Height;
                int width = this.Width - x * 2;
                int height = this.Height - y - this.ControlRect.Height;
                return new Rectangle(x, y, width, height);
            }
        }
        /// <summary>
        /// 控制按钮的区域
        /// </summary>
        private Rectangle ControlRect
        {
            get
            {
                int x = 10;
                int height = 30;
                int y = this.Height - height;
                int width = this.Width - x * 2;
                return new Rectangle(x, y, width, height);
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 将以动画的形式显示窗口，默认存在时间为 5 秒
        /// </summary>
        public void AnimalShow()
        {
            AnimalShow(null, null, true);
        }
        /// <summary>
        /// 将以动画的形式显示，默认存在时间为 5 秒
        /// </summary>
        /// <param name="caption">窗口标题</param>
        public void AnimalShow(string caption)
        {
            AnimalShow(caption, null, true);
        }
        /// <summary>
        /// 将以动画的形式显示，默认存在时间为 5 秒
        /// </summary>
        /// <param name="caption">窗口标题</param>
        /// <param name="text">窗口内容</param>
        public void AnimalShow(string caption, string text)
        {
            AnimalShow(caption, text, true);
        }
        /// <summary>
        /// 将以动画的形式显示，默认存在时间为 5 秒
        /// </summary>
        /// <param name="caption">窗口标题</param>
        /// <param name="text">窗口内容</param>
        /// <param name="timer">窗口是否计时消失</param>
        public void AnimalShow(string caption, string text, bool timer)
        {
            try
            {
                if (this.Notify == null)
                    this.Notify = new NotifyForm();
                if (this.Timer == null)
                    this.Timer = new Timer();
                if (!string.IsNullOrEmpty(caption)) this.Notify.TextShow = caption;
                if (!string.IsNullOrEmpty(text)) this.Notify.NotifyText = text;
                this.Timer.Interval = 100;
                this.Timer.Tick += new EventHandler(Timer_Tick);
                this.Timer.Enabled = timer;
                this.Notify.Show();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("NotifyForm.AnimalShow() :: " + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 将以动画的形式显示，默认存在时间为 5 秒
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
        /// 执行窗口动画操作
        /// </summary>
        protected void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (this.Notify != null)
                {
                    // 动态改变窗口位置
                    int pos = this.Notify.Height / 100;
                    int top = Screen.PrimaryScreen.WorkingArea.Height - this.Notify.Height;
                    if (this.Notify.Top > top + pos * 10)
                    {
                        for (int i = 0; i < 35; i++)
                        {
                            this.Notify.Top -= pos;
                        }
                    }
                    else
                    {
                        this.Notify.Top = top;
                        if (ShowInterval > Interval)
                        {
                            int count = 1;
                            if (ShowInterval > 30)
                            {
                                count = ShowInterval / 5;
                            }
                            this.Timer.Interval = count * 1000 < 0 ? int.MaxValue : count * 1000;
                            Interval += count;
                        }
                        else
                        {
                            this.Timer.Interval = 100;
                            if (this.Notify != null)
                            {
                                if (this.Notify.Opacity > 0)  // 动画降低窗口透明度
                                {
                                    this.Notify.Opacity -= 0.1;
                                }
                                else                                // 释放窗口资源
                                {
                                    Interval = 0;
                                    this.Timer.Enabled = false;
                                    this.Timer.Dispose();
                                    this.Timer = null;
                                    this.Notify.Dispose();
                                    this.Notify = null;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("NotifyForm.Timer_Tick(object, EventArgs) :: " + ex.Message);
                throw;
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// 在窗口加载时，初始化部分数据
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!this.DesignMode)
            {
                int xPos = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
                int yPos = Screen.PrimaryScreen.WorkingArea.Height;
                this.Location = new Point(xPos, yPos);

                this.TopMost = true;
                this.TopLevel = true;
                this.ShowIcon = false;
                base.IsResize = false;
                base.IsDrawBorder = true;
                this.SysButton = TSysButton.Close;
                base.BackColor = Color.FromArgb(0, 122, 204);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (this._notifyText != string.Empty)
            {
                Graphics g = e.Graphics;
                StringFormat sf = new StringFormat();
                sf.FormatFlags = StringFormatFlags.LineLimit;
                sf.Trimming = StringTrimming.EllipsisCharacter;
                using (Brush brush = new SolidBrush(this.NotifyForeColor))
                {
                    g.DrawString(this.NotifyText, this.NotifyFont, brush, this.NotifyTextRect, sf);
                }
            }
        }
        /// <summary>
        /// 填充背景
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            Graphics g = e.Graphics;
            Rectangle rect = this.ClientRectangle;
            // 左上角
            DrawHelper.DrawImage(g, this._backImage, 0, 0, 5, 5, 5, 5, 10, 10);
            // 左边
            DrawHelper.DrawImage(g, this._backImage, 0, 5, 5, this.Height - 10, 5, 10, 10, this._backImage.Height - 20);
            // 左下角
            DrawHelper.DrawImage(g, this._backImage, 0, this.Height - 5, 5, 5, 5, this._backImage.Height - 20, 5, 5);

            // 右上角
            DrawHelper.DrawImage(g, this._backImage, this.Width - 5, 0, 5, 5, this._backImage.Width - 10, 5, 5, 5);
            // 右边
            DrawHelper.DrawImage(g, this._backImage, this.Width - 5, 5, 5, this.Height - 10, this._backImage.Width - 10, 10, 5, this._backImage.Height - 20);
            // 右下角
            DrawHelper.DrawImage(g, this._backImage, this.Width - 5, this.Height - 5, 5, 5, this._backImage.Width - 10, this._backImage.Height - 10, 5, 5);

            // 上边
            DrawHelper.DrawImage(g, this._backImage, 5, 0, this.Width - 10, 5, 10, 5, this._backImage.Width - 20, 5);
            // 下边
            DrawHelper.DrawImage(g, this._backImage, 5, this.Height - 5, this.Width - 10, 5, 10, this._backImage.Height - 10, this._backImage.Width - 20, 5);
            // 填充
            DrawHelper.DrawImage(g, this._backImage, 5, 5, this.Width - 10, this.Height - 10, 10, 10, this._backImage.Width - 20, this._backImage.Height - 20);

            // 划分线
            g.DrawImage(
                this._splitImage,
                new Rectangle(this.ControlRect.X, this.ControlRect.Y, this._splitImage.Width, this._splitImage.Height),
                0,
                0,
                this._splitImage.Width,
                this._splitImage.Height,
                GraphicsUnit.Pixel);
        }

        #endregion
    }
}
