using Paway.Helper;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    /// TreeGridNode
    /// </summary>
    [ToolboxItem(false), DesignTimeVisible(false)]
    public class TreeGridNode : DataGridViewRow
    {
        internal TreeGridView _grid;
        internal Image _image;
        internal int _imageIndex;
        private int _index;
        internal bool _isFirstSibling;
        internal bool _isLastSibling;
        internal bool _isSited;
        private int _level;
        internal TreeGridNodeCollection _owner;
        internal TreeGridNode _parent;
        private TreeGridCell _treeCell;
        private bool childCellsCreated;
        private TreeGridNodeCollection childrenNodes;
        internal bool IsExpanded;
        internal bool IsRoot;

        /// <summary>
        /// 构造
        /// </summary>
        public TreeGridNode()
        {
            this.childCellsCreated = false;
            this.Site = null;
            this._index = -1;
            this._level = -1;
            this.IsExpanded = false;
            this._isSited = false;
            this._isFirstSibling = false;
            this._isLastSibling = false;
            this._imageIndex = -1;

            //设置非默认值
            this.DefaultCellStyle.ForeColor = Color.Black;
            this.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            this.DefaultCellStyle.SelectionForeColor = Color.Black;
            this.DefaultCellStyle.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.Height = 32;
            this.Resizable = DataGridViewTriState.False;
        }

        #region 更新值
        /// <summary>
        /// 更新节点
        /// </summary>
        /// <param name="obj">泛型实体</param>
        public void Update(object obj)
        {
            Update(obj.GetValues());
        }
        internal void Update(params object[] values)
        {
            int num = 0;
            if (values.Length > this.Cells.Count)
            {
                throw new ArgumentOutOfRangeException("values");
            }
            foreach (object obj2 in values)
            {
                this.Cells[num].Value = obj2;
                num++;
            }
        }

        #endregion

        #region 节点
        internal TreeGridNode(TreeGridView owner) : this()
        {
            this._grid = owner;
            this.IsExpanded = true;
        }
        internal virtual bool AddChildNode(TreeGridNode node)
        {
            node._parent = this;
            node._grid = this._grid;
            if (this._grid != null)
            {
                this.UpdateChildNodes(node);
            }
            if ((this._isSited || this.IsRoot) && (this.IsExpanded && !node._isSited))
            {
                this._grid.SiteNode(node);
            }
            return true;
        }
        internal virtual bool AddChildNodes(params TreeGridNode[] nodes)
        {
            foreach (TreeGridNode node in nodes)
            {
                this.AddChildNode(node);
            }
            return true;
        }
        internal virtual bool ClearNodes()
        {
            if (this.HasChildren)
            {
                for (int i = this.Nodes.Count - 1; i >= 0; i--)
                {
                    this.Nodes.RemoveAt(i);
                }
            }
            return true;
        }
        internal virtual bool InsertChildNode(int index, TreeGridNode node)
        {
            node._parent = this;
            node._grid = this._grid;
            if (this._grid != null)
            {
                this.UpdateChildNodes(node);
            }
            if ((this._isSited || this.IsRoot) && this.IsExpanded)
            {
                this._grid.SiteNode(node);
            }
            return true;
        }
        internal virtual bool InsertChildNodes(int index, params TreeGridNode[] nodes)
        {
            foreach (TreeGridNode node in nodes)
            {
                this.InsertChildNode(index, node);
            }
            return true;
        }
        internal virtual bool RemoveChildNode(TreeGridNode node)
        {
            if ((this.IsRoot || this._isSited) && this.IsExpanded)
            {
                this._grid.UnSiteNode(node);
            }
            node._grid = null;
            node._parent = null;
            return true;
        }
        private bool ShouldSerializeImage() => ((this._imageIndex == -1) && (this._image != null));
        private bool ShouldSerializeImageIndex() => ((this._imageIndex != -1) && (this._image == null));
        internal virtual void Sited()
        {
            this._isSited = true;
            this.childCellsCreated = true;
            foreach (DataGridViewCell cell in this.Cells)
            {
                if (cell is TreeGridCell tree)
                {
                    tree.Sited();
                }
            }
        }
        internal virtual void UnSited()
        {
            foreach (DataGridViewCell cell in this.Cells)
            {
                if (cell is TreeGridCell tree)
                {
                    tree.UnSited();
                }
            }
            this._isSited = false;
        }
        private void UpdateChildNodes(TreeGridNode node)
        {
            if (node.HasChildren)
            {
                foreach (TreeGridNode node2 in node.Nodes)
                {
                    node2._grid = node._grid;
                    this.UpdateChildNodes(node2);
                }
            }
        }

        #endregion

        #region 重载
        /// <summary>
        /// </summary>
        protected override DataGridViewCellCollection CreateCellsInstance()
        {
            DataGridViewCellCollection collection1 = base.CreateCellsInstance();
            collection1.CollectionChanged += new CollectionChangeEventHandler(this.Cells_CollectionChanged);
            return collection1;
        }
        private void Cells_CollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            if ((this._treeCell == null) && ((e.Action == CollectionChangeAction.Add) || (e.Action == CollectionChangeAction.Refresh)))
            {
                TreeGridCell element = null;
                if (e.Element == null)
                {
                    foreach (DataGridViewCell cell in base.Cells)
                    {
                        if (typeof(TreeGridCell).IsInstanceOfType(cell))
                        {
                            element = (TreeGridCell)cell;
                            break;
                        }
                    }
                }
                else
                {
                    element = e.Element as TreeGridCell;
                }
                if (element != null)
                {
                    this._treeCell = element;
                }
            }
        }
        /// <summary>
        /// </summary>
        public override object Clone()
        {
            TreeGridNode node = (TreeGridNode)base.Clone();
            node._level = this._level;
            node._grid = this._grid;
            node._parent = this.Parent;
            node._imageIndex = this._imageIndex;
            if (node._imageIndex == -1)
            {
                node.Image = this.Image;
            }
            node.IsExpanded = this.IsExpanded;
            return node;
        }
        /// <summary>
        /// </summary>
        public override string ToString()
        {
            StringBuilder builder1 = new StringBuilder(36);
            builder1.Append("TreeGridNode { Index=");
            builder1.Append(this.RowIndex.ToString(CultureInfo.CurrentCulture));
            builder1.Append(" }");
            return builder1.ToString();
        }

        #endregion

        #region 公开方法
        /// <summary>
        /// 关闭节点
        /// </summary>
        public virtual bool Collapse() => this._grid.CollapseNode(this);
        /// <summary>
        /// 展开节点
        /// </summary>
        public virtual bool Expand()
        {
            if (this._grid != null)
            {
                return this._grid.ExpandNode(this);
            }
            this.IsExpanded = true;
            return true;
        }
        /// <summary>
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public new DataGridViewCellCollection Cells
        {
            get
            {
                if (!this.childCellsCreated && (base.DataGridView == null))
                {
                    if (this._grid == null)
                    {
                        return null;
                    }
                    base.CreateCells(this._grid);
                    this.childCellsCreated = true;
                }
                return base.Cells;
            }
        }
        /// <summary>
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool HasChildren => ((this.childrenNodes != null) && (this.Nodes.Count > 0));
        /// <summary>
        /// </summary>
        [Browsable(false)]
        public Image Image
        {
            get
            {
                if ((this._image != null) || (this._imageIndex == -1))
                {
                    return this._image;
                }
                if ((this.ImageList != null) && (this._imageIndex < this.ImageList.Images.Count))
                {
                    return this.ImageList.Images[this._imageIndex];
                }
                return null;
            }
            set
            {
                this._image = value;
                if (this._image != null)
                {
                    this._imageIndex = -1;
                }
                if (this._isSited)
                {
                    this._treeCell.UpdateStyle();
                    if (this.Displayed)
                    {
                        this._grid.InvalidateRow(this.RowIndex);
                    }
                }
            }
        }
        /// <summary>
        /// </summary>
        [Browsable(false)]
        [TypeConverter(typeof(ImageIndexConverter)), Category("Appearance"), Editor("System.Windows.Forms.Design.ImageIndexEditor", typeof(UITypeEditor)), DefaultValue(-1), Description("...")]
        public int ImageIndex
        {
            get
            {
                return
              this._imageIndex;
            }
            set
            {
                this._imageIndex = value;
                if (this._imageIndex != -1)
                {
                    this._image = null;
                }
                if (this._isSited)
                {
                    this._treeCell.UpdateStyle();
                    if (this.Displayed)
                    {
                        this._grid.InvalidateRow(this.RowIndex);
                    }
                }
            }
        }
        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Windows.Forms.ImageList ImageList
        {
            get
            {
                if (this._grid != null)
                {
                    return this._grid.ImageList;
                }
                return null;
            }
        }
        /// <summary>
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int Index
        {
            get
            {
                if (this._index == -1)
                {
                    this._index = this._owner.IndexOf(this);
                }
                return this._index;
            }
            internal set
            {
                this._index = value;
            }
        }
        /// <summary>
        /// </summary>
        [Browsable(false)]
        public bool IsFirstSibling => (this.Index == 0);
        /// <summary>
        /// </summary>
        [Browsable(false)]
        public bool IsLastSibling
        {
            get
            {
                TreeGridNode parent = this.Parent;
                if ((parent != null) && parent.HasChildren)
                {
                    return (this.Index == (parent.Nodes.Count - 1));
                }
                return true;
            }
        }
        /// <summary>
        /// </summary>
        [Browsable(false)]
        public bool IsSited => this._isSited;
        /// <summary>
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Level
        {
            get
            {
                if (this._level == -1)
                {
                    int num = 0;
                    for (TreeGridNode node = this.Parent; node != null; node = node.Parent)
                    {
                        num++;
                    }
                    this._level = num;
                }
                return this._level;
            }
        }
        /// <summary>
        /// </summary>
        [Browsable(false)]
        public TreeGridNodeCollection Nodes
        {
            get
            {
                if (this.childrenNodes == null)
                {
                    this.childrenNodes = new TreeGridNodeCollection(this);
                }
                return this.childrenNodes;
            }
        }
        /// <summary>
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TreeGridNode Parent => this._parent;
        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Represents the index of this row in the Grid. Advanced usage."), Browsable(false)]
        public int RowIndex => base.Index;
        /// <summary>
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public ISite Site { get; set; }

        #endregion

    }
}

