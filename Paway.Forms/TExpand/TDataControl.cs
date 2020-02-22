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
    public partial class TDataControl<T> : TBaseControl where T : IId, IFind<T>, new()
    {
        /// <summary>
        /// 数据
        /// </summary>
        protected new List<T> List;
        /// <summary>
        /// 当前查询数据
        /// </summary>
        protected List<T> FList;
        /// <summary>
        /// 查询过滤器
        /// </summary>
        private Func<T, bool> predicate;
        /// <summary>
        /// 当前选中序号
        /// </summary>
        protected int Index;
        /// <summary>
        /// 当前Info
        /// </summary>
        protected T Info;
        /// <summary>
        /// 数据过滤条件
        /// </summary>
        protected string find;
        /// <summary>
        /// 查询开始时间
        /// </summary>
        private DateTime start;
        private IDataService server;
        /// <summary>
        /// 展开树标记
        /// </summary>
        private bool iExpandAll;

        /// <summary>
        /// 构造
        /// </summary>
        public TDataControl()
        {
            InitializeComponent();
            this.toolBar1.TDirection = Paway.Helper.TDirection.Vertical;
        }
        /// <summary>
        /// ReLoad
        /// </summary>
        protected override void ReLoad(bool first)
        {
            base.ReLoad(first);
            toolBar1.MStart();
        }
        /// <summary>
        /// OnLoad
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
            gridview1.TotalEvent += UpdateDesc;
            LoadEvent();
        }
        /// <summary>
        /// 使用树结构
        /// </summary>
        protected void UserTree(bool iExpandAll = false)
        {
            if (!gridview1.UserTree()) return;
            this.iExpandAll = iExpandAll;
            LoadEvent();
        }
        private void LoadEvent()
        {
            gridview1.Edit.CellFormatting -= Gridview1_CellFormatting;
            gridview1.Edit.CurrentCellChanged -= Gridview1_CurrentCellChanged;
            gridview1.Edit.RowDoubleClick -= Gridview1_RowDoubleClick;
            gridview1.Edit.RefreshChanged -= Gridview1_RefreshChanged;
            gridview1.Edit.CellEndEdit -= TDataGridView_CellEndEdit;
            gridview1.Edit.CheckedChanged -= TDataGridView_CheckedChanged;

            gridview1.Edit.CellFormatting += Gridview1_CellFormatting;
            gridview1.Edit.CurrentCellChanged += Gridview1_CurrentCellChanged;
            gridview1.Edit.RowDoubleClick += Gridview1_RowDoubleClick;
            gridview1.Edit.RefreshChanged += Gridview1_RefreshChanged;
            gridview1.Edit.CellEndEdit += TDataGridView_CellEndEdit;
            gridview1.Edit.CheckedChanged += TDataGridView_CheckedChanged;
        }
        private void UnLoadEvent()
        {
            //可能存在已释放问题
            if (gridview1 == null || gridview1.Edit == null || gridview1.Edit.IsDisposed) return;
            gridview1.Edit.CellFormatting -= Gridview1_CellFormatting;
            gridview1.Edit.CurrentCellChanged -= Gridview1_CurrentCellChanged;
            gridview1.Edit.RefreshChanged -= Gridview1_RefreshChanged;
        }
        private void Gridview1_RefreshChanged()
        {
            if (iExpandAll && gridview1.Edit is TreeGridView treeView)
            {
                treeView.ExpandAll();
            }
            if (!gridview1.Edit.ICheckBoxName.IsNullOrEmpty())
            {
                gridview1.Edit.ReadOnly = false;
                for (int i = 0; i < gridview1.Edit.Columns.Count; i++)
                {
                    gridview1.Edit.Columns[i].ReadOnly = gridview1.Edit.Columns[i].Name != gridview1.Edit.ICheckBoxName;
                }
            }
        }
        private void TDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (gridview1.Edit.Columns[e.ColumnIndex].Name == gridview1.Edit.ICheckBoxName)
            {
                var id = (int)gridview1.Edit.Rows[e.RowIndex].Cells[gridview1.Edit.IdColumn()].Value;
                var info = List.Find(c => c.Id == id);
                if (info != null)
                {
                    var result = (bool)((DataGridViewCheckBoxCell)gridview1.Edit.Rows[e.RowIndex].Cells[e.ColumnIndex]).Value;
                    info.SetValue(gridview1.Edit.ICheckBoxName, result);
                }
            }
        }
        private void TDataGridView_CheckedChanged(bool obj)
        {
            var aList = GetAll();
            for (int i = 0; i < aList.Count; i++)
            {
                aList[i].SetValue(gridview1.Edit.ICheckBoxName, obj);
            }
        }

        #region 权限-按钮
        private void InitVerofy()
        {
            toolBar1.Items.Clear();
            RefreshItem();
        }
        /// <summary>
        /// 添加新按钮
        /// </summary>
        protected ToolItem NewItem(string name, string tag = null, Image image = null)
        {
            var item = new ToolItem(name, image) { Tag = tag ?? name };
            toolBar1.Items.Add(item);
            toolBar1.TRefresh();
            return item;
        }
        /// <summary>
        /// 添加刷新按钮(默认)
        /// </summary>
        protected virtual ToolItem RefreshItem()
        {
            return NewItem("刷新(F5)", "刷新", Resources.refresh);
        }
        /// <summary>
        /// 添加添加按钮
        /// </summary>
        protected ToolItem AddItem()
        {
            return NewItem("添加(A)", "添加", Resources.add);
        }
        /// <summary>
        /// 添加编辑按钮
        /// </summary>
        protected ToolItem AddUpdate()
        {
            return NewItem("编辑(E)", "编辑", Resources.edit);
        }
        /// <summary>
        /// 添加删除按钮
        /// </summary>
        protected ToolItem AddDelete()
        {
            return NewItem("删除(D)", "删除", Resources.close);
        }

        #endregion

        #region 加数数据与样式
        /// <summary>
        /// 初始化数据
        /// </summary>
        protected virtual void InitData(List<T> list, IDataService server = null, bool iAdd = true)
        {
            try
            {
                if (server != null) this.server = server;
                if (iAdd)
                {
                    this.List = list as List<T>;
                }
                else
                {
                    this.List.Clear();
                    this.List.AddRange(list);
                }
                this.gridview1.UpdateData(this.List, false);
                this.RefreshData();
            }
            catch (Exception ex)
            {
                ex.Show();
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
            panel2.Visible = true;
        }

        #endregion

        #region 数据绑定
        /// <summary>
        /// 数据统计
        /// </summary>
        /// <returns></returns>
        protected virtual T OnTotal(List<T> list) { return default; }
        /// <summary>
        /// 添加数据
        /// </summary>
        protected virtual T OnAdd() { return default; }
        /// <summary>
        /// 插入数据到数据库
        /// </summary>
        protected virtual bool OnAddInfo(T info)
        {
            return server.Insert(info) > 0;
        }
        /// <summary>
        /// 编辑数据
        /// </summary>
        protected virtual Form OnUpdate(T info) { return new Form(); }
        /// <summary>
        /// 更新数据到数据库
        /// </summary>
        protected virtual bool OnUpdateInfo(T info)
        {
            return server.Update(info) > 0;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        protected virtual Tuple<string, string> OnDelete(T info) { return new Tuple<string, string>("Warn", "Confirm Delete?"); }
        /// <summary>
        /// 从数据库删除数据
        /// </summary>
        protected virtual bool OnDeleteInfo(T info)
        {
            return server.Delete(info) > 0;
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
                    var find = this.find;
                    if (find == null ||
                       (find.IndexOf("order by", StringComparison.OrdinalIgnoreCase) == -1 &&
                        find.IndexOf("group by", StringComparison.OrdinalIgnoreCase) == -1))
                    {
                        find = string.Format("{0} order by " + gridview1.Edit.IdColumn(), find ?? "1=1");
                    }
                    QueryStart(find);
                    break;
                case "添加":
                    T data = OnAdd();
                    if (data != null)
                    {
                        int index = -1;
                        if (!OnAddInfo(data)) break;
                        if (this.List.Find(c => c.Id == data.Id) == null) this.List.Add(data);
                        bool iFilter = false;
                        if (IFind())
                        {
                            var fList = new List<T>() { data }.AsParallel().Where(predicate);
                            iFilter = fList.Count() > 0;
                            if (iFilter) this.FList.Add(data);
                        }
                        if (!gridview1.IGroup)
                        {
                            if (!IFind() || iFilter)
                            {
                                if (gridview1.Edit is TreeGridView treeView)
                                {
                                    index = treeView.AddNode(treeView.Nodes, data);
                                }
                                else if (gridview1.Edit is TDataGridView gridView)
                                {
                                    gridView.AddRow(data);
                                    index = this.List.Count - 1;
                                }
                            }
                            else if (!IFind())
                            {
                                index = this.FList.FindIndex(c => c.Id == data.Id);
                            }
                            RefreshTotal(true);
                            gridview1.RefreshDesc();
                            gridview1.AutoCell(index);
                        }
                        else
                        {
                            if (IFind())
                            {
                                index = this.FList.FindIndex(c => c.Id == data.Id);
                                ToFind();
                            }
                            else
                            {
                                index = this.List.FindIndex(c => c.Id == data.Id);
                                this.RefreshData();
                            }
                            gridview1.AutoLast();
                        }
                    }
                    break;
                case "编辑":
                    if (this.Info == null)
                    {
                        ExceptionHelper.Show("Please select item");
                        break;
                    }
                    if (this.Info.Id < 0) return;
                    Form update = OnUpdate(this.Info);
                    if (update != null && update.ShowDialog(this) == DialogResult.OK)
                    {
                        int index = this.Index;
                        if (!OnUpdateInfo(this.Info)) break;
                        bool iFilter = false;
                        if (IFind())
                        {
                            var fList = new List<T>() { this.Info }.AsParallel().Where(predicate);
                            iFilter = fList.Count() == 0;
                        }
                        if (gridview1.Edit is TreeGridView treeView)
                        {
                            if (iFilter)
                            {
                                this.FList.Remove(this.Info);
                                treeView.DeleteNode(treeView.Nodes, this.Info.Id);
                            }
                            else
                            {
                                treeView.UpdateNode(treeView.Nodes, this.Info, this.Info.Id);
                            }
                        }
                        else if (gridview1.Edit is TDataGridView gridView)
                        {
                            gridView.UpdateRow(this.Info, this.Info.Id);
                        }
                        RefreshTotal(true);
                        gridview1.RefreshDesc();
                    }
                    break;
                case "删除":
                    if (this.Info == null)
                    {
                        ExceptionHelper.Show("Please select item");
                        break;
                    }
                    if (this.Info.Id < 0) return;
                    var delete = OnDelete(this.Info);
                    DialogResult result = MessageBox.Show(this, delete.Item2, delete.Item1, MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    if (result == DialogResult.OK)
                    {
                        var info = this.Info.Clone();
                        int index = this.Index;
                        if (!OnDeleteInfo(this.Info)) break;
                        if (!gridview1.IGroup)
                        {
                            if (gridview1.Edit is TreeGridView treeView)
                            {
                                treeView.DeleteNode(treeView.Nodes, info.Id);
                            }
                            else if (gridview1.Edit is TDataGridView gridView)
                            {
                                gridView.DeleteRow(info.Id);
                            }
                        }
                        this.List.RemoveAll(c => c.Id == info.Id);
                        if (IFind())
                        {
                            info = this.FList.Find(c => c.Id == info.Id);
                            this.FList.Remove(info);
                        }
                        if (!gridview1.IGroup)
                        {
                            RefreshTotal(true);
                            gridview1.RefreshDesc();
                            gridview1.Edit.AutoCell(iRefresh: false);
                        }
                        else if (!ToFind(index: index))
                        {
                            this.RefreshData(true);
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="iOffset">保存滚动条位置</param>
        public void RefreshData(bool iOffset = false)
        {
            if (this.IsDisposed) return;
            RefreshTotal();
            this.gridview1.AutoCell(iOffset);
        }
        /// <summary>
        /// 数据统计
        /// </summary>
        /// <param name="iUpdate">直接更新</param>
        protected virtual void RefreshTotal(bool iUpdate = false)
        {
            T data = List.Find(c => c.Id < 0);
            if (data != null) List.Remove(data);
            data = OnTotal(List);
            if (data != null)
            {
                data.Id = -1;
                List.Add(data);
            }
            if (IFind())
            {
                data = FList.Find(c => c.Id < 0);
                if (data != null) FList.Remove(data);
                data = OnTotal(FList);
                if (data != null)
                {
                    data.Id = -1;
                    FList.Add(data);
                }
            }
            if (data != null && iUpdate && gridview1.Edit is TDataGridView gridView)
            {
                var dt = gridView.DataSource as DataTable;
                var dr = dt.Rows.Find(data.Id);
                if (dr != null)
                {
                    var drNew = dt.NewRow();
                    drNew.ItemArray = data.ToDataRow().ItemArray;
                    dt.Rows.Remove(dr);
                    dt.Rows.Add(drNew);
                }
            }
        }

        #endregion

        #region 界面快捷方式
        /// <summary>
        /// 查询
        /// </summary>
        protected virtual List<T> OnFilter(string value)
        {
            this.predicate = Activator.CreateInstance<T>().Find(value);
            return this.List.AsParallel().Where(predicate).ToList();
        }
        /// <summary>
        /// 查询完成
        /// </summary>
        protected virtual void OnFound(List<T> list)
        {
            bool iTree = gridview1.Edit is TreeGridView;
            try
            {
                if (iTree) gridview1.Edit.CurrentCellChanged -= Gridview1_CurrentCellChanged;
                this.gridview1.DataSource = list;
            }
            finally
            {
                if (iTree) gridview1.Edit.CurrentCellChanged += Gridview1_CurrentCellChanged;
            }
            if (iTree) Gridview1_CurrentCellChanged(gridview1.Edit, EventArgs.Empty);
        }
        private void TbName_TextChanged(object sender, EventArgs e)
        {
            if (!ToFind(true))
            {
                OnFound(this.List);
                this.tbName.Focus();
            }
        }
        private bool IFind()
        {
            if (!this.ILoad || this.IsDisposed) return false;
            return (bool)this.Invoke(new Func<bool>(() =>
            {
                return panel2.Visible && !tbName.IError && !tbName.Text.Trim().IsNullOrEmpty();
            }));
        }
        private bool ToFind(bool focus = false, int index = 0)
        {
            if (!IFind()) return false;
            this.FList = OnFilter(tbName.Text.Trim());
            RefreshTotal();
            var offset = gridview1.Edit.FirstDisplayedScrollingRowIndex;
            OnFound(this.FList);
            gridview1.AutoCell(index);
            if (index > 0) gridview1.Edit.SetOffsetRowIndex(offset);
            if (focus) this.tbName.Focus();
            return true;
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
        /// 按键
        /// </summary>
        protected virtual bool OnKeyDown(Keys key)
        {
            switch (key)
            {
                case (Keys)Shortcut.CtrlF:
                    this.tbName.Focus();
                    break;
                case (Keys)Shortcut.CtrlA:
                case (Keys)Shortcut.CtrlC:
                case Keys.Enter:
                    if (panel2.Visible && tbName.ContainsFocus) return false;
                    if (gridview1.TPager.ITextFocus) return false;
                    break;
            }
            switch (key)
            {
                case Keys.F5:
                    toolBar1.TClickItem("刷新");
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
        /// 按键
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (OnKeyDown(keyData)) return true;
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        #region 快捷显示
        /// <summary>
        /// 单元格设置CellFormatting
        /// </summary>
        protected virtual void OnUpdateCell(string name, DataGridViewCellFormattingEventArgs e) { }
        /// <summary>
        /// 单元格焦点切换事件
        /// </summary>
        protected virtual void OnCurrentCellChanged() { }
        /// <summary>
        /// 行双击事件
        /// </summary>
        protected virtual bool OnRowDoubleClick(int rowIndex) { return false; }
        private void Gridview1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            string name = gridview1.Edit.Columns[e.ColumnIndex].Name;
            OnUpdateCell(name, e);
        }
        /// <summary>
        /// 单元格切换触发选中行实体更新
        /// </summary>
        private void Gridview1_CurrentCellChanged(object sender, EventArgs e)
        {
            TDataGridView gridview = sender as TDataGridView;
            if (gridview.CurrentCell != null)
            {
                this.Index = gridview.CurrentCell.RowIndex;
                var id = gridview.Rows[this.Index].Cells[gridview.IdColumn()].Value.ToInt();
                this.Info = this.List.Find(c => c.Id == id);
                OnCurrentCellChanged();
            }
            else
            {
                this.Index = -1;
                this.Info = default;
            }
        }
        /// <summary>
        /// 双击触发编辑方法
        /// </summary>
        private void Gridview1_RowDoubleClick(int rowIndex)
        {
            if (OnRowDoubleClick(rowIndex)) return;
            toolBar1.TClickItem("编辑");
        }

        #endregion

        #region 数据异步获取
        /// <summary>
        /// 异步查询
        /// </summary>
        protected override object OnFind(string find)
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
                string statu = string.Format(TConfig.Loading + " Time：{0:F2}s", time.TotalSeconds);
                this.gridview1.DataSource = new FindInfo(statu);
                return false;
            }
        }
        /// <summary>
        /// 获取结果
        /// </summary>
        protected override void OnFinished(object result)
        {
            if (result is List<T> list)
            {
                InitData(list, iAdd: false);
            }
            else
            {
                string statu = string.Format(TConfig.Loading + " Error：{0}", result);
                this.gridview1.DataSource = new FindInfo(statu);
            }
        }
        /// <summary>
        /// 更新统计描述
        /// </summary>
        protected virtual string UpdateDesc(object obj) { return null; }
        /// <summary>
        /// 获取当前界面数据List
        /// </summary>
        protected List<T> GetAll()
        {
            var dt = gridview1.DataSource;
            List<T> aList = new List<T>();
            if (dt is List<T> factoryList)
            {
                aList = factoryList;
            }
            else if (dt is List<object> objList)
            {
                aList = objList.ConvertAll(c => (T)c);
            }
            return aList;
        }
        /// <summary>
        /// 获取当前选择数据
        /// </summary>
        protected virtual List<T> GetSelect()
        {
            var aList = GetAll();
            if (!gridview1.Edit.ICheckBoxName.IsNullOrEmpty())
            {
                var iList = aList.FindAll(gridview1.Edit.ICheckBoxName, true);
                if (iList.Count > 0) return iList as List<T>;
            }
            List<T> list = new List<T>();
            for (int i = 0; i < gridview1.Edit.SelectedRows.Count; i++)
            {
                var id = gridview1.Edit.SelectedRows[i].Cells[gridview1.Edit.IdColumn()].Value.ToInt();
                var info = this.List.Find(c => c.Id == id);
                if (info != null) list.Add(info);
            }
            if (list.Count == 0 && this.Info != null)
            {
                list.Add(this.Info);
            }
            return list;
        }

        #endregion
    }
}
