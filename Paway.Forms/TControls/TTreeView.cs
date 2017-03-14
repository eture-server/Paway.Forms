using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Paway.Helper;
using Paway.Resource;

namespace Paway.Forms
{
    /// <summary>
    ///     选中与颜色重绘
    ///     设置Id,ParentId,Root自动添加树节点
    ///     多列显示[待续]
    /// </summary>
    public class TTreeView : TreeView
    {
        /// <summary>
        ///     构造
        /// </summary>
        public TTreeView()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();
            //选中与颜色重绘
            DrawMode = TreeViewDrawMode.OwnerDrawText;
            ItemHeight = 23;
            HideSelection = false;
            FullRowSelect = true;
            HotTracking = true;
            BorderStyle = BorderStyle.None;
            AfterCheck += DrawTreeView_AfterCheck;
        }

        #region 双击展开

        /// <summary>
        ///     双击展开项
        /// </summary>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            var node = GetNodeAt(e.Location);
            if (!node.Bounds.Contains(e.Location) && e.X > node.Bounds.X)
            {
                if (node.Nodes.Count > 0)
                {
                    if (node.IsExpanded)
                    {
                        node.Collapse();
                    }
                    else
                    {
                        node.Expand();
                    }
                }
            }
            base.OnMouseDoubleClick(e);
        }

        #endregion

        #region 变量

        /// <summary>
        ///     鼠标上一次移过的项
        /// </summary>
        private TreeNode lastnode;

        /// <summary>
        ///     +
        /// </summary>
        private readonly Image add = AssemblyHelper.GetImage("Controls.+.png");

        /// <summary>
        ///     -
        /// </summary>
        private readonly Image less = AssemblyHelper.GetImage("Controls.-.png");

        #endregion

        #region 属性

        private Color _select = Color.FromArgb(200, 207, 227, 253);

        /// <summary>
        ///     项被选中后的背景颜色(有焦点)
        /// </summary>
        [Browsable(true), Category("控件的重绘设置"), Description("项被选中后的背景颜色(有焦点)")]
        [DefaultValue(typeof(Color), "200, 207, 227, 253")]
        public Color ColorSelect
        {
            get { return _select; }
            set { _select = value; }
        }

        private Color _selectLine = Color.FromArgb(125, 162, 206);

        /// <summary>
        ///     项被选中后的边框颜色(有焦点)
        /// </summary>
        [Browsable(true), Category("控件的重绘设置"), Description("项被选中后的边框颜色(有焦点)")]
        [DefaultValue(typeof(Color), "125, 162, 206")]
        public Color ColorSelectLine
        {
            get { return _selectLine; }
            set { _selectLine = value; }
        }

        private Color _selectFore = Color.Black;

        /// <summary>
        ///     项被选中后的字体颜色(有焦点)
        /// </summary>
        [Browsable(true), Category("控件的重绘设置"), Description("项被选中后的字体颜色(有焦点)")]
        [DefaultValue(typeof(Color), "Black")]
        public Color ColorSelectFore
        {
            get { return _selectFore; }
            set { _selectFore = value; }
        }

        private Color _selectNoFocus = Color.FromArgb(200, 242, 242, 242);

        /// <summary>
        ///     项被选中后的背景颜色(无焦点)
        /// </summary>
        [Browsable(true), Category("控件的重绘设置"), Description("项被选中后的背景颜色(无焦点)")]
        [DefaultValue(typeof(Color), "200, 242, 242, 242")]
        public Color ColorSelectNoFocus
        {
            get { return _selectNoFocus; }
            set { _selectNoFocus = value; }
        }

        private Color _selectLineNoFocus = Color.FromArgb(218, 218, 218);

        /// <summary>
        ///     项被选中后的边框颜色(无焦点)
        /// </summary>
        [Browsable(true), Category("控件的重绘设置"), Description("项被选中后的边框颜色(无焦点)")]
        [DefaultValue(typeof(Color), "218, 218, 218")]
        public Color ColorSelectLineNoFocus
        {
            get { return _selectLineNoFocus; }
            set { _selectLineNoFocus = value; }
        }

        private Color _selectForeNoFocus = Color.Black;

        /// <summary>
        ///     项被选中后的字体颜色(无焦点)
        /// </summary>
        [Browsable(true), Category("控件的重绘设置"), Description("项被选中后的字体颜色(无焦点)")]
        [DefaultValue(typeof(Color), "Black")]
        public Color ColorSelectForeNoFocus
        {
            get { return _selectForeNoFocus; }
            set { _selectForeNoFocus = value; }
        }

        private Color _hot = Color.FromArgb(200, 244, 249, 255);

        /// <summary>
        ///     鼠标移过项时的背景颜色
        /// </summary>
        [Browsable(true), Category("控件的重绘设置"), Description("鼠标移过项时的背景颜色")]
        [DefaultValue(typeof(Color), "200, 244, 249, 255")]
        public Color ColorHot
        {
            get { return _hot; }
            set { _hot = value; }
        }

        private Color _hotLine = Color.FromArgb(185, 215, 252);

        /// <summary>
        ///     鼠标移过项时的边框颜色
        /// </summary>
        [Browsable(true), Category("控件的重绘设置"), Description("鼠标移过项时的边框颜色")]
        [DefaultValue(typeof(Color), "185, 215, 252")]
        public Color ColorHotLine
        {
            get { return _hotLine; }
            set { _hotLine = value; }
        }

        private Color _hotFore = Color.Black;

        /// <summary>
        ///     鼠标移过项时的字体颜色
        /// </summary>
        [Browsable(true), Category("控件的重绘设置"), Description("鼠标移过项时的字体颜色")]
        [DefaultValue(typeof(Color), "Black")]
        public Color ColorHotFore
        {
            get { return _hotFore; }
            set { _hotFore = value; }
        }

        /// <summary>
        ///     根节点
        /// </summary>
        [TypeConverter(typeof(StringConverter))]
        [Browsable(true), Category("控件的数据设置"), Description("根节点"), DefaultValue(null)]
        public object TRoot { get; set; }

        private object _parentId = "ParentId";

        /// <summary>
        ///     父节点字段
        /// </summary>
        [TypeConverter(typeof(StringConverter))]
        [Browsable(true), Category("控件的数据设置"), Description("父节点字段"), DefaultValue("ParentId")]
        public object TParentId
        {
            get { return _parentId; }
            set { _parentId = value; }
        }

        private object _id = "Id";

        /// <summary>
        ///     子节点字段
        /// </summary>
        [TypeConverter(typeof(StringConverter))]
        [Browsable(true), Category("控件的数据设置"), Description("子节点字段"), DefaultValue("Id")]
        public object TId
        {
            get { return _id; }
            set { _id = value; }
        }

        private bool _autoWidth = true;

        /// <summary>
        ///     自动调整宽度至整行
        /// </summary>
        [Browsable(true), Description("自动调整宽度至整行"), DefaultValue(true)]
        public bool TAutoWidth
        {
            get { return _autoWidth; }
            set
            {
                _autoWidth = value;
                if (_dataSource == null || TId == null || TParentId == null) return;
                var type = _dataSource.GetType();
                if (_dataSource is IList)
                {
                    var list = _dataSource as IList;
                    type = list.GetListType();
                }
                UpdateColumns(type);
            }
        }

        private object _dataSource;

        /// <summary>
        ///     数据源
        /// </summary>
        [Browsable(false), Category("控件的数据设置"), Description("数据源")]
        public object DataSource
        {
            get { return _dataSource; }
            set
            {
                _dataSource = value;
                RefreshData();
            }
        }

        /// <summary>
        ///     TreeView中的显示项
        /// </summary>
        private TreeItemCollection _items;

        /// <summary>
        ///     TreeView中的显示项
        /// </summary>
        [Description("TreeView中的显示项"), EditorBrowsable(EditorBrowsableState.Always),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TreeItemCollection Items
        {
            get
            {
                if (_items == null)
                    _items = new TreeItemCollection(this);
                return _items;
            }
        }

        #endregion

        #region 重载属性默认值

        /// <summary>
        ///     获取或设置绘制控件的模式。
        /// </summary>
        [Description("获取或设置绘制控件的模式")]
        [DefaultValue(typeof(TreeViewDrawMode), "OwnerDrawText")]
        public new TreeViewDrawMode DrawMode
        {
            get { return base.DrawMode; }
            set { base.DrawMode = value; }
        }

        /// <summary>
        ///     获取或设置树视图控件中每个树节点的高度。
        /// </summary>
        [Description("获取或设置树视图控件中每个树节点的高度")]
        [DefaultValue(23)]
        public new int ItemHeight
        {
            get { return base.ItemHeight; }
            set { base.ItemHeight = value; }
        }

        /// <summary>
        ///     获取或设置一个值，用以指示选定的树节点是否即使在树视图已失去焦点时仍会保持突出显示。
        /// </summary>
        [Description("获取或设置树视图控件中每个树节点的高度")]
        [DefaultValue(false)]
        public new bool HideSelection
        {
            get { return base.HideSelection; }
            set { base.HideSelection = value; }
        }

        /// <summary>
        ///     获取或设置一个值，用以指示选择突出显示是否跨越树视图控件的整个宽度。
        /// </summary>
        [Description("获取或设置一个值，用以指示选择突出显示是否跨越树视图控件的整个宽度。")]
        [DefaultValue(true)]
        public new bool FullRowSelect
        {
            get { return base.FullRowSelect; }
            set { base.FullRowSelect = value; }
        }

        /// <summary>
        ///     获取或设置一个值，用以指示当鼠标指针移过树节点标签时，树节点标签是否具有超链接的外观。
        /// </summary>
        [Description("获取或设置一个值，用以指示当鼠标指针移过树节点标签时，树节点标签是否具有超链接的外观。")]
        [DefaultValue(true)]
        public new bool HotTracking
        {
            get { return base.FullRowSelect; }
            set { base.FullRowSelect = value; }
        }

        /// <summary>
        ///     获取或设置树视图控件的边框样式
        /// </summary>
        [Description("获取或设置树视图控件的边框样式。")]
        [DefaultValue(typeof(BorderStyle), "None")]
        public new BorderStyle BorderStyle
        {
            get { return base.BorderStyle; }
            set { base.BorderStyle = value; }
        }

        #endregion

        #region 节点添加

        /// <summary>
        ///     刷新数据
        /// </summary>
        public void RefreshData()
        {
            Nodes.Clear();
            if (_dataSource == null || TId == null || TParentId == null) return;
            DataTable dt = null;
            var type = _dataSource.GetType();
            if (_dataSource is DataTable)
            {
                dt = _dataSource as DataTable;
            }
            else if (_dataSource is IList)
            {
                var list = _dataSource as IList;
                type = list.GetListType();
                dt = type.ToDataTable(list);
            }
            else if (type == typeof(string) || type.IsValueType)
            {
                var node = new TreeNode();
                node.Text = _dataSource.ToString();
                Nodes.Add(node);
            }
            UpdateColumns(type);
            if (dt != null && dt.Rows.Count > 0)
            {
                AddNodes(dt, type);
            }
        }

        /// <summary>
        ///     刷新Node
        /// </summary>
        public void UpdateNode(object id)
        {
            if (id == null || _dataSource == null || TId == null || TParentId == null) return;
            DataTable dt = null;
            var type = _dataSource.GetType();
            if (_dataSource is DataTable)
            {
                dt = _dataSource as DataTable;
            }
            else if (_dataSource is IList)
            {
                var list = _dataSource as IList;
                type = list.GetListType();
                dt = type.ToDataTable(list, id);
            }
            if (dt == null) return;
            var dr = dt.Select(string.Format("{0} = '{1}'", TId, id));
            if (dr.Length == 1)
            {
                var nodes = Nodes.Find(id.ToString(), true);
                if (nodes.Length == 1)
                {
                    UpdateNode(Nodes, dr[0], id.ToString());
                }
                else if (nodes.Length == 0)
                {
                    var parent = dr[0][TParentId.ToString()].ToString();
                    if (!AddNode(Nodes, dr[0], parent))
                    {
                        AddNode(Nodes, dr[0]);
                    }
                }
            }
            else if (dr.Length == 0)
            {
                DeleteNode(Nodes, id.ToString());
            }
        }

        private bool AddNode(TreeNodeCollection nodes, DataRow dr, string parent)
        {
            for (var i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Name == parent)
                {
                    AddNode(nodes[i].Nodes, dr);
                    return true;
                }
                if (AddNode(nodes[i].Nodes, dr, parent)) return true;
            }
            return false;
        }

        private void AddNode(TreeNodeCollection nodes, DataRow dr)
        {
            var node = new ItemNode(dr);
            node.Text = (_items.Count > 0 ? dr[_items[0].Name] : dr[TId.ToString()]).ToString();
            node.Name = dr[TId.ToString()].ToString();
            nodes.Add(node);
        }

        private bool UpdateNode(TreeNodeCollection nodes, DataRow dr, string name)
        {
            for (var i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Name == name)
                {
                    var old = nodes[i] as ItemNode;
                    var oldParent = old[TParentId.ToString()].ToString2();
                    var temp = nodes[i].Nodes;
                    nodes.RemoveAt(i);
                    var node = new ItemNode(dr);
                    node.Text = (_items.Count > 0 ? dr[_items[0].Name] : dr[TId.ToString()]).ToString();
                    node.Name = dr[TId.ToString()].ToString();
                    for (var j = 0; j < temp.Count; j++)
                    {
                        node.Nodes.Add(temp[j]);
                    }
                    var newParent = dr[TParentId.ToString()].ToString2();
                    if (oldParent == newParent)
                    {
                        nodes.Insert(i, node);
                    }
                    else
                    {
                        var parentNodes = Nodes.Find(newParent, true);
                        if (parentNodes.Length == 1)
                        {
                            parentNodes[0].Nodes.Add(node);
                        }
                        else
                        {
                            Nodes.Add(node);
                        }
                    }
                    return true;
                }
                if (UpdateNode(nodes[i].Nodes, dr, name)) return true;
            }
            return false;
        }

        private bool DeleteNode(TreeNodeCollection nodes, string name)
        {
            for (var i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Name == name)
                {
                    nodes.RemoveAt(i);
                    return true;
                }
                if (DeleteNode(nodes[i].Nodes, name)) return true;
            }
            return false;
        }

        private void AddNodes(DataTable dt, Type type)
        {
            DataRow[] dr = null;
            if (TRoot == null)
            {
                var props = TypeDescriptor.GetProperties(type);
                for (var i = 0; i < props.Count; i++)
                {
                    if (props[i].Name == TParentId.ToString())
                    {
                        if (!props[i].PropertyType.IsValueType && props[i].PropertyType != typeof(string))
                        {
                            TRoot = Activator.CreateInstance(props[i].PropertyType);
                        }
                        break;
                    }
                }
            }
            if (TRoot != null)
            {
                dr = dt.Select(string.Format("{0} = '{1}'", TId, TRoot));
                if (dr.Length > 0) throw new Exception("子节点不可与根节点相同");
                dr = dt.Select(string.Format("{0} = '{1}'", TParentId, TRoot));
            }
            else
            {
                dr = dt.Select(string.Format("{0} is null", TParentId));
            }
            for (var i = 0; i < dr.Length; i++)
            {
                var node = new ItemNode(dr[i]);
                node.Text = (_items.Count > 0 ? dr[i][_items[0].Name] : dr[i][TId.ToString()]).ToString();
                node.Name = dr[i][TId.ToString()].ToString();
                Nodes.Add(node);
                AddNodes(dt, node, dr[i][TId.ToString()].ToString());
            }
        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        protected virtual void AddNodes(DataTable dt, TreeNode parent, string parentId)
        {
            var dr = dt.Select(string.Format("{0} = '{1}'", TParentId, parentId));
            for (var i = 0; i < dr.Length; i++)
            {
                var node = new ItemNode(dr[i]);
                node.Text = (_items.Count > 0 ? dr[i][_items[0].Name] : dr[i][TId.ToString()]).ToString();
                node.Name = dr[i][TId.ToString()].ToString();
                parent.Nodes.Add(node);
                AddNodes(dt, node, dr[i][TId.ToString()].ToString());
            }
        }

        private string GetText(DataRow dr)
        {
            string result = null;
            for (var j = 0; j < _items.Count; j++)
            {
                result = string.Format("{0}{1}&&", result, dr[_items[j].Name]);
            }
            if (result == null) result = dr[TId.ToString()].ToString();
            else result = result.TrimEnd('&');
            return result;
        }

        /// <summary>
        ///     更新列名称
        /// </summary>
        public void UpdateColumns(Type type)
        {
            if (type == null || type == typeof(string) || type.IsValueType) return;
            if (Items.Count == 0)
            {
                var properties = TypeDescriptor.GetProperties(type);
                for (var i = 0; i < properties.Count; i++)
                {
                    var pro = type.GetProperty(properties[i].Name);
                    if (pro == null) continue;
                    var itemList = pro.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
                    if (itemList != null && itemList.Length != 0)
                    {
                        if (!itemList[0].Show)
                        {
                            Items.Add(new TreeItem());
                        }
                    }
                }
            }
            if (TAutoWidth)
            {
                var total = 0;
                for (var i = 0; i < Items.Count; i++)
                {
                    total += Items[i].Width;
                }
                for (var i = 0; i < Items.Count; i++)
                {
                    Items[i].Width = Items[i].Width * Width / total;
                }
            }
        }

        #endregion

        #region 重绘节点

        /// <summary>
        ///     重绘节点
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            OnPaint(g, Nodes, e.ClipRectangle);
        }

        private void OnPaint(Graphics g, TreeNodeCollection nodes, Rectangle rect)
        {
            for (var i = 0; i < nodes.Count; i++)
            {
                if (Rectangle.Union(rect, nodes[i].Bounds) != Rectangle.Empty)
                {
                    C_DrawNode(g, ForeColor, BackColor, nodes[i], 0);
                    C_DrawAdd(g, nodes[i]);
                }
                if (nodes[i].IsExpanded && nodes[i].Nodes.Count > 0)
                    OnPaint(g, nodes[i].Nodes, rect);
            }
        }

        /// <summary>
        ///     重绘Text
        /// </summary>
        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            base.OnDrawNode(e);
        }

        private void C_DrawNode(Graphics g, TreeNode node)
        {
            var rect = node.Bounds;
            rect = new Rectangle(rect.Location, new Size(Width - 4 - rect.X - 2, rect.Height - 1));
            if (node.IsSelected)
            {
                if (Focused)
                {
                    g.FillRectangle(new SolidBrush(ColorSelect), rect);
                    C_DrawString(g, node, rect, ColorSelectFore);
                    g.DrawRectangle(new Pen(ColorSelectLine), rect);
                }
                else
                {
                    g.FillRectangle(new SolidBrush(ColorSelectNoFocus), rect);
                    C_DrawString(g, node, rect, ColorSelectForeNoFocus);
                    g.DrawRectangle(new Pen(ColorSelectLineNoFocus), rect);
                }
            }
            else
            {
                g.FillRectangle(new SolidBrush(BackColor), rect);
                C_DrawString(g, node, rect, ForeColor);
            }
        }

        private void C_DrawNode(Graphics g, Color foreColor, Color backColor, TreeNode node, int height)
        {
            if (node == null) return;
            if (SelectedNode != null && SelectedNode.Name == node.Name)
            {
                C_DrawNode(g, node);
                return;
            }

            var rect = node.Bounds;
            rect = new Rectangle(rect.Location, new Size(Width - 4 - rect.X - 2, rect.Height - 1));
            g.FillRectangle(new SolidBrush(backColor), rect);
            C_DrawString(g, node, rect, foreColor);
        }

        /// <summary>
        ///     通过绘制实现多列
        /// </summary>
        private void C_DrawString(Graphics g, TreeNode node, Rectangle rect, Color foreColor)
        {
            if (rect.Height == 0) return;
            var item = node as ItemNode;
            if (item == null || _items.Count <= 1)
            {
                TextRenderer.DrawText(g, node.Text, Font, rect, foreColor, DrawHelper.TextEnd);
            }
            else
            {
                var x = Indent + 3;
                var left = 0;
                var format = TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis;
                for (var i = 0; i < _items.Count; i++)
                {
                    if (item[_items[i].Name] is DateTime && item[_items[i].Name].ToDateTime() == DateTime.MinValue)
                        continue;
                    var irect = new Rectangle(
                        rect.X + left, rect.Y,
                        _items[i].Width + (i == 0 ? x - rect.X : 0), rect.Height
                        );
                    left += irect.Width;
                    if (_items[i].Type == TreeItemType.Text)
                    {
                        var temp = format;
                        switch (_items[i].Alignment)
                        {
                            case StringAlignment.Center:
                                temp = temp | TextFormatFlags.HorizontalCenter;
                                break;
                            case StringAlignment.Far:
                                temp = temp | TextFormatFlags.Right;
                                break;
                        }
                        TextRenderer.DrawText(g, item[_items[i].Name].ToString(), Font, irect, foreColor, temp);
                    }
                    else
                    {
                        var obj = item[_items[i].Name];
                        DrawImage(g, obj, irect);
                    }
                }
            }
        }

        /// <summary>
        ///     +-号绘制
        /// </summary>
        private void C_DrawAdd(Graphics g, TreeNode node)
        {
            var rect = node.Bounds;
            rect = new Rectangle(rect.Location, new Size(Width - 4 - rect.X - 2, rect.Height - 1));
            if (node.IsExpanded)
            {
                var indent = (rect.Height - add.Height) / 2;
                var plusRect = new Rectangle(node.Bounds.Left - add.Width - 6, rect.Top + indent, add.Width, add.Height);
                g.DrawImage(add, plusRect);
            }
            else if (node.Nodes.Count > 0)
            {
                var indent = (rect.Height - less.Height) / 2;
                var plusRect = new Rectangle(node.Bounds.Left - less.Width - 6, rect.Top + indent, less.Width,
                    less.Height);
                g.DrawImage(less, plusRect);
            }
        }

        private void DrawImage(Graphics g, object obj, Rectangle irect)
        {
            if (obj != DBNull.Value)
            {
                var bitmap = obj as Bitmap;
                if (bitmap != null)
                {
                    irect.X += (irect.Width - bitmap.Width) / 2;
                    irect.Y += (irect.Height - bitmap.Height) / 2;
                    irect.Width = bitmap.Width;
                    irect.Height = bitmap.Height;
                    g.DrawImage(bitmap, irect);
                }
            }
        }

        #endregion

        #region 鼠标移过时

        /// <summary>
        ///     鼠标离开时
        /// </summary>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (lastnode != null)
            {
                var rect = lastnode.Bounds;
                rect = new Rectangle(1, rect.Top, Width, rect.Height);
                Invalidate(rect);
            }
            lastnode = null;
        }

        /// <summary>
        ///     移过行
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            var node = GetNodeAt(e.Location);
            if (node != null && lastnode != null && lastnode.Name == node.Name) return;

            if (lastnode != null)
            {
                var rect = lastnode.Bounds;
                rect = new Rectangle(1, rect.Top, Width, rect.Height);
                Invalidate(rect);
            }
            var g = CreateGraphics();
            C_DrawNode(g, ColorHotFore, ColorHot, node, 1);
            lastnode = node;
            g.Dispose();
        }

        /// <summary>
        ///     整行选中
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left) return;
            var node = GetNodeAt(e.Location);
            if (node != null)
            {
                SelectedNode = node;
            }
        }

        #endregion

        #region 节点选中事件
        private void DrawTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (!CheckBoxes) return;

            AfterCheck -= DrawTreeView_AfterCheck;
            ParentNodeCheck(e.Node.Parent);
            ChildNodeCheck(e.Node);
            AfterCheck += DrawTreeView_AfterCheck;
        }
        /// <summary>
        ///     父节点
        /// </summary>
        /// <param name="node"></param>
        private void ParentNodeCheck(TreeNode node)
        {
            if (node == null) return;
            var result = true;
            for (var i = 0; i < node.Nodes.Count; i++)
            {
                if (!node.Nodes[i].Checked)
                {
                    result = false;
                    break;
                }
            }
            node.Checked = result;
            ParentNodeCheck(node.Parent);
        }
        /// <summary>
        ///     子节点
        /// </summary>
        /// <param name="node"></param>
        private void ChildNodeCheck(TreeNode node)
        {
            for (var i = 0; i < node.Nodes.Count; i++)
            {
                node.Nodes[i].Checked = node.Checked;
                ChildNodeCheck(node.Nodes[i]);
            }
        }

        #endregion
    }
    /// <summary>
    ///     自定义树节点
    /// </summary>
    [Serializable]
    public class ItemNode : TreeNode
    {
        private readonly DataRow dr;

        /// <summary>
        ///     默认构造
        /// </summary>
        public ItemNode()
        {
        }

        /// <summary>
        ///     初始化，加载DataRow数据
        /// </summary>
        /// <param name="dr"></param>
        public ItemNode(DataRow dr)
        {
            this.dr = dr;
        }

        /// <summary>
        ///     TreeView中ItemNode的键值
        /// </summary>
        public object this[string key]
        {
            get { return dr[key]; }
        }

        /// <summary>
        ///     TreeView中ItemNode的键值
        /// </summary>
        public object this[int index]
        {
            get { return dr[index]; }
        }
    }

    /// <summary>
    ///     代表 TreeView 中项的集合。
    /// </summary>
    [ListBindable(false)]
    public class TreeItemCollection : List<TreeItem>
    {
        #region 变量

        /// <summary>
        ///     TreeView
        /// </summary>
        private TreeView _owner;

        #endregion

        #region 构造函数

        /// <summary>
        ///     初始化 Paway.Forms.TreeItemCollection 新的实例。
        /// </summary>
        /// <param name="owner">ToolBar</param>
        public TreeItemCollection(TreeView owner)
        {
            _owner = owner;
        }

        #endregion

        #region 方法

        /// <summary>
        ///     返回该项在集合中的索引值
        /// </summary>
        public int GetIndexOfRange(TreeItem item)
        {
            var result = -1;
            for (var i = 0; i < Count; i++)
            {
                if (item == base[i])
                {
                    result = i;
                    break;
                }
            }
            return result;
        }

        #endregion
    }

    /// <summary>
    ///     表示 TreeView 控件中的单个显示项。
    /// </summary>
    public class TreeItem
    {
        #region 属性

        /// <summary>
        ///     项显示的类型
        /// </summary>
        [DefaultValue(TreeItemType.Text)]
        public TreeItemType Type { get; set; }

        /// <summary>
        ///     项上绑定的字段
        /// </summary>
        [DefaultValue("toolItem")]
        public string Name { get; set; }

        private int width = 100;

        /// <summary>
        ///     项的长度
        /// </summary>
        [DefaultValue(100)]
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        /// <summary>
        ///     文本显示的位置,左或右
        /// </summary>
        [DefaultValue(StringAlignment.Near)]
        public StringAlignment Alignment { get; set; }

        /// <summary>
        ///     当前 Item 在 TreeItem 中的 Rectangle
        /// </summary>
        public Rectangle Rectangle { get; protected set; }

        /// <summary>
        ///     获取或设置包含有关控件的数据的对象。
        /// </summary>
        [TypeConverter(typeof(StringConverter))]
        public object Tag { get; set; }

        #endregion
    }

    /// <summary>
    ///     列中显示图片或文本
    /// </summary>
    public enum TreeItemType
    {
        /// <summary>
        ///     文本
        /// </summary>
        Text,

        /// <summary>
        ///     图像
        /// </summary>
        Image
    }
}