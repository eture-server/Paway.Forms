using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Paway.Helper;

namespace Paway.Forms
{
    /// <summary>
    /// DataGridView分页控件
    /// </summary>
    public partial class TDataGridViewPager : TControl
    {
        /// <summary>
        /// 页面切换的时候触发
        /// </summary>
        public event EventHandler PageChanged;

        #region 属性
        /// <summary>
        /// 导航栏
        /// </summary>
        [Category("Properties"), Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TPager TPager { get { return pager1; } }
        private TDataGridView tDataGridView1;
        /// <summary>
        /// 编辑控件
        /// </summary>
        [Category("Properties")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TDataGridView Edit { get { return this.tDataGridView1; } }

        /// <summary>
        /// 分页信息
        /// </summary>
        [Category("Properties")]
        public PagerInfo PagerInfo { get { return this.pager1.PagerInfo; } }

        private object dataSource;//数据源
        /// <summary>
        /// 获取或设置数据源
        /// </summary>
        [Category("Properties"), DefaultValue(null)]
        public object DataSource
        {
            get { return dataSource; }
            set
            {
                if (this.tDataGridView1.Columns != null)
                {
                    this.tDataGridView1.Columns.Clear();
                }
                dataSource = value;
                BingData();
            }
        }
        /// <summary>
        /// 数据类型
        /// </summary>
        private Type DType;

        /// <summary>
        /// 获取或设置当前页码
        /// </summary>
        [Browsable(false), Description("获取或设置当前页码")]
        public int CurrenetPageIndex
        {
            get { return PagerInfo.CurrentPageIndex; }
            set { PagerInfo.CurrentPageIndex = value; }
        }

        #endregion

        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        public TDataGridViewPager()
        {
            InitializeComponent();
            this.pager1.PageChanged += pager1_PageChanged;
        }

        #endregion

        #region 分页
        /// <summary>
        /// 刷新数据
        /// </summary>
        public void RefreshData()
        {
            BingData();
        }

        void pager1_PageChanged(object sender, EventArgs e)
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
        /// 分页加载数据
        /// </summary>
        public void BingData()
        {
            if (dataSource == null) return;
            if (dataSource is DataTable)
            {
                DataTable dt = dataSource as DataTable;
                PagerInfo.RecordCount = dt.Rows.Count;

                DataTable temp = dt.Clone();
                int index = PagerInfo.PageSize * (PagerInfo.CurrentPageIndex - 1);
                for (int i = index; i < index + PagerInfo.PageSize; i++)
                {
                    if (i >= dt.Rows.Count) break;
                    temp.Rows.Add(dt.Rows[i].ItemArray);
                }
                this.tDataGridView1.DataSource = temp;
                this.tDataGridView1.UpdateColumns(this.DType);
            }
            else if (dataSource is IList)
            {
                IList list = dataSource as IList;
                this.DType = list.GetListType();

                PagerInfo.RecordCount = list.Count;
                IList temp = DType.CreateList();

                int index = PagerInfo.PageSize * (PagerInfo.CurrentPageIndex - 1);
                for (int i = index; i < index + PagerInfo.PageSize; i++)
                {
                    if (i >= list.Count) break;
                    temp.Add(list[i]);
                }
                this.tDataGridView1.DataSource = temp;
            }
            else
            {
                this.tDataGridView1.DataSource = dataSource;
            }
        }

        /// <summary>
        /// 切换至指定页
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
        /// 切换至最后页
        /// </summary>
        public void ToLastPage()
        {
            PagerInfo.CurrentPageIndex = PagerInfo.PageCount;
        }

        #endregion

        #region 组件设计器生成的代码
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
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
            this.tDataGridView1.GridColor = System.Drawing.Color.LightBlue;
            this.tDataGridView1.IMove = false;
            this.tDataGridView1.IMoveColor = System.Drawing.Color.Empty;
            this.tDataGridView1.Location = new System.Drawing.Point(0, 0);
            this.tDataGridView1.Name = "tDataGridView1";
            this.tDataGridView1.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.tDataGridView1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
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
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tDataGridView1);
            this.Controls.Add(this.pager1);
            this.Name = "TDataGridViewPager";
            this.Size = new System.Drawing.Size(576, 203);
            ((System.ComponentModel.ISupportInitialize)(this.tDataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TPager pager1;
        private DataGridViewTextBoxColumn Id;

        #endregion
    }
}
