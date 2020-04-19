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
    public partial class TMultipleEnum : TCustom
    {
        /// <summary>
        /// 绑定数据
        /// </summary>
        private List<MultipleInfo> list;
        /// <summary>
        /// Enum类型
        /// </summary>
        private Type enumType;
        /// <summary>
        /// 选择事件
        /// </summary>
        public event Action<int> SelectedEvent;

        /// <summary>
        /// 构造
        /// </summary>
        public TMultipleEnum()
        {
            InitializeComponent();
            this.Visible = false;
        }

        #region 重载
        /// <summary>
        /// OnLoad
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitData(list);
        }
        /// <summary>
        /// 显示
        /// </summary>
        internal override void IShow()
        {
            if (this.Visible && this.Height == 0)
            {
                InitData(list);
            }
        }
        internal override void Gridview1_RefreshChanged(TDataGridView gridview1)
        {
            gridview1.GetColumn(nameof(IMultiple.Image)).Width = 42;
        }

        #endregion

        #region 数据加载
        /// <summary>
        /// 绑定TextBox
        /// </summary>
        /// <param name="textbox">TextBox</param>
        /// <param name="value">枚举值</param>
        /// <param name="action">过滤方法</param>
        /// <param name="count">控件按指定数量行高度设置</param>
        public void Init<T>(QQTextBox textbox, T value, Func<T, bool> action = null, int count = 5)
        {
            this.showCount = count;
            var list = new List<MultipleInfo>();
            this.enumType = typeof(T);
            var valueInt = enumType.Parse(value.ToString());
            foreach (var field in enumType.GetFields(TConfig.Flags))
            {
                var item = (T)field.GetRawConstantValue();
                if (action?.Invoke(item) == true) continue;
                var info = new MultipleInfo() { Name = field.Description() };
                var itemInt = (int)field.GetRawConstantValue();
                if (itemInt != 0 && (itemInt & valueInt) == itemInt) info.Selected = true;
                list.Add(info);
            }
            foreach (var item in list)
            {
                item.Image = item.Selected ? Resources.Controls_accept_16 : Resources.Controls_blank;
            }
            this.list = list;
            {
                textbox.Parent.Controls.Add(this);
                textbox.Parent.Controls.SetChildIndex(this, 0);
            }
            base.Init(textbox, gridview1);
            UpdateSelected();
            textbox.Edit.ReadOnly = true;
            gridview1.RowTemplate.Height = 24;
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
        private void InitData(List<MultipleInfo> list)
        {
            this.gridview1.DataSource = list;
            int count = list.Count;
            ShowHeight(count);
        }

        #endregion

    }
}
