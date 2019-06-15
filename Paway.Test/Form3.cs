﻿using Paway.Forms;
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
using System.Threading;

namespace Paway.Test
{
    public partial class Form3 : QQForm
    {
        public Form3()
        {
            InitializeComponent();
            btChange.Click += btChange_Click;
            toolBar1.MoveFinished += toolBar1_MoveFinished;
        }

        void toolBar1_MoveFinished(object sender, EventArgs e)
        {
            toolBar1.NormalImage = iChange ? Resources.noon : Resources.i1;
            toolBar1.MoveImage = iChange ? Resources.noon : Resources.i1;
            toolBar1.DownImage = iChange ? Resources.noon : Resources.i1;
        }

        private bool iChange;
        void btChange_Click(object sender, EventArgs e)
        {
            iChange = !iChange;
            toolBar1.NormalImage = null;
            toolBar1.TranImage = iChange ? Resources.i1 : Resources.noon;
            toolBar1.TranLaterImage = iChange ? Resources.noon : Resources.i1;
            toolBar1.MStart();
            using (new Progress())
            {
                Thread.Sleep(2000);
            }
        }
    }
}
