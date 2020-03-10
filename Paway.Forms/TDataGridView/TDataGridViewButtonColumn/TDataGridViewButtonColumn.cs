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
    /// 扩展列
    /// </summary>
    public class TDataGridViewButtonColumn : DataGridViewColumn
    {
        internal IButtonAttribute Button { get; private set; }
        /// <summary>
        /// 宿主datagridview扩展
        /// </summary>
        internal TDataGridView DataGridViewEx
        {
            get { return this.DataGridView as TDataGridView; }
        }
        /// <summary>
        /// 拷贝
        /// </summary>
        public override object Clone()
        {
            TDataGridViewButtonColumn column = (TDataGridViewButtonColumn)base.Clone();
            column.Button = Button;
            return column;
        }
        /// <summary>
        /// </summary>
        public TDataGridViewButtonColumn()
        {
            //建立单元格模板
            this.CellTemplate = new TDataGridViewButtonCell();
        }
        /// <summary>
        /// </summary>
        public TDataGridViewButtonColumn(IButtonAttribute button) : this()
        {
            this.Button = button;
        }
    }
}
