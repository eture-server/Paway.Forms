using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace Paway.Forms
{
    /// <summary>
    /// 扩展单元格
    /// </summary>
    public class TDataGridViewCheckBoxCell : DataGridViewCheckBoxCell
    {
        /// <summary>
        /// 本单元格是否被选中
        /// </summary>
        public Boolean Checked
        {
            get { return Convert.ToBoolean(this.Value); }
            set { this.Value = value; }
        }

        /// <summary>
        /// 宿主datagridview扩展
        /// </summary>
        private TDataGridView DataGridViewEx
        {
            get { return this.DataGridView as TDataGridView; }
        }

        /// <summary>
        /// 处理选中与取消选中
        /// </summary>
        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            this.Checked = !this.Checked;
            this.DataGridViewEx.OnCheckBoxCellCheckedChange(e.RowIndex, e.ColumnIndex, this.Checked);
            base.OnMouseClick(e);
        }
    }
}
