using System.Drawing;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    ///     360窗体示例
    /// </summary>
    public partial class Demo360 : _360Form
    {
        /// <summary>
        ///     构造
        /// </summary>
        public Demo360()
        {
            InitializeComponent();
            panel1.BackColor = panel2.BackColor;
            TDrawBelowBorder(panel1);
            TDrawBelowBorder(panel2);
            TMouseMove(toolBar1);
        }

        /// <summary>
        ///     底边线颜色
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (panel1.Visible)
            {
                e.Graphics.DrawLine(new Pen(panel2.BackColor), 2, Height - 2, Width - 3, Height - 2);
            }
        }
    }
}