using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Paway.Win32;

namespace Paway.Forms
{
    /// <summary>
    /// </summary>
    [ToolboxBitmap(typeof(TextBox))]
    public class QQTextBoxBase : TextBox
    {
        #region 构造

        /// <summary>
        /// </summary>
        public QQTextBoxBase()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.Selectable |
                ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, false);
            UpdateStyles();
        }

        #endregion

        #region 方法

        /// <summary>
        ///     绘制水印
        /// </summary>
        private void WmPaintWater(ref Message m)
        {
            if (IsDisposed) return;
            using (var g = Graphics.FromHwnd(Handle))
            {
                if (Text.Length == 0 && !string.IsNullOrEmpty(_waterText))
                {
                    var flags = TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter;
                    if (RightToLeft == RightToLeft.Yes)
                    {
                        flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
                    }
                    TextRenderer.DrawText(g, _waterText, new Font("宋体", 8.5f), ClientRectangle, _waterColor, flags);
                }
            }
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == (int)WindowsMessage.WM_PAINT)
                WmPaintWater(ref m); //绘制水印
        }

        #endregion

        #region 变量

        /// <summary>
        ///     水印文字
        /// </summary>
        private string _waterText;

        /// <summary>
        ///     水印文字的颜色
        /// </summary>
        private Color _waterColor = Color.DarkGray;

        #endregion

        #region 属性

        /// <summary>
        /// </summary>
        [Description("水印文字"), Category("自定义属性")]
        [DefaultValue(null)]
        public string WaterText
        {
            get { return _waterText; }
            set
            {
                _waterText = value;
                Invalidate();
            }
        }

        /// <summary>
        /// </summary>
        [Description("水印的颜色"), Category("自定义属性")]
        [DefaultValue(typeof(Color), "DarkGray")]
        public Color WaterColor
        {
            get { return _waterColor; }
            set
            {
                _waterColor = value;
                Invalidate();
            }
        }

        #endregion
    }
}