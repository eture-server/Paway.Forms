using System;
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
            TPager.PageChanged += pager1_PageChanged;
            this.Edit.CellClick += gridview1_CellClick;
            this.Edit.ColumnHeaderMouseClick += gridview1_ColumnHeaderMouseClick;
        }

        #endregion

        /// <summary>
        ///     页面切换的时候触发
        /// </summary>
        public event EventHandler PageChanged;

        #region 属性
        private bool _isort = true;
        /// <summary>
        /// 是否排序
        /// </summary>
        [Browsable(false), DefaultValue(true)]
        public bool ISort { get { return _isort; } set { _isort = value; } }
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
        [Category("Properties")]
        public PagerInfo PagerInfo
        {
            get { return TPager.PagerInfo; }
        }

        private object dataSource; //数据源

        /// <summary>
        ///     获取或设置数据源
        /// </summary>
        [Category("Properties"), DefaultValue(null)]
        public object DataSource
        {
            get { return dataSource; }
            set
            {
                if (Edit.Columns != null)
                {
                    Edit.Columns.Clear();
                }
                dataSource = value;
                if (dataSource is IList)
                {
                    var list = dataSource as IList;
                    DataType = list.GetListType();
                }
                else if (this.DataSource is DataTable)
                {
                    UpdateType(null);
                }
                else
                {
                    DataType = null;
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

        /// <summary>
        ///     数据类型
        /// </summary>
        protected Type DataType { get; set; }

        /// <summary>
        ///     获取或设置当前页码
        /// </summary>
        [Browsable(false), Description("获取或设置当前页码"), DefaultValue(1)]
        public int CurrentPageIndex
        {
            get { return PagerInfo.CurrentPageIndex; }
            set { PagerInfo.CurrentPageIndex = value; }
        }

        #endregion

        #region 全文排序
        private void gridview1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                Index = e.ColumnIndex;
                return;
            }
        }
        private void gridview1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (ISort) SortColumn();
        }
        /// <summary>
        /// 自定义排序
        /// </summary>
        protected void SortColumn()
        {
            if (this.DataSource == null) return;
            if (this.Edit.CurrentCell == null) return;
            DataGridViewColumn column = this.Edit.Columns[this.Index];
            var sort = column.HeaderCell.SortGlyphDirection;
            {
                if (sort == SortOrder.None) sort = SortOrder.Ascending;
                else if (sort == SortOrder.Ascending) sort = SortOrder.Descending;
                else if (sort == SortOrder.Descending) sort = SortOrder.Ascending;
            }
            column.HeaderCell.SortGlyphDirection = sort;
            if (this.DataSource is IList)
            {
                var list = this.DataSource as IList;
                List<object> tempList = new List<object>();
                for (int i = 0; i < list.Count; i++)
                {
                    tempList.Add(list[i]);
                }
                var properties = TypeDescriptor.GetProperties(DataType);
                PropertyDescriptor pro = null;
                for (int i = 0; i < properties.Count; i++)
                {
                    if (properties[i].Name == column.Name)
                        pro = properties[i];
                }
                string sorts = sort == SortOrder.Descending ? "desc" : "asc";
                if (DataType.GetProperty(column.Name).ISort())
                {
                    tempList.Sort((x, y) => TCompare(pro.GetValue(x), pro.GetValue(y), sorts));
                }
                else
                {
                    tempList.Sort((x, y) => TCompare(pro, x, y, sorts));
                }
                this.dataSource = tempList;
                RefreshData();
            }
            else if (this.DataSource is DataTable)
            {
                var dt = this.DataSource as DataTable;
                SortData(DataType, column.Name, dt, this.Index, sort);
            }
            else return;
            column = this.Edit.Columns[this.Index];
            column.HeaderCell.SortGlyphDirection = sort;
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
            var pro = type.GetProperty(name);
            if (pro.ISort())
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
            Dictionary<int, SortOrder> sortColumns = new Dictionary<int, SortOrder>();
            sortColumns.Add(index, sort);
            RowComparer comp = new RowComparer();
            comp.SortColumns = sortColumns;
            var query = dt.AsEnumerable().OrderBy(q => q, comp);
            return query.AsDataView().ToTable();
        }

        #endregion

        #region 分页
        private void pager1_PageChanged(object sender, EventArgs e)
        {
            if (PageChanged != null)
            {
                PageChanged(sender, e);
            }
            else
            {
                RefreshData();
            }
        }

        /// <summary>
        ///     刷新数据(分页加载数据)
        /// </summary>
        public void RefreshData()
        {
            if (dataSource == null) return;
            if (dataSource is DataTable)
            {
                var dt = dataSource as DataTable;
                PagerInfo.RecordCount = dt.Rows.Count;

                var temp = dt.Clone();
                var index = PagerInfo.PageSize * (PagerInfo.CurrentPageIndex - 1);
                for (var i = index; i < index + PagerInfo.PageSize; i++)
                {
                    if (i >= dt.Rows.Count) break;
                    temp.Rows.Add(dt.Rows[i].ItemArray);
                }
                Edit.DataSource = temp;
                Edit.UpdateColumns(DataType);
            }
            else if (dataSource is IList)
            {
                var list = dataSource as IList;

                PagerInfo.RecordCount = list.Count;
                var temp = DataType.CreateList();

                var index = PagerInfo.PageSize * (PagerInfo.CurrentPageIndex - 1);
                for (var i = index; i < index + PagerInfo.PageSize; i++)
                {
                    if (i >= list.Count) break;
                    temp.Add(list[i]);
                }
                Edit.DataSource = temp;
            }
            else
            {
                PagerInfo.RecordCount = 0;
                Edit.DataSource = dataSource;
            }
            UpdateColumnsSortMode(DataType);
        }
        /// <summary>
        ///     更新列排序模式
        /// </summary>
        private void UpdateColumnsSortMode(Type type)
        {
            if (type == null || type == typeof(string) || type.IsValueType) return;
            for (var i = 0; i < Edit.Columns.Count; i++)
            {
                Edit.Columns[i].SortMode = DataGridViewColumnSortMode.Programmatic;
            }
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