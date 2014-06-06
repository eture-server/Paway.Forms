using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using Paway.Helper;

namespace Paway.Forms
{
    /// <summary>
    /// 在分页属性变动委托
    /// </summary>
    /// <param name="info"></param>
    public delegate void PageInfoChanged(PagerInfo info);

    /// <summary>
    /// 分页工具栏
    /// </summary>
    public partial class TPager : UserControl
    {
        private int m_PageSize = 20;
        private int m_PageCount;
        private int m_RecordCount = 0;
        private int m_CurrentPageIndex = 1;
        private PagerInfo pagerInfo = null;
        /// <summary>
        /// 页面切换的时候触发
        /// </summary>
        public event EventHandler PageChanged;

        #region 分页信息
        /// <summary>
        /// 分页信息
        /// </summary>
        public PagerInfo PagerInfo
        {
            get
            {
                if (pagerInfo == null)
                {
                    pagerInfo = new PagerInfo();
                    pagerInfo.RecordCount = this.RecordCount;
                    pagerInfo.CurrenetPageIndex = this.CurrentPageIndex;
                    pagerInfo.PageSize = this.PageSize;

                    pagerInfo.PageInfoChanged += pagerInfo_PageInfoChanged;
                }
                else
                {
                    pagerInfo.CurrenetPageIndex = this.CurrentPageIndex;
                }

                return pagerInfo;
            }
        }

        void pagerInfo_PageInfoChanged(PagerInfo info)
        {
            this.RecordCount = info.RecordCount;
            this.CurrentPageIndex = info.CurrenetPageIndex;
            this.PageSize = info.PageSize;

            this.InitPageInfo();
        }

        #endregion

        #region 构造
        /// <summary> 
        /// 默认构造函数，设置分页初始信息
        /// </summary>
        public TPager() : this(0, 50) { }
        /// <summary> 
        /// 带参数的构造函数
        /// <param name="pageSize">每页记录数</param>
        /// <param name="recordCount">总记录数</param>
        /// </summary>
        public TPager(int recordCount, int pageSize)
        {
            InitializeComponent();

            this.m_PageSize = pageSize;
            this.m_RecordCount = recordCount;
            this.m_CurrentPageIndex = 1; //默认为第一页
            this.InitPageInfo();
            this.btnFirst.Click += btnFirst_Click;
            this.btnPrevious.Click += btnPrevious_Click;
            this.txtCurrentPage.KeyDown += txtCurrentPage_KeyDown;
            this.btnNext.Click += btnNext_Click;
            this.btnLast.Click += btnLast_Click;
        }

        #endregion

        #region 属性
        /// <summary>
        /// 设置或获取一页中显示的记录数目
        /// </summary>
        [Description("设置或获取一页中显示的记录数目"), DefaultValue(20), Category("分页")]
        public int PageSize
        {
            set
            {
                this.m_PageSize = value;
            }
            get
            {
                return this.m_PageSize;
            }
        }

        /// <summary>
        /// 获取记录总页数
        /// </summary>
        [Description("获取记录总页数"), DefaultValue(0), Category("分页")]
        public int PageCount
        {
            get
            {
                return this.m_PageCount;
            }
        }

        /// <summary>
        /// 设置或获取记录总数
        /// </summary>
        [Description("设置或获取记录总数"), Category("分页")]
        public int RecordCount
        {
            set
            {
                this.m_RecordCount = value;
            }
            get
            {
                return this.m_RecordCount;
            }
        }

        /// <summary>
        /// 当前的页面索引, 开始为1
        /// </summary>
        [Description("当前的页面索引, 开始为1"), DefaultValue(1), Category("分页")]
        [Browsable(false)]
        public int CurrentPageIndex
        {
            set
            {
                this.m_CurrentPageIndex = value;
            }
            get
            {
                return this.m_CurrentPageIndex;
            }
        }

        #endregion

        #region 分页
        /// <summary>
        /// 引发页面变化处理事件
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPageChanged(EventArgs e)
        {
            if (PageChanged != null)
            {
                PageChanged(this, e);
            }
        }

        /// <summary>
        /// 初始化分页信息
        /// </summary>
        /// <param name="info"></param>
        public void InitPageInfo(PagerInfo info)
        {
            InitPageInfo(info.RecordCount, info.PageSize);
        }
        /// <summary> 
        /// 初始化分页信息
        /// <param name="recordCount">总记录数</param>
        /// </summary>
        public void InitPageInfo(int recordCount)
        {
            InitPageInfo(recordCount, 0);
        }
        /// <summary> 
        /// 初始化分页信息
        /// <param name="pageSize">每页记录数</param>
        /// <param name="recordCount">总记录数</param>
        /// </summary>
        public void InitPageInfo(int recordCount, int pageSize)
        {
            this.m_RecordCount = recordCount;
            this.m_PageSize = pageSize;
            this.InitPageInfo();
        }

