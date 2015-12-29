using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Paway.Helper;
using Paway.Resource;

namespace Paway.Forms
{
    /// <summary>
    ///     TComboBox+边框
    /// </summary>
    public class TComboBox : TControl
    {
        #region override

        /// <summary>
        ///     边框图片
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            switch (_mouseState)
            {
                case TMouseState.Move:
                    using (var hotLine = AssemblyHelper.GetImage("QQ.TextBox.move.png"))
                    {
                        DrawHelper.RendererBackground(g, ClientRectangle, hotLine, true);
                    }
                    break;
            }
        }

        #endregion

        #region 设计器

        private void InitializeComponent()
        {
            Edit = new TComboBoxBase();
            SuspendLayout();
            // 
            // tComboBox1
            // 
            Edit.IntegralHeight = false;
            Edit.Location = new Point(1, 1);
            Edit.Name = "Edit";
            Edit.Size = new Size(121, 23);
            Edit.TabIndex = 0;
            // 
            // TComboBox
            // 
            Controls.Add(Edit);
            Font = new Font("微软雅黑", 9F);
            Name = "TComboBox";
            Size = new Size(123, 25);
            ResumeLayout(false);
        }

        #endregion

        #region 属性

        /// <summary>
        ///     编辑控件
        /// </summary>
        [Category("Properties")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TComboBoxBase Edit { get; private set; }

        private Image _borderImage = AssemblyHelper.GetImage("QQ.TextBox.normal.png");
        private TMouseState _mouseState = TMouseState.Normal;

        /// <summary>
        ///     绘制
        /// </summary>
        [DefaultValue(typeof(TMouseState), "Normal")]
        protected virtual TMouseState MouseState
        {
            get { return _mouseState; }
            set
            {
                if (_mouseState != value)
                {
                    _mouseState = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        ///     获取或设置控件显示的文字的字体
        /// </summary>
        [Description("获取或设置控件显示的文字的字体"), DefaultValue(typeof(Font), "微软雅黑, 9pt")]
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                if (Edit == null) return;
                if (value == null)
                {
                    value = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 1);
                }
                base.Font = value;
                Edit.Font = value;
                var hight = TextRenderer.MeasureText("你好", value).Height;
                Edit.ItemHeight = hight;
                Invalidate();
            }
        }

        /// <summary>
        ///     此组件的前景色，用于显示文本
        /// </summary>
        [Description("此组件的前景色，用于显示文本。"), Category("外观"), DefaultValue(typeof(Color), "Black")]
        public override Color ForeColor
        {
            get { return Edit.ForeColor; }
            set
            {
                if (Edit == null) return;
                if (value == Color.Empty)
                {
                    value = Color.Black;
                }
                Edit.ForeColor = value;
            }
        }

        #endregion

        #region 构造

        /// <summary>
        ///     构造
        /// </summary>
        public TComboBox()
        {
            InitializeComponent();
            TConfig.Init(this);
            Edit.SizeChanged += tComboBox1_SizeChanged;
        }

        private void tComboBox1_SizeChanged(object sender, EventArgs e)
        {
            Height = Edit.Height + 2;
            Edit.Width = Width - 2;
        }

        #endregion

        #region 鼠标移动时的背影事件

        /// <summary>
        ///     背影事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            MouseMove += TComboBox2_MouseMove;
            Edit.MouseMove += TComboBox2_MouseMove;
            MouseLeave += TComboBox2_MouseLeave;
            Edit.MouseLeave += TComboBox2_MouseLeave;
            Edit.DropDownClosed += TComboBox2_MouseLeave;
        }

        private void TComboBox2_MouseLeave(object sender, EventArgs e)
        {
            MouseState = TMouseState.Leave;
        }

        private void TComboBox2_MouseMove(object sender, MouseEventArgs e)
        {
            MouseState = TMouseState.Move;
        }

        #endregion
    }
}