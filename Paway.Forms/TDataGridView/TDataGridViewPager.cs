using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Paway.Helper;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Paway.Forms
{
    /// <summary>
    /// DataGridView分页控件
    /// </summary>
    public class TDataGridViewPager : TControl
    {
        #region 变量
        /// <summary>
        /// 排序列
        /// </summary>
        private int Index;

        private object dataSource; //数据源
        private Type _dataType;
        private Label lbBottom;
        private TPager pager1;
        private TDataGridView gridview1;
        private DataGridViewTextBoxColumn Id;
        private Label lbLeft;
        private Label lbRight;

        #endregion

        #region 属性
        /// <summary>
        /// 边框线(左右下)
        /// </summary>
        private Padding _tPadding = new Padding(1, 0, 1, 1);
        /// <summary>
        /// 边框线(左右下)
        /// </summary>
        [Description("边框线(左右下)")]
        [DefaultValue(typeof(Padding), "1,0,1,1")]
        public Padding TPadding
        {
            get { return _tPadding; }
            set
            {
                _tPadding = value;
                UpdateLine();
            }
        }
        private void UpdateLine()
        {
            lbLeft.Width = _tPadding.Left;
            lbRight.Width = _tPadding.Right;
            lbBottom.Height = _tPadding.Bottom;
            pager1.UpdateLine(_tPadding);
        }

        /// <summary>
        /// 导航栏
        /// </summary>
        [Category("Properties"), Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TPager TPager { get { return pager1; } }
        /// <summary>
        /// 编辑控件
        /// </summary>
        [Category("Properties")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TDataGridView Edit { get { return gridview1; } }

        /// <summary>
        /// 分页信息
        /// </summary>
        [Browsable(false)]
        public PagerInfo PagerInfo
        {
            get { return TPager.PagerInfo; }
        }
        /// <summary>
        /// 分页信息
        /// </summary>
        [Category("分页")]
        [Description("显示分页")]
        [DefaultValue(true)]
        public bool IGroup
        {
            get { return TPager.PagerInfo.IGroup; }
            set { TPager.PagerInfo.IGroup = value; }
        }
        /// <summary>
        /// 获取或设置每页显示的记录
        /// </summary>
        [Category("分页")]
        [Description("获取或设置每页显示的记录")]
        [DefaultValue(20)]
        public int PageSize
        {
            get { return TPager.PagerInfo.PageSize; }
            set { TPager.PagerInfo.PageSize = value; }
        }

        /// <summary>
        /// 获取或设置数据源
        /// </summary>
        [Browsable(false)]
        [Category("Properties")]
        [DefaultValue(null)]
        public virtual object DataSource
        {
            get { return dataSource; }
            set
            {
                UpdateData(value);
            }
        }

        /// <summary>
        /// 数据类型
        /// </summary>
        private Type DataType
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
        /// 获取或设置当前页码
        /// </summary>
        [Browsable(false), Description("获取或设置当前页码")]
        [DefaultValue(1)]
        public int CurrentPageIndex
        {
            get { return PagerInfo.CurrentPageIndex; }
            set { PagerInfo.CurrentPageIndex = value; }
        }

        #endregion

        #region 事件
        /// <summary>
        /// 页面切换前触发
        /// </summary>
        public event Func<bool> PageChanging;
        /// <summary>
        /// 统计消息
        /// </summary>
        public event Func<object, string> TotalEvent;

        #endregion

        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        public TDataGridViewPager()
        {
            InitializeComponent();
            TPager.PageChanged += Pager1_PageChanged;
            this.Edit.CellClick += Gridview1_CellClick;
            this.Edit.ColumnHeaderMouseClick += Gridview1_ColumnHeaderMouseClick;
            UpdateLine();
        }
        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lbBottom = new System.Windows.Forms.Label();
            this.pager1 = new Paway.Forms.TPager();
            this.gridview1 = new Paway.Forms.TDataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lbLeft = new System.Windows.Forms.Label();
            this.lbRight = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridview1)).BeginInit();
            this.SuspendLayout();
            // 
            // lbBottom
            // 
            this.lbBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(204)))), ((int)(((byte)(223)))));
            this.lbBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbBottom.Location = new System.Drawing.Point(0, 202);
            this.lbBottom.Name = "lbBottom";
            this.lbBottom.Size = new System.Drawing.Size(576, 1);
            this.lbBottom.TabIndex = 103;
            // 
            // pager1
            // 
            this.pager1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.pager1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pager1.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.pager1.Location = new System.Drawing.Point(0, 172);
            this.pager1.Name = "pager1";
            this.pager1.Size = new System.Drawing.Size(576, 30);
            this.pager1.TabIndex = 104;
            // 
            // gridview1
            // 
            this.gridview1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id});
            this.gridview1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridview1.Location = new System.Drawing.Point(0, 0);
            this.gridview1.Name = "gridview1";
            this.gridview1.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.gridview1.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.gridview1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
            this.gridview1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridview1.RowTemplate.Height = 32;
            this.gridview1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.gridview1.Size = new System.Drawing.Size(576, 172);
            this.gridview1.TabIndex = 105;
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            // 
            // lbLeft
            // 
            this.lbLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(204)))), ((int)(((byte)(223)))));
            this.lbLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbLeft.Location = new System.Drawing.Point(0, 0);
            this.lbLeft.Name = "lbLeft";
            this.lbLeft.Size = new System.Drawing.Size(1, 172);
            this.lbLeft.TabIndex = 106;
            // 
            // lbRight
            // 
            this.lbRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(204)))), ((int)(((byte)(223)))));
            this.lbRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.lbRight.Location = new System.Drawing.Point(575, 0);
            this.lbRight.Name = "lbRight";
            this.lbRight.Size = new System.Drawing.Size(1, 172);
            this.lbRight.TabIndex = 107;
            // 
            // TDataGridViewPager
            // 
            this.Controls.Add(this.lbRight);
            this.Controls.Add(this.lbLeft);
            this.Controls.Add(this.gridview1);
            this.Controls.Add(this.pager1);
            this.Controls.Add(this.lbBottom);
            this.Name = "TDataGridViewPager";
            this.Size = new System.Drawing.Size(576, 203);
            ((System.ComponentModel.ISupportInitialize)(this.gridview1)).EndInit();
            this.ResumeLayout(false);

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
            SortColumn(sort, column.Name);
        }
        /// <summary>
        /// 自定义排序数据
        /// </summary>
        protected virtual void SortColumn(SortOrder sort, string name)
        {
            if (this.DataSource is IList list)
            {
                List<object> tempList = new List<object>();
                foreach (var item in list)
                {
                    tempList.Add(item);
                }
                var index = this.Index;
                new Action(() =>
                {
                    tempList.Sort(name, sort == SortOrder.Ascending);
                    this.BeginInvoke(new Action(() =>
                    {
                        this.dataSource = tempList;
                        RefreshData();
                        if (index < this.Edit.Columns.Count)
                        {
                            var column = this.Edit.Columns[index];
                            if (column.Visible) column.HeaderCell.SortGlyphDirection = sort;
                        }
                    }));
                }).BeginInvoke(null, null);
            }
            else if (this.DataSource is DataTable)
            {
                var dt = this.DataSource as DataTable;
                this.DataSource = SortColumn(dt, this.Index, sort);
                var column = this.Edit.Columns[this.Index];
                column.HeaderCell.SortGlyphDirection = sort;
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

        #endregion

        #region 公开方法
        /// <summary>
        /// 更新Type
        /// </summary>
        public void UpdateType(Type type)
        {
            if (type != null) DataType = type;
        }
        /// <summary>
        /// 外部设置数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="iRefresh">是否刷新数据，默认True</param>
        public void UpdateData(object value, bool iRefresh = true)
        {
            dataSource = value;
            if (dataSource is IList list)
            {
                DataType = list.GenericType();
            }
            if (iRefresh) RefreshData();
        }
        /// <summary>
        /// 刷新数据(分页加载数据)
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
                var table = dt.Clone();
                var index = PagerInfo.PageSize * (PagerInfo.CurrentPageIndex - 1);
                int count = PagerInfo.IGroup ? (index + PagerInfo.PageSize) : dt.Rows.Count;
                for (var i = index; i < count && i < dt.Rows.Count; i++)
                {
                    table.Rows.Add(dt.Rows[i].ItemArray);
                }
                Edit.DataSource = table;
                Edit.UpdateColumns(DataType);
                Edit.OnRefreshChanged(DataType);
            }
            else if (dataSource is IList list)
            {
                var temp = DataType.GenericList();
                var index = PagerInfo.PageSize * (PagerInfo.CurrentPageIndex - 1);
                int count = PagerInfo.IGroup ? (index + PagerInfo.PageSize) : list.Count;
                for (var i = index; i < count && i < list.Count; i++)
                {
                    temp.Add(list[i]);
                }
                Edit.DataSource = temp;
            }
            else
            {
                Edit.DataSource = dataSource;
            }
            RefreshDesc();
        }
        /// <summary>
        /// 仅更新描述、引发统计事件
        /// </summary>
        internal void RefreshDesc()
        {
            if (dataSource == null) return;
            if (dataSource is DataTable)
            {
                var dt = dataSource as DataTable;
                PagerInfo.RecordCount = dt.Rows.Count;
                TPager.UpdateDesc(TotalEvent?.Invoke(dataSource));
            }
            else if (dataSource is IList list)
            {
                PagerInfo.RecordCount = list.Count;
                TPager.UpdateDesc(TotalEvent?.Invoke(dataSource));
            }
            else
            {
                PagerInfo.RecordCount = 1;
                TPager.UpdateDesc(null);
            }
        }

        /// <summary>
        /// 切换至指定页
        /// </summary>
        public void ToPage(int index)
        {
            if (index > PagerInfo.PageCount)
            {
                index = PagerInfo.PageCount;
            }
            PagerInfo.CurrentPageIndex = index;
        }

        /// <summary>
        /// 切换至最后页
        /// </summary>
        public void ToLastPage()
        {
            PagerInfo.CurrentPageIndex = PagerInfo.PageCount;
        }
        /// <summary>
        /// 自动选中最后一行
        /// </summary>
        public void AutoLast()
        {
            ToLastPage();
            int index = this.Edit.RowCount - 1;
            AutoCell(index);
        }
        /// <summary>
        /// 刷新数据并自动选中焦点
        /// </summary>
        /// <param name="iOffset">保存滚动条位置</param>
        public void AutoCell(bool iOffset = false)
        {
            int index = 0;
            if (this.Edit.CurrentCell != null)
                index = this.Edit.CurrentCell.RowIndex;
            var offset = this.Edit.FirstDisplayedScrollingRowIndex;
            this.RefreshData();
            AutoCell(index);
            if (iOffset) this.Edit.SetOffsetRowIndex(offset);
        }
        /// <summary>
        /// 自动选中焦点
        /// </summary>
        public void AutoCell(int index)
        {
            this.Edit.AutoCell(index);
        }
        /// <summary>
        /// 获取指定名称列
        /// </summary>
        public DataGridViewColumn GetColumn(string name)
        {
            return this.Edit.GetColumn(name);
        }
        /// <summary>
        /// 使用树结构
        /// </summary>
        public bool UserTree()
        {
            if (gridview1 is TreeGridView) return false;
            this.Controls.Remove(this.gridview1);
            gridview1 = new TreeGridView();
            gridview1.Dock = DockStyle.Fill;
            this.Controls.Add(this.gridview1);
            this.Controls.SetChildIndex(this.gridview1, 0);
            IGroup = false;
            return true;
        }

        #endregion

        #region Dispose
        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            if (Id != null)
            {
                Id.Dispose();
                Id = null;
            }
            if (gridview1 != null)
            {
                gridview1.Dispose();
                gridview1 = null;
            }
            if (pager1 != null)
            {
                pager1.Dispose();
                pager1 = null;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}