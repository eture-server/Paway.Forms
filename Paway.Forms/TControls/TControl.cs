using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    /// 自定义基控件
    /// </summary>
    public class TControl : UserControl, IControl
    {
        /// <summary>
        /// 构造
        /// </summary>
        public TControl()
        {
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.DoubleBuffer |
               ControlStyles.SupportsTransparentBackColor, true);
            this.UpdateStyles();
            this.BackColor = Color.Transparent;
        }

        #region 属性
        /// <summary>
        /// 获取或设置控件的背景色
        /// </summary>
        [Description("获取或设置控件的背景色"), DefaultValue(typeof(Color), "Transparent")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                base.BackColor = value;
                if (value.A > _trans)
                {
                    base.BackColor = Color.FromArgb(_trans, value.R, value.G, value.B);
                }
            }
        }

        private Color _colorMoveBack = Color.Transparent;
        /// <summary>
        /// 鼠标移入状态的背景颜色
        /// 背景色为Color.White时使用默认背景
        /// </summary>
        [Description("鼠标移入状态的背景颜色,背景色为Color.Transparent时使用默认背景")]
        [DefaultValue(typeof(Color), "Transparent")]
        public Color ColorMoveBack
        {
            get { return this._colorMoveBack; }
            set
            {
                _colorMoveBack = value;
                if (value.A > _trans)
                {
                    _colorMoveBack = Color.FromArgb(_trans, value.R, value.G, value.B);
                }
                Invalidate(true);
            }
        }

        private Color _colorDownBack = Color.Transparent;
        /// <summary>
        /// 选中状态的背景颜色
        /// 背景色为Color.White时使用默认背景
        /// </summary>
        [Description("选中状态的背景颜色,背景色为Color.Transparent时使用默认背景")]
        [DefaultValue(typeof(Color), "Transparent")]
        public Color ColorDownBack
        {
            get { return this._colorDownBack; }
            set
            {
                _colorDownBack = value;
                if (value.A > _trans)
                {
                    _colorDownBack = Color.FromArgb(_trans, value.R, value.G, value.B);
                }
                Invalidate(true);
            }
        }

        private int _trans = 255;
        /// <summary>
        /// 控件透明度
        /// </summary>
        [Description("透明度"), DefaultValue(255)]
        public int Trans
        {
            get { return _trans; }
            set
            {
                if (value < 0 || value > 255)
                {
                    value = 255;
                }
                _trans = value;
            }
        }

        #endregion

        #region 接口
        /// <summary>
        /// 坐标点是否包含在项中
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool Contain(Point p) { return false; }

        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // TControl
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Name = "TControl";
            this.ResumeLayout(false);

        }
    }
}
