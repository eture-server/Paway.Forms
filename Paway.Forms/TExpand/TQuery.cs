﻿using System;
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
    public partial class TQuery<T> : TCustom where T : IId, IFind<T>
    {
        private List<T> list;
        /// <summary>
        /// 最大显示数量
        /// </summary>
        private int totalCount;
        /// <summary>
        /// 显示类型
        /// </summary>
        private Type showType;
        /// <summary>
        /// 选择事件
        /// </summary>
        public event Action<object> SelectedEvent;
        /// <summary>
        /// 搜索结果过滤事件
        /// </summary>
        public event Action<List<T>> SearchFilterEvent;

        /// <summary>
        /// 构造
        /// </summary>
        public TQuery()
        {
            InitializeComponent();
            this.gridview1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridview1.ColumnHeadersVisible = false;
            this.Visible = false;
        }

        #region 重载
        /// <summary>
        /// OnLoad
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadData();
        }
        /// <summary>
        /// 显示
        /// </summary>
        internal override void IShow()
        {
            if (this.Visible && this.Height == 0)
            {
                LoadData();
            }
        }
        /// <summary>
        /// 双击选择并关闭
        /// </summary>
        internal override void Gridview1_DoubleClick(object sender, EventArgs e)
        {
            if (gridview1.CurrentCell != null)
            {
                int index = gridview1.CurrentCell.RowIndex;
                var id = this.gridview1.Rows[index].Cells[gridview1.IdColumn()].Value.ToInt();
                SelectedEvent?.Invoke(list.Find(c => c.Id == id));
                base.Gridview1_DoubleClick(sender, e);
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
            this.list = list;
            this.totalCount = totalCount;
            this.showCount = showCount;
            this.showType = obj?.GetType();
            {
                textbox.Parent.Controls.Add(this);
                textbox.Parent.Controls.SetChildIndex(this, 0);
            }
            base.Init(textbox, gridview1);
            textbox.TextChanged += TextBox_TextChanged;
        }
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            var value = this.TextBox.Text.ToLower();
            var count = LoadData();
            this.Visible = TextBox.ContainsFocus && !value.IsNullOrEmpty() && count > 0;
            if (gridview1 is TreeGridView treeView1)
            {
                if (!value.IsNullOrEmpty()) treeView1.ExpandAll();
            }
        }
        private int LoadData()
        {
            string value = this.TextBox.Text.ToLower();
            var searchList = this.list.AsParallel().Where(Activator.CreateInstance<T>().Find(value)).ToList();
            SearchFilterEvent?.Invoke(searchList);
            var tempList = new List<T>();
            int count = searchList.Count;
            if (count > this.totalCount && this.totalCount != 0) count = this.totalCount;
            for (int i = 0; i < count; i++)
            {
                tempList.Add(searchList[i]);
            }
            if (showType != null && gridview1 is TreeGridView treeView1)
            {
                var properties = showType.PropertiesCache();
                treeView1.TextColumn = properties[0].Name;
            }
            this.gridview1.DataSource = tempList;
            if (showType != null)
            {
                var properties = showType.PropertiesCache();
                for (int i = 0; i < this.gridview1.Columns.Count; i++)
                {
                    var property = properties.Property(this.gridview1.Columns[i].Name);
                    this.gridview1.Columns[i].Visible = property != null && property.IShow();
                }
            }

            ShowHeight(count);
            return searchList.Count;
        }

        #endregion
    }
}
