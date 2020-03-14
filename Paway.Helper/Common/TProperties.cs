using Paway.Helper;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Paway.Helper
{
    /// <summary>
    /// 扩展属性
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TProperties : IDisposable
    {
        #region 字段与属性
        /// <summary>
        /// 文本样式
        /// </summary>
        public StringFormat StringFormat { get; private set; } = new StringFormat();

        private Color _cDown = Color.Empty;

        private Color _cMove = Color.Empty;

        private Color _cNormal = Color.Empty;

        private Font _fDown = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Point, 1);

        private Font _fMove = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Point, 1);

        private Font _fNormal = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Point, 1);

        private StringAlignment _stringHorizontal = StringAlignment.Near;

        private StringAlignment _stringVertical = StringAlignment.Center;
        private readonly MethodBase parent;

        /// <summary>
        /// 默认字体
        /// </summary>
        [Description("默认字体")]
        [DefaultValue(typeof(Font), "微软雅黑, 12pt")]
        public Font FontNormal
        {
            get
            {
                if (_fNormal == null) _fNormal = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Point, 1);
                return _fNormal;
            }
            set
            {
                if (value == null) return;
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
                OnValueChange();
            }
        }

        /// <summary>
        /// 鼠标移过时的字体
        /// </summary>
        [Description("鼠标移过时的字体")]
        [DefaultValue(typeof(Font), "微软雅黑, 12pt")]
        public Font FontMove
        {
            get
            {
                if (_fMove == null) _fMove = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Point, 1);
                return _fMove;
            }
            set
            {
                _fMove = value;
                OnValueChange();
            }
        }

        /// <summary>
        /// 鼠标按下时的字体
        /// </summary>
        [Description("鼠标按下时的字体")]
        [DefaultValue(typeof(Font), "微软雅黑, 12pt")]
        public Font FontDown
        {
            get
            {
                if (_fDown == null) _fDown = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Point, 1);
                return _fDown;
            }
            set
            {
                _fDown = value;
                OnValueChange();
            }
        }

        /// <summary>
        /// 默认颜色
        /// </summary>
        [Description("默认颜色")]
        [DefaultValue(typeof(Color), "")]
        public Color ColorNormal
        {
            get { return _cNormal; }
            set
            {
                _cNormal = value;
                if (value != Color.Empty)
                {
                    if (parent == null)
                    {
                        if (_cMove == Color.Empty) _cMove = value.AddLight(30);
                        if (_cDown == Color.Empty) _cDown = value.AddLight(-30);
                    }
                    else if (parent.Name.Contains("TLineColor"))
                    {
                        if (_cMove == Color.Empty) _cMove = value;
                        if (_cDown == Color.Empty) _cDown = value;
                    }
                }
                OnValueChange();
            }
        }

        /// <summary>
        /// 鼠标移过时的颜色
        /// </summary>
        [Description("鼠标移过时的颜色")]
        [DefaultValue(typeof(Color), "")]
        public Color ColorMove
        {
            get { return _cMove; }
            set
            {
                _cMove = value;
                OnValueChange();
            }
        }

        /// <summary>
        /// 鼠标按下时的颜色
        /// </summary>
        [Description("鼠标按下时的颜色")]
        [DefaultValue(typeof(Color), "")]
        public Color ColorDown
        {
            get { return _cDown; }
            set
            {
                _cDown = value;
                OnValueChange();
            }
        }

        /// <summary>
        /// 文字垂直对齐方式
        /// </summary>
        [Description("文字垂直对齐方式")]
        [DefaultValue(StringAlignment.Center)]
        public StringAlignment StringVertical
        {
            get { return _stringVertical; }
            set
            {
                _stringVertical = value;
                StringFormat.LineAlignment = value;
                OnValueChange();
            }
        }

        /// <summary>
        /// 文字水平对齐方式
        /// </summary>
        [Description("文字水平对齐方式")]
        [DefaultValue(StringAlignment.Near)]
        public StringAlignment StringHorizontal
        {
            get { return _stringHorizontal; }
            set
            {
                _stringHorizontal = value;
                StringFormat.Alignment = value;
                OnValueChange();
            }
        }

        /// <summary>
        /// 值修改引发事件
        /// </summary>
        public event Action<bool> ValueChange;

        #endregion

        #region public method
        /// <summary>
        /// 构造
        /// 初始化
        /// </summary>
        public TProperties(MethodBase parent = null)
        {
            this.parent = parent;
            StringFormat.Alignment = _stringHorizontal;
            StringFormat.LineAlignment = _stringVertical;
        }

        /// <summary>
        /// 清空颜色
        /// </summary>
        public void Clear()
        {
            Reset(Color.Empty);
        }

        /// <summary>
        /// 重置颜色
        /// </summary>
        public void Reset(Color color, int value = 30)
        {
            _cNormal = color;
            if (color == Color.Empty)
            {
                _cMove = Color.Empty;
                _cDown = Color.Empty;
            }
            else
            {
                _cMove = color.AddLight(value);
                _cDown = color.AddLight(-value);
            }
            OnValueChange();
        }

        #region 按状态取值
        /// <summary>
        /// 动态获取文本高度
        /// </summary>
        public int AutoHeight(TMouseState state)
        {
            return TextRenderer.MeasureText("Hello", AutoFont(state)).Height;
        }
        /// <summary>
        /// 动态获取文本高度
        /// </summary>
        public int AutoWidth(TMouseState state, string text, Size size)
        {
            return TextRenderer.MeasureText(text, AutoFont(state), size).Width;
        }
        /// <summary>
        /// 获取当前颜色
        /// </summary>
        public Color AutoColor(TMouseState state)
        {
            switch (state)
            {
                case TMouseState.Move:
                case TMouseState.Up:
                    return ColorMove;
                case TMouseState.Down:
                    return ColorDown;
                case TMouseState.Normal:
                case TMouseState.Leave:
                default:
                    return ColorNormal;
            }
        }
        /// <summary>
        /// 获取当前字体
        /// </summary>
        public Font AutoFont(TMouseState state)
        {
            switch (state)
            {
                case TMouseState.Move:
                case TMouseState.Up:
                    return FontMove;
                case TMouseState.Down:
                    return FontDown;
                case TMouseState.Normal:
                case TMouseState.Leave:
                default:
                    return FontNormal;
            }
        }
        /// <summary>
        /// 获取当前文本布局
        /// </summary>
        public TextFormatFlags TextFormat()
        {
            var text = TextFormatFlags.EndEllipsis;
            switch (StringFormat.Alignment)
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
            switch (StringFormat.LineAlignment)
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

        /// <summary>
        /// 属性值
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return null;
        }

        #endregion

        #region private method
        private void OnValueChange()
        {
            var result = _fNormal == null || _fNormal.Name != "微软雅黑" || _fNormal.Size != 12f ||
                _fNormal.Style != FontStyle.Regular || _fNormal.Unit != GraphicsUnit.Point || _fNormal.GdiCharSet != 1;
            if (!result) result = _fMove == null || _fMove.Name != "微软雅黑" || _fMove.Size != 12f ||
                  _fMove.Style != FontStyle.Regular || _fMove.Unit != GraphicsUnit.Point || _fMove.GdiCharSet != 1;
            if (!result) result = _fDown == null || _fDown.Name != "微软雅黑" || _fDown.Size != 12f ||
                  _fDown.Style != FontStyle.Regular || _fDown.Unit != GraphicsUnit.Point || _fDown.GdiCharSet != 1;
            if (!result) result = _cNormal != Color.Empty;
            if (!result) result = _cMove != Color.Empty;
            if (!result) result = _cDown != Color.Empty;
            if (!result) result = _stringHorizontal != StringAlignment.Near;
            if (!result) result = _stringVertical != StringAlignment.Near;
            ValueChange?.Invoke(result);
        }

        #endregion

        #region IDisposable
        /// <summary>
        /// 标识此对象已释放
        /// </summary>
        private bool disposed = false;
        /// <summary>
        /// 参数为true表示释放所有资源，只能由使用者调用
        /// 参数为false表示释放非托管资源，只能由垃圾回收器自动调用
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;
                if (disposing)
                {
                    // TODO: 释放托管资源(托管的对象)。
                }
                // TODO: 释放未托管资源(未托管的对象)
                if (_fDown != null)
                {
                    _fDown.Dispose();
                    _fDown = null;
                }
                if (_fMove != null)
                {
                    _fMove.Dispose();
                    _fMove = null;
                }
                if (_fNormal != null)
                {
                    _fNormal.Dispose();
                    _fNormal = null;
                }
                if (StringFormat != null)
                {
                    StringFormat.Dispose();
                    StringFormat = null;
                }
            }
        }
        /// <summary>
        /// 析构，释放非托管资源
        /// </summary>
        ~TProperties()
        {
            Dispose(false);
        }
        /// <summary>
        /// 释放资源
        /// 由类的使用者，在外部显示调用，释放类资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}