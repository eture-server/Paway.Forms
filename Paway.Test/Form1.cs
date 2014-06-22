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
            toolClose.EditClick += toolClose_EditClick;
        }

        void toolClose_EditClick(object sender, EventArgs e)
        {
        }
    }
}
