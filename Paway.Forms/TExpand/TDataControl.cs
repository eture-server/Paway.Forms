using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Paway.Forms;
using Paway.Helper;
using System.Collections;
using Paway.Forms.Properties;

namespace Paway.Forms
{
    /// <summary>
    /// 数据控件示例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class TDataControl<T> : TBaseControl where T : IId, IFind<T>
    {
        /// <summary>
        /// 数据
        /// </summary>
        protected new List<T> List;
        /// <summary>
        /// 当前查询数据
        /// </summary>
        protected ParallelQuery<T> FList;
        /// <summary>
        /// 当前选中序号
        /// </summary>
        protected int Index;
        /// <summary>
        /// 当前Info
        /// </summary>
        protected T Info;
        private string find;
        /// <summary>
        /// 查询开始时间
        /// </summary>
        private DateTime start;
        private IDataService server;

        /// <summary>
        /// </summary>
        public TDataControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// </summary>
        public override void ReLoad()
        {
            base.ReLoad();
            toolBar1.MStart();
        }
        /// <summary>
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.List = new List<T>();
            InitVerofy();
            if (DesignMode) return;

            tbName.TextChanged += TbName_TextChanged;
            tbName.KeyDown += TbName_KeyDown;
            toolBar1.ItemClick += ToolBar1_ItemClick;
            gridview1.Edit.CellFormatting += Gridview1_CellFormatting;
            gridview1.Edit.CurrentCellChanged += Gridview1_CurrentCellChanged;
            gridview1.Edit.DoubleClick += Gridview1_DoubleClick;
            gridview1.PagerInfo.PageSize = 20;
        }

        #region 权限-按钮
        private void InitVerofy()
        {
            toolBar1.Items.Clear();
            RefreshItem();
        }
        /// <summary>
        /// </summary>
        public void NewItem(string name, string tag = null, Image image = null)
        {
            toolBar1.Items.Add(new ToolItem(name, image) { Tag = tag ?? name });
        }
        /// <summary>
        /// </summary>
        public void RefreshItem()
        {
            toolBar1.Items.Add(new ToolItem("刷新(F5)", Resources.refresh) { Tag = "刷新" });
        }
        /// <summary>
        /// </summary>
        public void AddItem()
        {
            toolBar1.Items.Add(new ToolItem("添加(A)", Resources.add) { Tag = "添加" });
        }
        /// <summary>
        /// </summary>
        public void AddEdit()
        {
            toolBar1.Items.Add(new ToolItem("编辑(E)", Resources.edit) { Tag = "编辑" });
        }
        /// <summary>
        /// </summary>
        public void AddDelete()
        {
            toolBar1.Items.Add(new ToolItem("删除(D)", Resources.close) { Tag = "删除" });
        }

        #endregion

