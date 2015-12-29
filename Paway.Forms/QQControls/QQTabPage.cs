using System.ComponentModel;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    /// </summary>
    public class QQTabPage : TabPage
    {
        private readonly Label MenuLabel = new Label();

        /// <summary>
        ///     用于展示右键菜单
        /// </summary>
        [Category("行为"), Description("右键菜单承接按钮。")]
        public ContextMenuStrip ContextMenuShow
        {
            get { return MenuLabel.ContextMenuStrip; }
            set { MenuLabel.ContextMenuStrip = value; }
        }
    }
}