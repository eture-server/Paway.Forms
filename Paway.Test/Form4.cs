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
    public partial class Form4 : QQForm
    {
        private readonly Timer timer = new Timer();
        public Form4()
        {
            InitializeComponent();
            timer.Interval = 10;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Enabled = true;
            btnReset.Click += btnReset_Click;
        }

        void btnReset_Click(object sender, EventArgs e)
        {
            this.tProgressBar1.Value = 0;
            new Action(Hello).BeginInvoke(null, null);
        }
        private void Hello()
        {
            System.Threading.Thread.Sleep(2000);
            ExceptionHelper.Show("Hello");
        }
        void timer_Tick(object sender, EventArgs e)
        {
            if (this.tProgressBar1.Value < 100) { this.tProgressBar1.Value++; }
        }
    }
}
