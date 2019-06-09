using Paway.Helper;
using Paway.Win32;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    ///     窗体示例
    /// </summary>
    public partial class IBaseForm : QQForm
    {
        /// <summary>
        /// 双Esc关闭
        /// </summary>
        protected bool iExit;
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
        public IBaseForm()
        {
            InitializeComponent();
            this.TextShow = string.Empty;
            this.TMouseMove(this.lbTitle);
            lbTitle.Paint += LbTitle_Paint;
        }

        #region 其它
        /// <summary>
        /// 激活窗体
        /// </summary>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Activate();
            ActivatedFirst(this);
        }
        /// <summary>
        /// 激活第一个控件焦点
        /// </summary>
        protected virtual void ActivatedFirst(Control control)
        {
            Control result = NextControl(Point.Empty, control);
            if (result != null)
            {
                result.Focus();
                if (result is QQTextBox textBox) textBox.Reset();
            }
        }
        internal void LbTitle_Paint(object sender, PaintEventArgs e)
        {
            if (lbTitle.Visible)
                e.Graphics.DrawLine(new Pen(BitmapHelper.RGBAddLight(lbTitle.BackColor, -40)), 0, lbTitle.Height - 1, lbTitle.Width, lbTitle.Height - 1);
        }
        /// <summary>
        ///     关闭时激发父窗体
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (Owner != null)
            {
                Owner.Activate();
            }
            base.OnFormClosing(e);
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
                    if (iExit)
                    {
                        ToolCancel_ItemClick(null, EventArgs.Empty);
                    }
                    iExit = true;
                    break;
                default:
                    iExit = false;
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
                ExceptionHelper.Show(this, ex);
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
                ExceptionHelper.Show(this, ex);
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
                ExceptionHelper.Show(this, ex);
            }
        }

        #endregion
    }
}