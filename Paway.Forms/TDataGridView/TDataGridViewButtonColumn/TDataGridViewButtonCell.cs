using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms.VisualStyles;
using Paway.Helper;
using System.Drawing.Drawing2D;

namespace Paway.Forms
{
    /// <summary>
    /// 扩展单元格
    /// <para>1> 主要是绘制大小固定的按钮，并且添加一些按钮的事件等</para>
    /// <para>2> 按钮可以支持根据内容来筛选显示,增加转换器</para>
    /// <para>3> 按钮的点击事件可以直接抛出</para>
    /// </summary>
    public class TDataGridViewButtonCell : DataGridViewButtonCell
    {
        #region 属性
        /// <summary>
        /// 当前按钮位置大小
        /// </summary>
        protected Rectangle btnRect = Rectangle.Empty;
        /// <summary>
        /// 当前按钮状态
        /// </summary>
        private TMouseState btnState = TMouseState.Normal;
        private TDataGridView DataGridViewEx
        {
            get { return this.DataGridView as TDataGridView; }
        }
        /// <summary>
        /// 所属列
        /// </summary>
        private TDataGridViewButtonColumn Column
        {
            get { return this.OwningColumn as TDataGridViewButtonColumn; }
        }

        #endregion

        #region 重绘
        /// <summary>
        /// 重绘
        /// </summary>
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            if (this.btnState == TMouseState.Normal && this.OwningRow.Index == rowIndex && this.OwningRow.Selected)
            {
                graphics.FillRectangle(new SolidBrush(cellStyle.SelectionBackColor), cellBounds);
            }
            else
            {// 不重绘单元格的背景颜色
                graphics.FillRectangle(new SolidBrush(cellStyle.BackColor), cellBounds);
            }
            // 填充单元格的边框
            base.PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);

            if (value is string text && !text.IsNullOrEmpty())
            {
                Image image = null;
                if (!this.Column.Button.ImageName.IsNullOrEmpty())
                {
                    image = (Image)this.Column.DataGridViewEx.Rows[rowIndex].Cells[this.Column.Button.ImageName].Value;
                }
                DrawTextButton(graphics, cellBounds, text, image);
            }
            else if (value is Image image)
            {
                DrawTextButton(graphics, cellBounds, null, image);
            }
            else
            {
                btnRect = Rectangle.Empty;
            }
        }
        private void DrawTextButton(Graphics g, Rectangle cellBounds, string text, Image image)
        {
            //计算button的区域
            var button = this.Column.Button;
            if (button.BackGround == null)
            {
                button.BackGround = new TProperties();
                button.BackGround.Reset(this.Column.DataGridViewEx.GridColor.AddLight(-30), 15);
                button.BackGround.ColorNormal = Color.Empty;
            }
            if (button.Text == null)
            {
                button.Text = new TProperties
                {
                    FontNormal = this.Column.DataGridViewEx.RowTemplate.DefaultCellStyle.Font
                };
                button.Text.StringFormat.Alignment = StringAlignment.Center;
                button.Text.ColorNormal = this.Column.DataGridViewEx.ForeColor;
                button.Text.ColorMove = Color.White;
                button.Text.ColorDown = Color.White;
            }
            var size = AutoSize(button, cellBounds, text);

            if (image != null)
            {
                if (size.Height < button.ImageSize.Height) size.Height = button.ImageSize.Height;
                size.Width += button.ImageSize.Width;
            }
            btnRect = RectangleCommon.GetSmallRectOfRectangle(cellBounds, size, out _);
            //绘制按钮
            this.DrawButton(g, button, text, image);
        }
        private Size AutoSize(IButtonAttribute button, Rectangle bounds, string text)
        {
            var size = button.Size;
            if (size.Width == 0)
            {
                size.Width = button.Text.AutoWidth(btnState, text, bounds.Size) + 10;
            }
            if (size.Height == 0)
            {
                size.Height = button.Text.AutoHeight(btnState) + 8;
            }
            return size;
        }
        private void DrawButton(Graphics g, IButtonAttribute button, string text, Image image)
        {
            var fillRect = new Rectangle(btnRect.X + button.Line / 2, btnRect.Y + button.Line / 2, btnRect.Width - button.Line, btnRect.Height - button.Line);
            var color = button.BackGround.AutoColor(btnState);
            using (var path = DrawHelper.CreateRoundPath(fillRect, button.Radiu))
            using (var solidBrush = new SolidBrush(color))
            {
                g.FillPath(solidBrush, path);
            }
            try
            {
                color = (color == Color.Empty ? Color.LightGray : color).AddLight(-15);
                if (button.Line % 2 == 0) g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                var line = button.Line; if (line < 1) line = 1;
                var drawRect = new Rectangle(btnRect.X, btnRect.Y, btnRect.Width - line, btnRect.Height - line);
                using (var path = DrawHelper.CreateRoundPath(drawRect, button.Radiu, button.Line))
                using (var pen = new Pen(color, button.Line))
                {
                    g.DrawPath(pen, path);
                }
            }
            finally
            {
                g.PixelOffsetMode = PixelOffsetMode.Default;
            }

            var rect = btnRect;
            if (image != null)
            {
                var width = button.Text.AutoWidth(btnState, text, btnRect.Size);
                var left = (rect.Width - width - button.ImageSize.Width) / 2;
                var top = (rect.Height - button.ImageSize.Height) / 2;
                var imageRect = new Rectangle(rect.X + left, rect.Y + top, button.ImageSize.Width, button.ImageSize.Height);
                BitmapHelper.DragImage(g, image, imageRect);
                rect.X += button.ImageSize.Width;
                rect.Width -= button.ImageSize.Width;
            }
            TextRenderer.DrawText(g, text, button.Text.AutoFont(btnState), rect, button.Text.AutoColor(btnState), button.Text.TextFormat());
        }

        #endregion

        #region 重载事件
        /// <summary>
        /// </summary>
        protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (IsInRegion(e.Location, e.RowIndex, e.ColumnIndex))
            {
                this.btnState = TMouseState.Move;
            }
            else
            {
                this.btnState = TMouseState.Normal;
            }
            this.DataGridView.InvalidateCell(this);
        }
        /// <summary>
        /// </summary>
        protected override void OnMouseLeave(int rowIndex)
        {
            base.OnMouseLeave(rowIndex);
            this.btnState = TMouseState.Normal;
            this.DataGridView.InvalidateCell(this);
        }
        /// <summary>
        /// </summary>
        protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (IsInRegion(e.Location, e.RowIndex, e.ColumnIndex))
            {
                this.btnState = TMouseState.Down;
            }
            else
            {
                this.btnState = TMouseState.Normal;
            }
            this.DataGridView.InvalidateCell(this);
        }
        /// <summary>
        /// </summary>
        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            if (IsInRegion(e.Location, e.RowIndex, e.ColumnIndex))
            {
                if (e.Button == MouseButtons.Left)
                {
                    this.DataGridViewEx.OnButtonClicked(e.RowIndex, e.ColumnIndex, this.Value);
                }
            }
            base.OnMouseClick(e);
        }
        /// <summary>
        /// 是否在Button按钮区域
        /// </summary>
        private bool IsInRegion(Point p, int rowIndex, int columnIndex)
        {
            Rectangle cellBounds = DataGridView[columnIndex, rowIndex].ContentBounds;
            RectangleCommon.GetSmallRectOfRectangle(cellBounds, btnRect.Size, out Rectangle m_absBtnRegion);
            return m_absBtnRegion.Contains(p);
        }

        #endregion
    }
}
