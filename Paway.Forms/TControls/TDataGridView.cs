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

        private CheckBox HeaderCheckBox = null;
        /// <summary>
        /// 
        /// </summary>
        public TDataGridView()
            : base()
        {
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.DoubleBuffer, true);
            this.UpdateStyles();

            this.CellMouseEnter += ComBoxGridView_CellMouseEnter;
            this.CellMouseLeave += ComBoxGridView_CellMouseLeave;
            this.RowPostPaint += TDataGridView_RowPostPaint;
            this.BackgroundColor = Color.White;
            this.BorderStyle = BorderStyle.None;
            InitializeComponent();
            timer.Interval = 30;
            this.timer.Tick += timer_Tick;
        }

        #region 事件
        /// <summary>
        /// 行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TDataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (!this.RowHeadersVisible) return;
            using (SolidBrush brush = new SolidBrush(this.RowHeadersDefaultCellStyle.ForeColor))
            {
                int linen = 0;
                linen = e.RowIndex + 1;
                string line = linen.ToString();
                e.Graphics.DrawString(line, e.InheritedRowStyle.Font, brush, new Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, this.RowHeadersWidth, e.RowBounds.Height), DrawParam.StringCenter);
            }
        }
        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            base.OnCellPainting(e);
            if (e.RowIndex == -1)
            {
                if (_isDrawCheckBox) DrawCombox(e);
            }
            else
            {
                if (_isDrawMerger) DrawCell(e);
                if (_isMultiText) DrawMultiText(e);
                if (_columnImageText != -1 && !string.IsNullOrEmpty(_columnImage)) DrawImageText(e);
            }
        }

        #endregion

        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置 System.Windows.Forms.DataGridView 的背景色。
        /// </summary>
        [DefaultValue(typeof(Color), "White")]
        public new Color BackgroundColor
        {
            get { return base.BackgroundColor; }
            set { base.BackgroundColor = value; }
        }
        /// <summary>
        /// 获取或设置 System.Windows.Forms.DataGridView 的边框样式。
        /// </summary>
        [DefaultValue(typeof(BorderStyle), "None")]
        public new BorderStyle BorderStyle
        {
            get { return base.BorderStyle; }
            set { base.BorderStyle = value; }
        }
        /// <summary>
        /// 设置数据源时设置图片列
        /// </summary>
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
        /// 刷新数据
        /// </summary>
        public void RefreshData()
        {
            this.DataSource = source;
        }
        /// <summary>
        /// 原数据源
        /// </summary>
        private object source;
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
            for (int i = 0; i < this.Columns.Count; i++)
            {
                if (this.Columns[i].Name == ColumnImageText)
                {
                    _columnImageText = i;
                    break;
                }
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

        private int _columnImageText = -1;
        /// <summary>
        /// 文本前绘制小图片-文本列
        /// </summary>
        [Browsable(true), Description("文本前绘制小图片-文本列"), DefaultValue(null)]
        public string ColumnImageText { get; set; }
        private string _columnImage;
        /// <summary>
        /// 文本前绘制小图片-图片列
        /// </summary>
        [Browsable(true), Description("文本前绘制小图片-图片列"), DefaultValue(null)]
        public string ColumnImage
        {
            get { return _columnImage; }
            set
            {
                _columnImage = value;
            }
        }

        private bool _isMultiText = false;
        /// <summary>
        /// 是否绘制多行文本
        /// </summary>
        [Browsable(true), Description("是否绘制多行文本"), DefaultValue(false)]
        public bool IsMultiText
        {
            get { return _isMultiText; }
            set
            {
                _isMultiText = value;
            }
        }

        private bool _isDrawMerger = false;
        /// <summary>
        /// 是否绘制合并列
        /// </summary>
        [Browsable(true), Description("是否绘制合并列"), DefaultValue(false)]
        public bool IsDrawMerger
        {
            get { return _isDrawMerger; }
            set
            {
                _isDrawMerger = value;
                if (value) this.MultiSelect = true;
            }
        }

        private bool _isDrawMove = true;
        /// <summary>
        /// 是否绘制鼠标移过时的颜色
        /// </summary>
        [Browsable(true), Description("是否绘制鼠标移过时的颜色"), DefaultValue(true)]
        public bool IsDrawMove
        {
            get { return _isDrawMove; }
            set
            {
                _isDrawMove = value;
            }
        }

        private Color _colorMove = Color.Azure;
        /// <summary>
        /// 鼠标移过的行颜色
        /// </summary>
        [Browsable(true), Description("鼠标移过的行颜色"), DefaultValue(typeof(Color), "Azure")]
        public Color ColorMove
        {
            get { return _colorMove; }
            set { _colorMove = value; }
        }

        private bool _isDrawCheckBox = false;
        /// <summary>
        /// 是否绘制CheckBox
        /// </summary>
        [Browsable(true), Description("是否绘制CheckBox"), DefaultValue(false)]
        public bool IsDrawCheckBox
        {
            get { return _isDrawCheckBox; }
            set
            {
                _isDrawCheckBox = value;
                if (HeaderCheckBox == null)
                {
                    HeaderCheckBox = new CheckBox();
                    HeaderCheckBox.Size = new Size(15, 15);
                    this.Controls.Add(HeaderCheckBox);

                    HeaderCheckBox.KeyUp += HeaderCheckBox_KeyUp;
                    HeaderCheckBox.MouseClick += HeaderCheckBox_MouseClick;
                    this.CurrentCellDirtyStateChanged += ComBoxGridView_CurrentCellDirtyStateChanged;
                }
                this.HeaderCheckBox.Visible = _isDrawCheckBox;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 绘制列数据在源中的序号
        /// </summary>
        [Browsable(true), Description("绘制列数据在源中的序号"), DefaultValue(0)]
        public int CheckBoxIndex { get; set; }

        /// <summary>
        /// 绘制列列Name
        /// </summary>
        [Browsable(true), Description("绘制列列Name")]
        public string CheckBoxName { get; set; }
        #endregion

        #region 鼠标移过的行颜色
        void ComBoxGridView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (!_isDrawMove) return;
            if (e.RowIndex >= 0)
            {
                this.Rows[e.RowIndex].DefaultCellStyle.BackColor = this.RowTemplate.DefaultCellStyle.BackColor;
            }
        }
        void ComBoxGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!_isDrawMove) return;
            if (e.RowIndex >= 0)
            {
                this.Rows[e.RowIndex].DefaultCellStyle.BackColor = _colorMove;
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
            if (string.IsNullOrEmpty(CheckBoxName)) return;
            if (!_isDrawCheckBox) return;

            foreach (DataGridViewRow Row in this.Rows)
            {
                ((DataGridViewCheckBoxCell)Row.Cells[CheckBoxName]).Value = HCheckBox.Checked;
            }
            this.RefreshEdit();
        }

        void ComBoxGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (!_isDrawCheckBox) return;
            if (this.CurrentCell is DataGridViewCheckBoxCell)
                this.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void DrawCombox(DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex != -1) return;
            if (!_isDrawCheckBox) return;
            if (e.ColumnIndex == CheckBoxIndex)
            {
                Rectangle oRectangle = this.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                Point oPoint = new Point();
                oPoint.X = oRectangle.Location.X + (oRectangle.Width - HeaderCheckBox.Width) / 2 + 1;
                oPoint.Y = oRectangle.Location.Y + (oRectangle.Height - HeaderCheckBox.Height) / 2 + 1;
                HeaderCheckBox.Location = oPoint;
            }
        }
        #endregion

        #region 合并列
        /// <summary>
        /// 画单元格
        /// </summary>
        /// <param name="e"></param>
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
                Graphics gpcEventArgs = e.Graphics;
                Color clrFore = e.CellStyle.ForeColor;
                Color clrBack = e.CellStyle.BackColor;
                if (this.Rows[e.RowIndex].Selected)
                {
                    clrFore = e.CellStyle.SelectionForeColor;
                    clrBack = e.CellStyle.SelectionBackColor;
                }
                Font fntText = e.CellStyle.Font;

                string strFirstLine = e.Value.ToString().Substring(0, e.Value.ToString().IndexOf("&"));
                string strSecondLine = e.Value.ToString().Substring(e.Value.ToString().IndexOf("&") + 1);

                Size sizText = TextRenderer.MeasureText(e.Graphics, strFirstLine, fntText);

                int intX = e.CellBounds.Left + e.CellStyle.Padding.Left;
                int intY = e.CellBounds.Top + e.CellStyle.Padding.Top;
                int intWidth = e.CellBounds.Width - (e.CellStyle.Padding.Left + e.CellStyle.Padding.Right);
                //int intHeight = sizText.Height + (e.CellStyle.Padding.Top + e.CellStyle.Padding.Bottom);
                int intHeight = e.CellBounds.Height - (e.CellStyle.Padding.Top + e.CellStyle.Padding.Bottom);
                intHeight = intHeight / 2;

                gpcEventArgs.FillRectangle(new SolidBrush(clrBack), new Rectangle(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width - 1, e.CellBounds.Height - 1));

                //划线
                Brush gridBrush = new SolidBrush(this.GridColor);
                Pen gridLinePen = new Pen(gridBrush, 1);
                Point p1 = new Point(e.CellBounds.Left + e.CellBounds.Width - 1, e.CellBounds.Top);
                Point p2 = new Point(e.CellBounds.Left + e.CellBounds.Width - 1, e.CellBounds.Top + e.CellBounds.Height - 1);
                Point p3 = new Point(e.CellBounds.Left, e.CellBounds.Top + e.CellBounds.Height - 1);
                Point[] ps = new Point[] { p1, p2, p3 };
                e.Graphics.DrawLines(gridLinePen, ps);

                clrFore = System.Drawing.Color.Black;
                //the first line
                TextRenderer.DrawText(e.Graphics, strFirstLine, fntText, new Rectangle(intX, intY, intWidth, intHeight), clrFore, DrawParam.TextEnd);

                fntText = e.CellStyle.Font;
                intY = intY + intHeight;

                clrFore = System.Drawing.Color.SteelBlue;
                //the seconde line
                TextRenderer.DrawText(e.Graphics, strSecondLine, fntText, new Rectangle(intX, intY, intWidth, intHeight), clrFore, DrawParam.TextEnd);

                e.Handled = true;
            }
        }
        #endregion

        #region 文本前绘制图片
        private void DrawImageText(DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == _columnImageText)
            {
                Brush foreColorBrush = new SolidBrush(e.CellStyle.ForeColor);
                Brush backColorBrush = new SolidBrush(e.CellStyle.BackColor);
                if (this.Rows[e.RowIndex].Selected)
                {
                    foreColorBrush = new SolidBrush(e.CellStyle.SelectionForeColor);
                    backColorBrush = new SolidBrush(e.CellStyle.SelectionBackColor);
                }

                // Erase the cell.
                e.Graphics.FillRectangle(backColorBrush, e.CellBounds);

                //划线
                Brush gridBrush = new SolidBrush(this.GridColor);
                Pen gridLinePen = new Pen(gridBrush, 1);
                Point p1 = new Point(e.CellBounds.Left + e.CellBounds.Width - 1, e.CellBounds.Top);
                Point p2 = new Point(e.CellBounds.Left + e.CellBounds.Width - 1, e.CellBounds.Top + e.CellBounds.Height - 1);
                Point p3 = new Point(e.CellBounds.Left, e.CellBounds.Top + e.CellBounds.Height - 1);
                Point[] ps = new Point[] { p1, p2, p3 };
                e.Graphics.DrawLines(gridLinePen, ps);

                //画图标
                Bitmap bitmap = this.Rows[e.RowIndex].Cells[ColumnImage].Value as Bitmap;
                Rectangle newRect = new Rectangle(e.CellBounds.X + 5, e.CellBounds.Y + (e.CellBounds.Height - bitmap.Height) / 2, bitmap.Width, bitmap.Height);
                e.Graphics.DrawImage(bitmap, newRect);

                //画字符串
                e.Graphics.DrawString(e.Value == null ? null : e.Value.ToString(), e.CellStyle.Font, foreColorBrush,
                    new Rectangle(e.CellBounds.Left + bitmap.Width + 10, e.CellBounds.Top, e.CellBounds.Width, e.CellBounds.Height), DrawParam.StringVertical);
                e.Handled = true;
            }
        }

        #endregion

        #region 扩展方法 - 动态显示项的图像
        private Timer timer = new Timer();
        private PictureBox pictureBox1;
        /// <summary>
        /// 动态图片行
        /// </summary>
        private int pIndex = -1;
        /// <summary>
        /// 动态显示的图片
        /// </summary>
        [Browsable(true), Description("动态显示的图片"), DefaultValue(null)]
        public Image ProgressImage
        {
            get { return pictureBox1.Image; }
            set { pictureBox1.Image = value; }
        }
        /// <summary>
        /// 设置显示行
        /// </summary>
        [Browsable(true), Description("设置动态显示行"), DefaultValue(-1)]
        public int ProgressIndex
        {
            get { return this.pIndex; }
            set
            {
                this.pIndex = value;
                this.timer.Enabled = value != -1 && !string.IsNullOrEmpty(ColumnImage) && !string.IsNullOrEmpty(ColumnImageText);
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (pIndex == -1 || pIndex >= this.Rows.Count) return;
            DataGridViewCell image = this.Rows[pIndex].Cells[ColumnImage];
            image.Value = this.pictureBox1.Image;
            DataGridViewCell cell = this.Rows[pIndex].Cells[ColumnImageText];
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
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
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