        /// <summary> 
        /// 初始化分页信息
        /// </summary>
        public void InitPageInfo()
        {
            if (this.m_PageSize < 1)
                this.m_PageSize = 10; //如果每页记录数不正确，即更改为10
            if (this.m_RecordCount < 0)
                this.m_RecordCount = 0; //如果记录总数不正确，即更改为0

            //取得总页数
            if (this.m_RecordCount % this.m_PageSize == 0)
            {
                this.m_PageCount = this.m_RecordCount / this.m_PageSize;
            }
            else
            {
                this.m_PageCount = this.m_RecordCount / this.m_PageSize + 1;
            }

            //设置当前页
            if (this.m_CurrentPageIndex > this.m_PageCount)
            {
                this.m_CurrentPageIndex = this.m_PageCount;
            }
            if (this.m_CurrentPageIndex < 1)
            {
                this.m_CurrentPageIndex = 1;
            }

            //设置按钮的可用性
            bool enable = (this.CurrentPageIndex > 1);
            this.btnPrevious.Enabled = enable;

            enable = (this.CurrentPageIndex < this.PageCount);
            this.btnNext.Enabled = enable;

            this.txtCurrentPage.Text = this.m_CurrentPageIndex.ToString();
            this.lblPageInfo.Text = string.Format("共 {0} 条记录，每页 {1} 条，共 {2} 页", this.m_RecordCount, this.m_PageSize, this.m_PageCount);
        }

        /// <summary>
        /// 刷新页面数据
        /// </summary>
        /// <param name="page">页码</param>
        public void RefreshData(int page)
        {
            this.m_CurrentPageIndex = page;
            OnPageChanged(EventArgs.Empty);
        }

        private void btnFirst_Click(object sender, System.EventArgs e)
        {
            this.RefreshData(1);
        }

        private void btnPrevious_Click(object sender, System.EventArgs e)
        {
            if (this.m_CurrentPageIndex > 1)
            {
                this.RefreshData(this.m_CurrentPageIndex - 1);
            }
            else
            {
                this.RefreshData(1);
            }
        }

        private void btnNext_Click(object sender, System.EventArgs e)
        {
            if (this.m_CurrentPageIndex < this.m_PageCount)
            {
                this.RefreshData(this.m_CurrentPageIndex + 1);
            }
            else if (this.m_PageCount < 1)
            {
                this.RefreshData(1);
            }
            else
            {
                this.RefreshData(this.m_PageCount);
            }
        }

        private void btnLast_Click(object sender, System.EventArgs e)
        {
            if (this.m_PageCount > 0)
            {
                this.RefreshData(this.m_PageCount);
            }
            else
            {
                this.RefreshData(1);
            }
        }

        private void txtCurrentPage_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int num = this.txtCurrentPage.Text.ToInt();

                if (num > this.m_PageCount)
                    num = this.m_PageCount;
                if (num < 1)
                    num = 1;

