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
            var toolItem1 = new ToolItem();
            var resources = new ComponentResourceManager(typeof(TPager));
            var toolItem2 = new ToolItem();
            var toolItem3 = new ToolItem();
            var toolItem4 = new ToolItem();
            lblPageInfo = new Label();
            txtCurrentPage = new TNumTestBox();
            toolLast = new ToolBar();
            toolNext = new ToolBar();
            toolPrevious = new ToolBar();
            toolFirst = new ToolBar();
            SuspendLayout();
            // 
            // lblPageInfo
            // 
            lblPageInfo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblPageInfo.AutoSize = true;
            lblPageInfo.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblPageInfo.ForeColor = Color.Black;
            lblPageInfo.Location = new Point(358, 7);
            lblPageInfo.Name = "lblPageInfo";
            lblPageInfo.Size = new Size(220, 14);
            lblPageInfo.TabIndex = 0;
            lblPageInfo.Text = "共 {0} 条记录，每页 {1} 条，共 {2} 页";
            // 
            // txtCurrentPage
            // 
            txtCurrentPage.ForeColor = Color.Black;
            txtCurrentPage.Icon = null;
            txtCurrentPage.IsPasswordChat = '\0';
            txtCurrentPage.IsTrans = true;
            txtCurrentPage.Lines = new[]
            {
                "1"
            };
            txtCurrentPage.Location = new Point(86, 3);
            txtCurrentPage.MaxLength = 10;
            txtCurrentPage.Name = "txtCurrentPage";
            txtCurrentPage.SelectedText = "";
            txtCurrentPage.Size = new Size(28, 25);
            txtCurrentPage.TabIndex = 1;
            // 
            // toolLast
            // 
            toolLast.ICheckEvent = true;
            toolLast.ImageSize = new Size(0, 0);
            toolItem1.Text = ">|";
            toolLast.Items.Add(toolItem1);
            toolLast.ItemSize = new Size(30, 24);
            toolLast.ItemSpace = 5;
            toolLast.Location = new Point(146, 1);
            toolLast.Name = "toolLast";
            toolLast.Padding = new Padding(0, 2, 0, 0);
            toolLast.SelectImage = (Image)resources.GetObject("toolLast.SelectImage");
            toolLast.Size = new Size(30, 28);
            toolLast.TabIndex = 44;
            toolLast.TBackGround.ColorDown = Color.FromArgb(31, 94, 167);
            toolLast.TBackGround.ColorMove = Color.FromArgb(108, 171, 244);
            toolLast.TBackGround.ColorNormal = Color.FromArgb(207, 221, 238);
            toolLast.TEvent = TEvent.Up;
            toolLast.TextFirst.ColorDown = Color.White;
            toolLast.TextFirst.ColorMove = Color.White;
            toolLast.TextFirst.ColorNormal = Color.Black;
            toolLast.TextFirst.FontDown = new Font("微软雅黑", 10F);
            toolLast.TextFirst.FontMove = new Font("微软雅黑", 10F);
            toolLast.TextFirst.FontNormal = new Font("微软雅黑", 10F);
            toolLast.TextFirst.StringVertical = StringAlignment.Center;
            toolLast.Trans = 150;
            // 
            // toolNext
            // 
            toolNext.ICheckEvent = true;
            toolNext.ImageSize = new Size(0, 0);
            toolItem2.Text = ">";
            toolNext.Items.Add(toolItem2);
            toolNext.ItemSize = new Size(30, 24);
            toolNext.ItemSpace = 5;
            toolNext.Location = new Point(114, 1);
            toolNext.Name = "toolNext";
            toolNext.Padding = new Padding(0, 2, 0, 0);
            toolNext.SelectImage = (Image)resources.GetObject("toolNext.SelectImage");
            toolNext.Size = new Size(30, 28);
            toolNext.TabIndex = 45;
            toolNext.TBackGround.ColorDown = Color.FromArgb(31, 94, 167);
            toolNext.TBackGround.ColorMove = Color.FromArgb(108, 171, 244);
            toolNext.TBackGround.ColorNormal = Color.FromArgb(207, 221, 238);
            toolNext.TEvent = TEvent.Up;
            toolNext.TextFirst.ColorDown = Color.White;
            toolNext.TextFirst.ColorMove = Color.White;
            toolNext.TextFirst.ColorNormal = Color.Black;
            toolNext.TextFirst.FontDown = new Font("微软雅黑", 10F);
            toolNext.TextFirst.FontMove = new Font("微软雅黑", 10F);
            toolNext.TextFirst.FontNormal = new Font("微软雅黑", 10F);
            toolNext.TextFirst.StringVertical = StringAlignment.Center;
            toolNext.Trans = 150;
            // 
            // toolPrevious
            // 
            toolPrevious.ICheckEvent = true;
            toolPrevious.ImageSize = new Size(0, 0);
            toolItem3.Text = "<";
            toolPrevious.Items.Add(toolItem3);
            toolPrevious.ItemSize = new Size(30, 24);
            toolPrevious.ItemSpace = 5;
            toolPrevious.Location = new Point(53, 1);
            toolPrevious.Name = "toolPrevious";
            toolPrevious.Padding = new Padding(0, 2, 0, 0);
            toolPrevious.SelectImage = (Image)resources.GetObject("toolPrevious.SelectImage");
            toolPrevious.Size = new Size(30, 28);
            toolPrevious.TabIndex = 46;
            toolPrevious.TBackGround.ColorDown = Color.FromArgb(31, 94, 167);
            toolPrevious.TBackGround.ColorMove = Color.FromArgb(108, 171, 244);
            toolPrevious.TBackGround.ColorNormal = Color.FromArgb(207, 221, 238);
            toolPrevious.TEvent = TEvent.Up;
            toolPrevious.TextFirst.ColorDown = Color.White;
            toolPrevious.TextFirst.ColorMove = Color.White;
            toolPrevious.TextFirst.ColorNormal = Color.Black;
            toolPrevious.TextFirst.FontDown = new Font("微软雅黑", 10F);
            toolPrevious.TextFirst.FontMove = new Font("微软雅黑", 10F);
            toolPrevious.TextFirst.FontNormal = new Font("微软雅黑", 10F);
            toolPrevious.TextFirst.StringVertical = StringAlignment.Center;
            toolPrevious.Trans = 150;
            // 
            // toolFirst
            // 
            toolFirst.ICheckEvent = true;
            toolFirst.ImageSize = new Size(0, 0);
            toolItem4.Text = "|<";
            toolFirst.Items.Add(toolItem4);
            toolFirst.ItemSize = new Size(30, 24);
            toolFirst.ItemSpace = 5;
            toolFirst.Location = new Point(21, 1);
            toolFirst.Name = "toolFirst";
            toolFirst.Padding = new Padding(0, 2, 0, 0);
            toolFirst.SelectImage = (Image)resources.GetObject("toolFirst.SelectImage");
            toolFirst.Size = new Size(30, 28);
            toolFirst.TabIndex = 47;
            toolFirst.TBackGround.ColorDown = Color.FromArgb(31, 94, 167);
            toolFirst.TBackGround.ColorMove = Color.FromArgb(108, 171, 244);
            toolFirst.TBackGround.ColorNormal = Color.FromArgb(207, 221, 238);
            toolFirst.TEvent = TEvent.Up;
            toolFirst.TextFirst.ColorDown = Color.White;
            toolFirst.TextFirst.ColorMove = Color.White;
            toolFirst.TextFirst.ColorNormal = Color.Black;
            toolFirst.TextFirst.FontDown = new Font("微软雅黑", 10F);
            toolFirst.TextFirst.FontMove = new Font("微软雅黑", 10F);
            toolFirst.TextFirst.FontNormal = new Font("微软雅黑", 10F);
            toolFirst.TextFirst.StringVertical = StringAlignment.Center;
            toolFirst.Trans = 150;
            // 
            // TPager
            // 
            AutoScaleDimensions = new SizeF(6F, 12F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(207, 221, 238);
            Controls.Add(toolFirst);
            Controls.Add(toolPrevious);
            Controls.Add(toolNext);
            Controls.Add(toolLast);
            Controls.Add(txtCurrentPage);
            Controls.Add(lblPageInfo);
            Cursor = Cursors.Hand;
            Name = "TPager";
            Size = new Size(606, 30);
            ResumeLayout(false);
            PerformLayout();
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