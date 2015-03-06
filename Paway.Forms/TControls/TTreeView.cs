using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Paway.Helper;
using System.Collections;
using System.Reflection;

namespace Paway.Forms
{
    /// <summary>
    /// 选中与颜色重绘
    /// 设置Id,ParentId,Root自动添加树节点
    /// 多列显示[待续]
    /// </summary>
    public class TTreeView : TreeView
    {
        /// <summary>
        /// 构造
        /// </summary>
        public TTreeView()
        {
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();
            //选中与颜色重绘
            this.DrawMode = TreeViewDrawMode.OwnerDrawText;
            this.ItemHeight = 21;
            this.HideSelection = false;
            this.AfterCheck += DrawTreeView_AfterCheck;
        }

        #region 变量
        /// <summary>
        /// 鼠标上一次移过的项
        /// </summary>
        private TreeNode lastnode = null;

        #endregion

        #region 属性
        private Color _select = Color.DodgerBlue;
        /// <summary>
        /// 项被选中后的背景颜色
        /// </summary>
        [Browsable(true), Category("控件的重绘设置"), Description("项被选中后的背景颜色")]
        [DefaultValue(typeof(Color), "DodgerBlue")]
        public Color ColorSelect { get { return _select; } set { _select = value; } }
        private Color _selectFore = Color.White;
        /// <summary>
        /// 项被选中后的字体颜色
        /// </summary>
        [Browsable(true), Category("控件的重绘设置"), Description("项被选中后的字体颜色")]
        [DefaultValue(typeof(Color), "White")]
        public Color ColorSelectFore { get { return _selectFore; } set { _selectFore = value; } }
        private Color _hot = Color.LightBlue;
        /// <summary>
        /// 鼠标移过项时的背景颜色
        /// </summary>
        [Browsable(true), Category("控件的重绘设置"), Description("鼠标移过项时的背景颜色")]
        [DefaultValue(typeof(Color), "LightBlue")]
        public Color ColorHot { get { return _hot; } set { _hot = value; } }
        private Color _hotFore = Color.White;
        /// <summary>
        /// 鼠标移过项时的字体颜色
        /// </summary>
        [Browsable(true), Category("控件的重绘设置"), Description("鼠标移过项时的字体颜色")]
        [DefaultValue(typeof(Color), "White")]
        public Color ColorHotFore { get { return _hotFore; } set { _hotFore = value; } }

