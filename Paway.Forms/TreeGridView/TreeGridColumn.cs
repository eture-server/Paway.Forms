using System;
using System.Drawing;
using System.Windows.Forms;

namespace Paway.Forms
{
    public class TreeGridColumn : DataGridViewTextBoxColumn
    {
        public TreeGridColumn()
        {
            this.CellTemplate = new TreeGridCell();
        }
    }
}

