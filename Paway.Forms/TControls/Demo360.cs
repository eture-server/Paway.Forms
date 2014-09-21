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
            this.TDrawBelowBorder(panel1);
            this.TMouseMove(toolBar1);
        }
        /// <summary>
        /// 底边线颜色
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (panel1.Visible)
            {
                e.Graphics.DrawLine(new Pen(panel2.BackColor), 2, this.Height - 2, this.Width - 3, this.Height - 2);
            }
        }
    }
}
