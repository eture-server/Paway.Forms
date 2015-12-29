using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    ///     扩展属性
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TProperties
    {
        private Color _cDown = Color.Empty;

        private Color _cMove = Color.Empty;

        private Color _cNormal = Color.Empty;

        private Color _cSpace = Color.Empty;

        private Font _fDown = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Point, 1);

        private Font _fMove = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Point, 1);

        private Font _fNormal = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Point, 1);

        private StringAlignment _stringHorizontal = StringAlignment.Near;

        private StringAlignment _stringVertical = StringAlignment.Near;

        /// <summary>
        ///     构造
        ///     初始化
        /// </summary>
        public TProperties()
        {
            HeightNormal = InitHeight(FontNormal);
            HeightMove = InitHeight(FontMove);
            HeightDown = InitHeight(FontDown);
            StringFormat.Alignment = _stringVertical;
            StringFormat.LineAlignment = _stringHorizontal;
            TextFormat = InitTextFormat(StringFormat);
        }

        /// <summary>
        ///     默认字体
        /// </summary>
        [Description("默认字体"), DefaultValue(typeof(Font), "微软雅黑, 12pt")]
        public Font FontNormal
        {
            get { return _fNormal; }
            set
            {
                _fNormal = value;
                if (_fMove.Name == "微软雅黑" && _fMove.Size == 12f && _fMove.Style == FontStyle.Regular &&
                    _fMove.Unit == GraphicsUnit.Point && _fMove.GdiCharSet == 1)
                {
                    _fMove = value;
                }
                if (_fDown.Name == "微软雅黑" && _fDown.Size == 12f && _fDown.Style == FontStyle.Regular &&
                    _fDown.Unit == GraphicsUnit.Point && _fDown.GdiCharSet == 1)
                {
                    _fDown = value;
                }
                HeightNormal = InitHeight(value);
                OnValueChange(value);
            }
        }

        /// <summary>
        ///     鼠标移过时的字体
        /// </summary>
        [Description("鼠标移过时的字体"), DefaultValue(typeof(Font), "微软雅黑, 12pt")]
        public Font FontMove
        {
            get { return _fMove; }
            set
            {
                _fMove = value;
                HeightMove = InitHeight(value);
            }
        }

        /// <summary>
        ///     鼠标按下时的字体
        /// </summary>
        [Description("鼠标按下时的字体"), DefaultValue(typeof(Font), "微软雅黑, 12pt")]
        public Font FontDown
        {
            get { return _fDown; }
            set
            {
                _fDown = value;
                HeightDown = InitHeight(value);
            }
        }

        /// <summary>
        ///     默认颜色
        /// </summary>
        [Description("默认颜色"), DefaultValue(typeof(Color), "")]
        public Color ColorNormal
        {
            get { return _cNormal; }
            set
            {
                _cNormal = value;
                OnValueChange(value);
            }
        }

        /// <summary>
        ///     鼠标移过时的颜色
        /// </summary>
        [Description("鼠标移过时的颜色"), DefaultValue(typeof(Color), "")]
        public Color ColorMove
        {
            get { return _cMove; }
            set
            {
                _cMove = value;
                OnValueChange(value);
            }
        }

        /// <summary>
        ///     鼠标按下时的颜色
        /// </summary>
        [Description("鼠标按下时的颜色"), DefaultValue(typeof(Color), "")]
        public Color ColorDown
        {
            get { return _cDown; }
            set
            {
                _cDown = value;
                OnValueChange(value);
            }
        }

        /// <summary>
        ///     项间隔的颜色
        /// </summary>
        [Description("项间隔的颜色"), DefaultValue(typeof(Color), "")]
        public Color ColorSpace
        {
            get { return _cSpace; }
            set
            {
                _cSpace = value;
                OnValueChange(value);
            }
        }

        /// <summary>
        ///     文字水平对齐
        /// </summary>
        [Description("文字水平对齐"), DefaultValue(typeof(StringAlignment), "Near")]
        public StringAlignment StringVertical
        {
            get { return _stringVertical; }
            set
            {
                _stringVertical = value;
                StringFormat.Alignment = value;
                TextFormat = InitTextFormat(StringFormat);
                OnValueChange(value);
            }
        }

        /// <summary>
        ///     文字垂直对齐
        /// </summary>
        [Description("文字垂直对齐"), DefaultValue(typeof(StringAlignment), "Near")]
        public StringAlignment StringHorizontal
        {
            get { return _stringHorizontal; }
            set
            {
                _stringHorizontal = value;
                StringFormat.LineAlignment = value;
                TextFormat = InitTextFormat(StringFormat);
                OnValueChange(value);
            }
        }

        /// <summary>
        ///     值修改引发事件
        /// </summary>
        public event EventHandler ValueChange;

        private void OnValueChange(object value)
        {
            if (ValueChange != null)
            {
                ValueChange(value, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     属性值
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return null;
        }

        #region 内部初始化字体单行高度

        internal int HeightNormal;
        internal int HeightMove;
        internal int HeightDown;

        private int InitHeight(Font font)
        {
            return TextRenderer.MeasureText("你好", font).Height;
        }

        #endregion

        #region 内部初始化文本布局

        internal StringFormat StringFormat = new StringFormat();
        internal TextFormatFlags TextFormat;

        private TextFormatFlags InitTextFormat(StringFormat format)
        {
            var text = TextFormatFlags.EndEllipsis;
            switch (format.Alignment)
            {
                case StringAlignment.Near:
                    text |= TextFormatFlags.Left;
                    break;
                case StringAlignment.Center:
                    text |= TextFormatFlags.HorizontalCenter;
                    break;
                case StringAlignment.Far:
                    text |= TextFormatFlags.Right;
                    break;
            }
            switch (format.LineAlignment)
            {
                case StringAlignment.Near:
                    text |= TextFormatFlags.Top;
                    break;
                case StringAlignment.Center:
                    text |= TextFormatFlags.VerticalCenter;
                    break;
                case StringAlignment.Far:
                    text |= TextFormatFlags.Bottom;
                    break;
            }
            return text;
        }

        #endregion
    }
}