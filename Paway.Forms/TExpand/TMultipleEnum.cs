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
using System.Reflection;
using Paway.Forms.Properties;

namespace Paway.Forms
{
    /// <summary>
    /// 枚举多选下拉框
    /// </summary>
    public partial class TMultipleEnum : MControl
    {
        /// <summary>
        /// 绑定QQTextBox
        /// </summary>
        private QQTextBox TextBox;
        /// <summary>
        /// 显示行数
        /// </summary>
        private int showCount;
        /// <summary>
        /// 绑定数据
        /// </summary>
        private List<MultipleInfo> list;
        /// <summary>
        /// Enum类型
        /// </summary>
        private Type enumType;
        /// <summary>
        /// 双Esc关闭
        /// </summary>
        private bool iExit;
        /// <summary>
        /// 选择事件
        /// </summary>
        public event Action<int> SelectedEvent;
        /// <summary>
        /// 数据刷新后触发
        /// </summary>
        public event Action<TDataGridView> RefreshChanged;

        /// <summary>
        /// 构造
        /// </summary>
        public TMultipleEnum()
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
            InitData(list);
        }

        #region 双击关闭
        private void Gridview1_DoubleClick(object sender, EventArgs e)
        {
            IHide();
        }

        #endregion

        #region 数据加载
        /// <summary>
        /// 绑定TextBox
        /// </summary>
        /// <param name="textbox">TextBox</param>
        /// <param name="value">枚举值</param>
        /// <param name="showCount">控件按指定数量行高度设置</param>
        public void Init(QQTextBox textbox, object value, int showCount = 5)
        {
            this.TextBox = textbox;
            this.showCount = showCount;
            var list = new List<MultipleInfo>();
            this.enumType = value.GetType();
            foreach (var item in Enum.GetValues(enumType))
            {
                var info = new MultipleInfo() { Name = ((Enum)item).Description() };
                if ((int)item != 0 && ((int)value & (int)item) == (int)item) info.Selected = true;
                list.Add(info);
            }
            foreach (var item in list)
            {
                item.Image = item.Selected ? Resources.Controls_accept_16 : Resources.Controls_blank;
            }
            this.list = list;
            UpdateSelected();
            {
                textbox.Parent.Controls.Add(this);
                textbox.Parent.Controls.SetChildIndex(this, 0);
            }
            this.Location = new Point(TextBox.Location.X + 1, TextBox.Location.Y + TextBox.Height - 2);
            this.Width = TextBox.Width - 2;
            textbox.Edit.LostFocus += TextBox_LostFocus;
            textbox.Edit.Click += TextBox_Click;
            textbox.Edit.ReadOnly = true;
            textbox.KeyDown += TextBox_KeyDown;
            gridview1.RowTemplate.Height = 24;
            gridview1.RefreshChanged += Gridview1_RefreshChanged;
            gridview1.CellClick += Gridview1_CellClick;
        }
        private void Gridview1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var id = (int)gridview1.Rows[e.RowIndex].Cells[nameof(IMultiple.Id)].Value;
            var info = list.Find(c => c.Id == id);
            var selected = !info.Selected;
            if (info.Name == TConfig.All)
            {
                foreach (var item in list)
                {
                    item.Selected = selected;
                    item.Image = item.Selected ? Resources.Controls_accept_16 : Resources.Controls_blank;
                }
                foreach (DataGridViewRow item in gridview1.Rows)
                {
                    item.Cells[nameof(IMultiple.Image)].Value = info.Image;
                }
            }
            else
            {
                info.Selected = selected;
                info.Image = info.Selected ? Resources.Controls_accept_16 : Resources.Controls_blank;
                gridview1.Rows[e.RowIndex].Cells[nameof(IMultiple.Image)].Value = info.Image;
            }
            var enumValue = UpdateSelected();
            SelectedEvent?.Invoke(enumValue);
        }
        private int UpdateSelected()
        {
            int enumValue = 0;
            var desc = string.Empty;
            foreach (var item in list)
            {
                if (item.Selected)
                {
                    desc += item.Name + ",";
                    enumValue += enumType.Parse(item.Name);
                }
            }
            if (list.Find(c => c.Name != TConfig.All && !c.Selected) == null) desc = TConfig.All;
            TextBox.Edit.Text = desc.Trim(',');
            return enumValue;
        }
        private void Gridview1_RefreshChanged()
        {
            gridview1.GetColumn(nameof(IMultiple.Image)).Width = 42;
            RefreshChanged?.Invoke(gridview1);
        }
        private void InitData(List<MultipleInfo> list)
        {
            this.gridview1.DataSource = list;
            int count = list.Count;
            if (count > this.showCount) count = this.showCount;
            this.Height = this.gridview1.RowTemplate.Height * count + (count == 0 ? 0 : 3);
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
                InitData(list);
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
