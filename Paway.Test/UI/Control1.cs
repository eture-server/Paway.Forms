using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Paway.Forms;

namespace Paway.Test.UI
{
    public partial class Control1 : MControl
    {
        public Control1()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            OnChanged(this, e);
        }
        public override void ReLoad()
        {
            base.ReLoad();
            toolbar.MStart();
        }
    }
}
