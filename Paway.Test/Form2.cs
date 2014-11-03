using Paway.Forms;
using Paway.Test.UI;
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
    public partial class Form2 : Demo360
    {
        public Form2()
        {
            InitializeComponent();
            toolBar1.SelectedItemChanged += toolBar1_SelectedItemChanged;
        }

        void toolBar1_SelectedItemChanged(object sender, EventArgs e)
        {
            ToolItem item = sender as ToolItem;
            MControl.ReSet(panel3);
            switch (item.Text)
            {
                case "左":
                    MControl control = MControl.ReLoad(panel3, typeof(Control1), TMDirection.Left);
                    break;
                case "右":
                    control = MControl.ReLoad(panel3, typeof(Control1), TMDirection.Right);
                    break;
                case "上":
                    control = MControl.ReLoad(panel3, typeof(Control2), TMDirection.Up);
                    break;
                case "下":
                    control = MControl.ReLoad(panel3, typeof(Control3), TMDirection.Transparent);
                    break;
                case "中":
                    control = MControl.ReLoad(panel3, typeof(Control1), TMDirection.Transparent);
                    break;
                case "色":
                    control = MControl.ReLoad(panel3, typeof(Control2), TMDirection.Transparent);
                    //control = MControl.ReLoad(panel3, typeof(TAlpha));
                    break;
            }
        }
    }
}