                this.RefreshData(num);
            }
        }

        #endregion

        #region 设计器支持所需的方法
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TPager));
            this.lblPageInfo = new System.Windows.Forms.Label();
            this.btnFirst = new Paway.Forms.QQButton();
            this.txtCurrentPage = new Paway.Forms.TNumTestBox();
            this.btnPrevious = new Paway.Forms.QQButton();
            this.btnNext = new Paway.Forms.QQButton();
            this.btnLast = new Paway.Forms.QQButton();
            this.SuspendLayout();
            // 
            // lblPageInfo
            // 
            this.lblPageInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPageInfo.AutoSize = true;
            this.lblPageInfo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPageInfo.ForeColor = System.Drawing.Color.Black;
            this.lblPageInfo.Location = new System.Drawing.Point(358, 7);
            this.lblPageInfo.Name = "lblPageInfo";
            this.lblPageInfo.Size = new System.Drawing.Size(220, 14);
            this.lblPageInfo.TabIndex = 0;
            this.lblPageInfo.Text = "共 {0} 条记录，每页 {1} 条，共 {2} 页";
            // 
            // btnFirst
            // 
            this.btnFirst.DownImage = ((System.Drawing.Image)(resources.GetObject("btnFirst.DownImage")));
            this.btnFirst.ForeColor = System.Drawing.Color.Black;
            this.btnFirst.Image = null;
            this.btnFirst.Location = new System.Drawing.Point(21, 1);
            this.btnFirst.MoveImage = ((System.Drawing.Image)(resources.GetObject("btnFirst.MoveImage")));
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnFirst.NormalImage")));
            this.btnFirst.Size = new System.Drawing.Size(33, 28);
            this.btnFirst.TabIndex = 5;
            this.btnFirst.Text = "|<";
            this.btnFirst.UseVisualStyleBackColor = false;
            // 
            // txtCurrentPage
            // 
            this.txtCurrentPage.Icon = null;
            this.txtCurrentPage.IsPasswordChat = '\0';
            this.txtCurrentPage.Lines = new string[] {
        "1"};
            this.txtCurrentPage.Location = new System.Drawing.Point(85, 3);
            this.txtCurrentPage.MaxLength = 10;
            this.txtCurrentPage.Name = "txtCurrentPage";
            this.txtCurrentPage.SelectedText = "";
            this.txtCurrentPage.Size = new System.Drawing.Size(28, 24);
            this.txtCurrentPage.TabIndex = 1;
            this.txtCurrentPage.WaterText = "";
            // 
            // btnPrevious
            // 
            this.btnPrevious.DownImage = ((System.Drawing.Image)(resources.GetObject("btnPrevious.DownImage")));
            this.btnPrevious.ForeColor = System.Drawing.Color.Black;
            this.btnPrevious.Image = null;
            this.btnPrevious.Location = new System.Drawing.Point(53, 1);
            this.btnPrevious.MoveImage = ((System.Drawing.Image)(resources.GetObject("btnPrevious.MoveImage")));
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnPrevious.NormalImage")));
            this.btnPrevious.Size = new System.Drawing.Size(33, 28);
            this.btnPrevious.TabIndex = 4;
            this.btnPrevious.Text = "<";
            this.btnPrevious.UseVisualStyleBackColor = false;
            // 
            // btnNext
            // 
            this.btnNext.DownImage = ((System.Drawing.Image)(resources.GetObject("btnNext.DownImage")));
            this.btnNext.ForeColor = System.Drawing.Color.Black;
            this.btnNext.Image = null;
            this.btnNext.Location = new System.Drawing.Point(113, 1);
            this.btnNext.MoveImage = ((System.Drawing.Image)(resources.GetObject("btnNext.MoveImage")));
            this.btnNext.Name = "btnNext";
            this.btnNext.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnNext.NormalImage")));
            this.btnNext.Size = new System.Drawing.Size(33, 28);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = false;
            // 
            // btnLast
            // 
            this.btnLast.DownImage = ((System.Drawing.Image)(resources.GetObject("btnLast.DownImage")));
            this.btnLast.ForeColor = System.Drawing.Color.Black;
            this.btnLast.Image = null;
            this.btnLast.Location = new System.Drawing.Point(145, 1);
            this.btnLast.MoveImage = ((System.Drawing.Image)(resources.GetObject("btnLast.MoveImage")));
            this.btnLast.Name = "btnLast";
            this.btnLast.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnLast.NormalImage")));
            this.btnLast.Size = new System.Drawing.Size(33, 28);
            this.btnLast.TabIndex = 3;
            this.btnLast.Text = ">|";
            this.btnLast.UseVisualStyleBackColor = false;
            // 
            // TPager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.Controls.Add(this.btnLast);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrevious);
            this.Controls.Add(this.txtCurrentPage);
            this.Controls.Add(this.btnFirst);
            this.Controls.Add(this.lblPageInfo);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Name = "TPager";
            this.Size = new System.Drawing.Size(606, 30);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPageInfo;
        private Paway.Forms.QQButton btnFirst;
        private Paway.Forms.TNumTestBox txtCurrentPage;
        private Paway.Forms.QQButton btnPrevious;
        private Paway.Forms.QQButton btnNext;
        private Paway.Forms.QQButton btnLast;

        #endregion
    }

    /// <summary>
    /// 分页属性
    /// </summary>
    [Serializable]
    [DataContract]
    public class PagerInfo
    {
        /// <summary>
        /// 在分页属性变动时发生
        /// </summary>
        public event PageInfoChanged PageInfoChanged;

        private int currenetPageIndex; //当前页码
        private int pageSize;//每页显示的记录
        private int recordCount;//记录总数

        #region 属性变量

        /// <summary>
        /// 获取或设置当前页码
        /// </summary>
        [DataMember]
        public int CurrenetPageIndex
        {
            get { return currenetPageIndex; }
            set
            {
                currenetPageIndex = value;

                if (PageInfoChanged != null)
                {
                    PageInfoChanged(this);
                }
            }
        }

        /// <summary>
        /// 获取或设置每页显示的记录
        /// </summary>
        [DataMember]
        public int PageSize
        {
            get { return pageSize; }
            set
            {
                pageSize = value;
                if (PageInfoChanged != null)
                {
                    PageInfoChanged(this);
                }
            }
        }

        /// <summary>
        /// 获取或设置记录总数
        /// </summary>
        [DataMember]
        public int RecordCount
        {
            get { return recordCount; }
            set
            {
                recordCount = value;
                if (PageInfoChanged != null)
                {
                    PageInfoChanged(this);
                }
            }
        }

        #endregion
    }
}
