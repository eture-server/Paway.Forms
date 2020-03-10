using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace Paway.Forms
{
    /// <summary>
    /// 扩展表头单元格
    /// <para>1> 主要可以再表头添加全选的checkbox</para>
    /// <para>2> 全选可以和下面的行进行联动</para>
    /// <para>3> 控制此列单元格中只有点击到checkbox时才起作用，不会点击单元格就起效果</para>
    /// <para>4> 封装了checkbox列的一些操作，不必是DataGridView中需要自定义操作</para>
    /// </summary>
    public class TDataGridViewCheckBoxColumnHeaderCell : DataGridViewColumnHeaderCell
    {
        #region 属性
        /// <summary>
        /// 用于标识当前的checkbox状态
        /// </summary>
        private CheckBoxState m_checkboxState = CheckBoxState.UncheckedNormal;
        /// <summary>
        /// 宿主DataGridView扩展
        /// </summary>
        private TDataGridView DataGridViewEx
        {
            get { return DataGridView as TDataGridView; }
        }
        /// <summary>
        /// 是否是鼠标经过的这种hot状态，不是即为normal
        /// </summary>
        private Boolean m_isHot = false;
        /// <summary>
        /// checkbox按钮区域
        /// </summary>
        private Rectangle m_chkboxRegion;
        /// <summary>
        /// 相对于本cell的位置
        /// </summary>
        private Rectangle m_absChkboxRegion;

        /// <summary>
        /// 全选按钮状态
        /// </summary>
        public CheckState CheckedAllState { get; set; } = CheckState.Unchecked;

        #endregion

        #region 重绘
        /// <summary>
        /// 重绘
        /// </summary>
        protected override void Paint(System.Drawing.Graphics graphics, System.Drawing.Rectangle clipBounds, System.Drawing.Rectangle cellBounds, int rowIndex, DataGridViewElementStates dataGridViewElementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, dataGridViewElementState, @"", @"", errorText, cellStyle, advancedBorderStyle, paintParts);
            this.m_chkboxRegion = RectangleCommon.GetSmallRectOfRectangle(cellBounds, CheckBoxRenderer.GetGlyphSize(graphics, CheckBoxState.UncheckedNormal), out m_absChkboxRegion);
            this.RenderCheckBox(graphics);
        }
        private void RenderCheckBox(Graphics graphics)
        {
            if (m_isHot)
                RenderCheckBoxHover(graphics);
            else
                RenderCheckBoxNormal(graphics);

            CheckBoxRenderer.DrawCheckBox(graphics, m_chkboxRegion.Location, m_checkboxState);
        }
        private void RenderCheckBoxNormal(Graphics graphics)
        {
            switch (CheckedAllState)
            {
                case CheckState.Unchecked:
                    this.m_checkboxState = CheckBoxState.UncheckedNormal;
                    break;
                case CheckState.Indeterminate:
                    this.m_checkboxState = CheckBoxState.MixedNormal;
                    break;
                case CheckState.Checked:
                    this.m_checkboxState = CheckBoxState.CheckedNormal;
                    break;
            }
        }
        private void RenderCheckBoxHover(Graphics graphics)
        {
            switch (CheckedAllState)
            {
                case CheckState.Unchecked:
                    this.m_checkboxState = CheckBoxState.UncheckedHot;
                    break;
                case CheckState.Indeterminate:
                    this.m_checkboxState = CheckBoxState.MixedHot;
                    break;
                case CheckState.Checked:
                    this.m_checkboxState = CheckBoxState.CheckedHot;
                    break;
            }
        }

        #endregion

        #region 重载事件
        /// <summary>
        /// </summary>
        protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (IsInCheckRegion(e.Location))
                m_isHot = true;
            this.DataGridView.InvalidateCell(this);
        }
        /// <summary>
        /// </summary>
        protected override void OnMouseLeave(int rowIndex)
        {
            base.OnMouseLeave(rowIndex);
            m_isHot = false;
            this.DataGridView.InvalidateCell(this);
        }
        /// <summary>
        /// </summary>
        protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseDown(e);
            m_isHot = IsInCheckRegion(e.Location);
            this.DataGridView.InvalidateCell(this);
        }
        /// <summary>
        /// </summary>
        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            Boolean value = false;
            if (IsInCheckRegion(e.Location))
            {
                switch (CheckedAllState)
                {
                    case CheckState.Unchecked:
                        CheckedAllState = CheckState.Checked;
                        value = true;
                        break;
                    case CheckState.Indeterminate:
                        CheckedAllState = CheckState.Checked;
                        value = true;
                        break;
                    case CheckState.Checked:
                        CheckedAllState = CheckState.Unchecked;
                        value = false;
                        break;
                }
                this.Value = value;
                this.DataGridViewEx.OnCheckAllCheckedChange(e.ColumnIndex, value);
            }
            base.OnMouseClick(e);
        }
        /// <summary>
        /// 是否在checkbox按钮区域
        /// </summary>
        protected bool IsInCheckRegion(Point p)
        {
            return this.m_absChkboxRegion.Contains(p);
        }

        #endregion
    }
}
