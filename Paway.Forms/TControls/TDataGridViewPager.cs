﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Paway.Helper;
using System.Collections.Generic;
using System.Reflection;

namespace Paway.Forms
{
    /// <summary>
    ///     DataGridView分页控件
    /// </summary>
    public class TDataGridViewPager : TControl
    {
        #region 构造

        /// <summary>
        ///     构造
        /// </summary>
        public TDataGridViewPager()
        {
            InitializeComponent();
            TPager.PageChanged += Pager1_PageChanged;
            this.Edit.CellClick += Gridview1_CellClick;
            this.Edit.ColumnHeaderMouseClick += Gridview1_ColumnHeaderMouseClick;
        }

        #endregion

        #region 事件
        /// <summary>
        ///     页面切换前触发
        /// </summary>
        public event Func<bool> PageChanging;

        #endregion

        #region 属性
        /// <summary>
        /// 排序列
        /// </summary>
        protected int Index;

        private TPager pager1;
        /// <summary>
        ///     导航栏
        /// </summary>
        [Category("Properties"), Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TPager TPager { get { return pager1; } }

        private TDataGridView tDataGridView1;
        /// <summary>
        ///     编辑控件
        /// </summary>
        [Category("Properties")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TDataGridView Edit { get { return tDataGridView1; } }

        /// <summary>
        ///     分页信息
        /// </summary>
        [Browsable(false)]
        public PagerInfo PagerInfo
        {
            get { return TPager.PagerInfo; }
        }
        /// <summary>
        ///     分页信息
        /// </summary>
        [Category("分页")]
        [Description("显示分页")]
        [DefaultValue(true)]
        public bool IPagerInfo
        {
            get { return TPager.Visible; }
            set
            {
                TPager.Visible = value;
                TPager.PagerInfo.PageSize = value ? 20 : int.MaxValue;
            }
        }
        /// <summary>
        ///     获取或设置每页显示的记录
        /// </summary>
        [Category("分页")]
        [Description("获取或设置每页显示的记录")]
        [DefaultValue(20)]
        public int PageSize
        {
            get { return TPager.PagerInfo.PageSize; }
            set { TPager.PagerInfo.PageSize = value; }
        }

        private object dataSource; //数据源

        /// <summary>
        ///     获取或设置数据源
        /// </summary>
        [Browsable(false)]
        [Category("Properties")]
        [DefaultValue(null)]
        public object DataSource
        {
            get { return dataSource; }
            set
            {
                dataSource = value;
                if (dataSource is IList || dataSource is IEnumerable)
                {
                    DataType = dataSource.GenericType();
                }
                else if (this.DataSource is DataTable)
                {
                    UpdateType(null);
                }
                RefreshData();
            }
        }
        /// <summary>
        /// 更新Type
        /// </summary>
        public virtual void UpdateType(Type type)
        {
            if (type != null)
                DataType = type;
        }
        /// <summary>
        /// 外部设置数据
        /// </summary>
        public void UpdateData(object value)
        {
            dataSource = value;
            RefreshData();
        }

        private Type _dataType;
        /// <summary>
        ///     数据类型
        /// </summary>
        protected Type DataType
        {
            get { return _dataType; }
            set
            {
                if (_dataType == null || value == null || _dataType.Name != value.Name)
                    Edit.Columns.Clear();
                _dataType = value;
            }
        }

        /// <summary>
        ///     获取或设置当前页码
        /// </summary>
        [Browsable(false), Description("获取或设置当前页码")]
        [DefaultValue(1)]
        public int CurrentPageIndex
        {
            get { return PagerInfo.CurrentPageIndex; }
            set { PagerInfo.CurrentPageIndex = value; }
        }

        #endregion

        #region 全文排序
        private void Gridview1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                Index = e.ColumnIndex;
                return;
            }
        }
        private void Gridview1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.DataSource == null) return;
            if (this.Edit.CurrentCell == null) return;
            DataGridViewColumn column = this.Edit.Columns[this.Index];
            if (column.SortMode == DataGridViewColumnSortMode.NotSortable) return;
            var sort = column.HeaderCell.SortGlyphDirection;
            {
                if (sort == SortOrder.None) sort = SortOrder.Ascending;
                else if (sort == SortOrder.Ascending) sort = SortOrder.Descending;
                else if (sort == SortOrder.Descending) sort = SortOrder.Ascending;
            }
            column.HeaderCell.SortGlyphDirection = sort;
            if (!SortColumn(sort, column.Name)) return;
            column = this.Edit.Columns[this.Index];
            column.HeaderCell.SortGlyphDirection = sort;
        }
        /// <summary>
        /// 自定义排序数据
        /// </summary>
        protected virtual bool SortColumn(SortOrder sort, string name)
        {
            if (this.DataSource is IList)
            {
                var list = this.DataSource as IList;
                List<object> tempList = new List<object>();
                for (int i = 0; i < list.Count; i++)
                {
                    tempList.Add(list[i]);
                }
                var properties = DataType.Properties();
                var descriptors = DataType.Descriptors();
                var property = properties.Find(c => c.Name == name);
                var descriptor = descriptors.Find(c => c.Name == name);
                string sorts = sort == SortOrder.Descending ? "desc" : "asc";
                if (property.ISort())
                {
                    tempList.Sort((x, y) => TCompare(descriptor.GetValue(x), descriptor.GetValue(y), sorts));
                }
                else
                {
                    tempList.Sort((x, y) => TCompare(descriptor, x, y, sorts));
                }
                this.dataSource = tempList;
                RefreshData();
            }
            else if (this.DataSource is DataTable)
            {
                var dt = this.DataSource as DataTable;
                SortData(DataType, name, dt, this.Index, sort);
            }
            else return false;
            return true;
        }
        private int TCompare(PropertyDescriptor pro, object obj1, object obj2, string sort)
        {
            int result = pro.TCompare(pro.GetValue(obj1), pro.GetValue(obj2));
            return sort == "asc" ? result : -result;
        }
        private int TCompare(object obj1, object obj2, string sort)
        {
            int result = obj1.TCompare(obj2);
            return sort == "asc" ? result : -result;
        }
        private void SortData(Type type, string name, DataTable dt, int index, SortOrder sort)
        {
            if (type == null) return;
            var properties = type.Properties();
            var property = properties.Find(c => c.Name == name);
            if (property.ISort())
            {
                this.DataSource = SortColumn(dt, index, sort);
            }
            else
            {
                string order = string.Format("{0} {1}", name, sort == SortOrder.Descending ? "desc" : "asc");
                DataRow[] rows = dt.Select(String.Empty, order);
                this.DataSource = rows.CopyToDataTable();
            }
        }
        private DataTable SortColumn(DataTable dt, int index, SortOrder sort)
        {
            Dictionary<int, SortOrder> sortColumns = new Dictionary<int, SortOrder>
            {
                { index, sort }
            };
            RowComparer comp = new RowComparer()
            {
                SortColumns = sortColumns
            };
            var query = dt.AsEnumerable().OrderBy(q => q, comp);
            return query.AsDataView().ToTable();
        }

        #endregion

        #region 分页
        private void Pager1_PageChanged(object sender, EventArgs e)
        {
            if (PageChanging != null)
            {
                if (PageChanging()) return;
            }
            RefreshData();
        }

        /// <summary>
        ///     刷新数据(分页加载数据)
        /// </summary>
        public virtual void RefreshData()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(RefreshData));
                return;
            }
            if (dataSource == null) return;
            if (dataSource is DataTable)
            {
                var dt = dataSource as DataTable;
                PagerInfo.RecordCount = dt.Rows.Count;

                var table = dt.Clone();
                var index = PagerInfo.PageSize * (PagerInfo.CurrentPageIndex - 1);
                for (var i = index; i < index + PagerInfo.PageSize && i < dt.Rows.Count; i++)
                {
                    table.Rows.Add(dt.Rows[i].ItemArray);
                }
                Edit.DataSource = table;
                Edit.UpdateColumns(DataType);
                Edit.OnRefreshChanged(DataType);
            }
            else if (dataSource is IList)
            {
                var list = dataSource as IList;

                PagerInfo.RecordCount = list.Count;
                var dataList = DataType.CreateList();
                var index = PagerInfo.PageSize * (PagerInfo.CurrentPageIndex - 1);
                for (var i = index; i < index + PagerInfo.PageSize && i < list.Count; i++)
                {
                    dataList.Add(list[i]);
                }
                Edit.DataSource = dataList;
            }
            else if (dataSource is IEnumerable)
            {
                UpdateSort(dataSource as IEnumerable);
            }
            else
            {
                PagerInfo.RecordCount = 0;
                Edit.DataSource = dataSource;
            }
        }
        /// <summary>
        /// IEnumerable
        /// </summary>
        protected void UpdateSort(IEnumerable query)
        {
            int i = 0;
            var index = PagerInfo.PageSize * (PagerInfo.CurrentPageIndex - 1);
            var temp = DataType.CreateList();
            foreach (var item in query)
            {
                i++;
                if (i > index && i <= index + PagerInfo.PageSize)
                {
                    temp.Add(item);
                }
            }
            PagerInfo.RecordCount = i;
            Edit.DataSource = temp;
        }

        /// <summary>
        ///     切换至指定页
        /// </summary>
        public void ToCurrentPage(int index)
        {
            if (index > PagerInfo.PageCount)
            {
                index = PagerInfo.PageCount;
            }
            PagerInfo.CurrentPageIndex = index;
        }

        /// <summary>
        ///     切换至最后页
        /// </summary>
        public void ToLastPage()
        {
            PagerInfo.CurrentPageIndex = PagerInfo.PageCount;
        }

        #endregion

        #region 公共方法
        /// <summary>
        /// 自动选中最后焦点
        /// </summary>
        public void AutoLast()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(AutoLast));
                return;
            }
            ToLastPage();
            int index = this.Edit.RowCount - 1;
            AutoCell(index);
        }
        /// <summary>
        /// 自动选中焦点
        /// </summary>
        public void AutoCell()
        {
            int index = 0;
            if (this.Edit.CurrentCell != null)
                index = this.Edit.CurrentCell.RowIndex;
            AutoCell(index);
        }
        /// <summary>
        /// 自动选中焦点
        /// </summary>
        public void AutoCell(int index)
        {
            this.RefreshData();
            this.Edit.AutoCell(index);
        }
        /// <summary>
        /// 获取指定名称列
        /// </summary>
        public DataGridViewColumn GetColumn(string name)
        {
            return this.Edit.GetColumn(name);
        }

        #endregion

        #region 组件设计器生成的代码

        /// <summary>
        ///     必需的设计器变量。
        /// </summary>
        private readonly IContainer components = null;

        /// <summary>
        ///     清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        ///     设计器支持所需的方法 - 不要
        ///     使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tDataGridView1 = new Paway.Forms.TDataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pager1 = new Paway.Forms.TPager();
            ((System.ComponentModel.ISupportInitialize)(this.tDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tDataGridView1
            // 
            this.tDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id});
            this.tDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tDataGridView1.Location = new System.Drawing.Point(0, 0);
            this.tDataGridView1.Name = "tDataGridView1";
            this.tDataGridView1.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.tDataGridView1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(232)))), ((int)(((byte)(255)))));
            this.tDataGridView1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.tDataGridView1.RowTemplate.Height = 32;
            this.tDataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.tDataGridView1.Size = new System.Drawing.Size(576, 173);
            this.tDataGridView1.TabIndex = 12;
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            // 
            // pager1
            // 
            this.pager1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.pager1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pager1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pager1.Location = new System.Drawing.Point(0, 173);
            this.pager1.Name = "pager1";
            this.pager1.Size = new System.Drawing.Size(576, 30);
            this.pager1.TabIndex = 11;
            // 
            // TDataGridViewPager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.Controls.Add(this.tDataGridView1);
            this.Controls.Add(this.pager1);
            this.Name = "TDataGridViewPager";
            this.Size = new System.Drawing.Size(576, 203);
            ((System.ComponentModel.ISupportInitialize)(this.tDataGridView1)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private DataGridViewTextBoxColumn Id;

        #endregion
    }
}