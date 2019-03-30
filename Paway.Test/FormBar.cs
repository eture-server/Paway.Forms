using Paway.Forms;
using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Paway.Test
{
    public partial class FormBar : Form
    {
        public FormBar()
        {
            InitializeComponent();
            toolBar2.SelectedItemChanged += ToolBar2_ItemClick;
        }
        private void ToolBar2_ItemClick(ToolItem item, EventArgs e)
        {
            ExceptionHelper.Show(item.Text);
            if (item.First == "0") toolBar2.TClickItem(-1);
        }
    }
}
