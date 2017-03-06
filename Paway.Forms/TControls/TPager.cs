using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;
using Paway.Helper;

namespace Paway.Forms
{
    /// <summary>
    ///     在分页属性变动委托
    /// </summary>
    /// <param name="info"></param>
    public delegate void PageInfoChanged(PagerInfo info);

    /// <summary>
    ///     分页工具栏
    /// </summary>
    public class TPager : TControl
    {
        private PagerInfo pagerInfo;
        private ToolBar toolFirst;
        private ToolBar toolLast;
        private ToolBar toolNext;
        private ToolBar toolPrevious;

        /// <summary>
        ///     页面切换的时候触发
        /// </summary>
        public event EventHandler PageChanged;

        #region 分页信息

        /// <summary>
        ///     分页信息
        /// </summary>
        public PagerInfo PagerInfo
        {
            get
            {
                if (pagerInfo == null)
                {
                    pagerInfo = new PagerInfo();
                    pagerInfo.PageInfoChanged += pagerInfo_PageInfoChanged;
                }
                return pagerInfo;
            }
        }

        private void pagerInfo_PageInfoChanged(PagerInfo info)
        {
            InitPageInfo(info.RecordCount, info.PageSize);
        }

        #endregion

        #region 构造

        /// <summary>
        ///     默认构造函数，设置分页初始信息
        /// </summary>
        public TPager()
            : this(0, 50)
        {
        }

        /// <summary>
        ///     带参数的构造函数
        ///     <param name="pageSize">每页记录数</param>
        ///     <param name="recordCount">总记录数</param>
        /// </summary>
        public TPager(int recordCount, int pageSize)
        {
            InitializeComponent();

            PagerInfo.PageSize = pageSize;
            PagerInfo.RecordCount = recordCount;
            PagerInfo.CurrentPageIndex = 1; //默认为第一页
            InitPageInfo();
            toolFirst.ItemClick += toolFirst_ItemClick;
            toolPrevious.ItemClick += toolPrevious_ItemClick;
            txtCurrentPage.Edit.GotFocus += txtCurrentPage_GotFocus;
            txtCurrentPage.Edit.LostFocus += txtCurrentPage_LostFocus;
            txtCurrentPage.KeyDown += txtCurrentPage_KeyDown;

            toolNext.ItemClick += toolNext_ItemClick;
            toolLast.ItemClick += toolLast_ItemClick;
        }

        private void txtCurrentPage_LostFocus(object sender, EventArgs e)
        {
            txtCurrentPage.IsTrans = true;
        }

        private void txtCurrentPage_GotFocus(object sender, EventArgs e)
        {
            txtCurrentPage.IsTrans = false;
        }

        #endregion

        #region 分页

        /// <summary>
        ///     引发页面变化处理事件
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
        ///     更新分页信息
        ///     <param name="pageSize">每页记录数</param>
        ///     <param name="recordCount">总记录数</param>
        /// </summary>
        public void InitPageInfo(int recordCount, int pageSize)
        {
            PagerInfo.RecordCount = recordCount;
            PagerInfo.PageSize = pageSize;
            InitPageInfo();
        }

        /// <summary>
        ///     更新分页信息
        /// </summary>
        public void InitPageInfo()
        {
            if (PagerInfo.PageSize < 1)
                PagerInfo.PageSize = 10; //如果每页记录数不正确，即更改为10
            if (PagerInfo.RecordCount < 0)
                PagerInfo.RecordCount = 0; //如果记录总数不正确，即更改为0

            //设置当前页
            if (PagerInfo.CurrentPageIndex > PagerInfo.PageCount)
            {
                PagerInfo.CurrentPageIndex = PagerInfo.PageCount;
            }
            if (PagerInfo.CurrentPageIndex < 1)
            {
                PagerInfo.CurrentPageIndex = 1;
            }

            //设置按钮的可用性
            var enable = PagerInfo.CurrentPageIndex > 1;
            toolPrevious.Enabled = enable;

            enable = PagerInfo.CurrentPageIndex < PagerInfo.PageCount;
            toolNext.Enabled = enable;

            txtCurrentPage.Text = PagerInfo.CurrentPageIndex.ToString();
            OnPageChanged(EventArgs.Empty);
            lblPageInfo.Text = string.Format("共 {0} 条记录，每页 {1} 条，共 {2} 页", PagerInfo.RecordCount, PagerInfo.PageSize,
                PagerInfo.PageCount);
        }

