using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Paway.Helper;

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
        }

        #endregion

        /// <summary>
        ///     页面切换的时候触发
        /// </summary>
        public event EventHandler PageChanged;

        #region 属性

        /// <summary>
        ///     导航栏
        /// </summary>
        [Category("Properties"), Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TPager TPager { get; private set; }

        /// <summary>
        ///     编辑控件
        /// </summary>
        [Category("Properties")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TDataGridView Edit { get; private set; }

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
                BingData();
            }
        }

        /// <summary>
        ///     数据类型
        /// </summary>
        private Type DType;

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

        #region 分页

        /// <summary>
        ///     刷新数据
        /// </summary>
        public void RefreshData()
        {
            BingData();
        }

        private void pager1_PageChanged(object sender, EventArgs e)
        {
            if (PageChanged != null)
            {
                PageChanged(sender, e);
            }
            else
            {
                BingData();
            }
        }

        /// <summary>
        ///     分页加载数据
        /// </summary>
        public void BingData()
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
                Edit.UpdateColumns(DType);
            }
            else if (dataSource is IList)
            {
                var list = dataSource as IList;
                DType = list.GetListType();

                PagerInfo.RecordCount = list.Count;
                var temp = DType.CreateList();

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
                Edit.DataSource = dataSource;
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
            Edit = new TDataGridView();
            Id = new DataGridViewTextBoxColumn();
            TPager = new TPager();
            ((ISupportInitialize)Edit).BeginInit();
            SuspendLayout();
            // 
            // tDataGridView1
            // 
            Edit.Columns.AddRange(Id);
            Edit.Dock = DockStyle.Fill;
            Edit.Location = new Point(0, 0);
            Edit.Name = "Edit";
            Edit.RowTemplate.DefaultCellStyle.ForeColor = Color.Black;
            Edit.RowTemplate.DefaultCellStyle.SelectionBackColor = Color.FromArgb(209, 232, 255);
            Edit.RowTemplate.DefaultCellStyle.SelectionForeColor = Color.Black;
            Edit.RowTemplate.Height = 32;
            Edit.RowTemplate.Resizable = DataGridViewTriState.False;
            Edit.Size = new Size(576, 173);
            Edit.TabIndex = 12;
            // 
            // Id
            // 
            Id.DataPropertyName = "Id";
            Id.HeaderText = "Id";
            Id.Name = "Id";
            Id.ReadOnly = true;
            // 
            // pager1
            // 
            TPager.BackColor = Color.FromArgb(207, 221, 238);
            TPager.Cursor = Cursors.Hand;
            TPager.Dock = DockStyle.Bottom;
            TPager.Location = new Point(0, 173);
            TPager.Name = "TPager";
            TPager.Size = new Size(576, 30);
            TPager.TabIndex = 11;
            // 
            // TDataGridViewPager
            // 
            AutoScaleDimensions = new SizeF(6F, 12F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(Edit);
            Controls.Add(TPager);
            Name = "TDataGridViewPager";
            Size = new Size(576, 203);
            ((ISupportInitialize)Edit).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridViewTextBoxColumn Id;

        #endregion
    }
}