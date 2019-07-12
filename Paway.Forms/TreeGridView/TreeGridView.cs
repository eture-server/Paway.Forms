using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Paway.Forms
{
    public class TreeGridView : TDataGridView
    {
        private TreeGridColumn _expandableColumn;
        internal ImageList _imageList;
        private bool _inExpandCollapse;
        internal bool _inExpandCollapseMouseCapture;
        private TreeGridNode _root;
        private bool _showLines = true;
        private Control hideScrollBarControl;
        internal VisualStyleRenderer rClosed = new VisualStyleRenderer(VisualStyleElement.TreeView.Glyph.Closed);
        internal VisualStyleRenderer rOpen = new VisualStyleRenderer(VisualStyleElement.TreeView.Glyph.Opened);

        [field: CompilerGenerated]
        public event Action<object, CollapsedEventArgs> NodeCollapsed;

        [field: CompilerGenerated]
        public event Action<object, CollapsingEventArgs> NodeCollapsing;

        [field: CompilerGenerated]
        public event Action<object, ExpandedEventArgs> NodeExpanded;

        [field: CompilerGenerated]
        public event Action<object, ExpandingEventArgs> NodeExpanding;

        public TreeGridView()
        {
            base.EditMode = DataGridViewEditMode.EditProgrammatically;
            this.RowTemplate = new TreeGridNode();

            base.AllowUserToAddRows = false;
            base.AllowUserToDeleteRows = false;
            this._root = new TreeGridNode(this);
            this._root.IsRoot = true;
            //注释掉会报错
            base.Rows.CollectionChanged += delegate { };
        }

        protected internal virtual bool CollapseNode(TreeGridNode node)
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
                CollapsedEventArgs args2 = new CollapsedEventArgs(node);
                this.OnNodeCollapsed(args2);
                this._inExpandCollapse = false;
                this.LockVerticalScrollBarUpdate(false);
                base.ResumeLayout(true);
                base.InvalidateCell(node.Cells[0]);
            }
            return !e.Cancel;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(base.Disposing);
            this.UnSiteAll();
        }

        protected internal virtual bool ExpandNode(TreeGridNode node)
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
                ExpandedEventArgs args2 = new ExpandedEventArgs(node);
                this.OnNodeExpanded(args2);
                this._inExpandCollapse = false;
                this.LockVerticalScrollBarUpdate(false);
                base.ResumeLayout(true);
                base.InvalidateCell(node.Cells[0]);
            }
            return !e.Cancel;
        }

        [Description("Returns the TreeGridNode for the given DataGridViewRow")]
        public TreeGridNode GetNodeForRow(int index) => this.GetNodeForRow(base.Rows[index]);

        [Description("Returns the TreeGridNode for the given DataGridViewRow")]
        public TreeGridNode GetNodeForRow(DataGridViewRow row) => (row as TreeGridNode);

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

        protected override void OnColumnAdded(DataGridViewColumnEventArgs e)
        {
            if (typeof(TreeGridColumn).IsAssignableFrom(e.Column.GetType()) && (this._expandableColumn == null))
            {
                this._expandableColumn = (TreeGridColumn)e.Column;
            }
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
            base.OnColumnAdded(e);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.hideScrollBarControl = new Control();
            this.hideScrollBarControl.Visible = false;
            this.hideScrollBarControl.Enabled = false;
            this.hideScrollBarControl.TabStop = false;
            base.Controls.Add(this.hideScrollBarControl);
        }

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

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!this._inExpandCollapseMouseCapture)
            {
                base.OnMouseMove(e);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            this._inExpandCollapseMouseCapture = false;
        }

        protected virtual void OnNodeCollapsed(CollapsedEventArgs e)
        {
            if (this.NodeCollapsed != null)
            {
                this.NodeCollapsed(this, e);
            }
        }

        protected virtual void OnNodeCollapsing(CollapsingEventArgs e)
        {
            if (this.NodeCollapsing != null)
            {
                this.NodeCollapsing(this, e);
            }
        }

        protected virtual void OnNodeExpanded(ExpandedEventArgs e)
        {
            if (this.NodeExpanded != null)
            {
                this.NodeExpanded(this, e);
            }
        }

        protected virtual void OnNodeExpanding(ExpandingEventArgs e)
        {
            if (this.NodeExpanding != null)
            {
                this.NodeExpanding(this, e);
            }
        }

        protected override void OnRowEnter(DataGridViewCellEventArgs e)
        {
            base.OnRowEnter(e);
            if ((base.SelectionMode == DataGridViewSelectionMode.CellSelect) || ((base.SelectionMode == DataGridViewSelectionMode.FullRowSelect) && !base.Rows[e.RowIndex].Selected))
            {
                base.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                base.Rows[e.RowIndex].Selected = true;
            }
        }

        protected override void OnRowsAdded(DataGridViewRowsAddedEventArgs e)
        {
            base.OnRowsAdded(e);
            for (int i = e.RowCount - 1; i >= 0; i--)
            {
                TreeGridNode node = base.Rows[e.RowIndex + i] as TreeGridNode;
                if (node != null)
                {
                    node.Sited();
                }
            }
        }

        protected internal virtual void SiteNode(TreeGridNode node)
        {
            TreeGridNode parent;
            int index = -1;
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

        protected internal virtual void SiteNode(TreeGridNode node, int index)
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

        protected internal void UnSiteAll()
        {
            this.UnSiteNode(this._root);
        }

        protected internal virtual void UnSiteNode(TreeGridNode node)
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

        public TreeGridNode CurrentNode => this.CurrentRow;

        public TreeGridNode CurrentRow => (base.CurrentRow as TreeGridNode);

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public object DataMember
        {
            get => null;
            set
            {
                throw new NotSupportedException("The TreeGridView does not support databinding");
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public object DataSource
        {
            get
            {
                return null;
            }
            set
            {
                throw new NotSupportedException("The TreeGridView does not support databinding");
            }
        }

        public System.Windows.Forms.ImageList ImageList
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

        public TreeGridNodeCollection Nodes => this._root.Nodes;

        public int RowCount
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public DataGridViewRowCollection Rows => base.Rows;

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewRow RowTemplate
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

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public bool VirtualMode
        {
            get
            {
                return false;
            }
            set
            {
                throw new NotSupportedException("The TreeGridView does not support virtual mode");
            }
        }

        [Description("Causes nodes to always show as expandable. Use the NodeExpanding event to add nodes."), DefaultValue(false)]
        public bool VirtualNodes { get; set; }
    }
}

