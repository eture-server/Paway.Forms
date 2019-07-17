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
        /// <summary>
        /// 绘制列数据在源中的序号
        /// </summary>
        private int _iCheckBoxIndex = -1;

        /// <summary>
        /// 要绘制的CheckBox
        /// </summary>
        private CheckBox _headerCheckBox;

        /// <summary>
        /// 文本图片列-文本列索引
        /// </summary>
        private int _tColumnIndex = -1;

        private string _iCheckBoxName;
        /// <summary>
        /// 原数据源
        /// </summary>
        private object source;

        private readonly Timer timer = new Timer();
        private PictureBox pictureBox1;
        /// <summary>
        /// 动态图片行
        /// </summary>
        private int pIndex = -1;

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
            /// 位置
            /// </summary>
            public readonly StringAlignment Position;

            /// <summary>
            /// 对应左行
            /// </summary>
            public readonly int Left;

            /// <summary>
            /// 对应右行
            /// </summary>
            public readonly int Right;

            public SpanInfo(string Text, StringAlignment Position, int Left, int Right)
            {
                this.Text = Text;
                this.Position = Position;
                this.Left = Left;
                this.Right = Right;
            }
        }

        /// <summary>
        /// 合并单元格的列
        /// </summary>
        private readonly List<int> SpanColumns = new List<int>();

        #endregion

        #region 属性
        /// <summary>
        /// 文本图片列-文本列
        /// </summary>
        [Browsable(true), Description("文本图片列-文本列")]
        [DefaultValue(null)]
        public string TColumnText { get; set; }

        /// <summary>
        /// 文本图片列-图片列
        /// </summary>
        [Browsable(true), Description("文本图片列-图片列")]
        [DefaultValue(null)]
        public string TColumnImage { get; set; }

        /// <summary>
        /// 是否绘制多行文本
        /// </summary>
        [Browsable(true), Description("是否绘制多行文本")]
        [DefaultValue(false)]
        public bool IMultiText { get; set; }

        /// <summary>
        /// CheckBox绘制列列Name
        /// </summary>
        [Browsable(true), Description("绘制列列Name")]
        [DefaultValue(null)]
        public string ICheckBoxName
        {
            get { return _iCheckBoxName; }
            set
            {
                _iCheckBoxName = value;
                if (_headerCheckBox == null)
                {
                    _headerCheckBox = new CheckBox()
                    {
                        Size = new Size(15, 15)
                    };
                    Controls.Add(_headerCheckBox);

                    _headerCheckBox.KeyUp += HeaderCheckBox_KeyUp;
                    _headerCheckBox.MouseClick += HeaderCheckBox_MouseClick;
                    CurrentCellDirtyStateChanged += ComBoxGridView_CurrentCellDirtyStateChanged;
                }
                _headerCheckBox.Visible = !_iCheckBoxName.IsNullOrEmpty();
                Invalidate();
            }
        }

        /// <summary>
        /// 动态文本图片列显示的图片
        /// </summary>
        [Browsable(true), Description("动态文本图片列显示的图片")]
        [DefaultValue(null)]
        public Image TProgressImage
        {
            get { return pictureBox1.Image; }
            set { pictureBox1.Image = value; }
        }

        /// <summary>
        /// 动态文本图片列显示行
        /// </summary>
        [Browsable(true), Description("动态文本图片列显示行")]
        [DefaultValue(-1)]
        public int TProgressIndex
        {
            get { return pIndex; }
            set
            {
                pIndex = value;
                timer.Enabled = value != -1 && !string.IsNullOrEmpty(TColumnImage) && !string.IsNullOrEmpty(TColumnText);
            }
        }

        #endregion

        #region 事件
        /// <summary>
        /// 数据刷新后触发
        /// </summary>
        public event Action RefreshChanged;
        /// <summary>
        /// CheckBox选中事件
        /// </summary>
        public event Action<bool> CheckedChanged;
        /// <summary>
        /// 合并单元格取消事件
        /// </summary>
        public event Func<int, string, bool> SpanEvent;

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
            BackgroundColor = Color.White;
            BorderStyle = BorderStyle.None;
            InitializeComponent();
            timer.Interval = 30;
            timer.Tick += Timer_Tick;

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

            pictureBox1.BackColor = Color.Transparent;
        }
        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            ((ISupportInitialize)pictureBox1).BeginInit();
            ((ISupportInitialize)this).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(0, 1);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1, 1);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // TDataGridView
            // 
            Controls.Add(pictureBox1);
            ((ISupportInitialize)pictureBox1).EndInit();
            ((ISupportInitialize)this).EndInit();
            ResumeLayout(false);
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
            get { return base.DataSource; }
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

        #region 内部方法
        /// <summary>
        /// 更新数据
        /// </summary>
        protected virtual void UpdateData(object value)
        {
            if (value == null)
            {
                base.DataSource = null;
                ClearBox();
                return;
            }
            Type type = null;
            if (value is IList list)
            {
                type = list.GenericType();
                base.DataSource = list.ToDataTable();
                if (list.Count == 0) ClearBox();
            }
            else if (value is DataTable dt)
            {
                base.DataSource = dt;
                if (dt.Rows.Count == 0) ClearBox();
            }
            else
            {
                type = value.GetType();
                var temp = type.GenericList();
                temp.Add(value);
                base.DataSource = temp;
                ClearBox();
            }
            UpdateColumns(type);
            OnRefreshChanged(type);
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
                }
            }
            OnRefreshChanged();
        }
        /// <summary>
        /// 引发数据更新后事件
        /// </summary>
        internal void OnRefreshChanged()
        {
            RefreshChanged?.Invoke();
        }

        #endregion

        #region 重绘
        #region 重载单元格绘制
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

        /// <summary>
        /// 在单元格需要绘制时发生
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            base.OnCellPainting(e);
            GridColor = BitmapHelper.RGBAddLight(RowTemplate.DefaultCellStyle.SelectionBackColor, -15);
            if (e.RowIndex == -1)
            {
                if (!_iCheckBoxName.IsNullOrEmpty()) DrawCombox(e);
                HeaderMerge(e);
            }
            else
            {
                DrawCell(e);
                if (IMultiText) DrawMultiText(e);
                if (_tColumnIndex != -1 && !string.IsNullOrEmpty(TColumnImage)) DrawImageText(e);
            }
        }

        #endregion

        #region 绘制多行文本
        /// <summary>
        /// 绘制多行文本
        /// </summary>
        /// <param name="e"></param>
        private void DrawMultiText(DataGridViewCellPaintingEventArgs e)
        {
            if (e.Value == null || e.Value.ToString().Trim() == string.Empty || !e.Value.ToString().Contains("&&"))
                return;

            if (Columns[e.ColumnIndex] is DataGridViewTextBoxColumn cell && cell.Visible && cell.ReadOnly)
            {
                var graphics = e.Graphics;
                var colorBack = e.CellStyle.BackColor;
                if (Rows[e.RowIndex].Selected)
                {
                    colorBack = e.CellStyle.SelectionBackColor;
                }
                using (var solidBrush = new SolidBrush(colorBack))
                {
                    DrawBounds(e.Graphics, solidBrush, e.CellBounds, e.RowIndex);
                }

                var index = e.Value.ToString().IndexOf("&&");
                var strFirst = e.Value.ToString().Substring(0, index);
                var strSecond = e.Value.ToString().Substring(index + 1);

                var fontFore = e.CellStyle.Font;
                var intX = e.CellBounds.Left + e.CellStyle.Padding.Left;
                var intY = e.CellBounds.Top + e.CellStyle.Padding.Top;
                var intWidth = e.CellBounds.Width - (e.CellStyle.Padding.Left + e.CellStyle.Padding.Right);
                var intHeight = e.CellBounds.Height - (e.CellStyle.Padding.Top + e.CellStyle.Padding.Bottom);
                intHeight /= 2;

                //the first line 
                TextFormatFlags format = DrawHelper.TextEnd;
                if (e.CellStyle.Alignment == DataGridViewContentAlignment.MiddleCenter)
                {
                    format = DrawHelper.TextCenter;
                }
                TextRenderer.DrawText(graphics, strFirst, fontFore, new Rectangle(intX, intY, intWidth, intHeight),
                    Color.Black, format);

                fontFore = e.CellStyle.Font;
                intY += intHeight;

                //the seconde line
                TextRenderer.DrawText(graphics, strSecond, fontFore, new Rectangle(intX, intY, intWidth, intHeight),
                    Color.SteelBlue, format);

                e.Handled = true;
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
            var blackColor = BitmapHelper.RGBAddLight(RowTemplate.DefaultCellStyle.SelectionBackColor, 27);
            var selectionBackColor = BitmapHelper.RGBAddLight(RowTemplate.DefaultCellStyle.SelectionBackColor, -10);
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
                var selectionBackColor = BitmapHelper.RGBAddLight(RowTemplate.DefaultCellStyle.SelectionBackColor, -10);
                Columns[e.ColumnIndex].HeaderCell.Style.BackColor = selectionBackColor;
            }
        }
        private void TDataGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 && e.ColumnIndex != -1)
            {
                var blackColor = BitmapHelper.RGBAddLight(RowTemplate.DefaultCellStyle.SelectionBackColor, 27);
                Columns[e.ColumnIndex].HeaderCell.Style.BackColor = blackColor;
            }
        }

        #endregion

        #region 绘制Combox
        /// <summary>
        /// 清空选择
        /// </summary>
        private void ClearBox()
        {
            if (_headerCheckBox != null) _headerCheckBox.Checked = false;
        }
        private void HeaderCheckBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
                HeaderCheckBoxClick((CheckBox)sender);
        }

        private void HeaderCheckBox_MouseClick(object sender, MouseEventArgs e)
        {
            HeaderCheckBoxClick((CheckBox)sender);
        }
        private void HeaderCheckBoxClick(CheckBox HCheckBox)
        {
            if (_iCheckBoxName.IsNullOrEmpty()) return;

            foreach (DataGridViewRow Row in Rows)
            {
                ((DataGridViewCheckBoxCell)Row.Cells[_iCheckBoxName]).Value = HCheckBox.Checked;
            }
            CheckedChanged?.Invoke(HCheckBox.Checked);
            RefreshEdit();
        }

        private void ComBoxGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (_iCheckBoxName.IsNullOrEmpty()) return;
            if (CurrentCell is DataGridViewCheckBoxCell)
                CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void DrawCombox(DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex != -1) return;
            if (_iCheckBoxName.IsNullOrEmpty()) return;
            _headerCheckBox.Visible = _iCheckBoxIndex != -1;
            if (e.ColumnIndex == _iCheckBoxIndex)
            {
                var oRectangle = GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                var oPoint = new Point()
                {
                    X = oRectangle.Location.X + (oRectangle.Width - _headerCheckBox.Width) / 2 + 1,
                    Y = oRectangle.Location.Y + (oRectangle.Height - _headerCheckBox.Height) / 2 + 1
                };
                _headerCheckBox.Location = oPoint;
            }
        }

        #endregion

        #region 合并行
        /// <summary>
        /// 合并行
        /// </summary>
        /// <param name="param"></param>
        public void AddSpanColumns(params int[] param)
        {
            SpanColumns.AddRange(param);
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
            TextFormatFlags format = DrawHelper.TextEnd;
            if (e.CellStyle.Alignment == DataGridViewContentAlignment.MiddleCenter)
            {
                format = DrawHelper.TextCenter;
            }
            TextRenderer.DrawText(e.Graphics, value, e.CellStyle.Font,
                new Rectangle(e.CellBounds.X, e.CellBounds.Y - cellheight * (UpRows - 1), e.CellBounds.Width, cellheight * count),
                e.CellStyle.ForeColor, format);
        }

        #endregion

        #region 绘制文本图片列
        /// <summary>
        /// 绘制文本图片列
        /// </summary>
        private void DrawImageText(DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == _tColumnIndex)
            {
                var foreColor = e.CellStyle.ForeColor;
                var backColor = e.CellStyle.BackColor;
                if (Rows[e.RowIndex].Selected)
                {
                    foreColor = e.CellStyle.SelectionForeColor;
                    backColor = e.CellStyle.SelectionBackColor;
                }
                Brush foreBrush = new SolidBrush(foreColor);
                using (Brush backBrush = new SolidBrush(backColor))
                {
                    DrawBounds(e.Graphics, backBrush, e.CellBounds, e.RowIndex);
                }

                //画图标
                var bitmap = Rows[e.RowIndex].Cells[TColumnImage].Value as Bitmap;
                if (bitmap != null)
                {
                    var newRect = new Rectangle(e.CellBounds.X + 5,
                        e.CellBounds.Y + (e.CellBounds.Height - bitmap.Height) / 2, bitmap.Width, bitmap.Height);
                    e.Graphics.DrawImage(bitmap, newRect);
                }

                //画字符串
                e.Graphics.DrawString(e.Value?.ToString(), e.CellStyle.Font, foreBrush,
                new Rectangle(e.CellBounds.Left + (bitmap == null ? 0 : bitmap.Width) + 10, e.CellBounds.Top,
                    e.CellBounds.Width, e.CellBounds.Height), DrawHelper.StringVertical);
                foreBrush.Dispose();

                e.Handled = true;
            }
        }

        /// <summary>
        /// 边框
        /// </summary>
        private void DrawBounds(Graphics g, Brush brush, Rectangle rect, int index)
        {
            // Erase the cell.
            g.FillRectangle(brush, rect);
            //首行线
            using (var pen = new Pen(GridColor))
            {
                if (index == 0 && !ColumnHeadersVisible)
                {
                    g.DrawLine(pen, new Point(rect.X, rect.Top), new Point(rect.Right, rect.Top));
                }
                //划线
                var p1 = new Point(rect.Right - 1, rect.Top);
                var p2 = new Point(rect.Right - 1, rect.Bottom - 1);
                var p3 = new Point(rect.Left, rect.Bottom - 1);
                Point[] ps = { p1, p2, p3 };
                g.DrawLines(pen, ps);
            }
        }

        #endregion

        #region 动态文本图片列
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (pIndex == -1 || pIndex >= Rows.Count) return;
            var image = Rows[pIndex].Cells[TColumnImage];
            image.Value = pictureBox1.Image;
            var cell = Rows[pIndex].Cells[TColumnText];
            InvalidateCell(cell);
        }

        #endregion

        #region 二维表头
        private void HeaderMerge(DataGridViewCellPaintingEventArgs e)
        {
            if (SpanRows.ContainsKey(e.ColumnIndex)) //被合并的列
            {
                //画边框
                var g = e.Graphics;
                e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);

                int left = e.CellBounds.Left,
                    top = e.CellBounds.Top + 2,
                    right = e.CellBounds.Right,
                    bottom = e.CellBounds.Bottom;
                switch (SpanRows[e.ColumnIndex].Position)
                {
                    case StringAlignment.Near:
                        left += 2;
                        break;
                    case StringAlignment.Far:
                        right -= 2;
                        break;
                }

                using (var fontBrush = new SolidBrush(e.CellStyle.ForeColor))
                {
                    if (e.State == DataGridViewElementStates.Selected)
                    {
                        fontBrush.Color = e.CellStyle.SelectionForeColor;
                    }
                    //画上半部分底色
                    using (var backBrush = new SolidBrush(ColumnHeadersDefaultCellStyle.BackColor))
                    {
                        g.FillRectangle(backBrush, left - 2, top - 1, right - left + 3, (bottom - top) / 2 + 1);
                    }
                    //画中线
                    using (var pen = new Pen(GridColor))
                    {
                        g.DrawLine(pen, left - 2, (top + bottom) / 2, right, (top + bottom) / 2);
                    }
                    //写小标题
                    g.DrawString(string.Format("{0}", e.Value), e.CellStyle.Font, fontBrush,
                        new Rectangle(left, (top + bottom) / 2, right - left, (bottom - top) / 2), DrawHelper.StringCenter);
                    left = GetColumnDisplayRectangle(SpanRows[e.ColumnIndex].Left, true).Left - 2;

                    if (left < 0) left = GetCellDisplayRectangle(-1, -1, true).Width;
                    right = GetColumnDisplayRectangle(SpanRows[e.ColumnIndex].Right, true).Right - 2;
                    if (right < 0) right = Width;

                    g.DrawString(SpanRows[e.ColumnIndex].Text, e.CellStyle.Font, fontBrush,
                        new Rectangle(left, top, right - left, (bottom - top) / 2), DrawHelper.StringCenter);
                    e.Handled = true;
                }
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
        /// <param name="ColIndex">列的索引</param>
        /// <param name="ColCount">需要合并的列数</param>
        /// <param name="Text">合并列后的文本</param>
        public void AddSpanHeader(int ColIndex, int ColCount, string Text)
        {
            if (ColCount < 2)
            {
                throw new ArgumentException("行宽应大于1。");
            }
            //将这些列加入列表
            var Right = ColIndex + ColCount - 1; //同一大标题下的最后一列的索引
            SpanRows[ColIndex] = new SpanInfo(Text, StringAlignment.Near, ColIndex, Right); //添加标题下的最左列
            SpanRows[Right] = new SpanInfo(Text, StringAlignment.Far, ColIndex, Right); //添加该标题下的最右列
            for (var i = ColIndex + 1; i < Right; i++) //中间的列
            {
                SpanRows[i] = new SpanInfo(Text, StringAlignment.Center, ColIndex, Right);
            }
        }

        #endregion
        #endregion

        #region 公开方法-加载数据
        /// <summary>
        /// 刷新数据
        /// </summary>
        public void RefreshData()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(RefreshData));
                return;
            }
            DataSource = source;
        }

        /// <summary>
        /// 更新列名称
        /// </summary>
        public void UpdateColumns(Type type)
        {
            if (type == null || type == typeof(string) || type.IsValueType) return;

            _iCheckBoxIndex = -1;
            for (var i = 0; i < Columns.Count; i++)
            {
                if (Columns[i].Name == _iCheckBoxName)
                {
                    _iCheckBoxIndex = i;
                }
                if (Columns[i].Name == TColumnText)
                {
                    _tColumnIndex = i;
                }
                var property = type.Property(Columns[i].Name);
                if (property == null) continue;
                Columns[i].Visible = property.IShow(out string text);
                Columns[i].HeaderText = text;
            }
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
        /// 自动选中焦点
        /// </summary>
        public void AutoCell()
        {
            int index = 0;
            if (this.CurrentCell != null)
                index = this.CurrentCell.RowIndex;
            this.RefreshData();
            AutoCell(index);
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
            for (int i = 0; i < this.Columns.Count; i++)
            {
                if (this.Columns[i].Visible)
                {
                    this.CurrentCell = this[i, index];
                    break;
                }
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
            }
            if (_headerCheckBox != null)
            {
                _headerCheckBox.Dispose();
                _headerCheckBox = null;
            }
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
            if (pictureBox1 != null)
            {
                pictureBox1.Image = null;
                pictureBox1.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}