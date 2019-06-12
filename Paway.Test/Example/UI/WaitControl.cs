using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Paway.Test
{
    public partial class WaitControl : BaseControl
    {
        public WaitControl()
        {
            InitializeComponent();
            label1.Text = Config.Loading;
        }
        protected override bool OnRefresh(MEventArgs m)
        {
            switch (m.MType)
            {
                case MType.Wait:
                    this.label1.ForeColor = m.Result ? Color.DodgerBlue : Color.Red;
                    this.label1.Text = m.Message;
                    break;
            }
            return true;
        }
    }
}
