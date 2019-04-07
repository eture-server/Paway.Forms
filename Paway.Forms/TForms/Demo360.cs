using System.Drawing;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    ///     360窗体示例
    /// </summary>
    public partial class Demo360 : Form360
    {
        /// <summary>
        ///     构造
        /// </summary>
        public Demo360()
        {
            InitializeComponent();
            TDrawBelowBorder(panel1);
            TDrawBelowBorder(panel2);
            TMouseMove(toolBar1);
        }
    }
}