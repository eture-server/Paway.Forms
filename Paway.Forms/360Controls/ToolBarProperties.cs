using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Paway.Forms
{
    /// <summary>
    /// ToolBar属性分类
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ToolBarProperties
    {
        private TProperties _text;
        /// <summary>
        /// 文字
        /// </summary>
        [DefaultValue(typeof(TProperties), "Properties")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public TProperties Text
        {
            get
            {
                if (_text == null)
                    _text = new TProperties();
                return _text;
            }
            set { _text = value; }
        }
        private TProperties _textSencond;
        /// <summary>
        /// 文字
        /// </summary>
        [DefaultValue(typeof(TProperties), "Properties")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public TProperties TextSencond
        {
            get
            {
                if (_textSencond == null)
                    _textSencond = new TProperties();
                return _textSencond;
            }
            set { _textSencond = value; }
        }
        private TProperties _desc;
        /// <summary>
        /// 正文描述
        /// </summary>
        [DefaultValue(typeof(TProperties), "Properties")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public TProperties Desc
        {
            get
            {
                if (_desc == null)
                    _desc = new TProperties();
                return _desc;
            }
            set { _desc = value; }
        }
        private TProperties _headDesc;
        /// <summary>
        /// 头部描述
        /// </summary>
        [DefaultValue(typeof(TProperties), "Properties")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public TProperties HeadDesc
        {
            get
            {
                if (_headDesc == null)
                    _headDesc = new TProperties();
                return _headDesc;
            }
            set { _headDesc = value; }
        }
        private TProperties _endDesc;
        /// <summary>
        /// 尾部描述
        /// </summary>
        [DefaultValue(typeof(TProperties), "Properties")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public TProperties EndDesc
        {
            get
            {
                if (_endDesc == null)
                    _endDesc = new TProperties();
                return _endDesc;
            }
            set { _endDesc = value; }
        }
        private TProperties _backGround;
        /// <summary>
        /// 背景
        /// </summary>
        [DefaultValue(typeof(TProperties), "Properties")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public TProperties BackGround
        {
            get
            {
                if (_backGround == null)
                    _backGround = new TProperties();
                return _backGround;
            }
            set { _backGround = value; }
        }

        /// <summary>
        /// 属性值
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "ToolBar.Properties";
        }
    }
}
