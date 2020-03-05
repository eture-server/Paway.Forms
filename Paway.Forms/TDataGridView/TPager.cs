using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;
using Paway.Helper;

namespace Paway.Forms
{
    /// <summary>
    /// 分页工具栏
    /// </summary>
    public class TPager : TControl
    {
        #region 变量
        private PagerInfo pagerInfo;
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private IContainer components = null;
        private Label lbLeft;
        private Label lbRight;
        private Label lbDesc;
        private TControl panel1;
        private ToolBar toolEnd;
        private ToolBar toolNext;
        private TControl tControl2;
        private TNumTestBox txtCurrentPage;
        private ToolBar toolLast;
        private ToolBar toolFirst;
        private Label lblPageInfo;

        #endregion

        #region 属性
        /// <summary>
        /// 分页信息
        /// </summary>
        [Browsable(false)]
        public PagerInfo PagerInfo
        {
            get
            {
                if (pagerInfo == null)
                {
                    pagerInfo = new PagerInfo();
                    pagerInfo.PageInfoChanged += PagerInfo_PageInfoChanged;
                    PagerInfo.CountChanged += PagerInfo_CountChanged;
                }
                return pagerInfo;
            }
        }
        /// <summary>
        /// 获取或设置当鼠标指针位于控件上时显示的光标
        /// </summary>
        [Description("获取或设置当鼠标指针位于控件上时显示的光标")]
        [DefaultValue(typeof(Cursor), "Hand")]
        public override Cursor Cursor
        {
            get { return base.Cursor; }
            set { base.Cursor = value; }
        }
        /// <summary>
        /// 输入有焦点
        /// </summary>
        public bool ITextFocus { get { return txtCurrentPage.ContainsFocus; } }

        #endregion

        #region 事件
        /// <summary>
        /// 页面切换的时候触发
        /// </summary>
        public event EventHandler PageChanged;

        #endregion

        #region 构造
        /// <summary>
        /// 默认构造函数，设置分页初始信息
        /// </summary>
        public TPager() : this(0, 20) { }
        internal void UpdateLine(Padding padding, Color color)
        {
            lbLeft.Width = padding.Left;
            lbRight.Width = padding.Right;
            lbLeft.BackColor = color;
            lbRight.BackColor = color;
        }

