using Paway.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Paway.Test
{
    public partial class FormGridView : Form
    {
        public FormGridView()
        {
            InitializeComponent();
            button1.Click += Button1_Click;
            gridview1.CurrentCellChanged += TDataGridView1_CurrentCellChanged;
        }
        int index = -1;
        Bitmap bitmap;
        Bitmap last;
        /// <summary>
        /// 模式动态执行过程
        /// </summary>
        private void TDataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (this.gridview1.CurrentCell == null) return;
            if (this.gridview1.CurrentCell.RowIndex == index) return;
            int lastIndex = index;

            index = this.gridview1.CurrentCell.RowIndex;
            this.gridview1.TProgressIndex = index;
            bitmap = this.gridview1.Rows[index].Cells[gridview1.TColumnImage].Value as Bitmap;
            if (lastIndex > -1)
            {
                this.gridview1.Rows[lastIndex].Cells[gridview1.TColumnImage].Value = last;
            }
            if (bitmap != null)
            {
                last = bitmap.Clone() as Bitmap;
            }
            else last = null;
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            var server = new SQLiteService();
            var list = server.Find<TestInfo>("1=1 limit 100");

            gridview3.DataSource = list;
            gridview2.DataSource = list;
            gridview1.DataSource = list;
        }
    }
}
