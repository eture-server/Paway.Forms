using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace Paway.Forms
{
    /// <summary>
    /// 扩展列
    /// </summary>
    public class TDataGridViewCheckBoxColumn : DataGridViewCheckBoxColumn
    {
        /// <summary>
        /// 本列是否全选，方便在Cell中获取
        /// </summary>
        public Boolean IsCheckedAll
        {
            get { return (this.HeaderCell as TDataGridViewCheckBoxColumnHeaderCell).CheckedAllState == CheckState.Checked; }
        }

        /// <summary>
        /// </summary>
        public TDataGridViewCheckBoxColumn()
        {
            //建立表头单元格
            this.HeaderCell = new TDataGridViewCheckBoxColumnHeaderCell() as DataGridViewColumnHeaderCell;
            //建立单元格模板
            this.CellTemplate = new TDataGridViewCheckBoxCell();
        }
    }

}
