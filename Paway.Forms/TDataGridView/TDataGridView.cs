using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Paway.Helper;

namespace Paway.Forms
{
    /// <summary>
    /// 表头全选功能的实现
    /// </summary>
    public class TDataGridView : DataGridView
    {
        #region 变量
        private Type type;

        /// <summary>
        /// 原数据源
        /// </summary>
        private object source;

        /// <summary>
        /// 需要2维表头的列
        /// </summary>
        private readonly Dictionary<int, SpanInfo> SpanRows = new Dictionary<int, SpanInfo>();

        /// <summary>
        /// 表头信息
        /// </summary>
        private struct SpanInfo
        {
            /// <summary>
            /// 列主标题
            /// </summary>
            public readonly string Text;

            /// <summary>
            /// 对应左行
            /// </summary>
            public readonly int Left;

            /// <summary>
            /// 对应右行
            /// </summary>
            public readonly int Right;

            /// <summary>
            /// 显示子列标记
            /// </summary>
            public readonly bool IAll;

            /// <summary>
            /// 水平对齐方式
            /// </summary>
            public readonly StringAlignment Alignment;

            public SpanInfo(string Text, int Left, int Right, bool iAll, StringAlignment alignment)
            {
                this.Text = Text;
                this.Left = Left;
                this.Right = Right;
                this.IAll = iAll;
                this.Alignment = alignment;
            }
        }

        /// <summary>
        /// 合并单元格的列
        /// </summary>
        private readonly List<int> SpanColumns = new List<int>();

        #endregion

        #region 事件
        /// <summary>
        /// 数据刷新后触发
        /// </summary>
        public event Action<TDataGridView> RefreshChanged;
        /// <summary>
        /// CheckBox选中事件
        /// </summary>
        public event Action<int, int, bool> CheckedChanged;
        /// <summary>
        /// 合并单元格取消事件
        /// </summary>
        public event Func<int, string, bool> SpanEvent;
        /// <summary>
        /// 行双击事件
        /// </summary>
        public event Action<TDataGridView, int> RowDoubleClick;
        /// <summary>
        /// 按钮点击事件
        /// </summary>
        public event Action<int, int, object> ButtonClicked;

        #endregion

        #region 构造函数
        /// <summary>
        /// 构造
        /// </summary>
        public TDataGridView()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

            CellMouseEnter += ComBoxGridView_CellMouseEnter;
            CellMouseLeave += ComBoxGridView_CellMouseLeave;
            CellFormatting += TDataGridView_CellFormatting;
            CellMouseDown += TDataGridView_CellMouseDown;
            CellMouseUp += TDataGridView_CellMouseUp;
            DoubleClick += TDataGridView_DoubleClick;
            BackgroundColor = Color.White;
            BorderStyle = BorderStyle.None;

            //设置默认值
            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            ColumnHeadersHeight = 30;
            MultiSelect = false;
            RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            RowHeadersVisible = false;
            RowHeadersWidth = 21;
            ScrollBars = ScrollBars.Both;
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ReadOnly = true;
            EnableHeadersVisualStyles = false;

            //设置非默认值
            this.RowTemplate.DefaultCellStyle.ForeColor = Color.Black;
            this.RowTemplate.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            this.RowTemplate.DefaultCellStyle.SelectionForeColor = Color.Black;
            this.RowTemplate.Height = 32;
            this.RowTemplate.Resizable = DataGridViewTriState.False;
        }

        #endregion

        #region 重载属性默认值
        /// <summary>
        /// 获取或设置 DataGridView 的背景色。
        /// </summary>
        [Description("获取或设置 DataGridView 的背景色")]
        [DefaultValue(typeof(Color), "White")]
        public new Color BackgroundColor
        {
            get { return base.BackgroundColor; }
            set { base.BackgroundColor = value; }
        }

        /// <summary>
        /// 获取或设置 DataGridView 的边框样式。
        /// </summary>
        [Description("获取或设置 DataGridView 的边框样式")]
        [DefaultValue(BorderStyle.None)]
        public new BorderStyle BorderStyle
        {
            get { return base.BorderStyle; }
            set { base.BorderStyle = value; }
        }

