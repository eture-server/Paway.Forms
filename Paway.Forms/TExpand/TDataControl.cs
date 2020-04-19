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
        /// 搜索状态
        /// </summary>
        protected bool IFilter;

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
            this.gridview1.DataSource = this.List;
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
        protected void UserTree(bool iExpandAll = false, bool iDoubleExpand = true)
        {
            if (!gridview1.UserTree(iExpandAll, iDoubleExpand)) return;
            LoadEvent();
        }
        private void LoadEvent()
        {
            UnLoadEvent();
            gridview1.Edit.CellFormatting += Gridview1_CellFormatting;
            gridview1.Edit.CurrentCellChanged += Gridview1_CurrentCellChanged;
            gridview1.Edit.RowDoubleClick += Gridview1_RowDoubleClick;
            gridview1.Edit.RefreshChanged += Gridview1_RefreshChanged;
            gridview1.Edit.CheckedChanged += Gridview1_CheckedChanged;
            gridview1.Edit.CellClick += Gridview1_CellClick;
            gridview1.Edit.ButtonClicked += Gridview1_ButtonClicked;
        }
        private void UnLoadEvent()
        {
            //可能存在已释放问题
            if (gridview1 == null || gridview1.Edit == null || gridview1.Edit.IsDisposed) return;
            gridview1.Edit.CellFormatting -= Gridview1_CellFormatting;
            gridview1.Edit.CurrentCellChanged -= Gridview1_CurrentCellChanged;
            gridview1.Edit.RowDoubleClick -= Gridview1_RowDoubleClick;
            gridview1.Edit.RefreshChanged -= Gridview1_RefreshChanged;
            gridview1.Edit.CheckedChanged -= Gridview1_CheckedChanged;
            gridview1.Edit.CellClick -= Gridview1_CellClick;
            gridview1.Edit.ButtonClicked -= Gridview1_ButtonClicked;
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
        protected ToolItem NewItem(string name, Shortcut keys = Shortcut.None, Image image = null)
        {
            return NewItem(name, null, keys, image);
        }
        /// <summary>
        /// 添加新按钮
        /// </summary>
        protected ToolItem NewItem(string name, string tag = null, Shortcut keys = Shortcut.None, Image image = null)
        {
            var item = new ToolItem(name, keys, image) { Tag = tag ?? name };
            toolBar1.Items.Add(item);
            toolBar1.TRefresh();
            return item;
        }
        /// <summary>
        /// 添加刷新按钮(默认)
        /// </summary>
        protected virtual ToolItem RefreshItem()
        {
            return NewItem("刷新", Shortcut.F5, Resources.refresh);
        }
        /// <summary>
        /// 添加添加按钮
        /// </summary>
        protected ToolItem AddItem()
        {
            return NewItem("添加", Shortcut.CtrlA, Resources.add);
        }
        /// <summary>
        /// 添加编辑按钮
        /// </summary>
        protected ToolItem AddUpdate()
        {
            return NewItem("编辑", Shortcut.CtrlE, Resources.edit);
        }
        /// <summary>
        /// 添加删除按钮
        /// </summary>
        protected ToolItem AddDelete()
        {
            return NewItem("删除", Shortcut.CtrlD, Resources.close);
        }

        #endregion

        #region 加数数据与样式
        /// <summary>
        /// 初始化数据
        /// </summary>
        protected virtual void InitData(List<T> list, IDataService server = null, bool iInit = true)
        {
            try
            {
                if (server != null) this.server = server;
                if (list == null) return;
                if (iInit)
                {
                    this.List = list as List<T>;
                }
                else
                {
                    this.List.Clear();
                    this.List.AddRange(list);
                }
                this.IFilter = false;
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
        protected virtual Form OnUpdate(T info) { return null; }
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
                            iFilter = !IFindText() || new List<T>() { data }.AsParallel().Where(predicate).Count() > 0;
                            if (iFilter)
                            {
                                if (this.FList.Find(c => c.Id == data.Id) == null) this.FList.Add(data);
                            }
                        }
                        if (gridview1.IGroup != GroupType.Group)
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
                            iFilter = predicate != null && new List<T>() { this.Info }.AsParallel().Where(predicate).Count() == 0;
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
                        if (gridview1.IGroup != GroupType.Group)
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
                        if (gridview1.IGroup != GroupType.Group)
                        {
                            RefreshTotal(true);
                            gridview1.RefreshDesc();
                            gridview1.Edit.AutoCell(iRefresh: false);
                        }
                        else if (IFind())
                        {
                            ToFind(index: index);
                        }
                        else
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
            List.RemoveAll(c => c.Id < 0);
            var data = OnTotal(List);
            if (data != null)
            {
                data.Id = -1;
                List.Add(data);
            }
            if (IFind())
            {
                FList.RemoveAll(c => c.Id < 0);
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
        protected virtual void OnFound(List<T> list, bool iFilter = true)
        {
            this.IFilter = iFilter;
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
            if (IFindText())
            {
                ToFind(true);
            }
            else
            {
                OnFound(this.List, false);
                this.tbName.Focus();
            }
        }
        private bool IFind()
        {
            return IFilter || IFindText();
        }
        private bool IFindText()
        {
            if (!this.ILoad || this.IsDisposed) return false;
            return (bool)this.Invoke(new Func<bool>(() =>
            {
                return panel2.Visible && !tbName.IError && !tbName.Text.Trim().IsNullOrEmpty();
            }));
        }
        private bool ToFind(bool focus = false, int index = 0)
        {
            if (focus) this.FList = OnFilter(tbName.Text.Trim());
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
                case Keys.Back:
                    if (panel2.Visible && tbName.ContainsFocus) return false;
                    if (gridview1.TPager.ITextFocus) return false;
                    break;
            }
            var item = toolBar1.Items.Show.FirstOrDefault(c => c.Keys == key);
            if (item != null)
            {
                toolBar1.TClickItem(item);
                return false;
            }
            switch (key)
            {
                case Keys.F5:
                    toolBar1.TClickItem("刷新");
                    break;
                case (Keys)Shortcut.CtrlA:
                    toolBar1.TClickItem("添加");
                    break;
                case Keys.Back:
                case (Keys)Shortcut.CtrlB:
                    toolBar1.TClickItem("返回");
                    break;
                case Keys.Delete:
                case (Keys)Shortcut.CtrlD:
                    toolBar1.TClickItem("删除");
                    break;
                case Keys.Enter:
                case (Keys)Shortcut.CtrlE:
                    toolBar1.TClickItem("编辑");
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
        /// <summary>
        /// 数据刷新事件
        /// </summary>
        protected virtual void OnRefreshChanged() { }
        /// <summary>
        /// 行单击事件
        /// </summary>
        protected virtual void OnCellClick(T info, string name) { }
        /// <summary>
        /// 按钮单击事件
        /// </summary>
        protected virtual void OnButtonClicked(T info, string name, object value) { }
        private void Gridview1_ButtonClicked(int rowIndex, int columnIndex, object value)
        {
            if (rowIndex == -1 || columnIndex == -1) return;
            try
            {
                string name = gridview1.Edit.Columns[columnIndex].Name;
                OnButtonClicked(this.Info, name, value);
            }
            catch (Exception ex)
            {
                ex.Show();
            }
        }
        private void Gridview1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1) return;
            try
            {
                string name = gridview1.Edit.Columns[e.ColumnIndex].Name;
                OnCellClick(this.Info, name);
            }
            catch (Exception ex)
            {
                ex.Show();
            }
        }
        private void Gridview1_CheckedChanged(int rowIndex, int columnIndex, bool value)
        {
            if (rowIndex != -1)
            {
                if (this.Info != null)
                {
                    var name = gridview1.Edit.Columns[columnIndex].Name;
                    this.Info.SetValue(name, value);
                }
            }
            else
            {
                var aList = GetAll();
                var name = gridview1.Edit.Columns[columnIndex].Name;
                for (int i = 0; i < aList.Count; i++)
                {
                    aList[i].SetValue(name, value);
                }
            }
        }
        private void Gridview1_RefreshChanged(TDataGridView gridview1)
        {
            try
            {
                OnRefreshChanged();
            }
            catch (Exception ex)
            {
                ex.Show();
            }
        }
        /// <summary>
        /// 双击触发编辑方法
        /// </summary>
        private void Gridview1_RowDoubleClick(TDataGridView gridView, int rowIndex)
        {
            if (OnRowDoubleClick(rowIndex)) return;
            toolBar1.TClickItem("编辑");
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
                try
                {
                    OnCurrentCellChanged();
                }
                catch (Exception ex)
                {
                    ex.Show();
                }
            }
            else
            {
                this.Index = -1;
                this.Info = default;
            }
        }
        private void Gridview1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            string name = gridview1.Edit.Columns[e.ColumnIndex].Name;
            OnUpdateCell(name, e);
        }

        #endregion

        #region 数据异步获取
        /// <summary>
        /// 开始获取
        /// </summary>
        protected override bool QueryStart(string find = null)
        {
            tbName.TextChanged -= TbName_TextChanged;
            tbName.Text = null;
            tbName.TextChanged += TbName_TextChanged;
            var findBy = find ?? this.find;
            if (findBy == null ||
               (findBy.IndexOf("order by", StringComparison.OrdinalIgnoreCase) == -1 &&
                findBy.IndexOf("group by", StringComparison.OrdinalIgnoreCase) == -1))
            {
                findBy = string.Format("{0} order by " + gridview1.Edit.IdColumn(), findBy ?? "1=1");
            }

            if (base.QueryStart(findBy))
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
        /// 异步查询
        /// </summary>
        protected override object OnFind(string find)
        {
            return server.Find<T>(find);
        }
        /// <summary>
        /// 获取结果
        /// </summary>
        protected override void OnFinished(object result)
        {
            if (result is List<T> list)
            {
                InitData(list, null, false);
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
        protected virtual List<T> GetSelect(string name = null)
        {
            var aList = GetAll();
            if (name != null)
            {
                var iList = aList.FindAll(name, true);
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