        #region 当前页更新

        /// <summary>
        ///     刷新页面数据
        /// </summary>
        /// <param name="page">页码</param>
        public void RefreshData(int page)
        {
            PagerInfo.CurrentPageIndex = page;
        }

        private void toolFirst_ItemClick(object sender, EventArgs e)
        {
            RefreshData(1);
        }

        private void toolPrevious_ItemClick(object sender, EventArgs e)
        {
            if (PagerInfo.CurrentPageIndex > 1)
            {
                RefreshData(PagerInfo.CurrentPageIndex - 1);
            }
            else
            {
                RefreshData(1);
            }
        }

        private void toolNext_ItemClick(object sender, EventArgs e)
        {
            if (PagerInfo.CurrentPageIndex < PagerInfo.PageCount)
            {
                RefreshData(PagerInfo.CurrentPageIndex + 1);
            }
            else if (PagerInfo.PageCount < 1)
            {
                RefreshData(1);
            }
            else
            {
                RefreshData(PagerInfo.PageCount);
            }
        }

        private void toolLast_ItemClick(object sender, EventArgs e)
        {
            if (PagerInfo.PageCount > 0)
            {
                RefreshData(PagerInfo.PageCount);
            }
            else
            {
                RefreshData(1);
            }
        }

        private void txtCurrentPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var num = txtCurrentPage.Text.ToInt();

                if (num > PagerInfo.PageCount)
                    num = PagerInfo.PageCount;
                if (num < 1)
                    num = 1;