        /// <summary>
        /// 设置数据源时设置图片列
        /// </summary>
        [Browsable(false)]
        [Description("设置数据源时设置图片列")]
        [AttributeProvider(typeof(IListSource))]
        [RefreshProperties(RefreshProperties.Repaint)]
        [DefaultValue(null)]
        public new object DataSource
        {
            get { return base.DataSource ?? source; }
            set
            {
                source = value;
                UpdateData(value);
            }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示是否向用户显示添加行的选项。
        /// </summary>
        [Description("获取或设置一个值，该值指示是否向用户显示添加行的选项")]
        [DefaultValue(false)]
        public new bool AllowUserToAddRows
        {
            get { return base.AllowUserToAddRows; }
            set { base.AllowUserToAddRows = value; }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示是否允许用户从 DataGridView 中删除行。
        /// </summary>
        [Description("获取或设置一个值，该值指示是否允许用户从 DataGridView 中删除行")]
        [DefaultValue(false)]
        public new bool AllowUserToDeleteRows
        {
            get { return base.AllowUserToDeleteRows; }
            set { base.AllowUserToDeleteRows = value; }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示如何确定列宽。
        /// </summary>
        [Description("获取或设置一个值，该值指示如何确定列宽")]
        [DefaultValue(DataGridViewAutoSizeColumnsMode.Fill)]
        public new DataGridViewAutoSizeColumnsMode AutoSizeColumnsMode
        {
            get { return base.AutoSizeColumnsMode; }
            set { base.AutoSizeColumnsMode = value; }
        }

        /// <summary>
        /// 获取 DataGridView 的单元格边框样式。
        /// </summary>
        [Description("获取 DataGridView 的单元格边框样式")]
        [DefaultValue(DataGridViewCellBorderStyle.SingleHorizontal)]
        public new DataGridViewCellBorderStyle CellBorderStyle
        {
            get { return base.CellBorderStyle; }
            set { base.CellBorderStyle = value; }
        }

        /// <summary>
        /// 获取应用于列标题的边框样式。
        /// </summary>
        [Description("获取应用于列标题的边框样式")]
        [DefaultValue(DataGridViewHeaderBorderStyle.Single)]
        public new DataGridViewHeaderBorderStyle ColumnHeadersBorderStyle
        {
            get { return base.ColumnHeadersBorderStyle; }
            set { base.ColumnHeadersBorderStyle = value; }
        }

        /// <summary>
        /// 获取或设置列标题行的高度（以像素为单位）。
        /// </summary>
        [Description("获取或设置列标题行的高度（以像素为单位）")]
        [DefaultValue(30)]
        public new int ColumnHeadersHeight
        {
            get { return base.ColumnHeadersHeight; }
            set { base.ColumnHeadersHeight = value; }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示是否可以调整列标题的高度，以及它是由用户调整还是根据标题的内容自动调整。
        /// </summary>
        [Description("获取或设置一个值，该值指示是否可以调整列标题的高度，以及它是由用户调整还是根据标题的内容自动调整")]
        [DefaultValue(DataGridViewColumnHeadersHeightSizeMode.DisableResizing)]
        public new DataGridViewColumnHeadersHeightSizeMode ColumnHeadersHeightSizeMode
        {
            get { return base.ColumnHeadersHeightSizeMode; }
            set { base.ColumnHeadersHeightSizeMode = value; }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示是否允许用户一次选择 DataGridView 的多个单元格、行或列。
        /// </summary>
        [Description("获取或设置一个值，该值指示是否允许用户一次选择 DataGridView 的多个单元格、行或列")]
        [DefaultValue(false)]
        public new bool MultiSelect
        {
            get { return base.MultiSelect; }
            set { base.MultiSelect = value; }
        }

        /// <summary>
        /// 获取或设置行标题单元格的边框样式。
        /// </summary>
        [Description("获取或设置行标题单元格的边框样式")]
        [DefaultValue(DataGridViewHeaderBorderStyle.Single)]
        public new DataGridViewHeaderBorderStyle RowHeadersBorderStyle
        {
            get { return base.RowHeadersBorderStyle; }
            set { base.RowHeadersBorderStyle = value; }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示是否显示包含行标题的列。
        /// </summary>
        [Description("获取或设置一个值，该值指示是否显示包含行标题的列")]
        [DefaultValue(false)]
        public new bool RowHeadersVisible
        {
            get { return base.RowHeadersVisible; }
            set { base.RowHeadersVisible = value; }
        }

        /// <summary>
        /// 获取或设置包含行标题的列的宽度（以像素为单位）。
        /// </summary>
        [Description("获取或设置包含行标题的列的宽度（以像素为单位）")]
        [DefaultValue(21)]
        public new int RowHeadersWidth
        {
            get { return base.RowHeadersWidth; }
            set { base.RowHeadersWidth = value; }
        }

        /// <summary>
        /// 获取或设置要在 DataGridView 控件中显示的滚动条的类型。
        /// </summary>
        [Description("获取或设置要在 DataGridView 控件中显示的滚动条的类型")]
        [DefaultValue(ScrollBars.Both)]
        public new ScrollBars ScrollBars
        {
            get { return base.ScrollBars; }
            set { base.ScrollBars = value; }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示如何选择 DataGridView 的单元格。
        /// </summary>
        [Description("获取或设置一个值，该值指示如何选择 DataGridView 的单元格")]
        [DefaultValue(DataGridViewSelectionMode.FullRowSelect)]
        public new DataGridViewSelectionMode SelectionMode
        {
            get { return base.SelectionMode; }
            set { base.SelectionMode = value; }
        }

        /// <summary>
        /// 获取和设置网格线的颜色（自动设置）
        /// </summary>
        [Description("获取和设置网格线的颜色")]
        [Browsable(false), DefaultValue(typeof(Color), "149, 204, 223")]
        public new Color GridColor
        {
            get { return base.GridColor; }
            set { base.GridColor = value; }
        }

        /// <summary>
        /// 该值指示用户是否可以编辑。
        /// </summary>
        [Description("该值指示用户是否可以编辑")]
        [DefaultValue(true)]
        public new bool ReadOnly
        {
            get { return base.ReadOnly; }
            set { base.ReadOnly = value; }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示在对应用程序启用了可视样式的情况下，行标题和列标题是否使用用户当前主题的可视样式。
        /// 如果对标题启用了可视样式，为 true；否则为 false。默认值为 true。
        /// </summary>
        [DefaultValue(false)]
        [IODescriptionAttribute("DataGridView_EnableHeadersVisualStylesDescr")]
        public new bool EnableHeadersVisualStyles
        {
            get { return base.EnableHeadersVisualStyles; }
            set { base.EnableHeadersVisualStyles = value; }
        }

        #endregion

        #region 内部方法-加载数据
        /// <summary>
        /// 刷新数据
        /// </summary>
        private void RefreshData()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(RefreshData));
                return;
            }
            DataSource = source;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        internal virtual void UpdateData(object value)
        {
            if (value == null)
            {
                base.DataSource = null;
                return;
            }
            if (value is IList list)
            {
                this.type = list.GenericType();
                var dt = list.ToDataTable();
                dt.PrimaryKey = new DataColumn[] { dt.Columns[this.type.TableKeys()] };
                base.DataSource = dt;
            }
            else if (value is DataTable dt)
            {
                base.DataSource = dt;
            }
            else
            {
                this.type = value.GetType();
                var temp = this.type.GenericList();
                temp.Add(value);
                base.DataSource = temp;
            }
            UpdateColumns(this.type);
            OnRefreshChanged(this.type);
        }
        /// <summary>
        /// 更新列名称
        /// </summary>
        internal void UpdateColumns(Type type)
        {
            if (type == null || type == typeof(string) || type.IsValueType) return;
            this.type = type;

            var properties = type.PropertiesCache();
            for (var i = 0; i < Columns.Count; i++)
            {
                var column = Columns[i];
                var property = properties.Property(column.Name);
                if (property == null) continue;
                if (property.ICheckBox())
                {
                    column = new TDataGridViewCheckBoxColumn
                    {
                        Name = Columns[i].Name,
                        DataPropertyName = Columns[i].DataPropertyName,
                        DisplayIndex = Columns[i].DisplayIndex
                    };
                    Columns.RemoveAt(i);
                    Columns.Insert(i, column);
                }
                else if (property.IButton(out IButtonAttribute button))
                {
                    column = new TDataGridViewButtonColumn(button)
                    {
                        Name = Columns[i].Name,
                        DataPropertyName = Columns[i].DataPropertyName,
                        DisplayIndex = Columns[i].DisplayIndex
                    };
                    Columns.RemoveAt(i);
                    Columns.Insert(i, column);
                }
                column.Visible = property.IShow();
                column.HeaderText = property.Text();
            }
        }
        /// <summary>
        /// 设置排序模式并引发事件
        /// </summary>
        internal void OnRefreshChanged(Type type)
        {
            if (type != null && type != typeof(string) && !type.IsValueType)
            {
                for (var i = 0; i < Columns.Count; i++)
                {
                    Columns[i].SortMode = DataGridViewColumnSortMode.Programmatic;
                    if (type.Property(Columns[i].Name).ICheckBox() || type.Property(Columns[i].Name).IButton(out _))
                        Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
            OnRefreshChanged();
        }
        /// <summary>
        /// 引发数据更新后事件
        /// </summary>
        internal void OnRefreshChanged()
        {
            RefreshChanged?.Invoke(this);
        }
        /// <summary>
        /// 添加行
        /// </summary>
        internal void AddRow(object info)
        {
            var dt = this.DataSource as DataTable;
            var dr = dt.NewRow();
            dr.ItemArray = info.ToDataRow().ItemArray;
            dt.Rows.Add(dr);
        }
        /// <summary>
        /// 更新行
        /// </summary>
        public void UpdateRow(object info)
        {
            var id = info.GetValue(IdColumn());
            UpdateRow(info, id);
        }
        /// <summary>
        /// 更新行
        /// </summary>
        public void UpdateRow(object info, object id)
        {
            var dt = this.DataSource as DataTable;
            var dr = dt.Rows.Find(id);
            if (dr != null) dr.ItemArray = info.ToDataRow().ItemArray;
        }
        /// <summary>
        /// 删除行
        /// </summary>
        internal void DeleteRow(object id)
        {
            var dt = this.DataSource as DataTable;
            var dr = dt.Rows.Find(id);
            if (dr != null) dt.Rows.Remove(dr);
        }

        #endregion

        #region 公共方法
        /// <summary>
        /// 获取指定名称列
        /// </summary>
        public DataGridViewColumn GetColumn(string name)
        {
            for (int i = 0; i < this.Columns.Count; i++)
            {
                if (this.Columns[i].Name == name)
                {
                    return this.Columns[i];
                }
            }
            return new DataGridViewColumn();
        }
        /// <summary>
        /// 获取主键列
        /// </summary>
        public string IdColumn()
        {
            if (this.Columns.Contains(nameof(IId.Id))) return nameof(IId.Id);
            if (this.type != null)
            {
                var properties = this.type.PropertiesCache();
                for (int i = 0; i < Columns.Count; i++)
                {
                    var property = properties.Property(Columns[i].Name);
                    if (property == null) continue;
                    if (property.Name == nameof(IId.Id))
                    {
                        return Columns[i].Name;
                    }
                }
            }
            if (this.type != null)
            {
                var properties = this.type.PropertiesCache();
                for (int i = 0; i < Columns.Count; i++)
                {
                    var property = properties.Property(Columns[i].Name);
                    if (property == null) continue;
                    if (property.IShow()) return Columns[i].Name;
                }
            }
            throw new WarningException("主找到主键。");
        }
        /// <summary>
        /// 刷新数据并自动选中焦点
        /// </summary>
        /// <param name="iOffset">保存滚动条位置</param>
        /// <param name="iRefresh">刷新数据</param>
        public void AutoCell(bool iOffset = false, bool iRefresh = true)
        {
            int index = 0;
            if (this.CurrentCell != null)
                index = this.CurrentCell.RowIndex;
            var offset = this.FirstDisplayedScrollingRowIndex;
            if (iRefresh) this.RefreshData();
            AutoCell(index);
            if (iOffset) SetOffsetRowIndex(offset);
        }
        /// <summary>
        /// 自动选中焦点
        /// </summary>
        public void AutoCell(int index)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<int>(AutoCell), index);
                return;
            }
            if (index < 0) index = 0;
            if (this.Rows.Count == 0) return;
            if (index > this.RowCount - 1)
            {
                index = this.RowCount - 1;
            }
            var id = this.Rows[index].Cells[IdColumn()].Value.ToInt();
            if (id == -1) index--;
            if (index < 0) index = 0;
            for (int i = 0; i < this.Columns.Count; i++)
            {
                if (this.Columns[i].Visible)
                {
                    this.CurrentCell = this[i, index];
                    this.Rows[index].Selected = true;
                    break;
                }
            }
        }
        /// <summary>
        /// 设置显示行
        /// </summary>
        internal void SetOffsetRowIndex(int offset)
        {
            if (offset > this.RowCount - 1) offset = this.RowCount - 1;
            if (offset != -1) this.FirstDisplayedScrollingRowIndex = offset;
        }
        private void TDataGridView_DoubleClick(object sender, EventArgs e)
        {
            if (e is MouseEventArgs me)
            {
                var hit = this.HitTest(me.X, me.Y);
                if (hit.RowIndex > -1)
                {
                    if (this.type != null)
                    {
                        var property = type.Property(Columns[hit.ColumnIndex].Name);
                        if (property?.IButton(out _) == true) return;
                    }
                    RowDoubleClick?.Invoke(this, hit.RowIndex);
                }
            }
        }

        #endregion

        #region 重绘
        /// <summary>
        /// 在单元格需要绘制时发生
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            base.OnCellPainting(e);
            GridColor = RowTemplate.DefaultCellStyle.SelectionBackColor.AddLight(-15);
            if (e.RowIndex == -1)
            {
                HeaderMerge(e);
            }
            else
            {
                DrawCell(e);
            }
        }

        #region 绘制行号
        /// <summary>
        /// 绘制行号
        /// </summary>
        private void TDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (!this.RowHeadersVisible) return;
            for (int i = 0; i < this.Columns.Count; i++)
            {
                if (this.Columns[i].Visible)
                {
                    if (e.ColumnIndex == i)
                        Rows[e.RowIndex].HeaderCell.Value = (e.RowIndex + 1).ToString();
                    break;
                }
            }
        }

        #endregion

        #region 鼠标移过的行颜色
        private void ComBoxGridView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            CellMouseColor(e, RowTemplate.DefaultCellStyle.BackColor, RowTemplate.DefaultCellStyle.SelectionBackColor);
        }
        private void ComBoxGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            var blackColor = RowTemplate.DefaultCellStyle.SelectionBackColor.AddLight(27);
            var selectionBackColor = RowTemplate.DefaultCellStyle.SelectionBackColor.AddLight(-10);
            CellMouseColor(e, blackColor, selectionBackColor);
        }
        private void CellMouseColor(DataGridViewCellEventArgs e, Color blackColor, Color selectionBackColor)
        {
            if (e.RowIndex < 0)
            {
                if (e.ColumnIndex != -1)
                {
                    Columns[e.ColumnIndex].HeaderCell.Style.BackColor = blackColor;
                }
                return;
            }
            Rows[e.RowIndex].DefaultCellStyle.BackColor = blackColor;
            if (Rows[e.RowIndex].Selected)
            {
                Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = selectionBackColor;
            }
            else if (e.ColumnIndex > -1 && Rows[e.RowIndex].Cells[e.ColumnIndex].Selected)
            {
                Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionBackColor = selectionBackColor;
            }
            else
            {
                for (int i = 0; i < this.Columns.Count; i++)
                {
                    if (Rows[e.RowIndex].Cells[i].Selected)
                    {
                        Rows[e.RowIndex].Cells[i].Style.SelectionBackColor = selectionBackColor;
                    }
                }
            }
        }
        private void TDataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 && e.ColumnIndex != -1)
            {
                var selectionBackColor = RowTemplate.DefaultCellStyle.SelectionBackColor.AddLight(-10);
                Columns[e.ColumnIndex].HeaderCell.Style.BackColor = selectionBackColor;
            }
        }
        private void TDataGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 && e.ColumnIndex != -1)
            {
                var blackColor = RowTemplate.DefaultCellStyle.SelectionBackColor.AddLight(27);
                Columns[e.ColumnIndex].HeaderCell.Style.BackColor = blackColor;
            }
        }

        #endregion

        #region 合并行
        /// <summary>
        /// 合并行
        /// </summary>
        public void AddSpanColumns(Type type, params string[] param)
        {
            if (param == null) return;
            SpanColumns.Clear();
            var properties = type.PropertiesCache();
            for (int i = 0; i < properties.Count; i++)
            {
                if (param.Contains(properties[i].Name)) SpanColumns.Add(i);
            }
        }
        /// <summary>
        /// 合并单元格
        /// </summary>
        private void DrawCell(DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1) return;
            if (SpanColumns.Contains(e.ColumnIndex) && Columns[e.ColumnIndex] is DataGridViewTextBoxColumn cell && cell.Visible && cell.ReadOnly)
            {
                Brush gridBrush = new SolidBrush(GridColor);
                var backBrush = new SolidBrush(e.CellStyle.BackColor);
                var fontBrush = new SolidBrush(e.CellStyle.ForeColor);
                int cellwidth;
                //上面相同的行数
                var UpRows = 0;
                //下面相同的行数
                var DownRows = 0;
                //总行数
                var count = 0;

                var curValue = e.Value == null ? "" : e.Value.ToString().Trim();
                if (string.IsNullOrEmpty(curValue)) return;
                cellwidth = e.CellBounds.Width;
                var gridLinePen = new Pen(gridBrush);
                var select = false;
                {
                    #region 获取下面的行数

                    for (var i = e.RowIndex; i < Rows.Count; i++)
                    {
                        if (Rows[i].Cells[e.ColumnIndex].Value == null) break;
                        if (i > e.RowIndex && SpanEvent != null && SpanEvent(i, Columns[e.ColumnIndex].DataPropertyName)) break;
                        if (Rows[i].Cells[e.ColumnIndex].Value.ToString().Equals(curValue))
                        {
                            if (Rows[i].Cells[e.ColumnIndex].Selected) select = true;
                            DownRows++;
                            cellwidth = cellwidth < Rows[i].Cells[e.ColumnIndex].Size.Width
                                ? cellwidth : Rows[i].Cells[e.ColumnIndex].Size.Width;
                        }
                        else break;
                    }

                    #endregion

                    #region 获取上面的行数

                    for (var i = e.RowIndex - 1; i >= 0; i--)
                    {
                        if (Rows[i].Cells[e.ColumnIndex].Value == null) break;
                        if (SpanEvent != null && SpanEvent(i + 1, Columns[e.ColumnIndex].DataPropertyName)) break;
                        if (Rows[i].Cells[e.ColumnIndex].Value.ToString().Equals(curValue))
                        {
                            if (Rows[i].Cells[e.ColumnIndex].Selected) select = true;
                            UpRows++;
                            cellwidth = cellwidth < Rows[i].Cells[e.ColumnIndex].Size.Width
                                ? cellwidth : Rows[i].Cells[e.ColumnIndex].Size.Width;
                        }
                        else break;
                    }

                    #endregion

                    count = DownRows + UpRows;
                    if (count < 2) return;
                }
                if (select)
                {
                    #region 选中下面的行

                    for (var i = e.RowIndex; i < Rows.Count; i++)
                    {
                        if (Rows[i].Cells[e.ColumnIndex].Value == null) break;
                        if (i > e.RowIndex && SpanEvent != null && SpanEvent(i, Columns[e.ColumnIndex].DataPropertyName)) break;
                        if (Rows[i].Cells[e.ColumnIndex].Value.ToString().Equals(curValue))
                        {
                            Rows[i].Cells[e.ColumnIndex].Selected = true;
                        }
                        else break;
                    }

                    #endregion

                    #region 选中上面的行

                    for (var i = e.RowIndex - 1; i >= 0; i--)
                    {
                        if (Rows[i].Cells[e.ColumnIndex].Value == null) break;
                        if (SpanEvent != null && SpanEvent(i + 1, Columns[e.ColumnIndex].DataPropertyName)) break;
                        if (Rows[i].Cells[e.ColumnIndex].Value.ToString().Equals(curValue))
                        {
                            Rows[i].Cells[e.ColumnIndex].Selected = true;
                        }
                        else break;
                    }

                    #endregion
                }
                if (Rows[e.RowIndex].Cells[e.ColumnIndex].Selected)
                {
                    backBrush.Color = e.CellStyle.SelectionBackColor;
                    fontBrush.Color = e.CellStyle.SelectionForeColor;
                }
                //以背景色填充
                e.Graphics.FillRectangle(backBrush, e.CellBounds);
                //画字符串
                PaintingFont(e, UpRows + 1, count);
                if (DownRows == 1)
                {
                    e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                }
                // 画右边线
                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);

                e.Handled = true;
            }
        }
        /// <summary>
        /// 画字符串
        /// </summary>
        private void PaintingFont(DataGridViewCellPaintingEventArgs e, int UpRows, int count)
        {
            var value = e.Value?.ToString();
            var cellheight = e.CellBounds.Height;
            var y = e.CellBounds.Y - cellheight * (UpRows - 1);
            var height = cellheight * count;
            if (y < this.ColumnHeadersHeight)
            {
                height += (y - this.ColumnHeadersHeight) * 2;
                y = this.ColumnHeadersHeight;
            }
            var rect = new Rectangle(e.CellBounds.X, y, e.CellBounds.Width, height);
            //TextRenderer.DrawText(e.Graphics, value, e.CellStyle.Font, rect, e.CellStyle.ForeColor, format);
            //允许分行显示
            using (var brush = new SolidBrush(e.CellStyle.ForeColor))
            {
                e.Graphics.DrawString(value, e.CellStyle.Font, brush, rect, DrawHelper.VerticalCenter);
            }
        }

        #endregion

        #region 二维表头
        private void HeaderMerge(DataGridViewCellPaintingEventArgs e)
        {
            if (this.type == typeof(FindInfo)) return;
            if (SpanRows.ContainsKey(e.ColumnIndex)) //被合并的列
            {
                //画边框
                var g = e.Graphics;
                e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);

                int left = e.CellBounds.Left,
                    top = e.CellBounds.Top + 2,
                    right = e.CellBounds.Right,
                    bottom = e.CellBounds.Bottom;
                //自动左右列
                for (int i = SpanRows[e.ColumnIndex].Left; i <= SpanRows[e.ColumnIndex].Right; i++)
                {
                    if (Columns.Count > i && Columns[i].Visible)
                    {
                        if (i == e.ColumnIndex) left += 2;
                        break;
                    }
                }
                for (int i = SpanRows[e.ColumnIndex].Right; i >= SpanRows[e.ColumnIndex].Left; i--)
                {
                    if (Columns.Count > i && Columns[i].Visible)
                    {
                        if (i == e.ColumnIndex) right -= 2;
                        break;
                    }
                }

                using (var fontBrush = new SolidBrush(e.CellStyle.ForeColor))
                {
                    if (e.State == DataGridViewElementStates.Selected)
                    {
                        fontBrush.Color = e.CellStyle.SelectionForeColor;
                    }
                    var height = bottom - top;
                    if (SpanRows[e.ColumnIndex].IAll) height /= 2;
                    //画上半部分底色
                    using (var backBrush = new SolidBrush(ColumnHeadersDefaultCellStyle.BackColor))
                    {
                        g.FillRectangle(backBrush, left - 2, top - 1, right - left + 3, height);
                    }
                    if (SpanRows[e.ColumnIndex].IAll)
                    { //画中线
                        using (var pen = new Pen(GridColor))
                        {
                            g.DrawLine(pen, left - 2, (top + bottom) / 2 - 1, right, (top + bottom) / 2 - 1);
                        }
                        //写小标题
                        g.DrawString(string.Format("{0}", e.Value), e.CellStyle.Font, fontBrush,
                            new Rectangle(left, (top + bottom) / 2, right - left, height), DrawHelper.StringFormat(SpanRows[e.ColumnIndex].Alignment));
                    }

                    //计算表头位置并绘制
                    left = 0;
                    //实际宽度
                    var width = 0;
                    //显示宽度
                    var showWidth = 0;
                    for (int i = SpanRows[e.ColumnIndex].Left; i <= SpanRows[e.ColumnIndex].Right; i++)
                    {
                        if (Columns.Count > i && Columns[i].Visible)
                        {
                            var rectangle = GetColumnDisplayRectangle(i, false);
                            if (left == 0) left = rectangle.Left - 2;
                            if (showWidth <= 0) left += rectangle.Width - Columns[i].Width;
                            showWidth += rectangle.Width;
                            width += Columns[i].Width;
                        }
                    }
                    g.DrawString(SpanRows[e.ColumnIndex].Text, e.CellStyle.Font, fontBrush,
                        new Rectangle(left + 2, top, width, height), DrawHelper.StringFormat(SpanRows[e.ColumnIndex].Alignment));

                    e.Handled = true;
                }
            }
        }
        /// <summary>
        /// 水平滚动时刷新二维表头
        /// </summary>
        protected override void OnScroll(ScrollEventArgs e)
        {
            base.OnScroll(e);
            if (SpanRows.Count > 0 && e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                this.Invalidate(new Rectangle(this.Location, new Size(this.Width, this.ColumnHeadersHeight)));
            }
        }
        /// <summary>
        /// 取消合并列
        /// </summary>
        public void ClearSpanHeader()
        {
            SpanRows.Clear();
        }
        /// <summary>
        /// 合并列
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="colIndexName">索引列的名称</param>
        /// <param name="colCount">需要合并的列数</param>
        /// <param name="text">合并列后的文本</param>
        /// <param name="iAll">显示子列标记</param>
        /// <param name="alignment">水平对齐方式</param>
        public void AddSpanHeader(Type type, string colIndexName, int colCount, string text, bool iAll = true, StringAlignment alignment = StringAlignment.Center)
        {
            var colIndex = 0;
            var properties = type.PropertiesCache();
            for (int i = 0; i < properties.Count; i++)
            {
                if (properties[i].Name.Equals(colIndexName, StringComparison.OrdinalIgnoreCase))
                {
                    colIndex = i;
                    break;
                }
            }
            //将这些列加入列表
            var Right = colIndex + colCount - 1; //同一大标题下的最后一列的索引
            SpanRows[colIndex] = new SpanInfo(text, colIndex, Right, iAll, alignment); //添加标题下的最左列
            SpanRows[Right] = new SpanInfo(text, colIndex, Right, iAll, alignment); //添加该标题下的最右列
            for (var i = colIndex + 1; i < Right; i++) //中间的列
            {
                SpanRows[i] = new SpanInfo(text, colIndex, Right, iAll, alignment);
            }
            //添加单列标题
            if (colCount == 1) SpanRows[colIndex] = new SpanInfo(text, colIndex, Right, iAll, alignment);
        }

        #endregion

        #endregion

        #region 自定义列
        /// <summary>
        /// checkbox的单元格改变事件
        /// </summary>
        internal void OnCheckBoxCellCheckedChange(int rowIndex, int columnIndex, bool value)
        {
            bool existsChecked = false, existsNoChecked = false;
            TDataGridViewCheckBoxCell cellEx;
            foreach (DataGridViewRow row in this.Rows)
            {
                cellEx = row.Cells[columnIndex] as TDataGridViewCheckBoxCell;
                if (cellEx == null) return;
                existsChecked |= cellEx.Checked;
                existsNoChecked |= !cellEx.Checked;
            }

            if (this.Columns[columnIndex].HeaderCell as TDataGridViewCheckBoxColumnHeaderCell == null) return;

            CheckState oldState = (this.Columns[columnIndex].HeaderCell as TDataGridViewCheckBoxColumnHeaderCell).CheckedAllState;

            if (existsChecked)
                (this.Columns[columnIndex].HeaderCell as TDataGridViewCheckBoxColumnHeaderCell).CheckedAllState = existsNoChecked ? CheckState.Indeterminate : CheckState.Checked;
            else
                (this.Columns[columnIndex].HeaderCell as TDataGridViewCheckBoxColumnHeaderCell).CheckedAllState = CheckState.Unchecked;

            if (oldState != (this.Columns[columnIndex].HeaderCell as TDataGridViewCheckBoxColumnHeaderCell).CheckedAllState)
                this.InvalidateColumn(columnIndex);
            CheckedChanged?.Invoke(rowIndex, columnIndex, value);
        }
        /// <summary>
        /// 全选中/取消全选中
        /// </summary>
        internal void OnCheckAllCheckedChange(int columnIndex, bool isCheckedAll)
        {
            TDataGridViewCheckBoxCell cellEx;
            foreach (DataGridViewRow row in this.Rows)
            {
                cellEx = row.Cells[columnIndex] as TDataGridViewCheckBoxCell;
                if (cellEx == null) continue;
                cellEx.Checked = isCheckedAll;
            }
            CheckedChanged?.Invoke(-1, columnIndex, isCheckedAll);
        }
        /// <summary>
        /// TDataGridViewButtonColumn中的按钮点击事件
        /// </summary>
        internal void OnButtonClicked(int rowIndex, int columnIndex, object value)
        {
            ButtonClicked?.Invoke(rowIndex, columnIndex, value);
        }

        #endregion
    }
}