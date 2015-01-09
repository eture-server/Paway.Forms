using Paway.Forms;
using Paway.Test.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Paway.Helper;

namespace Paway.Test
{
    public partial class Form3 : QQForm
    {
        public Form3()
        {
            InitializeComponent();
            btChange.Click += btChange_Click;
        }

        private bool iChange;
        void btChange_Click(object sender, EventArgs e)
        {
            toolBar1.NormalImage = iChange ? Resources.noon : Resources.i1;
            toolBar1.MoveImage = iChange ? Resources.noon : Resources.i1;
            toolBar1.DownImage = iChange ? Resources.noon : Resources.i1;
            toolBar1.MStart();
            iChange = !iChange;
        }
    }
}