                RefreshData(num);
            }
        }

        #endregion

        #endregion

        #region 设计器支持所需的方法

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
            Paway.Forms.ToolItem toolItem1 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem2 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem3 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem4 = new Paway.Forms.ToolItem();
            this.lblPageInfo = new System.Windows.Forms.Label();
            this.txtCurrentPage = new Paway.Forms.TNumTestBox();
            this.toolLast = new Paway.Forms.ToolBar();
            this.toolNext = new Paway.Forms.ToolBar();
            this.toolPrevious = new Paway.Forms.ToolBar();
            this.toolFirst = new Paway.Forms.ToolBar();
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
            // txtCurrentPage
            // 
            this.txtCurrentPage.ForeColor = System.Drawing.Color.Black;
            this.txtCurrentPage.Icon = null;
            this.txtCurrentPage.IsPasswordChat = '\0';
            this.txtCurrentPage.IsTrans = true;
            this.txtCurrentPage.Lines = new string[] {
        "1"};
            this.txtCurrentPage.Location = new System.Drawing.Point(86, 3);
            this.txtCurrentPage.MaxLength = 10;
            this.txtCurrentPage.Name = "txtCurrentPage";
            this.txtCurrentPage.SelectedText = "";
            this.txtCurrentPage.Size = new System.Drawing.Size(28, 25);
            this.txtCurrentPage.TabIndex = 1;
            // 
            // toolLast
            // 
            this.toolLast.ICheckEvent = true;
            this.toolLast.ImageSize = new System.Drawing.Size(0, 0);
            toolItem1.Text = ">|";
            this.toolLast.Items.Add(toolItem1);
            this.toolLast.ItemSize = new System.Drawing.Size(30, 24);
            this.toolLast.ItemSpace = 5;
            this.toolLast.Location = new System.Drawing.Point(146, 1);
            this.toolLast.Name = "toolLast";
            this.toolLast.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.toolLast.Size = new System.Drawing.Size(30, 28);
            this.toolLast.TabIndex = 44;
            this.toolLast.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(94)))), ((int)(((byte)(167)))));
            this.toolLast.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(171)))), ((int)(((byte)(244)))));
            this.toolLast.TBackGround.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.toolLast.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolLast.TextFirst.ColorMove = System.Drawing.Color.White;
            this.toolLast.TextFirst.ColorNormal = System.Drawing.Color.Black;
            this.toolLast.TextFirst.FontDown = new System.Drawing.Font("微软雅黑", 10F);
            this.toolLast.TextFirst.FontMove = new System.Drawing.Font("微软雅黑", 10F);
            this.toolLast.TextFirst.FontNormal = new System.Drawing.Font("微软雅黑", 10F);
            this.toolLast.TextFirst.StringVertical = System.Drawing.StringAlignment.Center;
            this.toolLast.Trans = 150;
            // 
            // toolNext
            // 
            this.toolNext.ICheckEvent = true;
            this.toolNext.ImageSize = new System.Drawing.Size(0, 0);
            toolItem2.Text = ">";
            this.toolNext.Items.Add(toolItem2);
            this.toolNext.ItemSize = new System.Drawing.Size(30, 24);
            this.toolNext.ItemSpace = 5;
            this.toolNext.Location = new System.Drawing.Point(114, 1);
            this.toolNext.Name = "toolNext";
            this.toolNext.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.toolNext.Size = new System.Drawing.Size(30, 28);
            this.toolNext.TabIndex = 45;
            this.toolNext.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(94)))), ((int)(((byte)(167)))));
            this.toolNext.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(171)))), ((int)(((byte)(244)))));
            this.toolNext.TBackGround.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.toolNext.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolNext.TextFirst.ColorMove = System.Drawing.Color.White;
            this.toolNext.TextFirst.ColorNormal = System.Drawing.Color.Black;
            this.toolNext.TextFirst.FontDown = new System.Drawing.Font("微软雅黑", 10F);
            this.toolNext.TextFirst.FontMove = new System.Drawing.Font("微软雅黑", 10F);
            this.toolNext.TextFirst.FontNormal = new System.Drawing.Font("微软雅黑", 10F);
            this.toolNext.TextFirst.StringVertical = System.Drawing.StringAlignment.Center;
            this.toolNext.Trans = 150;
            // 
            // toolPrevious
            // 
            this.toolPrevious.ICheckEvent = true;
            this.toolPrevious.ImageSize = new System.Drawing.Size(0, 0);
            toolItem3.Text = "<";
            this.toolPrevious.Items.Add(toolItem3);
            this.toolPrevious.ItemSize = new System.Drawing.Size(30, 24);
            this.toolPrevious.ItemSpace = 5;
            this.toolPrevious.Location = new System.Drawing.Point(53, 1);
            this.toolPrevious.Name = "toolPrevious";
            this.toolPrevious.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.toolPrevious.Size = new System.Drawing.Size(30, 28);
            this.toolPrevious.TabIndex = 46;
            this.toolPrevious.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(94)))), ((int)(((byte)(167)))));
            this.toolPrevious.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(171)))), ((int)(((byte)(244)))));
            this.toolPrevious.TBackGround.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.toolPrevious.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolPrevious.TextFirst.ColorMove = System.Drawing.Color.White;
            this.toolPrevious.TextFirst.ColorNormal = System.Drawing.Color.Black;
            this.toolPrevious.TextFirst.FontDown = new System.Drawing.Font("微软雅黑", 10F);
            this.toolPrevious.TextFirst.FontMove = new System.Drawing.Font("微软雅黑", 10F);
            this.toolPrevious.TextFirst.FontNormal = new System.Drawing.Font("微软雅黑", 10F);
            this.toolPrevious.TextFirst.StringVertical = System.Drawing.StringAlignment.Center;
            this.toolPrevious.Trans = 150;
            // 
            // toolFirst
            // 
            this.toolFirst.ICheckEvent = true;
            this.toolFirst.ImageSize = new System.Drawing.Size(0, 0);
            toolItem4.Text = "|<";
            this.toolFirst.Items.Add(toolItem4);
            this.toolFirst.ItemSize = new System.Drawing.Size(30, 24);
            this.toolFirst.ItemSpace = 5;
            this.toolFirst.Location = new System.Drawing.Point(21, 1);
            this.toolFirst.Name = "toolFirst";
            this.toolFirst.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.toolFirst.Size = new System.Drawing.Size(30, 28);
            this.toolFirst.TabIndex = 47;
            this.toolFirst.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(94)))), ((int)(((byte)(167)))));
            this.toolFirst.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(171)))), ((int)(((byte)(244)))));
            this.toolFirst.TBackGround.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.toolFirst.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolFirst.TextFirst.ColorMove = System.Drawing.Color.White;
            this.toolFirst.TextFirst.ColorNormal = System.Drawing.Color.Black;
            this.toolFirst.TextFirst.FontDown = new System.Drawing.Font("微软雅黑", 10F);
            this.toolFirst.TextFirst.FontMove = new System.Drawing.Font("微软雅黑", 10F);
            this.toolFirst.TextFirst.FontNormal = new System.Drawing.Font("微软雅黑", 10F);
            this.toolFirst.TextFirst.StringVertical = System.Drawing.StringAlignment.Center;
            this.toolFirst.Trans = 150;
            // 
            // TPager
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.Controls.Add(this.toolFirst);
            this.Controls.Add(this.toolPrevious);
            this.Controls.Add(this.toolNext);
            this.Controls.Add(this.toolLast);
            this.Controls.Add(this.txtCurrentPage);
            this.Controls.Add(this.lblPageInfo);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Name = "TPager";
            this.Size = new System.Drawing.Size(606, 30);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblPageInfo;
        private TNumTestBox txtCurrentPage;

        #endregion
    }

    /// <summary>
    ///     分页属性
    /// </summary>
    [Serializable]
    [DataContract]
    public class PagerInfo
    {
        private int currentPageIndex = 1; //当前页码
        private int pageSize = 20; //每页显示的记录
        private int recordCount; //记录总数

        /// <summary>
        ///     在分页属性变动时发生
        /// </summary>
        public event PageInfoChanged PageInfoChanged;

        #region 属性变量

        /// <summary>
        ///     获取或设置当前页码
        /// </summary>
        [DataMember]
        public int CurrentPageIndex
        {
            get { return currentPageIndex; }
            set
            {
                if (value < 1) value = 1;
                if (currentPageIndex != value)
                {
                    currentPageIndex = value;
                    if (PageInfoChanged != null)
                    {
                        PageInfoChanged(this);
                    }
                }
            }
        }

        /// <summary>
        ///     获取或设置每页显示的记录
        /// </summary>
        [DataMember]
        [Description("获取或设置每页显示的记录"), DefaultValue(20), Category("分页")]
        public int PageSize
        {
            get { return pageSize; }
            set
            {
                if (value < 1) value = 1;
                if (pageSize != value)
                {
                    pageSize = value;
                    if (PageInfoChanged != null)
                    {
                        PageInfoChanged(this);
                    }
                }
            }
        }

        /// <summary>
        ///     获取或设置记录总数
        /// </summary>
        [DataMember]
        [Description("获取或设置记录总数"), DefaultValue(0), Category("分页")]
        public int RecordCount
        {
            get { return recordCount; }
            set
            {
                if (value < 0) value = 0;
                if (recordCount != value)
                {
                    recordCount = value;
                    if (PageInfoChanged != null)
                    {
                        PageInfoChanged(this);
                    }
                }
            }
        }

        /// <summary>
        ///     获取记录总页数
        /// </summary>
        [DataMember]
        [Description("获取记录总页数"), DefaultValue(0), Category("分页")]
        public int PageCount
        {
            get
            {
                if (PageSize == 0) PageSize = 1;
                if (RecordCount % PageSize == 0)
                {
                    return recordCount / pageSize;
                }
                return recordCount / pageSize + 1;
            }
        }

        #endregion
    }
}