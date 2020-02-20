using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Paway.Helper;
using Paway.Forms;
using System.Collections;

namespace Paway.Forms
{
    /// <summary>
    /// 搜索(树结构)
    /// </summary>
    public partial class TQueryTree<T> : TQuery<T> where T : IParent, IFind<T>
    {
        /// <summary>
        /// 构造
        /// </summary>
        public TQueryTree()
        {
            this.Controls.Remove(this.gridview1);
            this.gridview1 = new TreeGridView();
            this.gridview1.Dock = DockStyle.Fill;
            this.gridview1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridview1.ColumnHeadersVisible = false;
            this.Controls.Add(this.gridview1);
            this.Controls.SetChildIndex(this.gridview1, 0);
        }
    }
}