        /// <summary>
        /// 带参数的构造函数
        /// <param name="pageSize">每页记录数</param>
        /// <param name="recordCount">总记录数</param>
        /// </summary>
        public TPager(int recordCount, int pageSize)
        {
            InitializeComponent();

            PagerInfo.PageSize = pageSize;
            PagerInfo.RecordCount = recordCount;
            PagerInfo.CurrentPageIndex = 1; //默认为第一页
            InitPageInfo();
            toolFirst.ItemClick += ToolFirst_ItemClick;
            toolLast.ItemClick += ToolLast_ItemClick;
            txtCurrentPage.Edit.GotFocus += TxtCurrentPage_GotFocus;
            txtCurrentPage.Edit.LostFocus += TxtCurrentPage_LostFocus;
            txtCurrentPage.KeyDown += TxtCurrentPage_KeyDown;

            toolNext.ItemClick += ToolNext_ItemClick;
            toolEnd.ItemClick += ToolEnd_ItemClick;
        }
        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            Paway.Forms.ToolItem toolItem1 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem2 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem3 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem4 = new Paway.Forms.ToolItem();
            this.lbLeft = new System.Windows.Forms.Label();
            this.lbRight = new System.Windows.Forms.Label();
            this.lblPageInfo = new System.Windows.Forms.Label();
            this.lbDesc = new System.Windows.Forms.Label();
            this.panel1 = new Paway.Forms.TControl();
            this.toolEnd = new Paway.Forms.ToolBar();
            this.toolNext = new Paway.Forms.ToolBar();
            this.tControl2 = new Paway.Forms.TControl();
            this.txtCurrentPage = new Paway.Forms.TNumTestBox();
            this.toolLast = new Paway.Forms.ToolBar();
            this.toolFirst = new Paway.Forms.ToolBar();
            this.panel1.SuspendLayout();
            this.tControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbLeft
            // 
            this.lbLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(204)))), ((int)(((byte)(223)))));
            this.lbLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbLeft.Location = new System.Drawing.Point(0, 0);
            this.lbLeft.Name = "lbLeft";
            this.lbLeft.Size = new System.Drawing.Size(1, 30);
            this.lbLeft.TabIndex = 102;
            // 
            // lbRight
            // 
            this.lbRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(204)))), ((int)(((byte)(223)))));
            this.lbRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.lbRight.Location = new System.Drawing.Point(605, 0);
            this.lbRight.Name = "lbRight";
            this.lbRight.Size = new System.Drawing.Size(1, 30);
            this.lbRight.TabIndex = 104;
            // 
            // lblPageInfo
            // 
            this.lblPageInfo.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblPageInfo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPageInfo.ForeColor = System.Drawing.Color.Black;
            this.lblPageInfo.Location = new System.Drawing.Point(264, 0);
            this.lblPageInfo.Name = "lblPageInfo";
            this.lblPageInfo.Padding = new System.Windows.Forms.Padding(5, 8, 20, 0);
            this.lblPageInfo.Size = new System.Drawing.Size(341, 30);
            this.lblPageInfo.TabIndex = 105;
            this.lblPageInfo.Text = "共 {0:#,0} 条记录，每页 {1:#,0} 条，共 {2:#,0} 页";
            this.lblPageInfo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbDesc
            // 
            this.lbDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbDesc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbDesc.ForeColor = System.Drawing.Color.Black;
            this.lbDesc.Location = new System.Drawing.Point(184, 0);
            this.lbDesc.Name = "lbDesc";
            this.lbDesc.Padding = new System.Windows.Forms.Padding(20, 8, 0, 0);
            this.lbDesc.Size = new System.Drawing.Size(80, 30);
            this.lbDesc.TabIndex = 108;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toolEnd);
            this.panel1.Controls.Add(this.toolNext);
            this.panel1.Controls.Add(this.tControl2);
            this.panel1.Controls.Add(this.toolLast);
            this.panel1.Controls.Add(this.toolFirst);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.panel1.Location = new System.Drawing.Point(1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.panel1.Size = new System.Drawing.Size(183, 30);
            this.panel1.TabIndex = 107;
            // 
            // toolEnd
            // 
            this.toolEnd.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolEnd.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.toolEnd.IClickEvent = true;
            toolItem1.Hit = "End";
            toolItem1.Text = ">|";
            this.toolEnd.Items.Add(toolItem1);
            this.toolEnd.ItemSize = new System.Drawing.Size(30, 24);
            this.toolEnd.ItemSpace = 5;
            this.toolEnd.Location = new System.Drawing.Point(153, 0);
            this.toolEnd.Name = "toolEnd";
            this.toolEnd.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.toolEnd.Size = new System.Drawing.Size(30, 30);
            this.toolEnd.TabIndex = 27;
            this.toolEnd.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(94)))), ((int)(((byte)(167)))));
            this.toolEnd.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(171)))), ((int)(((byte)(244)))));
            this.toolEnd.TBackGround.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.toolEnd.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolEnd.TextFirst.ColorMove = System.Drawing.Color.White;
            this.toolEnd.TextFirst.ColorNormal = System.Drawing.Color.Black;
            this.toolEnd.TextFirst.FontDown = new System.Drawing.Font("微软雅黑", 10F);
            this.toolEnd.TextFirst.FontMove = new System.Drawing.Font("微软雅黑", 10F);
            this.toolEnd.TextFirst.FontNormal = new System.Drawing.Font("微软雅黑", 10F);
            this.toolEnd.TextFirst.StringHorizontal = System.Drawing.StringAlignment.Center;
            this.toolEnd.Trans = 150;
            // 
            // toolNext
            // 
            this.toolNext.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolNext.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.toolNext.IClickEvent = true;
            toolItem2.Hit = "Next";
            toolItem2.Text = ">";
            this.toolNext.Items.Add(toolItem2);
            this.toolNext.ItemSize = new System.Drawing.Size(30, 24);
            this.toolNext.ItemSpace = 5;
            this.toolNext.Location = new System.Drawing.Point(123, 0);
            this.toolNext.Name = "toolNext";
            this.toolNext.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.toolNext.Size = new System.Drawing.Size(30, 30);
            this.toolNext.TabIndex = 26;
            this.toolNext.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(94)))), ((int)(((byte)(167)))));
            this.toolNext.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(171)))), ((int)(((byte)(244)))));
            this.toolNext.TBackGround.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.toolNext.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolNext.TextFirst.ColorMove = System.Drawing.Color.White;
            this.toolNext.TextFirst.ColorNormal = System.Drawing.Color.Black;
            this.toolNext.TextFirst.FontDown = new System.Drawing.Font("微软雅黑", 10F);
            this.toolNext.TextFirst.FontMove = new System.Drawing.Font("微软雅黑", 10F);
            this.toolNext.TextFirst.FontNormal = new System.Drawing.Font("微软雅黑", 10F);
            this.toolNext.TextFirst.StringHorizontal = System.Drawing.StringAlignment.Center;
            this.toolNext.Trans = 150;
            // 
            // tControl2
            // 
            this.tControl2.Controls.Add(this.txtCurrentPage);
            this.tControl2.Dock = System.Windows.Forms.DockStyle.Left;
            this.tControl2.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.tControl2.Location = new System.Drawing.Point(80, 0);
            this.tControl2.Name = "tControl2";
            this.tControl2.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.tControl2.Size = new System.Drawing.Size(43, 30);
            this.tControl2.TabIndex = 22;
            // 
            // txtCurrentPage
            // 
            this.txtCurrentPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCurrentPage.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtCurrentPage.ITrans = true;
            this.txtCurrentPage.Lines = new string[] {
        "0"};
            this.txtCurrentPage.Location = new System.Drawing.Point(0, 2);
            this.txtCurrentPage.MaxLength = 10;
            this.txtCurrentPage.Name = "txtCurrentPage";
            this.txtCurrentPage.Size = new System.Drawing.Size(43, 28);
            this.txtCurrentPage.TabIndex = 27;
            this.txtCurrentPage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolLast
            // 
            this.toolLast.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolLast.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.toolLast.IClickEvent = true;
            toolItem3.Hit = "Last";
            toolItem3.Text = "<";
            this.toolLast.Items.Add(toolItem3);
            this.toolLast.ItemSize = new System.Drawing.Size(30, 24);
            this.toolLast.ItemSpace = 5;
            this.toolLast.Location = new System.Drawing.Point(50, 0);
            this.toolLast.Name = "toolLast";
            this.toolLast.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.toolLast.Size = new System.Drawing.Size(30, 30);
            this.toolLast.TabIndex = 21;
            this.toolLast.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(94)))), ((int)(((byte)(167)))));
            this.toolLast.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(171)))), ((int)(((byte)(244)))));
            this.toolLast.TBackGround.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.toolLast.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolLast.TextFirst.ColorMove = System.Drawing.Color.White;
            this.toolLast.TextFirst.ColorNormal = System.Drawing.Color.Black;
            this.toolLast.TextFirst.FontDown = new System.Drawing.Font("微软雅黑", 10F);
            this.toolLast.TextFirst.FontMove = new System.Drawing.Font("微软雅黑", 10F);
            this.toolLast.TextFirst.FontNormal = new System.Drawing.Font("微软雅黑", 10F);
            this.toolLast.TextFirst.StringHorizontal = System.Drawing.StringAlignment.Center;
            this.toolLast.Trans = 150;
            // 
            // toolFirst
            // 
            this.toolFirst.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolFirst.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.toolFirst.IClickEvent = true;
            toolItem4.Hit = "First";
            toolItem4.Text = "|<";
            this.toolFirst.Items.Add(toolItem4);
            this.toolFirst.ItemSize = new System.Drawing.Size(30, 24);
            this.toolFirst.Location = new System.Drawing.Point(20, 0);
            this.toolFirst.Name = "toolFirst";
            this.toolFirst.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.toolFirst.Size = new System.Drawing.Size(30, 30);
            this.toolFirst.TabIndex = 16;
            this.toolFirst.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(94)))), ((int)(((byte)(167)))));
            this.toolFirst.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(171)))), ((int)(((byte)(244)))));
            this.toolFirst.TBackGround.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.toolFirst.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolFirst.TextFirst.ColorMove = System.Drawing.Color.White;
            this.toolFirst.TextFirst.ColorNormal = System.Drawing.Color.Black;
            this.toolFirst.TextFirst.FontDown = new System.Drawing.Font("微软雅黑", 10F);
            this.toolFirst.TextFirst.FontMove = new System.Drawing.Font("微软雅黑", 10F);
            this.toolFirst.TextFirst.FontNormal = new System.Drawing.Font("微软雅黑", 10F);
            this.toolFirst.TextFirst.StringHorizontal = System.Drawing.StringAlignment.Center;
            this.toolFirst.Trans = 150;
            // 
            // TPager
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.Controls.Add(this.lbDesc);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lbLeft);
            this.Controls.Add(this.lblPageInfo);
            this.Controls.Add(this.lbRight);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Name = "TPager";
            this.Size = new System.Drawing.Size(606, 30);
            this.panel1.ResumeLayout(false);
            this.tControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void TxtCurrentPage_LostFocus(object sender, EventArgs e)
        {
            txtCurrentPage.ITrans = true;
        }

        private void TxtCurrentPage_GotFocus(object sender, EventArgs e)
        {
            txtCurrentPage.ITrans = false;
        }

        #endregion

        #region 公开方法
        /// <summary>
        /// 刷新页面数据
        /// </summary>
        /// <param name="page">页码</param>
        public void RefreshData(int page)
        {
            PagerInfo.CurrentPageIndex = page;
        }

        #endregion

        #region 分页
        /// <summary>
        /// 引发页面变化处理事件
        /// </summary>
        /// <param name="e"></param>
        private void OnPageChanged(EventArgs e)
        {
            PageChanged?.Invoke(this, e);
        }
        private void PagerInfo_PageInfoChanged(PagerInfo info)
        {
            InitPageInfo(info.RecordCount, info.PageSize);
        }
        private void PagerInfo_CountChanged(PagerInfo info)
        {
            InitPageInfo(true);
        }

        /// <summary>
        /// 更新分页信息
        /// <param name="pageSize">每页记录数</param>
        /// <param name="recordCount">总记录数</param>
        /// </summary>
        public void InitPageInfo(int recordCount, int pageSize)
        {
            PagerInfo.RecordCount = recordCount;
            PagerInfo.PageSize = pageSize;
            InitPageInfo();
        }

        /// <summary>
        /// 更新分页信息
        /// </summary>
        public void InitPageInfo(bool iTotal = false)
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
            if (!txtCurrentPage.Edit.Focused) lblPageInfo.Focus();
            toolLast.Enabled = PagerInfo.CurrentPageIndex > 1;
            toolNext.Enabled = PagerInfo.CurrentPageIndex < PagerInfo.PageCount;

            txtCurrentPage.Text = PagerInfo.CurrentPageIndex.ToString();
            if (!iTotal) OnPageChanged(EventArgs.Empty);
            if (PagerInfo.IGroup)
            {
                panel1.Visible = true;
                lblPageInfo.Text = string.Format("共 {0:#,0} 条记录，每页 {1:#,0} 条，共 {2:#,0} 页", PagerInfo.RecordCount, PagerInfo.PageSize, PagerInfo.PageCount);
            }
            else
            {
                panel1.Visible = false;
                lblPageInfo.Text = string.Format("共 {0:#,0} 条记录", PagerInfo.RecordCount);
            }
        }
        /// <summary>
        /// 更新统计描述
        /// </summary>
        public void UpdateDesc(string desc)
        {
            lbDesc.Text = desc;
        }

        #region 当前页更新
        private void ToolFirst_ItemClick(ToolItem item, EventArgs e)
        {
            RefreshData(1);
        }

        private void ToolLast_ItemClick(ToolItem item, EventArgs e)
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

        private void ToolNext_ItemClick(ToolItem item, EventArgs e)
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

        private void ToolEnd_ItemClick(ToolItem item, EventArgs e)
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

        private void TxtCurrentPage_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    var num = txtCurrentPage.Text.ToInt();

                    if (num > PagerInfo.PageCount)
                        num = PagerInfo.PageCount;
                    if (num < 1)
                        num = 1;

                    txtCurrentPage.Text = num.ToString();
                    txtCurrentPage.SelectionStart = txtCurrentPage.Text.Length;
                    RefreshData(num);
                    toolNext.Focus();
                    break;
                case Keys.Escape:
                    txtCurrentPage.Text = PagerInfo.CurrentPageIndex.ToString();
                    lblPageInfo.Focus();
                    break;
            }
        }

        #endregion

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
            if (toolFirst != null)
            {
                toolFirst.Dispose();
                toolFirst = null;
            }
            if (toolEnd != null)
            {
                toolEnd.Dispose();
                toolEnd = null;
            }
            if (toolNext != null)
            {
                toolNext.Dispose();
                toolNext = null;
            }
            if (toolLast != null)
            {
                toolLast.Dispose();
                toolLast = null;
            }
            if (lblPageInfo != null)
            {
                lblPageInfo.Dispose();
                lblPageInfo = null;
            }
            if (txtCurrentPage != null)
            {
                txtCurrentPage.Dispose();
                txtCurrentPage = null;
            }
            if (lbDesc != null)
            {
                lbDesc.Dispose();
                lbDesc = null;
            }
            if (panel1 != null)
            {
                panel1.Dispose();
                panel1 = null;
            }
            if (tControl2 != null)
            {
                tControl2.Dispose();
                tControl2 = null;
            }
            if (components != null)
            {
                components.Dispose();
                components = null;
            }
            base.Dispose(disposing);
        }

        #endregion
    }

    /// <summary>
    /// 分页属性
    /// </summary>
    [Serializable]
    public class PagerInfo
    {
        private int currentPageIndex = 1; //当前页码
        private int pageSize = 20; //每页显示的记录
        private int recordCount; //记录总数
        private bool iGroup = true; //分页

        /// <summary>
        /// 在分页属性变动时发生
        /// </summary>
        public event Action<PagerInfo> PageInfoChanged;
        /// <summary>
        /// 在分页属性变动时发生
        /// </summary>
        public event Action<PagerInfo> CountChanged;

        #region 属性
        /// <summary>
        /// 获取或设置当前页码
        /// </summary>
        public int CurrentPageIndex
        {
            get { return currentPageIndex; }
            set
            {
                if (value < 1) value = 1;
                if (currentPageIndex != value)
                {
                    currentPageIndex = value;
                    PageInfoChanged?.Invoke(this);
                }
            }
        }

        /// <summary>
        /// 获取或设置每页显示的记录
        /// </summary>
        [Description("获取或设置每页显示的记录"), Category("分页")]
        [DefaultValue(20)]
        public int PageSize
        {
            get { return pageSize; }
            set
            {
                if (value < 1) value = 1;
                if (pageSize != value)
                {
                    pageSize = value;
                    PageInfoChanged?.Invoke(this);
                }
            }
        }

        /// <summary>
        /// 分页信息
        /// </summary>
        [Category("分页")]
        [Description("显示分页")]
        [DefaultValue(true)]
        public bool IGroup
        {
            get { return iGroup; }
            set
            {
                iGroup = value;
                PageInfoChanged?.Invoke(this);
            }
        }

        /// <summary>
        /// 获取或设置记录总数
        /// </summary>
        [Description("获取或设置记录总数"), Category("分页")]
        [DefaultValue(0)]
        public int RecordCount
        {
            get { return recordCount; }
            set
            {
                if (value < 0) value = 0;
                if (recordCount != value)
                {
                    recordCount = value;
                    CountChanged?.Invoke(this);
                }
            }
        }

        /// <summary>
        /// 获取记录总页数
        /// </summary>
        [Description("获取记录总页数"), Category("分页")]
        [DefaultValue(0)]
        public int PageCount
        {
            get
            {
                if (pageSize == 0) pageSize = 1;
                if (RecordCount % pageSize == 0)
                {
                    return RecordCount / pageSize;
                }
                return RecordCount / pageSize + 1;
            }
        }

        #endregion
    }
}