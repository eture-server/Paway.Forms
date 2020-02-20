using Paway.Helper;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Paway.Forms
{
    /// <summary>
    /// TreeGridView
    /// </summary>
    public class TreeGridView : TDataGridView
    {
        private readonly TreeGridNode _root;
        private TreeGridColumn _expandableColumn;
        private bool _inExpandCollapse;
        private bool _showLines = true;
        private Control hideScrollBarControl;

        internal ImageList _imageList;
        internal bool _inExpandCollapseMouseCapture;
        internal VisualStyleRenderer rClosed = new VisualStyleRenderer(VisualStyleElement.TreeView.Glyph.Closed);
        internal VisualStyleRenderer rOpen = new VisualStyleRenderer(VisualStyleElement.TreeView.Glyph.Opened);

        #region 事件
        /// <summary>
        /// 节点展开开始事件
        /// </summary>
        public event Action<object, ExpandingEventArgs> NodeExpanding;
        /// <summary>
        /// 节点展开完成事件
        /// </summary>
        public event Action<object, TreeGridNode> NodeCollapsed;
        /// <summary>
        /// 节点关闭开始事件
        /// </summary>
        public event Action<object, CollapsingEventArgs> NodeCollapsing;
        /// <summary>
        /// 节点关闭完成事件
        /// </summary>
        public event Action<object, TreeGridNode> NodeExpanded;
        private void OnNodeCollapsed(TreeGridNode node)
        {
            this.NodeCollapsed?.Invoke(this, node);
        }
        private void OnNodeCollapsing(CollapsingEventArgs e)
        {
            this.NodeCollapsing?.Invoke(this, e);
        }
        private void OnNodeExpanded(TreeGridNode node)
        {
            this.NodeExpanded?.Invoke(this, node);
        }
        private void OnNodeExpanding(ExpandingEventArgs e)
        {
            this.NodeExpanding?.Invoke(this, e);
        }

        #endregion

        /// <summary>
        /// 构造
        /// </summary>
        public TreeGridView()
        {
            this.RowTemplate = new TreeGridNode();
            this._root = new TreeGridNode(this)
            {
                IsRoot = true
            };
            //注释掉会报错
            base.Rows.CollectionChanged += delegate { };
            this.RowDoubleClick += TreeGridView_RowDoubleClick;
        }

        #region 自动绑定数据
        /// <summary>
        /// 设置树形列名称
        /// </summary>
        [Browsable(true), Description("自定义树形显示列")]
        public string TextColumn { get; set; }
        /// <summary>
        /// 双击展开节点
        /// </summary>
        [Browsable(true), Description("双击展开节点")]
        [DefaultValue(true)]
        public bool IDoubleExpand { get; set; } = true;

        /// <summary>
        /// 自动设置节点数据
        /// </summary>
        protected override void UpdateData(object value)
        {
            Nodes.Clear();
            if (value == null) return;
            Type type;
            if (value is IList list)
            {
                type = list.GenericType();
                AutoNodes(type, list);
            }
            else
            {
                type = value.GetType();
                if (type.IsClass && type != typeof(string))
                {
                    var temp = type.GenericList();
                    temp.Add(value);
                    AutoNodes(type, temp);
                }
            }
            OnRefreshChanged();
        }
        private void AutoNodes(Type type, IList list)
        {
            AutoColumns(type);
            var tempList = list.FindAll(nameof(IParent.ParentId), 0);
            foreach (var temp in tempList)
            {
                var node = Nodes.Add(temp.GetValues());
                AddNodes(type, list, node, (int)temp.GetValue(nameof(IParent.Id)));
            }
        }
        private void AddNodes(Type type, IList list, TreeGridNode parent, int parentId)
        {
            var tempList = list.FindAll(nameof(IParent.ParentId), parentId);
            foreach (var temp in tempList)
            {
                var node = parent.Nodes.Add(temp.GetValues());
                AddNodes(type, list, node, (int)temp.GetValue(nameof(IParent.Id)));
            }
        }
        private void AutoColumns(Type type)
        {
            if (type == null || type == typeof(string) || type.IsValueType) return;
            Type iType = type.GetInterface(typeof(IParent).FullName);
            if (iType == null) throw new ArgumentException("Data type error, interface not implemented: IParentId.");

            Columns.Clear();
            var iTree = false;
            var iColumn = TextColumn != null && type.Property(TextColumn) != null;
            foreach (var property in type.PropertiesValue())
            {
                var visible = property.IShow();
                DataGridViewColumn column;
                if (!iTree && ((!iColumn && visible) || (iColumn && TextColumn.Equals(property.Name, StringComparison.OrdinalIgnoreCase))))
                {
                    iTree = true;
                    column = new TreeGridColumn();
                }
                else
                {
                    Type dbType = property.PropertyType;
                    if (dbType.IsGenericType) dbType = Nullable.GetUnderlyingType(dbType);
                    if (dbType == typeof(Image) || dbType == typeof(Bitmap))
                        column = new DataGridViewImageColumn();
                    else column = new DataGridViewTextBoxColumn();
                }
                column.Visible = visible;
                column.HeaderText = property.TextName();
                column.Name = property.Name;
                Columns.Add(column);
            }
        }
        /// <summary>
        /// 展开所有节点
        /// </summary>
        public void ExpandAll()
        {
            ExpandAll(this.Nodes);
        }
        private void ExpandAll(TreeGridNodeCollection nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Nodes.Count > 0)
                {
                    nodes[i].Expand();
                    ExpandAll(nodes[i].Nodes);
                }
            }
        }
        /// <summary>
        /// 关闭(折叠)所有节点
        /// </summary>
        public void CollapseAll()
        {
            CollapseAll(this.Nodes);
        }
        private void CollapseAll(TreeGridNodeCollection nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Nodes.Count > 0)
                {
                    CollapseAll(nodes[i].Nodes);
                    nodes[i].Collapse();
                }
            }
        }

        /// <summary>
        /// 添加节点
        /// </summary>
        internal int AddNode(TreeGridNodeCollection nodes, object info)
        {
            var parentId = (int)info.GetValue(nameof(IParent.ParentId));
            if (parentId == 0)
            {
                return nodes.Add(info.GetValues()).RowIndex;
            }
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Cells[IdColumn()].Value.ToInt() == parentId)
                {
                    if (!nodes[i].IsExpanded) nodes[i].Expand();
                    return nodes[i].Nodes.Add(info.GetValues()).RowIndex;
                }
                if (nodes[i].Nodes.Count > 0)
                {
                    if (!nodes[i].IsExpanded) nodes[i].Expand();
                    var index = AddNode(nodes[i].Nodes, info);
                    if (index > -1) return index;
                }
            }
            return -1;
        }
        /// <summary>
        /// 更新节点
        /// </summary>
        public bool UpdateNode(object info)
        {
            return UpdateNode(this.Nodes, info, (int)info.GetValue(nameof(IParent.Id)));
        }
        /// <summary>
        /// 更新节点
        /// </summary>
        internal bool UpdateNode(TreeGridNodeCollection nodes, object info, int id)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Cells[IdColumn()].Value.ToInt() == id)
                {
                    nodes[i].Update(info);
                    return true;
                }
                if (nodes[i].IsExpanded && nodes[i].Nodes.Count > 0) if (UpdateNode(nodes[i].Nodes, info, id)) return true;
            }
            return false;
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        internal bool DeleteNode(TreeGridNodeCollection nodes, int id)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Cells[IdColumn()].Value.ToInt() == id)
                {
                    nodes.RemoveAt(i);
                    return true;
                }
                if (nodes[i].IsExpanded && nodes[i].Nodes.Count > 0) if (DeleteNode(nodes[i].Nodes, id)) return true;
            }
            return false;
        }

        #endregion

        #region 内部
        internal virtual bool ExpandNode(TreeGridNode node)
        {
            if (node.IsExpanded && !this.VirtualNodes)
            {
                return false;
            }
            ExpandingEventArgs e = new ExpandingEventArgs(node);
            this.OnNodeExpanding(e);
            if (!e.Cancel)
            {
                this.LockVerticalScrollBarUpdate(true);
                base.SuspendLayout();
                this._inExpandCollapse = true;
                node.IsExpanded = true;
                foreach (TreeGridNode node2 in node.Nodes)
                {
                    this.SiteNode(node2);
                }
                this.OnNodeExpanded(node);
                this._inExpandCollapse = false;
                this.LockVerticalScrollBarUpdate(false);
                base.ResumeLayout(true);
                base.InvalidateCell(node.Cells[0]);
            }
            return !e.Cancel;
        }
        internal virtual bool CollapseNode(TreeGridNode node)
        {
            if (!node.IsExpanded)
            {
                return false;
            }
            CollapsingEventArgs e = new CollapsingEventArgs(node);
            this.OnNodeCollapsing(e);
            if (!e.Cancel)
            {
                this.LockVerticalScrollBarUpdate(true);
                base.SuspendLayout();
                this._inExpandCollapse = true;
                node.IsExpanded = false;
                foreach (TreeGridNode node2 in node.Nodes)
                {
                    this.UnSiteNode(node2);
                }
                this.OnNodeCollapsed(node);
                this._inExpandCollapse = false;
                this.LockVerticalScrollBarUpdate(false);
                base.ResumeLayout(true);
                base.InvalidateCell(node.Cells[0]);
            }
            return !e.Cancel;
        }
        private void LockVerticalScrollBarUpdate(bool lockUpdate)
        {
            if (!this._inExpandCollapse)
            {
                if (lockUpdate)
                {
                    base.VerticalScrollBar.Parent = this.hideScrollBarControl;
                }
                else
                {
                    base.VerticalScrollBar.Parent = this;
                }
            }
        }
        internal virtual void SiteNode(TreeGridNode node)
        {
            TreeGridNode parent;
            int index;
            node._grid = this;
            if ((node.Parent != null) && !node.Parent.IsRoot)
            {
                if (node.Index > 0)
                {
                    parent = node.Parent.Nodes[node.Index - 1];
                }
                else
                {
                    parent = node.Parent;
                }
            }
            else if (node.Index > 0)
            {
                parent = node.Parent.Nodes[node.Index - 1];
            }
            else
            {
                parent = null;
            }
            if (parent != null)
            {
                while (parent.Level >= node.Level)
                {
                    if (parent.RowIndex >= (base.Rows.Count - 1))
                    {
                        break;
                    }
                    parent = base.Rows[parent.RowIndex + 1] as TreeGridNode;
                }
                if (parent == node.Parent)
                {
                    index = parent.RowIndex + 1;
                }
                else if (parent.Level < node.Level)
                {
                    index = parent.RowIndex;
                }
                else
                {
                    index = parent.RowIndex + 1;
                }
            }
            else
            {
                index = 0;
            }
            this.SiteNode(node, index);
            if (node.IsExpanded)
            {
                foreach (TreeGridNode node3 in node.Nodes)
                {
                    this.SiteNode(node3);
                }
            }
        }
        internal virtual void SiteNode(TreeGridNode node, int index)
        {
            if (index < base.Rows.Count)
            {
                base.Rows.Insert(index, node);
            }
            else
            {
                base.Rows.Add(node);
            }
        }
        internal void UnSiteAll()
        {
            this.UnSiteNode(this._root);
        }
        internal virtual void UnSiteNode(TreeGridNode node)
        {
            if (node.IsSited || node.IsRoot)
            {
                foreach (TreeGridNode node2 in node.Nodes)
                {
                    this.UnSiteNode(node2);
                }
                if (!node.IsRoot)
                {
                    base.Rows.Remove(node);
                    node.UnSited();
                }
            }
        }
        private void TreeGridView_RowDoubleClick(int obj)
        {
            if (IDoubleExpand && this.CurrentNode != null && this.CurrentNode.Nodes.Count > 0)
            {
                if (!this.CurrentNode.IsExpanded) this.CurrentNode.Expand();
                else this.CurrentNode.Collapse();
            }
        }

        #endregion

        #region 重载
        /// <summary>
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(base.Disposing);
            if (hideScrollBarControl != null) hideScrollBarControl.Dispose();
            this.UnSiteAll();
        }
        /// <summary>
        /// </summary>
        protected override void OnColumnAdded(DataGridViewColumnEventArgs e)
        {
            if (typeof(TreeGridColumn).IsAssignableFrom(e.Column.GetType()) && (this._expandableColumn == null))
            {
                this._expandableColumn = (TreeGridColumn)e.Column;
            }
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
            base.OnColumnAdded(e);
        }
        /// <summary>
        /// </summary>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.hideScrollBarControl = new Control
            {
                Visible = false,
                Enabled = false,
                TabStop = false
            };
            base.Controls.Add(this.hideScrollBarControl);
        }
        /// <summary>
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (!e.Handled)
            {
                if (((e.KeyCode == Keys.F2) && (base.CurrentCellAddress.X > -1)) && (base.CurrentCellAddress.Y > -1))
                {
                    if (!base.CurrentCell.Displayed)
                    {
                        base.FirstDisplayedScrollingRowIndex = base.CurrentCellAddress.Y;
                    }
                    base.SelectionMode = DataGridViewSelectionMode.CellSelect;
                    this.BeginEdit(true);
                }
                else if ((e.KeyCode == Keys.Enter) && !base.IsCurrentCellInEditMode)
                {
                    base.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    base.CurrentCell.OwningRow.Selected = true;
                }
            }
        }
        /// <summary>
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!this._inExpandCollapseMouseCapture)
            {
                base.OnMouseMove(e);
            }
        }
        /// <summary>
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            this._inExpandCollapseMouseCapture = false;
        }
        /// <summary>
        /// </summary>
        protected override void OnRowEnter(DataGridViewCellEventArgs e)
        {
            base.OnRowEnter(e);
            if ((base.SelectionMode == DataGridViewSelectionMode.CellSelect) || ((base.SelectionMode == DataGridViewSelectionMode.FullRowSelect) && !base.Rows[e.RowIndex].Selected))
            {
                base.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                base.Rows[e.RowIndex].Selected = true;
            }
        }
        /// <summary>
        /// </summary>
        protected override void OnRowsAdded(DataGridViewRowsAddedEventArgs e)
        {
            base.OnRowsAdded(e);
            for (int i = e.RowCount - 1; i >= 0; i--)
            {
                if (base.Rows[e.RowIndex + i] is TreeGridNode node)
                {
                    node.Sited();
                }
            }
        }

        #endregion

        #region public
        /// <summary>
        /// </summary>
        [Description("Returns the TreeGridNode for the given DataGridViewRow")]
        public TreeGridNode GetNodeForRow(int index) => this.GetNodeForRow(base.Rows[index]);
        /// <summary>
        /// </summary>
        [Description("Returns the TreeGridNode for the given DataGridViewRow")]
        public TreeGridNode GetNodeForRow(DataGridViewRow row) => (row as TreeGridNode);
        /// <summary>
        /// </summary>
        public TreeGridNode CurrentNode => this.CurrentRow;
        /// <summary>
        /// </summary>
        public new TreeGridNode CurrentRow => (base.CurrentRow as TreeGridNode);
        /// <summary>
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public new object DataMember
        {
            get => null;
            set
            {
                throw new NotSupportedException("The TreeGridView does not support databinding.");
            }
        }
        /// <summary>
        /// </summary>
        public ImageList ImageList
        {
            get
            {
                return this._imageList;
            }
            set
            {
                this._imageList = value;
            }
        }
        /// <summary>
        /// </summary>
        public TreeGridNodeCollection Nodes => this._root.Nodes;
        /// <summary>
        /// </summary>
        public new int RowCount
        {
            get
            {
                return this.Nodes.Count;
            }
            set
            {
                for (int i = 0; i < value; i++)
                {
                    this.Nodes.Add(new TreeGridNode());
                }
            }
        }
        /// <summary>
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public new DataGridViewRowCollection Rows => base.Rows;
        /// <summary>
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public new DataGridViewRow RowTemplate
        {
            get
            {
                return base.RowTemplate;
            }
            set
            {
                base.RowTemplate = value;
            }
        }
        /// <summary>
        /// </summary>
        [DefaultValue(true)]
        public bool ShowLines
        {
            get
            {
                return this._showLines;
            }
            set
            {
                if (value != this._showLines)
                {
                    this._showLines = value;
                    base.Invalidate();
                }
            }
        }
        /// <summary>
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool VirtualMode
        {
            get
            {
                return false;
            }
            set
            {
                throw new NotSupportedException("The TreeGridView does not support virtual mode.");
            }
        }
        /// <summary>
        /// </summary>
        [Description("Causes nodes to always show as expandable. Use the NodeExpanding event to add nodes."), DefaultValue(false)]
        public bool VirtualNodes { get; set; }

        #endregion
    }
}

