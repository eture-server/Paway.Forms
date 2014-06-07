using Paway.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    /// 360窗体示例
    /// </summary>
    public partial class Demo360 : _360Form
    {
        /// <summary>
        /// 构造
        /// </summary>
        public Demo360()
        {
            InitializeComponent();
            this.panel1.BackColor = this.panel2.BackColor;
            this.panel1.Paint += panel2_Paint;
            this.toolBar1.MouseDown += toolBar1_MouseDown;
        }

        void panel2_Paint(object sender, PaintEventArgs e)
        {
            base.DrawBelowBorder(panel1);
        }

        void toolBar1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            for (int i = 0; i < this.toolBar1.Items.Count; i++)
            {
                if (this.toolBar1.Items[i].Rectangle.Contains(e.Location))
                {
                    return;
                }
            }
            if (this.toolBar1.Contain(e.Location)) return;
            if (this.WindowState != FormWindowState.Maximized)
            {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(Handle, 274, 61440 + 9, 0);
            }
        }
    }
}
