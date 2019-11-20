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
using System.Collections;

namespace Paway.Forms
{
    /// <summary>
    /// 搜索
    /// </summary>
    public partial class TQuery<T> : MControl where T : IId, IFind<T>
    {
        /// <summary>
        /// 绑定QQTextBox
        /// </summary>
        private QQTextBox TextBox;
        private List<T> list;
        /// <summary>
        /// 最大显示数量
        /// </summary>
        private int totalCount;
        /// <summary>
        /// 显示行数
        /// </summary>
        private int showCount;
        /// <summary>
        /// 显示类型
        /// </summary>
        private Type showType;
        /// <summary>
        /// 双Esc关闭
        /// </summary>
        protected bool iExit;
        /// <summary>
        /// 选择事件
        /// </summary>
        public event Action<object> SelectedEvent;
        /// <summary>
        /// 数据刷新后触发
        /// </summary>
        public event Action<TDataGridView> RefreshChanged;

        /// <summary>
        /// 构造
        /// </summary>
        public TQuery()
        {
            InitializeComponent();
            this.Visible = false;
        }
        /// <summary>
        /// OnLoad
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            gridview1.DoubleClick += Gridview1_DoubleClick;
            this.Validated += TQuery_Validated;
            InitData();
        }

        #region 双击选择
        private void Gridview1_DoubleClick(object sender, EventArgs e)
        {
            if (gridview1.CurrentCell != null)
            {
                int index = gridview1.CurrentCell.RowIndex;
                var id = this.gridview1.Rows[index].Cells[nameof(IId.Id)].Value.ToInt();
                SelectedEvent?.Invoke(list.Find(c => c.Id == id));
                IHide();
            }
        }

        #endregion

        #region 数据加载
        /// <summary>
        /// 绑定TextBox
        /// </summary>
        /// <param name="textbox">TextBox</param>
        /// <param name="list">数据源</param>
        /// <param name="obj">按指定实体名称显示</param>
        /// <param name="showCount">控件按指定数量行高度设置</param>
        /// <param name="totalCount">控件最大显示数量</param>
        public void Init(QQTextBox textbox, List<T> list, object obj = null, int showCount = 5, int totalCount = 20)
        {
            this.TextBox = textbox;
            this.list = list;
            this.totalCount = totalCount;
            this.showCount = showCount;
            this.showType = obj != null ? obj.GetType() : null;
            {
                textbox.Parent.Controls.Add(this);
                textbox.Parent.Controls.SetChildIndex(this, 0);
            }
            this.Location = new Point(TextBox.Location.X + 1, TextBox.Location.Y + TextBox.Height - 2);
            this.Width = TextBox.Width - 2;
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
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            var value = this.TextBox.Text.ToLower();
            var count = InitData();
            this.Visible = TextBox.ContainsFocus && !value.IsNullOrEmpty() && count > 0;
        }
        private int InitData()
        {
            string value = this.TextBox.Text.ToLower();
            var searchList = this.list.AsParallel().Where(Activator.CreateInstance<T>().Find(value)).ToList();
            var tempList = new List<T>();
            int count = searchList.Count;
            if (count > this.totalCount) count = this.totalCount;
            for (int i = 0; i < count; i++)
            {
                tempList.Add(searchList[i]);
            }
            this.gridview1.DataSource = tempList;
            if (showType != null)
            {
                var properties = showType.Properties();
                for (int i = 0; i < this.gridview1.Columns.Count; i++)
                {
                    var property = properties.Property(this.gridview1.Columns[i].Name);
                    this.gridview1.Columns[i].Visible = property != null && property.IShow();
                }
            }

            if (count > this.showCount) count = this.showCount;
            this.Height = this.gridview1.RowTemplate.Height * count + (count == 0 ? 0 : 3);
            return searchList.Count;
        }

        #endregion

        #region 显示控制
        private void TextBox_Click(object sender, EventArgs e)
        {
            this.Visible = !this.Visible;
            IShow();
        }
        private void IShow()
        {
            if (this.Visible && this.Height == 0)
            {
                InitData();
            }
        }
        private void TextBox_LostFocus(object sender, EventArgs e)
        {
            if (!this.ContainsFocus) this.Visible = false;
        }
        private void TQuery_Validated(object sender, EventArgs e)
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
