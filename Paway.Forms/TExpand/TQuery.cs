using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Paway.Helper;
using Paway.Forms;

namespace Paway.Forms
{
    /// <summary>
    /// 搜索
    /// </summary>
    public partial class TQuery<T> : MControl where T : IId, IFind<T>
    {
        private QQTextBox TextBox;
        private List<T> list;
        /// <summary>
        /// 最大显示数量
        /// </summary>
        private int totalCount;
        /// <summary>
        /// 显示行数()
        /// </summary>高度
        private int showCount;
        /// <summary>
        /// 选择事件
        /// </summary>
        public event Action<long> SelectedEvent;
        /// <summary>
        ///     数据刷新后触发
        /// </summary>
        public event Action<TDataGridView> RefreshChanged;

        /// <summary>
        /// 构造
        /// </summary>
        public TQuery()
        {
            InitializeComponent();
            this.Size = new Size(205, 32);
        }

        #region 数据加载
        /// <summary>
        /// 绑定TextBox
        /// </summary>
        public void InitData(QQTextBox textbox, List<T> list, int totalCount = 10, int showCount = 3)
        {
            this.TextBox = textbox;
            this.list = list;
            this.totalCount = totalCount;
            this.showCount = showCount;
            textbox.Edit.LostFocus += TextBox_LostFocus;
            textbox.Edit.Click += TextBox_Click;
            textbox.TextChanged += TextBox_TextChanged;
            textbox.KeyDown += TextBox_KeyDown;
            gridview1.RefreshChanged += Gridview1_RefreshChanged;
        }
        private void Gridview1_RefreshChanged()
        {
            RefreshChanged?.Invoke(gridview1);
        }
        private void TextBox_Click(object sender, EventArgs e)
        {
            this.Location = new Point(TextBox.Location.X + 1, TextBox.Location.Y + TextBox.Height - 2);
            this.Width = TextBox.Width - 2;
            this.Visible = !this.Visible;
        }
        private void TextBox_LostFocus(object sender, EventArgs e)
        {
            if (!this.ContainsFocus) this.Visible = false;
        }
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            this.Location = new Point(TextBox.Location.X + 1, TextBox.Location.Y + TextBox.Height - 2);
            this.Width = TextBox.Width - 2;
            string value = this.TextBox.Text.ToLower();
            var tempList = list.AsParallel().Where(Activator.CreateInstance<T>().Find(value));
            InitData(tempList);
            this.Visible = !value.IsNullOrEmpty() && tempList.Count() > 0;
        }
        private void InitData(ParallelQuery<T> list)
        {
            var tempList = new List<T>();
            foreach (var item in list)
            {
                tempList.Add(item);
                if (tempList.Count > this.totalCount) break;
            }
            this.gridview1.DataSource = tempList;

            int count = tempList.Count();
            if (count > this.showCount) count = this.showCount;
            this.Height = this.gridview1.RowTemplate.Height * count + (count == 0 ? 0 : 3);
        }
        /// <summary>
        /// OnLoad
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            gridview1.DoubleClick += Gridview1_DoubleClick;
            this.Validated += I_Language_Validated;
            InitData(list.AsParallel());
        }
        private void I_Language_Validated(object sender, EventArgs e)
        {
            if (this.TextBox != null) this.Visible = this.TextBox.ContainsFocus;
            else this.Visible = false;
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (gridview1.Rows.Count > 0)
                    {
                        this.Visible = true;
                        this.Focus();
                        gridview1.AutoCell(0);
                    }
                    break;
                case Keys.Enter:
                    if (this.Visible)
                    {
                        Gridview1_DoubleClick(this, EventArgs.Empty);
                    }
                    else
                    {
                        SelectedEvent?.Invoke(0);
                    }
                    break;
            }
        }

        #endregion

        #region 返回
        private void Gridview1_DoubleClick(object sender, EventArgs e)
        {
            if (gridview1.CurrentCell != null)
            {
                int index = gridview1.CurrentCell.RowIndex;
                long id = this.gridview1.Rows[index].Cells[nameof(IId.Id)].Value.ToLong();
                SelectedEvent?.Invoke(id);
                this.Visible = false;
            }
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
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion
    }
}
