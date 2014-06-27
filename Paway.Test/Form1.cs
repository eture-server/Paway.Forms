using Paway.Forms;
using Paway.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Paway.Test
{
    public partial class Form1 : QQForm
    {
        public Form1()
        {
            InitializeComponent();
            toolClose.ItemClick += toolClose_ItemClick;
        }

        void toolClose_ItemClick(object sender, EventArgs e)
        {
        }
    }
}