        #region 加数数据与样式
        /// <summary>
        /// 查询参数
        /// </summary>
        protected void InitData(IDataService server, string find = null)
        {
            if (this.DesignMode) return;
            this.server = server;
            this.find = find;
            find = string.Format("{0} order by Id", this.find ?? "1=1");
            QueryStart(find);
        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        protected virtual void InitData(IDataService server, List<T> list, bool iAdd = true)
        {
            try
            {
                this.server = server;
                if (iAdd)
                {
                    this.List = list as List<T>;
                }
                else
                {
                    this.List.Clear();
                    this.List.AddRange(list);
                }
                this.gridview1.DataSource = this.List;
                RefreshTotal();
            }
            catch (Exception ex)
            {
                ExceptionHelper.Show(ex);
            }
        }
        /// <summary>
        /// 无工具栏
        /// </summary>
        protected void NoTool()
        {
            panel1.Visible = false;
        }
        /// <summary>
        /// 查询
        /// </summary>
        protected void HaveQuery()
        {
            tbName.Visible = true;
        }

        #endregion

        #region 数据绑定
        /// <summary>
        /// 数据统计
        /// </summary>
        /// <returns></returns>
        protected virtual T OnTotal() { return default(T); }
        /// <summary>
        /// 添加数据
        /// </summary>
        protected virtual T OnAdd() { return default(T); }
        /// <summary>
        /// 插入数据到数据库
        /// </summary>
        protected virtual void OnAddInfo(T info)
        {
            server.Insert(info);
        }
        /// <summary>
        /// 编辑数据
        /// </summary>
        protected virtual Form OnEdit(T info) { return new Form(); }
        /// <summary>
        /// 更新数据到数据库
        /// </summary>
        protected virtual void OnEditInfo(T info)
        {
            server.Update(info);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        protected virtual Tuple<string, string> OnDelete(T info) { return new Tuple<string, string>("Warn", "Confirm Delete?"); }
        /// <summary>
        /// 从数据库删除数据
        /// </summary>
        protected virtual void OnDeleteInfo(T info)
        {
            server.Delete(info);
        }

        #endregion

        #region 预置增删改
        /// <summary>
        /// 点击响应
        /// </summary>
        protected override void OnItemClick(ToolItem item)
        {
            switch (item.Tag.ToString())
            {
                case "刷新":
                    tbName.TextChanged -= TbName_TextChanged;
                    tbName.Text = null;
                    tbName.TextChanged += TbName_TextChanged;
                    string find = string.Format("{0} order by " + nameof(IId.Id), this.find ?? "1=1");
                    QueryStart(find);
                    break;
                case "添加":
                    T data = OnAdd();
                    if (data != null)
                    {
                        OnAddInfo(data);
                        this.List.Add(data);
                        if (ToFind())
                        {
                            gridview1.AutoLast();
                        }
                        else
                        {
                            RefreshTotal();
                            int index = this.List.FindIndex(c => c.Id == data.Id);
                            this.gridview1.CurrentPageIndex = index / this.gridview1.PagerInfo.PageSize + 1;
                            index %= this.gridview1.PagerInfo.PageSize;
                            gridview1.Edit.AutoCell(index);
                        }
                    }
                    break;
                case "编辑":
                    if (this.Info == null)
                    {
                        ExceptionHelper.Show("请选择数据项");
                        break;
                    }
                    if (this.Info.Id < 0) return;
                    Form edit = OnEdit(this.Info);
                    if (edit != null && edit.ShowDialog(this) == DialogResult.OK)
                    {
                        OnEditInfo(this.Info);
                        if (ToFind())
                        {
                            gridview1.AutoLast();
                        }
                        else
                        {
                            int index = this.Index;
                            RefreshTotal();
                            gridview1.Edit.AutoCell(index);
                        }
                    }
                    break;
                case "删除":
                    if (this.Info == null)
                    {
                        ExceptionHelper.Show("请选择数据项");
                        break;
                    }
                    if (this.Info.Id < 0) return;
                    var delete = OnDelete(this.Info);
                    DialogResult result = MessageBox.Show(this, delete.Item2, delete.Item1, MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    if (result == DialogResult.OK)
                    {
                        OnDeleteInfo(this.Info);
                        this.List.Remove(this.Info);
                        if (ToFind())
                        {
                            gridview1.AutoLast();
                        }
                        else
                        {
                            int index = this.Index;
                            RefreshTotal();
                            gridview1.Edit.AutoCell(index);
                        }
                    }
                    break;
            }
        }
        private void RefreshTotal()
        {
            T data = List.Find(c => c.Id < 0);
            if (data != null) List.Remove(data);
            data = OnTotal();
            if (data != null)
            {
                data.Id = -1;
                List.Add(data);
            }
            this.gridview1.AutoCell();
        }

        #endregion

        #region 界面快捷方式
        /// <summary>
        /// 查询
        /// </summary>
        protected virtual ParallelQuery<T> OnFind(string value)
        {
            return this.List.AsParallel().Where((Activator.CreateInstance<T>().Find(value)));
        }
        private void TbName_TextChanged(object sender, EventArgs e)
        {
            if (!ToFind())
            {
                this.gridview1.DataSource = this.List;
            }
        }
        private bool ToFind()
        {
            if (!tbName.Visible && tbName.IError) return false;
            string value = tbName.Text.Trim();
            if (!value.IsNullOrEmpty())
            {
                this.FList = OnFind(tbName.Text);
                this.gridview1.DataSource = FList;
                return true;
            }
            return false;
        }
        private void TbName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (tbName.IError) return;
                this.gridview1.Edit.Focus();
            }
        }

        #endregion

        #region 按键
        /// <summary>
        /// </summary>
        protected virtual bool OnKeyDown(Keys key)
        {
            switch (key)
            {
                case Keys.F5:
                    toolBar1.TClickItem("刷新");
                    break;
                case Keys.F:
                case (Keys)Shortcut.CtrlF:
                    tbName.Focus();
                    break;
                case (Keys)Shortcut.CtrlA:
                    toolBar1.TClickItem("添加");
                    break;
                case Keys.Enter:
                case (Keys)Shortcut.CtrlE:
                    toolBar1.TClickItem("编辑");
                    break;
                case Keys.Delete:
                case (Keys)Shortcut.CtrlD:
                    toolBar1.TClickItem("删除");
                    break;
                case (Keys)Shortcut.CtrlQ:
                    toolBar1.TClickItem("查询");
                    break;
            }
            return false;
        }
        /// <summary>
        /// </summary>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (OnKeyDown(keyData)) return true;
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        #region 快捷显示
        /// <summary>
        /// CellFormatting
        /// </summary>
        protected virtual void OnUpdateCell(string name, DataGridViewCellFormattingEventArgs e) { }
        /// <summary>
        /// </summary>
        protected virtual void OnCurrentCellChanged() { }
        private void Gridview1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            string name = gridview1.Edit.Columns[e.ColumnIndex].Name;
            OnUpdateCell(name, e);
        }
        private void Gridview1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (this.gridview1.Edit.CurrentCell != null)
            {
                this.Index = this.gridview1.Edit.CurrentCell.RowIndex;
                long id = this.gridview1.Edit.Rows[this.Index].Cells["Id"].Value.ToLong();
                this.Info = this.List.Find(c => c.Id == id);
                OnCurrentCellChanged();
            }
            else
            {
                this.Index = -1;
                this.Info = default(T);
            }
        }
        private void Gridview1_DoubleClick(object sender, EventArgs e)
        {
            toolBar1.TClickItem("编辑");
        }

        #endregion

        #region 数据异步获取
        /// <summary>
        /// 异步查询
        /// </summary>
        protected override object OnFind(object argument)
        {
            return server.Find<T>(find);
        }
        /// <summary>
        /// 开始获取
        /// </summary>
        protected override bool QueryStart(string find = null)
        {
            if (base.QueryStart(find))
            {
                this.gridview1.Edit.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                this.gridview1.DataSource = new FindInfo();
                start = DateTime.Now;
                return true;
            }
            else
            {
                TimeSpan time = DateTime.Now.Subtract(start);
                string statu = string.Format("Loading...用时：{0:F2}秒", time.TotalSeconds);
                this.gridview1.DataSource = new FindInfo(statu);
                return false;
            }
        }
        /// <summary>
        /// 获取结果
        /// </summary>
        protected override void OnFinished(object result)
        {
            if (result is IList)
            {
                InitData(server, result as List<T>, false);
            }
            else
            {
                string statu = string.Format("数据加载失败：{0}", result);
                this.gridview1.DataSource = new FindInfo(statu);
            }
        }

        #endregion

        /// <summary>
        /// 正在加载
        /// </summary>
        [Serializable]
        internal class FindInfo
        {
            [Property(IShow = false)]
            public long Id { get; set; }

            [Property(Text = "状态")]
            public string State { get; set; }

            public FindInfo()
            {
                this.State = "正在加载...";
            }
            public FindInfo(string name)
            {
                this.State = name;
            }
        }
    }
}