        /// <summary>
        /// 根节点
        /// </summary>
        [TypeConverter(typeof(StringConverter))]
        [Browsable(true), Category("控件的数据设置"), Description("根节点"), DefaultValue(null)]
        public object TRoot { get; set; }
        private object _parentId = "ParentId";
        /// <summary>
        /// 父节点字段
        /// </summary>
        [TypeConverter(typeof(StringConverter))]
        [Browsable(true), Category("控件的数据设置"), Description("父节点字段"), DefaultValue("ParentId")]
        public object TParentId { get { return _parentId; } set { _parentId = value; } }
        private object _id = "Id";
        /// <summary>
        /// 子节点字段
        /// </summary>
        [TypeConverter(typeof(StringConverter))]
        [Browsable(true), Category("控件的数据设置"), Description("子节点字段"), DefaultValue("Id")]
        public object TId { get { return _id; } set { _id = value; } }
        private bool _autoWidth = true;
        /// <summary>
        /// 自动调整宽度至整行
        /// </summary>
        [Browsable(true), Description("自动调整宽度至整行"), DefaultValue(true)]
        public bool TAutoWidth
        {
            get { return _autoWidth; }
            set
            {
                _autoWidth = value;
                if (_dataSource == null || TId == null || TParentId == null) return;
                Type type = _dataSource.GetType();
                if (_dataSource is IList)
                {
                    IList list = _dataSource as IList;
                    type = list.GetListType();
                }
                UpdateColumns(type);
            }
        }
        private object _dataSource;
        /// <summary>
        /// 数据源
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
        /// TreeView中的显示项
        /// </summary>
        private TreeItemCollection _items = null;
        /// <summary>
        /// TreeView中的显示项
        /// </summary>
        [Description("TreeView中的显示项"), EditorBrowsable(EditorBrowsableState.Always), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TreeItemCollection Items
        {
            get
            {
                if (this._items == null)
                    this._items = new TreeItemCollection(this);
                return this._items;
            }
        }

        #endregion

        #region 节点添加
        /// <summary>
        /// 刷新数据
        /// </summary>
        public void RefreshData()
        {
            this.Nodes.Clear();
            if (_dataSource == null || TId == null || TParentId == null) return;
            DataTable dt = null;
            Type type = _dataSource.GetType();
            if (_dataSource is DataTable)
            {
                dt = _dataSource as DataTable;
            }
            else if (_dataSource is IList)
            {
                IList list = _dataSource as IList;
                type = list.GetListType();
                dt = type.ToDataTable(list);
            }
            else if (type == typeof(String) || type.IsValueType)
            {
                TreeNode node = new TreeNode();
                node.Text = _dataSource.ToString();
                this.Nodes.Add(node);
            }
            UpdateColumns(type);
            if (dt != null && dt.Rows.Count > 0)
            {
                AddNodes(dt, type);
            }
        }
        /// <summary>
        /// 刷新Node
        /// </summary>
        public void UpdateNode(object id)
        {
            if (id == null || _dataSource == null || TId == null || TParentId == null) return;
            DataTable dt = null;
            Type type = _dataSource.GetType();
            if (_dataSource is DataTable)
            {
                dt = _dataSource as DataTable;
            }
            else if (_dataSource is IList)
            {
                IList list = _dataSource as IList;
                type = list.GetListType();
                dt = type.ToDataTable(list, id);
            }
            if (dt == null) return;
            DataRow[] dr = dt.Select(string.Format("{0} = '{1}'", TId, id));
            if (dr.Length == 1)
            {
                TreeNode[] nodes = this.Nodes.Find(id.ToString(), true);
                if (nodes.Length == 1)
                {
                    UpdateNode(this.Nodes, dr[0], id.ToString());
                }
                else if (nodes.Length == 0)
                {
                    string parent = dr[0][TParentId.ToString()].ToString();
                    if (!AddNode(this.Nodes, dr[0], parent))
                    {
                        AddNode(this.Nodes, dr[0]);
                    }
                }
            }
            else if (dr.Length == 0)
            {
                DeleteNode(this.Nodes, id.ToString());
            }
        }
        private bool AddNode(TreeNodeCollection nodes, DataRow dr, string parent)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Name == parent)
                {
                    AddNode(nodes[i].Nodes, dr);
                    return true;
                }
                else
                {
                    if (AddNode(nodes[i].Nodes, dr, parent)) return true;
                }
            }
            return false;
        }
        private void AddNode(TreeNodeCollection nodes, DataRow dr)
        {
            ItemNode node = new ItemNode(dr);
            node.Text = (_items.Count > 0 ? dr[_items[0].Name] : dr[TId.ToString()]).ToString();
            node.Name = dr[TId.ToString()].ToString();
            nodes.Add(node);
        }
        private bool UpdateNode(TreeNodeCollection nodes, DataRow dr, string name)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Name == name)
                {
                    ItemNode old = nodes[i] as ItemNode;
                    string oldParent = old[TParentId.ToString()].ToString2();
                    TreeNodeCollection temp = nodes[i].Nodes;
                    nodes.RemoveAt(i);
                    ItemNode node = new ItemNode(dr);
                    node.Text = (_items.Count > 0 ? dr[_items[0].Name] : dr[TId.ToString()]).ToString();
                    node.Name = dr[TId.ToString()].ToString();
                    for (int j = 0; j < temp.Count; j++)
                    {
                        node.Nodes.Add(temp[j]);
                    }
                    string newParent = dr[TParentId.ToString()].ToString2();
                    if (oldParent == newParent)
                    {
                        nodes.Insert(i, node);
                    }
                    else
                    {
                        TreeNode[] parentNodes = this.Nodes.Find(newParent, true);
                        if (parentNodes.Length == 1)
                        {
                            parentNodes[0].Nodes.Add(node);
                        }
                        else
                        {
                            this.Nodes.Add(node);
                        }
                    }
                    return true;
                }
                else
                {
                    if (UpdateNode(nodes[i].Nodes, dr, name)) return true;
                }
            }
            return false;
        }
        private bool DeleteNode(TreeNodeCollection nodes, string name)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Name == name)
                {
                    nodes.RemoveAt(i);
                    return true;
                }
                else
                {
                    if (DeleteNode(nodes[i].Nodes, name)) return true;
                }
            }
            return false;
        }
        private void AddNodes(DataTable dt, Type type)
        {
            DataRow[] dr = null;
            if (TRoot == null)
            {
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(type);
                for (int i = 0; i < props.Count; i++)
                {
                    if (props[i].Name == TParentId.ToString())
                    {
                        if (!props[i].PropertyType.IsValueType && props[i].PropertyType != typeof(String))
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
            for (int i = 0; i < dr.Length; i++)
            {
                ItemNode node = new ItemNode(dr[i]);
                node.Text = (_items.Count > 0 ? dr[i][_items[0].Name] : dr[i][TId.ToString()]).ToString();
                node.Name = dr[i][TId.ToString()].ToString();
                this.Nodes.Add(node);
                AddNodes(dt, node, dr[i][TId.ToString()].ToString());
            }
        }
        private void AddNodes(DataTable dt, TreeNode parent, string id)
        {
            DataRow[] dr = dt.Select(string.Format("{0} = '{1}'", TParentId, id));
            for (int i = 0; i < dr.Length; i++)
            {
                ItemNode node = new ItemNode(dr[i]);
                node.Text = (_items.Count > 0 ? dr[i][_items[0].Name] : dr[i][TId.ToString()]).ToString();
                node.Name = dr[i][TId.ToString()].ToString();
                parent.Nodes.Add(node);
                AddNodes(dt, node, dr[i][TId.ToString()].ToString());
            }
        }
        private string GetText(DataRow dr)
        {
            string result = null;
            for (int j = 0; j < this._items.Count; j++)
            {
                result = string.Format("{0}{1}&", result, dr[_items[j].Name]);
            }
            if (result == null) result = dr[TId.ToString()].ToString();
            else result = result.TrimEnd('&');
            return result;
        }
        /// <summary>
        /// 更新列名称
        /// </summary>
        public void UpdateColumns(Type type)
        {
            if (type == null || type == typeof(String) || type.IsValueType) return;
            if (Items.Count == 0)
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(type);
                for (int i = 0; i < properties.Count; i++)
                {
                    PropertyInfo pro = type.GetProperty(properties[i].Name);
                    if (pro == null) continue;
                    PropertyAttribute[] itemList = pro.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
                    if (itemList != null && itemList.Length != 0)
                    {
                        if (!itemList[0].Show)
                        {
                            this.Items.Add(new TreeItem());
                        }
                    }
                }
            }
            if (TAutoWidth)
            {
                int total = 0;
                for (int i = 0; i < Items.Count; i++)
                {
                    total += Items[i].Width;
                }
                for (int i = 0; i < Items.Count; i++)
                {
                    Items[i].Width = Items[i].Width * this.Width / total;
                }
            }
        }

        #endregion

        #region 选中与颜色重绘
        /// <summary>
        /// 移过行
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            TreeNode node = this.GetNodeAt(e.Location);
            if (node != null && lastnode != null && lastnode.Name == node.Name) return;

            C_DrawNode(ColorHotFore, ColorHot, node);
            C_DrawNode(this.ForeColor, this.BackColor, lastnode);
            lastnode = node;
        }
        /// <summary>
        /// 双击展开项
        /// </summary>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            TreeNode node = this.GetNodeAt(e.Location);
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
        /// <summary>
        /// 整行选中
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left) return;
            TreeNode node = this.GetNodeAt(e.Location);
            if (node != null)
            {
                this.SelectedNode = node;
            }
        }
        /// <summary>
        /// 重绘Text
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            base.OnDrawNode(e);
            Color foreColor;
            Color backColor;
            if ((e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected)
            {
                foreColor = this.ColorSelectFore;
                backColor = this.ColorSelect;
            }
            else
            {
                foreColor = this.ForeColor;
                backColor = this.BackColor;
            }
            Rectangle rect = new Rectangle(new Point(e.Bounds.Location.X + 1, e.Bounds.Location.Y), new Size(this.Width - e.Bounds.X - 1, e.Bounds.Height));
            e.Graphics.FillRectangle(new SolidBrush(backColor), rect);
            C_DrawString(e.Graphics, e.Node, rect, foreColor);
        }
        private void C_DrawNode(Color foreColor, Color backColor, TreeNode node)
        {
            if (node == null) return;
            if (this.SelectedNode != null && this.SelectedNode.Name == node.Name) return;
            Graphics g = this.CreateGraphics();
            Rectangle rect = new Rectangle(node.Bounds.Location, new Size(this.Width - node.Bounds.X, node.Bounds.Height));

            g.FillRectangle(new SolidBrush(backColor), rect);
            C_DrawString(g, node, rect, foreColor);
            g.Dispose();
        }
        /// <summary>
        /// 通过绘制实现多列
        /// </summary>
        private void C_DrawString(Graphics g, TreeNode node, Rectangle rect, Color foreColor)
        {
            if (rect.Height == 0) return;
            ItemNode item = node as ItemNode;
            if (item == null || _items.Count <= 1)
            {
                TextRenderer.DrawText(g, node.Text, this.Font, rect, foreColor, DrawParam.TextEnd);
            }
            else
            {
                int x = this.Indent + 3;
                int left = 0;
                TextFormatFlags format = TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis;
                for (int i = 0; i < _items.Count; i++)
                {
                    if (item[_items[i].Name] is DateTime && item[_items[i].Name].ToDateTime() == DateTime.MinValue) continue;
                    Rectangle irect = new Rectangle(
                        rect.X + left, rect.Y,
                        _items[i].Width + (i == 0 ? x - rect.X : 0), rect.Height
                        );
                    left += irect.Width;
                    if (_items[i].Type == TreeItemType.Text)
                    {
                        TextFormatFlags temp = format;
                        switch (_items[i].Alignment)
                        {
                            case StringAlignment.Center:
                                temp = temp | TextFormatFlags.HorizontalCenter;
                                break;
                            case StringAlignment.Far:
                                temp = temp | TextFormatFlags.Right;
                                break;
                        }
                        TextRenderer.DrawText(g, item[_items[i].Name].ToString(), this.Font, irect, foreColor, temp);
                    }
                    else
                    {
                        object obj = item[_items[i].Name];
                        DrawImage(g, obj, irect);
                    }
                }
            }
        }
        private void DrawImage(Graphics g, object obj, Rectangle irect)
        {
            if (obj != DBNull.Value)
            {
                Bitmap bitmap = obj as Bitmap;
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

        #region 节点选中事件
        void DrawTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            this.AfterCheck -= DrawTreeView_AfterCheck;
            ParentNodeCheck(e.Node.Parent);
            for (int i = 0; i < e.Node.Nodes.Count; i++)
            {
                e.Node.Nodes[i].Checked = e.Node.Checked;
            }
            this.AfterCheck += DrawTreeView_AfterCheck;
        }
        /// <summary>
        /// 父节点
        /// </summary>
        /// <param name="node"></param>
        private void ParentNodeCheck(TreeNode node)
        {
            if (node == null) return;
            bool result = true;
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                if (!node.Nodes[i].Checked)
                {
                    result = false;
                    break;
                }
            }
            node.Checked = result;
        }

        #endregion
    }

    /// <summary>
    /// 自定义树节点
    /// </summary>
    [Serializable]
    public class ItemNode : TreeNode
    {
        private DataRow dr = null;
        /// <summary>
        /// 默认构造
        /// </summary>
        public ItemNode()
        { }
        /// <summary>
        /// 初始化，加载DataRow数据
        /// </summary>
        /// <param name="dr"></param>
        public ItemNode(DataRow dr)
        {
            this.dr = dr;
        }
        /// <summary>
        /// TreeView中ItemNode的键值
        /// </summary>
        public object this[string key]
        {
            get
            {
                return dr[key];
            }
        }
        /// <summary>
        /// TreeView中ItemNode的键值
        /// </summary>
        public object this[int index]
        {
            get
            {
                return dr[index];
            }
        }
    }

    /// <summary>
    /// 代表 TreeView 中项的集合。
    /// </summary>
    [ListBindable(false)]
    public class TreeItemCollection : List<TreeItem>
    {
        #region 变量
        /// <summary>
        /// TreeView
        /// </summary>
        private TreeView _owner = null;

        #endregion

        #region 构造函数
        /// <summary>
        /// 初始化 Paway.Forms.TreeItemCollection 新的实例。
        /// </summary>
        /// <param name="owner">ToolBar</param>
        public TreeItemCollection(TreeView owner)
        {
            this._owner = owner;
        }

        #endregion

        #region 方法
        /// <summary>
        /// 返回该项在集合中的索引值
        /// </summary>
        public int GetIndexOfRange(TreeItem item)
        {
            int result = -1;
            for (int i = 0; i < base.Count; i++)
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
    /// 表示 TreeView 控件中的单个显示项。
    /// </summary>
    public class TreeItem
    {
        #region 属性
        /// <summary>
        /// Item 显示的类型
        /// </summary>
        [DefaultValue(TreeItemType.Text)]
        public TreeItemType Type { get; set; }
        /// <summary>
        /// Item 上绑定的文本字段
        /// </summary>
        [DefaultValue("toolItem")]
        public string Name { get; set; }
        private int width = 100;
        /// <summary>
        /// 项的长度
        /// </summary>
        public int Width { get { return width; } set { width = value; } }
        /// <summary>
        /// 文本显示的位置,左或右
        /// </summary>
        [DefaultValue(StringAlignment.Near)]
        public StringAlignment Alignment { get; set; }
        /// <summary>
        /// 当前 Item 在 TreeItem 中的 Rectangle
        /// </summary>
        public Rectangle Rectangle { get; protected set; }
        /// <summary>
        /// 获取或设置包含有关控件的数据的对象。 
        /// </summary>
        [TypeConverter(typeof(StringConverter))]
        public object Tag { get; set; }

        #endregion
    }

    /// <summary>
    /// 列中显示图片或文本
    /// </summary>
    public enum TreeItemType
    {
        /// <summary>
        /// 文本
        /// </summary>
        Text,
        /// <summary>
        /// 图像
        /// </summary>
        Image,
    }
}
