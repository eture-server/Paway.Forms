using Paway.Forms;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            toolBar1.SelectedItemChanged += toolBar1_SelectedItemChanged;
        }

        void toolBar1_SelectedItemChanged(object sender, EventArgs e)
        {
            this.Text = toolBar1.SelectedItem.Text;
        }
    }
}
