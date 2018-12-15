using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Paway.Forms;
using Paway.Helper;

namespace Paway.Test
{
    public partial class Control2 : MControl
    {
        public Control2()
        {
            InitializeComponent();
        }
        public override void ReLoad()
        {
            base.ReLoad();
            toolLeft.MStart(TMDirection.Left);
            toolRight.MStart(TMDirection.Right);
            toolUp.MStart(TMDirection.Up);
            toolDown.MStart(TMDirection.Down);
            toolCenter.MStart(TMDirection.Center);
            toolTran.MStart(TMDirection.Transparent);
        }
    }
}
