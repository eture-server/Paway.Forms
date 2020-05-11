using Paway.Helper;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    /// 窗体示例
    /// </summary>
    public partial class TBaseForm : QQForm
    {
        /// <summary>
        /// Esc按下时间
        /// </summary>
        private DateTime exitTime;
        private int interval = NativeMethods.GetDoubleClickTime();
        /// <summary>
        /// 双Esc关闭
        /// </summary>
        protected bool IExit;
        /// <summary>
        /// 响应回车到下一控件
        /// </summary>
        protected bool IEnter = true;
        /// <summary>
        /// 响应回车点击事件
        /// </summary>
        protected bool IEnterClick = false;

        /// <summary>
        /// 构造
        /// </summary>
        public TBaseForm()
        {
            InitializeComponent();
            this.TextShow = string.Empty;
            this.TMouseMove(this.lbTitle);
            this.TMouseMove(this.panel1);
            lbTitle.TextChanged += delegate { this.OnSizeChanged(EventArgs.Empty); };
        }

        #region 其它
        /// <summary>
        /// 激活窗体
        /// </summary>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Activate();
            ActivatedControl(panel1);
        }
        /// <summary>
        /// 激活第一个控件焦点
        /// </summary>
        protected virtual void ActivatedControl(Control control)
        {
            Control result = NextControl(Point.Empty, control);
            if (result != null)
            {
                result.Focus();
            }
        }
        /// <summary>
        /// 关闭时激发父窗体
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (Owner != null)
            {
                Owner.Activate();
            }
            base.OnFormClosing(e);
        }
        /// <summary>
        /// 调整Title位置
        /// </summary>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (lbTitle != null)
            {
                var size = TextRenderer.MeasureText(lbTitle.Text, lbTitle.Font);
                this.lbTitle.Location = new Point((this.Width - size.Width) / 2, (this.Padding.Top - size.Height * 2 / 3) / 2);
            }
        }

        #endregion

        #region 按键事件
        /// <summary>
        /// 按键响应
        /// </summary>
        protected virtual void OnKeyDown(Keys key) { }
        /// <summary>
        /// 按键
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Tab:
                    Control current = CurrentPoint(this);
                    Control result = NextControl(current.Location, current.Parent);
                    if (result == null)
                    {
                        result = NextControl(Point.Empty, current.Parent);
                    }
                    if (result != null)
                    {
                        result.Focus();
                    }
                    return true;
                case Keys.Enter | Keys.Control:
                    ToolOk_ItemClick(null, EventArgs.Empty);
                    break;
                case Keys.Enter:
                    if (IEnter) EnterFoucs();
                    break;
            }
            switch (keyData)
            {
                case Keys.Escape:
                    if (IExit && DateTime.Now.Subtract(exitTime).TotalMilliseconds < interval)
                    {
                        ToolCancel_ItemClick(null, EventArgs.Empty);
                        break;
                    }
                    IExit = true;
                    exitTime = DateTime.Now;
                    break;
                default:
                    IExit = false;
                    OnKeyDown(keyData);
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void EnterFoucs()
        {
            Control current = CurrentPoint(this);
            if (current is QQTextBox && (current as QQTextBox).Edit.Multiline) return;
            Control result = NextControl(current.Location, current.Parent);
            if (result != null)
            {
                result.Focus();
            }
            else if (IEnterClick)
            {
                this.Focus();
                ToolOk_ItemClick(null, EventArgs.Empty);
            }
        }
        /// <summary>
        /// 获取下一个控件
        /// </summary>
        private Control NextControl(Point current, Control parent)
        {
            Control result = null;
            Point next = new Point(this.Width, this.Height);
            foreach (Control item in parent.Controls)
            {
                if (item.Location.Y < next.Y || (item.Location.Y == next.Y && item.Location.X < next.X))
                    if (item.Location.Y > current.Y || (item.Location.Y == current.Y && item.Location.X > current.X))
                    {
                        if (item.Visible && item.Enabled &&
                            ((item is QQTextBox && (item as QQTextBox).Edit.Enabled && !(item as QQTextBox).Edit.ReadOnly) ||
                            item is DateTimePicker ||
                            (item is NumericUpDown && !(item as NumericUpDown).ReadOnly)))
                        {
                            next = item.Location;
                            result = item;
                        }
                    }
            }
            return result;
        }
        /// <summary>
        /// 当前焦点控件
        /// </summary>
        private Control CurrentPoint(Control parent)
        {
            foreach (Control item in parent.Controls)
            {
                if (item.ContainsFocus)
                {
                    if (item.GetType() == typeof(TControl) || item.GetType() == typeof(Panel))
                    {
                        return CurrentPoint(item);
                    }
                    return item;
                }
            }
            return this;
        }

        #endregion

        #region 按钮事件
        /// <summary>
        /// 按钮响应
        /// </summary>
        protected virtual void OnItemClick(ToolItem item) { }
        /// <summary>
        /// 确认响应
        /// </summary>
        protected virtual bool OnCommit() { return true; }
        /// <summary>
        /// 异常
        /// </summary>
        protected virtual void OnFailed() { }
        /// <summary>
        /// 取消响应
        /// </summary>
        protected virtual bool OnCancel() { return true; }
        /// <summary>
        /// 其它响应
        /// </summary>
        protected void ToolBar1_ItemClick(ToolItem item, EventArgs e)
        {
            try
            {
                OnItemClick(item);
            }
            catch (Exception ex)
            {
                ex.Show(this);
            }
        }
        /// <summary>
        /// 确认OK
        /// </summary>
        protected void ToolOk_ItemClick(ToolItem item, EventArgs e)
        {
            try
            {
                if (this.DialogResult == DialogResult.OK) return;
                if (OnCommit())
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                OnFailed();
                ex.Show(this);
            }
        }
        /// <summary>
        /// 取消Cancel
        /// </summary>
        protected void ToolCancel_ItemClick(ToolItem item, EventArgs e)
        {
            try
            {
                if (this.DialogResult == DialogResult.Cancel) return;
                if (OnCancel())
                {
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                ex.Show(this);
            }
        }

        #endregion
    }
}