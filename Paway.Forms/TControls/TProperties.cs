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
    /// 扩展属性
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TProperties
    {
        private Font _fNormal = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)1);
        /// <summary>
        /// 默认字体
        /// </summary>
        [Description("默认字体"), DefaultValue(typeof(Font), "宋体, 9pt")]
        public Font FontNormal
        {
            get { return _fNormal; }
            set { _fNormal = value; }
        }

        private Font _fMove = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)1);
        /// <summary>
        /// 鼠标移过时的字体
        /// </summary>
        [Description("鼠标移过时的字体"), DefaultValue(typeof(Font), "宋体, 9pt")]
        public Font FontMove
        {
            get { return _fMove; }
            set { _fMove = value; }
        }

        private Font _fDown = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)1);
        /// <summary>
        /// 鼠标按下时的字体
        /// </summary>
        [Description("鼠标按下时的字体"), DefaultValue(typeof(Font), "宋体, 9pt")]
        public Font FontDown
        {
            get { return _fDown; }
            set { _fDown = value; }
        }

        private Color _cNormal = Color.Empty;
        /// <summary>
        /// 默认颜色
        /// </summary>
        [Description("默认颜色"), DefaultValue(typeof(Color), "Empty")]
        public Color ColorNormal
        {
            get { return _cNormal; }
            set { _cNormal = value; }
        }

        private Color _cMove = Color.Empty;
        /// <summary>
        /// 鼠标移过时的颜色
        /// </summary>
        [Description("鼠标移过时的颜色"), DefaultValue(typeof(Color), "Empty")]
        public Color ColorMove
        {
            get { return _cMove; }
            set { _cMove = value; }
        }

        private Color _cDown = Color.Empty;
        /// <summary>
        /// 鼠标按下时的颜色
        /// </summary>
        [Description("鼠标按下时的颜色"), DefaultValue(typeof(Color), "Empty")]
        public Color ColorDown
        {
            get { return _cDown; }
            set { _cDown = value; }
        }

        private Color _cSpace = Color.Empty;
        /// <summary>
        /// 项间隔的颜色
        /// </summary>
        [Description("项间隔的颜色"), DefaultValue(typeof(Color), "Empty")]
        public Color ColorSpace
        {
            get { return _cSpace; }
            set { _cSpace = value; }
        }

        private StringAlignment _stringVertical = StringAlignment.Center;
        /// <summary>
        /// 文字水平对齐
        /// </summary>
        [Description("文字水平对齐"), DefaultValue(typeof(StringAlignment), "Center")]
        public StringAlignment StringVertical
        {
            get { return _stringVertical; }
            set { _stringVertical = value; }
        }

        private StringAlignment _stringHorizontal = StringAlignment.Center;
        /// <summary>
        /// 文字垂直对齐
        /// </summary>
        [Description("文字垂直对齐"), DefaultValue(typeof(StringAlignment), "Center")]
        public StringAlignment StringHorizontal
        {
            get { return _stringHorizontal; }
            set { _stringHorizontal = value; }
        }

        /// <summary>
        /// 属性值
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return null;
        }
    }
}
