using Paway.Win32;
using System.Drawing;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    ///     QQ窗体示例
    /// </summary>
    public partial class QQDemo : QQForm
    {
        /// <summary>
        ///     构造
        /// </summary>
        public QQDemo()
        {
            InitializeComponent();
            //this.TDrawBelowBorder(panel1);
            TMouseMove(panel1);
        }

        /// <summary>
        ///     关闭时激发父窗体
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (Owner != null)
            {
                Owner.Activate();
            }
            base.OnFormClosing(e);
        }

        /// <summary>
        ///     底边线颜色
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (panel1.Visible)
            {
                e.Graphics.DrawLine(new Pen(panel1.BackColor), 2, Height - 2, Width - 3, Height - 2);
            }
        }
    }
}