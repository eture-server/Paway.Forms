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
        /// 
        /// </summary>
        public TTreeView()
        {
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.DoubleBuffer, true);
            this.UpdateStyles();

            //选中与颜色重绘
            this.DrawMode = TreeViewDrawMode.OwnerDrawText;
            this.ItemHeight = 21;
            this.HideSelection = false;
            this.AfterCheck += DrawTreeView_AfterCheck;
        }

        #region 选中与颜色重绘
        #region 属性
        private Color _colorSelect = Color.DodgerBlue;
        /// <summary>
        /// 项被选中后的背景颜色
        /// </summary>
        [Browsable(true), Category("控件的重绘设置"), Description("项被选中后的背景颜色")]
        [DefaultValue(typeof(Color), "DodgerBlue")]
        public Color ColorSelect
        {
            get { return _colorSelect; }
            set
            {
                _colorSelect = value;
                this.Invalidate();
            }
        }
        private Color _colorSelectFore = Color.White;
        /// <summary>
        /// 项被选中后的字体颜色
        /// </summary>
        [Browsable(true), Category("控件的重绘设置"), Description("项被选中后的字体颜色")]
        [DefaultValue(typeof(Color), "White")]
        public Color ColorSelectFore
        {
            get { return _colorSelectFore; }
            set
            {
                _colorSelectFore = value;
                this.Invalidate();
            }
        }
        private Color _colorHot = Color.LightBlue;
        /// <summary>
        /// 鼠标移过项时的背景颜色
        /// </summary>
        [Browsable(true), Category("控件的重绘设置"), Description("鼠标移过项时的背景颜色")]
        [DefaultValue(typeof(Color), "LightBlue")]
        public Color ColorHot
        {
            get { return _colorHot; }
            set
            {
                _colorHot = value;
                this.Invalidate();
            }
        }
        private Color _colorHotFore = Color.White;
        /// <summary>
        /// 鼠标移过项时的字体颜色
        /// </summary>
        [Browsable(true), Category("控件的重绘设置"), Description("鼠标移过项时的字体颜色")]
        [DefaultValue(typeof(Color), "White")]
        public Color ColorHotFore
        {
            get { return _colorHotFore; }
            set
            {
                _colorHotFore = value;
                this.Invalidate();
            }
        }
        private TreeNode lastnode = null;

        #endregion

        #region 方法
        /// <summary>
        /// 移过整行
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            TreeNode node = this.GetNodeAt(e.Location);
            if (node != null && lastnode != null && lastnode.Name == node.Name) return;

            C_DrawNode(_colorHotFore, _colorHot, node);
            C_DrawNode(this.ForeColor, this.BackColor, lastnode);
            lastnode = node;
        }
        /// <summary>
        /// 整行双击
        /// </summary>
        /// <param name="e"></param>
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
                foreColor = this._colorSelectFore;
                backColor = this._colorSelect;
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
        }
        /// <summary>
        /// 通过绘制实现多列
        /// </summary>
        /// <param name="g"></param>
        /// <param name="node"></param>
        /// <param name="rect"></param>
        /// <param name="foreColor"></param>
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
                for (int i = 0; i < _items.Count; i++)
                {
                    if (_items[i].LDirection == TLocation.Right) continue;
                    Rectangle irect = new Rectangle(
                        rect.X + left, rect.Y,
                        _items[i].Width + (i == 0 ? x - rect.X : 0), rect.Height
                        );
                    left += _items[i].Width + x - rect.X;
                    if (_items[i].Type == TreeItemType.Text)
                    {
                        TextRenderer.DrawText(g, item[_items[i].Name].ToString(), this.Font, irect, foreColor, DrawParam.TextEnd);
                    }
                    else
                    {
                        object obj = item[_items[i].Name];
                        DrawImage(g, obj, irect);
                    }
                }
                int right = 0;
                for (int i = _items.Count - 1; i >= 0; i--)
                {
                    if (_items[i].LDirection == TLocation.Left) continue;
                    Rectangle irect = new Rectangle(
                        this.Width - _items[i].Width - right, rect.Y,
                        _items[i].Width, rect.Height
                        );
                    right += _items[i].Width;
                    if (_items[i].Type == TreeItemType.Text)
                    {
                        TextRenderer.DrawText(g, item[_items[i].Name].ToString(), this.Font, irect, foreColor, DrawParam.TextEnd);
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

        #endregion

        #region 节点添加
        #region 属性
        private object _root = 0;
        /// <summary>
        /// 根节点
        /// </summary>
        [TypeConverter(typeof(StringConverter))]
        [Browsable(true), Category("控件的数据设置"), Description("根节点"), DefaultValue(0)]
        public object Root
        {
            get { return _root; }
            set { _root = value; }
        }
        private object _parentId = "ParentId";
        /// <summary>
        /// 父节点字段
        /// </summary>
        [TypeConverter(typeof(StringConverter))]
        [Browsable(true), Category("控件的数据设置"), Description("父节点字段"), DefaultValue("ParentId")]
        public object ParentId
        {
            get { return _parentId; }
            set { _parentId = value; }
        }
        private object _id = "Id";
        /// <summary>
        /// 子节点字段
        /// </summary>
        [TypeConverter(typeof(StringConverter))]
        [Browsable(true), Category("控件的数据设置"), Description("子节点字段"), DefaultValue("Id")]
        public object Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private object _dataSource;
        /// <summary>
        /// 数据源
        /// </summary>
        [Browsable(true), Category("控件的数据设置"), Description("数据源")]
        public object DataSource
        {
            get { return _dataSource; }
            set
            {
                _dataSource = value;
                InitData();
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

        #region 方法
        private void InitData()
        {
            if (_dataSource == null || _id == null || _parentId == null || _root == null) return;
            DataTable dt = null;
            if (_dataSource is DataTable)
            {
                dt = _dataSource as DataTable;
            }
            else if (_dataSource is IList)
            {
                IList list = _dataSource as IList;
                Type type = list.GetType();
                Type[] types = type.GetGenericArguments();
                if (types.Length != 1) return;
                dt = types[0].ToDataTable(list);
            }
            else if (_dataSource is string)
            {
                TreeNode node = new TreeNode();
                node.Text = _dataSource.ToString();
                this.Nodes.Add(node);
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                AddNodes(dt);
            }
        }
        private void AddNodes(DataTable dt)
        {
            DataRow[] dr = dt.Select(string.Format("{0} = '{1}'", _id, _root));
            if (dr.Length > 0) throw new Exception("子节点不可与根节点相同");
            dr = dt.Select(string.Format("{0} = '{1}'", _parentId, _root));
            for (int i = 0; i < dr.Length; i++)
            {
                ItemNode node = new ItemNode(dr[i]);
                node.Text = (_items.Count > 0 ? dr[i][_items[0].Name] : dr[i][_id.ToString()]).ToString();
                node.Name = dr[i][_id.ToString()].ToString();
                this.Nodes.Add(node);
                AddNodes(dt, node, dr[i][_id.ToString()].ToString());
            }
        }
        private void AddNodes(DataTable dt, TreeNode parent, string id)
        {
            DataRow[] dr = dt.Select(string.Format("{0} = '{1}'", _parentId, id));
            for (int i = 0; i < dr.Length; i++)
            {
                ItemNode node = new ItemNode(dr[i]);
                node.Text = (_items.Count > 0 ? dr[i][_items[0].Name] : dr[i][_id.ToString()]).ToString();
                node.Name = dr[i][_id.ToString()].ToString();
                parent.Nodes.Add(node);
                AddNodes(dt, node, dr[i][_id.ToString()].ToString());
            }
        }
        private string GetText(DataRow dr)
        {
            string result = null;
            for (int j = 0; j < this._items.Count; j++)
            {
                result = string.Format("{0}{1}&", result, dr[_items[j].Name]);
            }
            if (result == null) result = dr[_id.ToString()].ToString();
            else result = result.TrimEnd('&');
            return result;
        }
        #endregion

        #endregion

        #region 节点点击事件
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
        /// <param name="item"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 项的长度
        /// </summary>
        public int Width { get; set; }
        private TLocation _lDirection = TLocation.Left;
        /// <summary>
        /// 文本显示的位置,左或右
        /// </summary>
        [DefaultValue(TLocation.Left)]
        public TLocation LDirection { get { return _lDirection; } set { _lDirection = value; } }
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
