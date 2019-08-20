using System;
using System.Drawing;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    /// TreeGridColumn
    /// </summary>
    public class TreeGridColumn : DataGridViewTextBoxColumn
    {
        /// <summary>
        /// </summary>
        public TreeGridColumn()
        {
            this.CellTemplate = new TreeGridCell();
        }
    }
}

