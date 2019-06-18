using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Paway.Forms.Properties;
using Paway.Helper;

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

        private readonly Image add = Resources.Controls_add;
        private readonly Image less = Resources.Controls_less;

        private readonly Image check_normal = Resources.QQ_CheckBox_normal;
        private readonly Image check_tick = Resources.QQ_CheckBox_tick_normal;
        private readonly Image check_hight = Resources.QQ_CheckBox__tick_highlight;

        #endregion

        #region 属性
        /// <summary>
        /// 正在排序
        /// </summary>
        [Browsable(false), Description("正在排序")]
        [DefaultValue(false)]
        public bool Checking { get; set; }

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

        private object _tRoot = "0";
        /// <summary>
        ///     根节点
        /// </summary>
        [TypeConverter(typeof(StringConverter))]
        [Browsable(true), Category("控件的数据设置"), Description("根节点")]
        [DefaultValue("0")]
        public object TRoot { get { return _tRoot; } set { _tRoot = value; } }

        private object _parentId = "ParentId";

        /// <summary>
        ///     父节点字段
        /// </summary>
        [TypeConverter(typeof(StringConverter))]
        [Browsable(true), Category("控件的数据设置"), Description("父节点字段")]
        [DefaultValue("ParentId")]
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
        [Browsable(true), Category("控件的数据设置"), Description("子节点字段")]
        [DefaultValue("Id")]
        public object TId
        {
            get { return _id; }
            set { _id = value; }
        }

        private bool _iAutoWidth = false;
        /// <summary>
        ///     自动调整宽度至整行
        /// </summary>
        [Browsable(true), Description("自动调整宽度至整行")]
        [DefaultValue(false)]
        public bool IAutoWidth
        {
            get { return _iAutoWidth; }
            set
            {
                _iAutoWidth = value;
                if (_dataSource == null || TId == null || TParentId == null) return;
                var type = _dataSource.GetType();
                if (_dataSource is IEnumerable)
                {
                    type = _dataSource.GenericType();
                }
                UpdateColumns(type);
            }
        }

        private object _dataSource;

        /// <summary>
        ///     数据源
        /// </summary>
        [Browsable(false), Category("控件的数据设置"), Description("数据源")]
        [DefaultValue(null)]
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
                    _items = new TreeItemCollection();
                return _items;
            }
        }

        #endregion

        #region 事件
        /// <summary>
        /// 节点移动完成事件
        /// </summary>
        public event EventHandler<TreeEventArgs> DropCompleteEvent;
        /// <summary>
        /// 节点移动判断事件
        /// </summary>
        public event EventHandler<TreeEventArgs> DropMoveEvent;

        #endregion

        #region 重载属性默认值
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
            get { return base.HotTracking; }
            set { base.HotTracking = value; }
        }

        /// <summary>
        ///     获取或设置树视图控件的边框样式
        /// </summary>
        [Description("获取或设置树视图控件的边框样式。")]
        [DefaultValue(BorderStyle.None)]
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
            else if (_dataSource is IEnumerable)
            {
                var list = _dataSource as IEnumerable;
                type = list.GenericType();
                dt = list.ToDataTable();
            }
            else if (type == typeof(string) || type.IsValueType)
            {
                var node = new TreeNode()
                {
                    Text = _dataSource.ToString()
                };
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
            if (_dataSource is DataTable)
            {
                dt = _dataSource as DataTable;
            }
            else if (_dataSource is IEnumerable)
            {
                var list = _dataSource as IEnumerable;
                dt = list.ToDataTable();
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
            var node = CreateNode(dr);
            nodes.Add(node);
            if (node.Parent != null)
            {
                node.Parent.Expand();
                try
                {
                    Checking = true;
                    ParentNodeCheck(node.Parent, true);
                }
                finally
                {
                    Checking = false;
                }
            }
            this.SelectedNode = node;
        }
        private ItemNode CreateNode(DataRow dr)
        {
            var node = new ItemNode(dr);
            node.Text = CreateText(dr);
            node.Name = dr[TId.ToString()].ToString();
            return node;
        }
        private string CreateText(DataRow dr)
        {
            string text = null;
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Type == TreeItemType.Text)
                    text = string.Format("{0}{1},", text, dr[_items[i].Name]);
                else
                    text = string.Format("{0}Image,", text);
            }
            if (text == null) text = dr[TId.ToString()].ToString();
            else text = text.TrimEnd(',');
            return text;
        }

        /// <summary>
        ///     更新Node
        /// </summary>
        public void UpdateNode(TreeNode node)
        {
            ItemNode item = (ItemNode)node;
            string name = item[TId.ToString()].ToString();
            UpdateNode(Nodes, item.DataRow, name);
        }
        private bool UpdateNode(TreeNodeCollection nodes, DataRow dr, string name)
        {
            for (var i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Name == name)
                {
                    var item = (ItemNode)nodes[i];
                    item.DataRow = dr;
                    item.Text = CreateText(dr);
                    item.Name = dr[TId.ToString()].ToString();
                    this.SelectedNode = item;
                    return true;
                }
                if (UpdateNode(nodes[i].Nodes, dr, name)) return true;
            }
            return false;
        }

        /// <summary>
        ///     删除Node
        /// </summary>
        public void DeleteNode(TreeNode node)
        {
            ItemNode item = (ItemNode)node;
            string name = item[TId.ToString()].ToString();
            DeleteNode(Nodes, name);
        }
        private bool DeleteNode(TreeNodeCollection nodes, string name)
        {
            for (var i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Name == name)
                {
                    var parent = nodes[i].Parent;
                    nodes[i].Remove();
                    try
                    {
                        Checking = true;
                        ParentNodeCheck(parent, true);
                    }
                    finally
                    {
                        Checking = false;
                    }
                    return true;
                }
                if (DeleteNode(nodes[i].Nodes, name)) return true;
            }
            return false;
        }

        private void AddNodes(DataTable dt, Type type)
        {
            DataRow[] dr;
            if (TRoot == null)
            {
                var properties = type.Properties();
                var property = properties.Find(c => c.Name == TParentId.ToString());
                if (property != null && !property.PropertyType.IsValueType && property.PropertyType != typeof(string))
                {
                    TRoot = Activator.CreateInstance(property.PropertyType);
                }
            }
            if (TRoot != null)
            {
                dr = dt.Select(string.Format("[{0}] = '{1}'", TId, TRoot));
                if (dr.Length > 0) throw new Exception("子节点不可与根节点相同");
                dr = dt.Select(string.Format("[{0}] = '{1}'", TParentId, TRoot));
            }
            else
            {
                dr = dt.Select(string.Format("[{0}] is null", TParentId));
            }
            for (var i = 0; i < dr.Length; i++)
            {
                var node = CreateNode(dr[i]);
                Nodes.Add(node);
                AddNodes(dt, node);
            }
        }
        /// <summary>
        /// 添加子节点
        /// </summary>
        protected virtual void AddNodes(DataTable dt, ItemNode parent)
        {
            var dr = dt.Select(string.Format("[{0}] = '{1}'", TParentId, parent[TId.ToString()]));
            for (var i = 0; i < dr.Length; i++)
            {
                var node = CreateNode(dr[i]);
                parent.Nodes.Add(node);
                AddNodes(dt, node);
            }
        }

        /// <summary>
        ///     更新列名称
        /// </summary>
        public void UpdateColumns(Type type)
        {
            if (type == null || type == typeof(string) || type.IsValueType) return;
            if (Items.Count == 0)
            {
                var properties = type.Properties();
                foreach (var property in properties)
                {
                    if (property.IShow(out string text))
                    {
                        Items.Add(new TreeItem());
                    }
                }
            }
            AutoWidth();
        }
        private void AutoWidth()
        {
            if (_iAutoWidth)
            {
                var total = 0;
                for (var i = 0; i < Items.Count; i++)
                {
                    total += Items[i].Width;
                }
                if (total == 0 || Width == 0) return;
                for (var i = 0; i < Items.Count; i++)
                {
                    Items[i].Width = Items[i].Width * Width / total;
                }
            }
        }

        #endregion

        #region 重绘节点
        /// <summary>
        /// 自动调整大小
        /// </summary>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AutoWidth();
        }
        /// <summary>
        ///     重绘节点
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            OnPaint(e.Graphics, Nodes, e.ClipRectangle);
        }

        private void OnPaint(Graphics g, TreeNodeCollection nodes, Rectangle rect)
        {
            for (var i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];
                if (Rectangle.Union(rect, node.Bounds) != Rectangle.Empty)
                {
                    C_DrawNode(g, node.ForeColor == Color.Empty ? ForeColor : node.ForeColor, node.BackColor == Color.Empty ? BackColor : node.BackColor, nodes[i]);
                    C_DrawAdd(g, node);
                }
                if (node.IsExpanded && node.Nodes.Count > 0)
                    OnPaint(g, node.Nodes, rect);
            }
        }

        private void C_DrawNode(Graphics g, TreeNode node)
        {
            var rect = node.Bounds;
            rect = new Rectangle(rect.Location, new Size(Width - rect.X, rect.Height - 1));
            if (node.IsSelected)
            {
                if (Focused)
                {
                    g.FillRectangle(new SolidBrush(ColorSelect), rect);
                    C_DrawString(g, node, rect, ColorSelectFore);
                    g.DrawRectangle(new Pen(node.BackColor == Color.Empty ? ColorSelectLine : node.BackColor), rect);
                }
                else
                {
                    g.FillRectangle(new SolidBrush(node.BackColor == Color.Empty ? ColorSelectNoFocus : node.BackColor), rect);
                    C_DrawString(g, node, rect, node.ForeColor == Color.Empty ? ColorSelectForeNoFocus : node.ForeColor);
                    g.DrawRectangle(new Pen(ColorSelectLineNoFocus), rect);
                }
            }
        }

        private void C_DrawNode(Graphics g, Color foreColor, Color backColor, TreeNode node)
        {
            if (node == null) return;
            if (SelectedNode != null && SelectedNode.Name == node.Name)
            {
                C_DrawNode(g, node);
                return;
            }

            var rect = node.Bounds;
            rect = new Rectangle(rect.Location, new Size(Width - rect.X, rect.Height - 1));
            g.FillRectangle(new SolidBrush(backColor), new RectangleF(rect.X, rect.Y, rect.Width, rect.Height + 1));
            C_DrawString(g, node, rect, foreColor);
        }

        /// <summary>
        ///     通过绘制实现多列
        /// </summary>
        private void C_DrawString(Graphics g, TreeNode node, Rectangle rect, Color foreColor)
        {
            if (rect.Height == 0) return;
            var item = (ItemNode)node;
            if (item == null || _items.Count <= 1)
            {
                TextRenderer.DrawText(g, node.Text, Font, rect, foreColor, DrawHelper.TextEnd);
            }
            else
            {
                var left = 0;
                var format = TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis;
                for (var i = 0; i < _items.Count; i++)
                {
                    if (item[_items[i].Name] is DateTime && item[_items[i].Name].ToDateTime() == DateTime.MinValue)
                        continue;
                    var irect = new Rectangle(
                        rect.X + left, rect.Y,
                        _items[i].Width - (_items[i].IAlign ? Indent * (node.Level + 1) : 0), rect.Height);
                    left += irect.Width;
                    if (_items[i].Type == TreeItemType.Text)
                    {
                        var temp = format;
                        switch (_items[i].Alignment)
                        {
                            case StringAlignment.Center:
                                temp |= TextFormatFlags.HorizontalCenter;
                                break;
                            case StringAlignment.Far:
                                temp |= TextFormatFlags.Right;
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
            rect = new Rectangle(rect.Location, new Size(Width - rect.X, rect.Height - 1));
            int interval = 6;
            if (this.CheckBoxes) interval += 13;
            if (node.Nodes.Count > 0)
            {
                if (node.IsExpanded)
                {
                    var indent = (rect.Height - add.Height) / 2;
                    var plusRect = new Rectangle(rect.X - add.Width - interval, rect.Top + indent, add.Width, add.Height);
                    g.DrawImage(add, plusRect);
                }
                else
                {
                    var indent = (rect.Height - less.Height) / 2;
                    var lessRect = new Rectangle(rect.X - less.Width - interval, rect.Top + indent, less.Width, less.Height);
                    g.DrawImage(less, lessRect);
                }
            }
            if (this.CheckBoxes)
            {
                var item = (ItemNode)node;
                Image image = item.Checked ? check_tick : item.CheckHight ? check_hight : check_normal;
                var indent = (rect.Height - image.Height) / 2;
                rect = new Rectangle(node.Bounds.X - image.Height, rect.Top + indent, image.Height, image.Height);
                g.DrawImage(image, rect);
            }
        }

        private void DrawImage(Graphics g, object obj, Rectangle irect)
        {
            if (obj != DBNull.Value)
            {
                if (obj is Bitmap bitmap)
                {
                    double w1 = irect.Width * 1.0 / bitmap.Width;
                    double h1 = irect.Height * 1.0 / bitmap.Height;
                    double wh = Math.Min(w1, h1);
                    if (wh > 1) irect.X += 1;
                    irect.Width = (int)(bitmap.Width * wh);
                    int height = irect.Height;
                    irect.Height = (int)(bitmap.Height * wh);
                    irect.Y += (height - irect.Height) / 2;
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
            Color foreColor = (node == null || node.ForeColor == Color.Empty) ? ColorHotFore : node.ForeColor;
            Color backColor = (node == null || node.BackColor == Color.Empty) ? ColorHot : node.BackColor;
            C_DrawNode(g, foreColor, backColor, node);
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
            if (Checking) return;

            Checking = true;
            AfterCheck -= DrawTreeView_AfterCheck;
            try
            {
                (e.Node as ItemNode).CheckHight = false;
                ParentNodeCheck(e.Node.Parent);
                ChildNodeCheck(e.Node);
                this.Refresh();
            }
            finally
            {
                Checking = false;
                AfterCheck += DrawTreeView_AfterCheck;
            }
        }
        /// <summary>
        /// 检查选中点
        /// </summary>
        public void Checked()
        {
            this.Checking = true;
            Checked(this.Nodes);
            this.Checking = false;
        }
        private void Checked(TreeNodeCollection nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Nodes.Count > 0)
                {
                    Checked(nodes[i].Nodes);
                }
                if (i == nodes.Count - 1)
                {
                    ParentNodeCheck(nodes[i].Parent);
                }
            }
        }
        /// <summary>
        ///     父节点
        /// </summary>
        private void ParentNodeCheck(TreeNode node, bool iInvalidate = false)
        {
            if (node == null) return;
            bool icheck = true, hight = false;
            for (var i = 0; i < node.Nodes.Count; i++)
            {
                var item = (ItemNode)node.Nodes[i];
                if (!item.Checked)
                {
                    icheck = false;
                }
                if (item.Checked || item.CheckHight)
                {
                    hight = true;
                }
            }
            if (node.Nodes.Count > 0)
            {
                node.Checked = icheck;
                (node as ItemNode).CheckHight = hight;
            }
            if (iInvalidate)
            {
                var rect = node.Bounds;
                rect = new Rectangle(1, rect.Top, Width, rect.Height);
                Invalidate(rect);
            }
            ParentNodeCheck(node.Parent, iInvalidate);
        }
        /// <summary>
        ///     子节点
        /// </summary>
        private void ChildNodeCheck(TreeNode node, bool iInvalidate = false)
        {
            for (var i = 0; i < node.Nodes.Count; i++)
            {
                node.Nodes[i].Checked = node.Checked;
                if (iInvalidate)
                {
                    var rect = node.Nodes[i].Bounds;
                    rect = new Rectangle(1, rect.Top, Width, rect.Height);
                    Invalidate(rect);
                }
                ChildNodeCheck(node.Nodes[i], iInvalidate);
            }
        }

        #endregion

        #region 移动事件
        private void InitDrop()
        {
            this.DragEnter += TreeView1_DragEnter;
            this.ItemDrag += TreeView1_ItemDrag;
            this.DragOver += TreeView1_DragOver;
            this.DragDrop += TreeView1_DragDrop;
        }
        private void TreeView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        private void TreeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }
        private void TreeView1_DragOver(object sender, DragEventArgs e)
        {
            var fromNode = (ItemNode)e.Data.GetData(typeof(ItemNode));
            if (fromNode == null) return;

            Point point = this.PointToClient(new Point(e.X, e.Y));
            var toNode = (ItemNode)this.GetNodeAt(point);
            if (toNode != null && fromNode != toNode)
            {
                TreeEventArgs te = new TreeEventArgs(fromNode, toNode);
                DropMoveEvent?.Invoke(this, te);
                if (te.Cancel)
                {
                    e.Effect = DragDropEffects.None;
                }
                else
                {
                    e.Effect = DragDropEffects.Move;
                    if (toNode.Nodes.Count > 0 && !toNode.IsExpanded)
                    {
                        toNode.Expand();
                    }
                }
            }
        }
        private void TreeView1_DragDrop(object sender, DragEventArgs e)
        {
            var fromNode = (TreeNode)e.Data.GetData(typeof(ItemNode));
            if (fromNode == null) return;

            Point point = this.PointToClient(new Point(e.X, e.Y));
            var toNode = this.GetNodeAt(point);
            if (toNode != null && fromNode != toNode)
            {
                var lastParent = fromNode.Parent;
                DropCompleteEvent?.Invoke(this, new TreeEventArgs(fromNode, toNode));
                if (lastParent != null && lastParent.Nodes.Count == 0)
                    this.UpdateNode(lastParent);
                if (fromNode.Parent != null && fromNode.Parent.Nodes.Count == 1)
                    fromNode.Parent.Expand();
            }
        }

        #endregion

        #region Dispose
        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (add != null)
                    add.Dispose();
                if (less != null)
                    less.Dispose();
                if (check_normal != null)
                    check_normal.Dispose();
                if (check_tick != null)
                    check_tick.Dispose();
                if (check_hight != null)
                    check_hight.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
    /// <summary>
    ///     提供树事件数据
    /// </summary>
    public class TreeEventArgs : EventArgs
    {
        /// <summary>
        ///  从此节点移出
        /// </summary>
        public TreeNode FromNode { get; private set; }
        /// <summary>
        ///  移入此节点
        /// </summary>
        public TreeNode ToNode { get; private set; }
        /// <summary>
        /// 是否取消
        /// </summary>
        public bool Cancel { get; set; }
        /// <summary>
        /// 提供树事件数据
        /// </summary>
        public TreeEventArgs(TreeNode fromNode, TreeNode toNode)
        {
            this.FromNode = fromNode;
            this.ToNode = toNode;
        }
    }

    /// <summary>
    ///     自定义树节点
    /// </summary>
    [Serializable]
    public class ItemNode : TreeNode
    {
        private DataRow dr;
        /// <summary>
        ///     DataRow数据
        /// </summary>
        public DataRow DataRow
        {
            get { return dr; }
            set { dr = value; }
        }

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
        /// 部分选中
        /// </summary>
        public bool CheckHight { get; set; }

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
        #region 构造函数

        /// <summary>
        ///     初始化 Paway.Forms.TreeItemCollection 新的实例。
        /// </summary>
        public TreeItemCollection() { }

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
        public string Name { get; set; }

        /// <summary>
        ///     项的长度
        /// </summary>
        [DefaultValue(100)]
        public int Width { get; set; } = 100;

        /// <summary>
        ///     文本显示的位置,左或右
        /// </summary>
        [DefaultValue(StringAlignment.Near)]
        public StringAlignment Alignment { get; set; }

        /// <summary>
        ///     当前 Item 在 TreeItem 中的 Rectangle
        /// </summary>
        [Browsable(false)]
        public Rectangle Rectangle { get; protected set; }

        /// <summary>
        /// 对齐
        /// </summary>
        [DefaultValue(false)]
        public bool IAlign { get; set; }

        #endregion
    }
}