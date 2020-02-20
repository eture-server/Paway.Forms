using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    /// 自定义弹出下拉框控件基础方法
    /// </summary>
    public class TCustom : MControl
    {
        /// <summary>
        /// 绑定QQTextBox
        /// </summary>
        internal QQTextBox TextBox;
        /// <summary>
        /// 显示行数
        /// </summary>
        internal int showCount;
        /// <summary>
        /// 双Esc关闭
        /// </summary>
        private bool iExit;
        /// <summary>
        /// 数据刷新后触发
        /// </summary>
        public event Action<TDataGridView> RefreshChanged;
        private TDataGridView gridview1;

        internal void Init(QQTextBox textbox, TDataGridView gridview1)
        {
            this.TextBox = textbox;
            this.gridview1 = gridview1;
            textbox.Edit.LostFocus += TextBox_LostFocus;
            textbox.Edit.Click += TextBox_Click;
            textbox.KeyDown += TextBox_KeyDown;
            gridview1.RefreshChanged += Gridview1_RefreshChanged;
            gridview1.DoubleClick += Gridview1_DoubleClick;
            this.Location = new Point(TextBox.Location.X + 1, TextBox.Location.Y + TextBox.Height - 2);
            this.Width = TextBox.Width - 2;
        }
        internal virtual void Gridview1_RefreshChanged()
        {
            RefreshChanged?.Invoke(gridview1);
        }
        /// <summary>
        /// 双击选择并关闭
        /// </summary>
        internal virtual void Gridview1_DoubleClick(object sender, EventArgs e)
        {
            IHide();
        }
        /// <summary>
        /// OnLoad
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Validated += TCustom_Validated;
        }
        internal void ShowHeight(int count)
        {
            if (count > this.showCount) count = this.showCount;
            this.Height = this.gridview1.RowTemplate.Height * count + (count == 0 ? 0 : 2);
        }

        #region 显示控制
        /// <summary>
        /// 显示
        /// </summary>
        internal virtual void IShow() { }
        private void TextBox_Click(object sender, EventArgs e)
        {
            this.Visible = !this.Visible;
            IShow();
        }
        private void TextBox_LostFocus(object sender, EventArgs e)
        {
            if (!this.ContainsFocus) this.Visible = false;
        }
        private void TCustom_Validated(object sender, EventArgs e)
        {
            if (this.TextBox != null) this.Visible = this.TextBox.ContainsFocus;
            else this.Visible = false;
        }
        private void IHide()
        {
            this.Visible = false;
            TextBox.Focus();
        }

        #endregion

        #region 按键显示与返回
        private void TShow()
        {
            this.Visible = true;
            IShow();
            this.Focus();
            gridview1.AutoCell();
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    if (TextBox.SelectionStart == TextBox.Text.Length)
                    {
                        TShow();
                    }
                    break;
                case Keys.Down:
                    TShow();
                    break;
                case Keys.Enter:
                    if (this.Visible)
                    {
                        Gridview1_DoubleClick(this, EventArgs.Empty);
                    }
                    break;
            }
            Escape(e.KeyCode);
        }
        /// <summary>
        /// 按键
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Enter:
                    Gridview1_DoubleClick(this, EventArgs.Empty);
                    break;
                case Keys.Up:
                    if (gridview1.CurrentCell?.RowIndex == 0)
                    {
                        this.TextBox.Focus();
                    }
                    break;
            }
            Escape(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void Escape(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Escape:
                    if (iExit)
                    {
                        iExit = false;
                        IHide();
                    }
                    else iExit = true;
                    break;
                default:
                    iExit = false;
                    break;
            }
        }

        #endregion
    }
}
