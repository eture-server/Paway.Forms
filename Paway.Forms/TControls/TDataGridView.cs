using Paway.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    /// 表头全选功能的实现
    /// </summary>
    public class TDataGridView : DataGridView
    {
        #region 构造函数
        /// <summary>
        /// 构造
        /// </summary>
        public TDataGridView()
            : base()
        {
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();

            this.CellMouseEnter += ComBoxGridView_CellMouseEnter;
            this.CellMouseLeave += ComBoxGridView_CellMouseLeave;
            this.RowPostPaint += TDataGridView_RowPostPaint;
            this.BackgroundColor = Color.White;
            this.BorderStyle = BorderStyle.None;
            InitializeComponent();
            timer.Interval = 30;
            this.timer.Tick += timer_Tick;

            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            this.ColumnHeadersHeight = 30;
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.MultiSelect = false;
            this.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            this.RowHeadersVisible = false;
            this.RowHeadersWidth = 21;
            this.ScrollBars = ScrollBars.Vertical;
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.pictureBox1.BackColor = Color.Transparent;
        }

        #endregion

        #region 变量
        /// <summary>
        /// 绘制列数据在源中的序号
        /// </summary>
        private int _iCheckBoxIndex { get; set; }
        /// <summary>
        /// 要绘制的CheckBox
        /// </summary>
        private CheckBox _headerCheckBox = null;
        /// <summary>
        /// 文本图片列-文本列索引
        /// </summary>
        private int _tColumnIndex = -1;

        #endregion

        #region 属性
        /// <summary>
        /// 文本图片列-文本列
        /// </summary>
        [Browsable(true), Description("文本图片列-文本列"), DefaultValue(null)]
        public string TColumnText { get; set; }
        /// <summary>
        /// 文本图片列-图片列
        /// </summary>
        [Browsable(true), Description("文本图片列-图片列"), DefaultValue(null)]
        public string TColumnImage { get; set; }

        /// <summary>
        /// 是否绘制多行文本
        /// </summary>
        [Browsable(true), Description("是否绘制多行文本"), DefaultValue(false)]
        public bool IMultiText { get; set; }

        /// <summary>
        /// 是否合并绘制列
        /// </summary>
        private bool _iMerger = false;
        /// <summary>
        /// 是否合并绘制列
        /// </summary>
        [Browsable(true), Description("是否合并绘制列"), DefaultValue(false)]
        public bool IMerger
        {
            get { return _iMerger; }
            set
            {
                _iMerger = value;
                if (value) this.MultiSelect = true;
            }
        }

        /// <summary>
        /// 是否绘制鼠标移过时的颜色
        /// </summary>
        [Browsable(true), Description("是否绘制鼠标移过时的颜色"), DefaultValue(true)]
        public bool IMove { get; set; }
        /// <summary>
        /// 鼠标移过的行颜色
        /// </summary>
        [Browsable(true), Description("鼠标移过的行颜色"), DefaultValue(typeof(Color), "Azure")]
        public Color IMoveColor { get; set; }

        /// <summary>
        /// 是否绘制CheckBox
        /// </summary>
        private bool _iCheckBox = false;
        /// <summary>
        /// 是否绘制CheckBox
        /// </summary>
        [Browsable(true), Description("是否绘制CheckBox"), DefaultValue(false)]
        public bool ICheckBox
        {
            get { return _iCheckBox; }
            set
            {
                _iCheckBox = value;
                if (_headerCheckBox == null)
                {
                    _headerCheckBox = new CheckBox();
                    _headerCheckBox.Size = new Size(15, 15);
                    this.Controls.Add(_headerCheckBox);

                    _headerCheckBox.KeyUp += HeaderCheckBox_KeyUp;
                    _headerCheckBox.MouseClick += HeaderCheckBox_MouseClick;
                    this.CurrentCellDirtyStateChanged += ComBoxGridView_CurrentCellDirtyStateChanged;
                }
                this._headerCheckBox.Visible = _iCheckBox;
                this.Invalidate();
            }
        }
        /// <summary>
        /// 绘制列列Name
        /// </summary>
        [Browsable(true), Description("绘制列列Name"), DefaultValue(null)]
        public string ICheckBoxName { get; set; }

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
        [DefaultValue(typeof(BorderStyle), "None")]
        public new BorderStyle BorderStyle
        {
            get { return base.BorderStyle; }
            set { base.BorderStyle = value; }
        }
        /// <summary>
        /// 设置数据源时设置图片列
        /// </summary>
        [Description("设置数据源时设置图片列")]
        [AttributeProvider(typeof(IListSource))]
        [RefreshProperties(RefreshProperties.Repaint), DefaultValue(null)]
        public new object DataSource
        {
            get { return base.DataSource; }
            set
            {
                this.source = value;
                UpdateColumns(value);
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
        [DefaultValue(typeof(DataGridViewAutoSizeColumnsMode), "Fill")]
        public new DataGridViewAutoSizeColumnsMode AutoSizeColumnsMode
        {
            get { return base.AutoSizeColumnsMode; }
            set { base.AutoSizeColumnsMode = value; }
        }
        /// <summary>
        /// 获取 DataGridView 的单元格边框样式。
        /// </summary>
        [Description("获取 DataGridView 的单元格边框样式")]
        [DefaultValue(typeof(DataGridViewCellBorderStyle), "SingleHorizontal")]
        public new DataGridViewCellBorderStyle CellBorderStyle
        {
            get { return base.CellBorderStyle; }
            set { base.CellBorderStyle = value; }
        }
        /// <summary>
        /// 获取应用于列标题的边框样式。
        /// </summary>
        [Description("获取应用于列标题的边框样式")]
        [DefaultValue(typeof(DataGridViewHeaderBorderStyle), "None")]
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
        [DefaultValue(typeof(DataGridViewColumnHeadersHeightSizeMode), "DisableResizing")]
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
        [DefaultValue(typeof(DataGridViewHeaderBorderStyle), "Single")]
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
        [DefaultValue(typeof(ScrollBars), "Vertical")]
        public new ScrollBars ScrollBars
        {
            get { return base.ScrollBars; }
            set { base.ScrollBars = value; }
        }
        /// <summary>
        /// 获取或设置一个值，该值指示如何选择 DataGridView 的单元格。
        /// </summary>
        [Description("获取或设置一个值，该值指示如何选择 DataGridView 的单元格")]
        [DefaultValue(typeof(DataGridViewSelectionMode), "FullRowSelect")]
        public new DataGridViewSelectionMode SelectionMode
        {
            get { return base.SelectionMode; }
            set { base.SelectionMode = value; }
        }

        #endregion

        #region 加载数据
        /// <summary>
        /// 原数据源
        /// </summary>
        private object source;
        /// <summary>
        /// 刷新数据
        /// </summary>
        public void RefreshData()
        {
            this.DataSource = source;
        }
        /// <summary>
        /// 更新列名称
        /// </summary>
        private void UpdateColumns(object value)
        {
            if (value == null) return;
            Type type = null;
            if (value is IList)
            {
                IList list = value as IList;
                type = list.GetListType();
                base.DataSource = type.ToDataTable(value as IList);
            }
            else if (value is DataTable)
            {
                base.DataSource = value;
            }
            else
            {
                type = value.GetType();
                IList temp = type.CreateList();
                temp.Add(value);
                base.DataSource = temp;
            }
            UpdateColumns(type);
        }
        /// <summary>
        /// 更新列名称
        /// </summary>
        public void UpdateColumns(Type type)
        {
            if (type == null || type == typeof(String) || type.IsValueType) return;

            for (int i = 0; i < this.Columns.Count; i++)
            {
                if (this.Columns[i].Name == ICheckBoxName)
                {
                    _iCheckBoxIndex = i;
                }
                if (this.Columns[i].Name == TColumnText)
                {
                    _tColumnIndex = i;
                }
                PropertyInfo pro = type.GetProperty(this.Columns[i].Name);
                if (pro == null) continue;
                PropertyAttribute[] itemList = pro.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
                this.Columns[i].Visible = true;
                if (itemList != null && itemList.Length != 0)
                {
                    if (!itemList[0].Show)
                    {
                        this.Columns[i].Visible = false;
                        continue;
                    }
                    if (itemList[0].CnName != null)
                    {
                        this.Columns[i].HeaderText = itemList[0].CnName;
                    }
                }
            }
        }

        #endregion

        #region 事件
        /// <summary>
        /// 行号
        /// </summary>
        void TDataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (!this.RowHeadersVisible) return;
            using (SolidBrush brush = new SolidBrush(e.InheritedRowStyle.ForeColor))
            {
                string line = (e.RowIndex + 1).ToString();
                e.Graphics.DrawString(line, e.InheritedRowStyle.Font, brush,
                    new Rectangle(e.RowBounds.X, e.RowBounds.Y, this.RowHeadersWidth, e.RowBounds.Height),
                    DrawParam.StringCenter);
                e.Graphics.DrawLine(new Pen(GridColor), new Point(e.RowBounds.X, e.RowBounds.Bottom - 1),
                    new Point(e.RowBounds.X + RowHeadersWidth, e.RowBounds.Bottom - 1));
            }
        }
        /// <summary>
        /// 在单元格需要绘制时发生
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            base.OnCellPainting(e);
            if (e.RowIndex == -1)
            {
                if (_iCheckBox) DrawCombox(e);
            }
            else
            {
                if (_iMerger) DrawCell(e);
                if (IMultiText) DrawMultiText(e);
                if (_tColumnIndex != -1 && !string.IsNullOrEmpty(TColumnImage)) DrawImageText(e);
            }
        }

        #endregion

        #region 鼠标移过的行颜色
        void ComBoxGridView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (!IMove) return;
            if (e.RowIndex >= 0)
            {
                this.Rows[e.RowIndex].DefaultCellStyle.BackColor = this.RowTemplate.DefaultCellStyle.BackColor;
            }
        }
        void ComBoxGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!IMove) return;
            if (e.RowIndex >= 0)
            {
                this.Rows[e.RowIndex].DefaultCellStyle.BackColor = IMoveColor;
            }
        }

        #endregion

        #region 绘制Combox
        void HeaderCheckBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
                HeaderCheckBoxClick((CheckBox)sender);
        }
        void HeaderCheckBox_MouseClick(object sender, MouseEventArgs e)
        {
            HeaderCheckBoxClick((CheckBox)sender);
        }
        private void HeaderCheckBoxClick(CheckBox HCheckBox)
        {
            if (!_iCheckBox || string.IsNullOrEmpty(ICheckBoxName)) return;

            foreach (DataGridViewRow Row in this.Rows)
            {
                ((DataGridViewCheckBoxCell)Row.Cells[ICheckBoxName]).Value = HCheckBox.Checked;
            }
            this.RefreshEdit();
        }
        void ComBoxGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (!_iCheckBox) return;
            if (this.CurrentCell is DataGridViewCheckBoxCell)
                this.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
        private void DrawCombox(DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex != -1) return;
            if (!_iCheckBox) return;
            if (e.ColumnIndex == _iCheckBoxIndex)
            {
                Rectangle oRectangle = this.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                Point oPoint = new Point();
                oPoint.X = oRectangle.Location.X + (oRectangle.Width - _headerCheckBox.Width) / 2 + 1;
                oPoint.Y = oRectangle.Location.Y + (oRectangle.Height - _headerCheckBox.Height) / 2 + 1;
                _headerCheckBox.Location = oPoint;
            }
        }

        #endregion

        #region 合并单元格
        /// <summary>
        /// 合并单元格
        /// </summary>
        private void DrawCell(DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1) return;
            if (e.CellStyle.Alignment == DataGridViewContentAlignment.NotSet)
            {
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            Brush gridBrush = new SolidBrush(this.GridColor);
            SolidBrush backBrush = new SolidBrush(e.CellStyle.BackColor);
            SolidBrush fontBrush = new SolidBrush(e.CellStyle.ForeColor);
            int cellwidth;
            //上面相同的行数
            int UpRows = 0;
            //下面相同的行数
            int DownRows = 0;
            //总行数
            int count = 0;
            DataGridViewTextBoxColumn cell = this.Columns[e.ColumnIndex] as DataGridViewTextBoxColumn;
            if (cell != null && cell.Visible && cell.ReadOnly)
            {
                cellwidth = e.CellBounds.Width;
                Pen gridLinePen = new Pen(gridBrush);
                string curValue = e.Value == null ? "" : e.Value.ToString().Trim();
                bool select = false;
                if (!string.IsNullOrEmpty(curValue))
                {
                    #region 获取下面的行数
                    for (int i = e.RowIndex; i < this.Rows.Count; i++)
                    {
                        if (this.Rows[i].Cells[e.ColumnIndex].Value == null) break;
                        if (this.Rows[i].Cells[e.ColumnIndex].Value.ToString().Equals(curValue))
                        {
                            if (this.Rows[i].Cells[e.ColumnIndex].Selected) select = true;
                            //this.Rows[i].Cells[e.ColumnIndex].Selected = this.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected;
                            DownRows++;
                            if (e.RowIndex != i)
                            {
                                cellwidth = cellwidth < this.Rows[i].Cells[e.ColumnIndex].Size.Width ? cellwidth : this.Rows[i].Cells[e.ColumnIndex].Size.Width;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    #endregion
                    #region 获取上面的行数
                    for (int i = e.RowIndex; i >= 0; i--)
                    {
                        if (this.Rows[i].Cells[e.ColumnIndex].Value.ToString().Equals(curValue))
                        {
                            if (this.Rows[i].Cells[e.ColumnIndex].Selected) select = true;
                            //this.Rows[i].Cells[e.ColumnIndex].Selected = this.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected;
                            UpRows++;
                            if (e.RowIndex != i)
                            {
                                cellwidth = cellwidth < this.Rows[i].Cells[e.ColumnIndex].Size.Width ? cellwidth : this.Rows[i].Cells[e.ColumnIndex].Size.Width;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    #endregion
                    count = DownRows + UpRows - 1;
                    if (count < 2)
                    {
                        return;
                    }
                }
                if (select)
                {
                    #region 选中下面的行
                    for (int i = e.RowIndex; i < this.Rows.Count; i++)
                    {
                        if (this.Rows[i].Cells[e.ColumnIndex].Value == null) break;
                        if (this.Rows[i].Cells[e.ColumnIndex].Value.ToString().Equals(curValue))
                        {
                            this.Rows[i].Cells[e.ColumnIndex].Selected = true;
                        }
                        else
                        {
                            break;
                        }
                    }
                    #endregion
                    #region 选中上面的行
                    for (int i = e.RowIndex; i >= 0; i--)
                    {
                        if (this.Rows[i].Cells[e.ColumnIndex].Value == null) break;
                        if (this.Rows[i].Cells[e.ColumnIndex].Value.ToString().Equals(curValue))
                        {
                            this.Rows[i].Cells[e.ColumnIndex].Selected = true;
                        }
                        else
                        {
                            break;
                        }
                    }
                    #endregion
                }
                if (this.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected)
                {
                    backBrush.Color = e.CellStyle.SelectionBackColor;
                    fontBrush.Color = e.CellStyle.SelectionForeColor;
                }
                //以背景色填充
                e.Graphics.FillRectangle(backBrush, e.CellBounds);
                //画字符串
                PaintingFont(e, cellwidth, UpRows, DownRows, count);
                if (DownRows == 1)
                {
                    e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                    count = 0;
                }
                // 画右边线
                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);

                e.Handled = true;
            }
        }
        /// <summary>
        /// 画字符串
        /// </summary>
        /// <param name="e"></param>
        /// <param name="cellwidth"></param>
        /// <param name="UpRows"></param>
        /// <param name="DownRows"></param>
        /// <param name="count"></param>
        private void PaintingFont(DataGridViewCellPaintingEventArgs e, int cellwidth, int UpRows, int DownRows, int count)
        {
            SolidBrush fontBrush = new SolidBrush(e.CellStyle.ForeColor);
            string value = e.Value == null ? null : e.Value.ToString();
            int fontheight = (int)e.Graphics.MeasureString(value, e.CellStyle.Font).Height;
            int fontwidth = (int)e.Graphics.MeasureString(value, e.CellStyle.Font).Width;
            int cellheight = e.CellBounds.Height;

            if (e.CellStyle.Alignment == DataGridViewContentAlignment.BottomCenter)
            {
                e.Graphics.DrawString(value, e.CellStyle.Font, fontBrush, e.CellBounds.X + (cellwidth - fontwidth) / 2, e.CellBounds.Y + cellheight * DownRows - fontheight);
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.BottomLeft)
            {
                e.Graphics.DrawString(value, e.CellStyle.Font, fontBrush, e.CellBounds.X, e.CellBounds.Y + cellheight * DownRows - fontheight);
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.BottomRight)
            {
                e.Graphics.DrawString(value, e.CellStyle.Font, fontBrush, e.CellBounds.X + cellwidth - fontwidth, e.CellBounds.Y + cellheight * DownRows - fontheight);
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.MiddleCenter)
            {
                e.Graphics.DrawString(value, e.CellStyle.Font, fontBrush, e.CellBounds.X + (cellwidth - fontwidth) / 2, e.CellBounds.Y - cellheight * (UpRows - 1) + (cellheight * count - fontheight) / 2);
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.MiddleLeft)
            {
                e.Graphics.DrawString(value, e.CellStyle.Font, fontBrush, e.CellBounds.X, e.CellBounds.Y - cellheight * (UpRows - 1) + (cellheight * count - fontheight) / 2);
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.MiddleRight)
            {
                e.Graphics.DrawString(value, e.CellStyle.Font, fontBrush, e.CellBounds.X + cellwidth - fontwidth, e.CellBounds.Y - cellheight * (UpRows - 1) + (cellheight * count - fontheight) / 2);
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.TopCenter)
            {
                e.Graphics.DrawString(value, e.CellStyle.Font, fontBrush, e.CellBounds.X + (cellwidth - fontwidth) / 2, e.CellBounds.Y - cellheight * (UpRows - 1));
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.TopLeft)
            {
                e.Graphics.DrawString(value, e.CellStyle.Font, fontBrush, e.CellBounds.X, e.CellBounds.Y - cellheight * (UpRows - 1));
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.TopRight)
            {
                e.Graphics.DrawString(value, e.CellStyle.Font, fontBrush, e.CellBounds.X + cellwidth - fontwidth, e.CellBounds.Y - cellheight * (UpRows - 1));
            }
            else
            {
                e.Graphics.DrawString(value, e.CellStyle.Font, fontBrush, e.CellBounds.X + (cellwidth - fontwidth) / 2, e.CellBounds.Y - cellheight * (UpRows - 1) + (cellheight * count - fontheight) / 2);
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
            if (e.Value == null || e.Value.ToString().Trim() == String.Empty || e.Value.ToString().IndexOf("&") == -1) return;

            DataGridViewTextBoxColumn cell = this.Columns[e.ColumnIndex] as DataGridViewTextBoxColumn;
            if (cell != null && cell.Visible && cell.ReadOnly)
            {
                Graphics graphics = e.Graphics;
                Color colorFore = e.CellStyle.ForeColor;
                Color colorBack = e.CellStyle.BackColor;
                if (this.Rows[e.RowIndex].Selected)
                {
                    colorFore = e.CellStyle.SelectionForeColor;
                    colorBack = e.CellStyle.SelectionBackColor;
                }
                DrawBounds(e.Graphics, new SolidBrush(colorBack), e.CellBounds, e.RowIndex);

                int index = e.Value.ToString().IndexOf("&");
                string strFirst = e.Value.ToString().Substring(0, index);
                string strSecond = e.Value.ToString().Substring(index + 1);

                Font fontFore = e.CellStyle.Font;
                Size sizText = TextRenderer.MeasureText(graphics, strFirst, fontFore);
                int intX = e.CellBounds.Left + e.CellStyle.Padding.Left;
                int intY = e.CellBounds.Top + e.CellStyle.Padding.Top;
                int intWidth = e.CellBounds.Width - (e.CellStyle.Padding.Left + e.CellStyle.Padding.Right);
                int intHeight = e.CellBounds.Height - (e.CellStyle.Padding.Top + e.CellStyle.Padding.Bottom);
                intHeight = intHeight / 2;

                colorFore = Color.Black;
                //the first line
                TextRenderer.DrawText(graphics, strFirst, fontFore, new Rectangle(intX, intY, intWidth, intHeight), colorFore, DrawParam.TextEnd);

                fontFore = e.CellStyle.Font;
                intY = intY + intHeight;

                colorFore = Color.SteelBlue;
                //the seconde line
                TextRenderer.DrawText(graphics, strSecond, fontFore, new Rectangle(intX, intY, intWidth, intHeight), colorFore, DrawParam.TextEnd);

                e.Handled = true;
            }
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
                Brush foreBrush = new SolidBrush(e.CellStyle.ForeColor);
                Brush backBrush = new SolidBrush(e.CellStyle.BackColor);
                if (this.Rows[e.RowIndex].Selected)
                {
                    foreBrush = new SolidBrush(e.CellStyle.SelectionForeColor);
                    backBrush = new SolidBrush(e.CellStyle.SelectionBackColor);
                }
                DrawBounds(e.Graphics, backBrush, e.CellBounds, e.RowIndex);

                //画图标
                Bitmap bitmap = this.Rows[e.RowIndex].Cells[TColumnImage].Value as Bitmap;
                if (bitmap != null)
                {
                    Rectangle newRect = new Rectangle(e.CellBounds.X + 5, e.CellBounds.Y + (e.CellBounds.Height - bitmap.Height) / 2, bitmap.Width, bitmap.Height);
                    e.Graphics.DrawImage(bitmap, newRect);
                }

                //画字符串
                e.Graphics.DrawString(e.Value == null ? null : e.Value.ToString(), e.CellStyle.Font, foreBrush,
                    new Rectangle(e.CellBounds.Left + (bitmap == null ? 0 : bitmap.Width) + 10, e.CellBounds.Top, e.CellBounds.Width, e.CellBounds.Height), DrawParam.StringVertical);

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
            if (index == 0 && !ColumnHeadersVisible)
            {
                g.DrawLine(new Pen(GridColor), new Point(rect.X, rect.Top), new Point(rect.Right, rect.Top));
            }

            //划线
            Point p1 = new Point(rect.Right - 1, rect.Top);
            Point p2 = new Point(rect.Right - 1, rect.Bottom - 1);
            Point p3 = new Point(rect.Left, rect.Bottom - 1);
            Point[] ps = new Point[] { p1, p2, p3 };
            g.DrawLines(new Pen(GridColor), ps);
        }

        #endregion

        #region 动态文本图片列
        private Timer timer = new Timer();
        private PictureBox pictureBox1;
        /// <summary>
        /// 动态图片行
        /// </summary>
        private int pIndex = -1;
        /// <summary>
        /// 动态文本图片列显示的图片
        /// </summary>
        [Browsable(true), Description("动态文本图片列显示的图片"), DefaultValue(null)]
        public Image TProgressImage
        {
            get { return pictureBox1.Image; }
            set { pictureBox1.Image = value; }
        }
        /// <summary>
        /// 动态文本图片列显示行
        /// </summary>
        [Browsable(true), Description("动态文本图片列显示行"), DefaultValue(-1)]
        public int TProgressIndex
        {
            get { return this.pIndex; }
            set
            {
                this.pIndex = value;
                this.timer.Enabled = value != -1 && !string.IsNullOrEmpty(TColumnImage) && !string.IsNullOrEmpty(TColumnText);
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (pIndex == -1 || pIndex >= this.Rows.Count) return;
            DataGridViewCell image = this.Rows[pIndex].Cells[TColumnImage];
            image.Value = this.pictureBox1.Image;
            DataGridViewCell cell = this.Rows[pIndex].Cells[TColumnText];
            this.InvalidateCell(cell);
        }

        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1, 1);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // TDataGridView
            // 
            this.Controls.Add(this.pictureBox1);
            this.RowTemplate.Height = 23;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
    }
}
