using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Paway.Helper;
using Paway.Win32;

namespace Paway.Forms
{
    /// <summary>
    /// 数值编辑:TextBox
    /// design by: csust.hulihui(mailto: ehulh@163.com, home at: http://blog.csdn.net/hulihui)
    /// creation date: 2007-9-17
    /// modified date: 2008-9-29
    /// 1) ContextMenu delete operator is not captueed
    /// 2) add property AllowNegative
    /// 3) call base.OnTextChanged when mouse Paste/Cut/Clear
    /// 4) bug when cut negative number
    /// 5) bug when ReadOnly changed, override property ReadOnly.(2008-9-29)
    /// 6) bug when input 1 0 and 1 but displays 11.(2008-10-23)
    /// 7) use culture number format. (2008-10-23)
    /// 8) do not clear value when input minus. (2008-11-14)
    /// </summary>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(TextBox))]
    public class TNumTestBox : QQTextBox
    {
        #region  member fields
        private const int m_MaxDecimalLength = 10; // max dot length
        private const int m_MaxValueLength = 27; // decimal can be 28 bits.

        private int m_decimalLength;
        private bool m_allowNegative = true;
        private string m_valueFormatStr = string.Empty;

        private readonly char m_decimalSeparator = '.';
        private readonly char m_negativeSign = '-';

        #endregion

        #region 属性
        /// <summary>
        /// </summary>
        [DefaultValue("0")]
        public override string Text
        {
            get { return base.Text; }
            set
            {
                if (decimal.TryParse(value, out decimal val))
                {
                    base.Text = val.ToString(m_valueFormatStr);
                }
                else
                {
                    base.Text = 0.ToString(m_valueFormatStr);
                }
            }
        }
        /// <summary>
        /// 关闭多行编辑
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DefaultValue(false)]
        public override bool Multiline
        {
            get { return base.Multiline; }
            set { base.Multiline = false; }
        }/// <summary>
         /// </summary>
        [Category("Custom")]
        [Description("Set/Get dot length(0 is integer, 10 is maximum).")]
        [DefaultValue(0)]
        public int DecimalLength
        {
            get { return m_decimalLength; }
            set
            {
                if (m_decimalLength != value)
                {
                    if (value < 0 || value > m_MaxDecimalLength)
                    {
                        m_decimalLength = 0;
                    }
                    else
                    {
                        m_decimalLength = value;
                    }
                    SetValueFormatStr();
                    base.Text = Value.ToString(m_valueFormatStr);
                }
            }
        }

        /// <summary>
        /// </summary>
        [Category("Custom")]
        [Description("Get decimal value of textbox.")]
        [DefaultValue(0)]
        public decimal Value
        {
            get
            {
                if (decimal.TryParse(base.Text, out decimal val))
                {
                    return val;
                }
                return 0;
            }
        }

        /// <summary>
        /// </summary>
        [Category("Custom")]
        [Description("Get integer value of textbox.")]
        public int IntValue
        {
            get
            {
                var val = Value;
                return (int)val;
            }
        }

        /// <summary>
        /// </summary>
        [Category("Custom")]
        [Description("Number can be negative or not.")]
        [DefaultValue(true)]
        public bool AllowNegative
        {
            get { return m_allowNegative; }
            set
            {
                if (m_allowNegative != value)
                {
                    m_allowNegative = value;
                }
            }
        }

        #endregion

        #region 隐藏正则属性
        /// <summary>
        /// </summary>
        [Browsable(false)]
        public override string Regex
        {
            get { return null; }
        }

        /// <summary>
        /// </summary>
        [Browsable(false)]
        public override RegexType RegexType
        {
            get { return RegexType.None; }
        }

        #endregion

        #region 构造
        /// <summary>
        /// </summary>
        public TNumTestBox()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

            base.Text = "0";
            MaxLength = 10;

            var ci = Thread.CurrentThread.CurrentCulture;
            m_decimalSeparator = ci.NumberFormat.CurrencyDecimalSeparator[0];
            m_negativeSign = ci.NumberFormat.NegativeSign[0];

            SetValueFormatStr();
        }

        #endregion

        #region 重绘
        /// <summary>
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)WindowsMessage.WM_PASTE) // mouse paste
            {
                ClearSelection();
                SendKeys.Send(Clipboard.GetText());
                OnTextChanged(EventArgs.Empty);
            }
            else if (m.Msg == (int)WindowsMessage.WM_COPY) // mouse copy
            {
                Clipboard.SetText(SelectedText);
            }
            else if (m.Msg == (int)WindowsMessage.WM_CUT) // mouse cut or ctrl+x shortcut
            {
                Clipboard.SetText(SelectedText);
                ClearSelection();
                OnTextChanged(EventArgs.Empty);
            }
            else if (m.Msg == (int)WindowsMessage.WM_CLEAR)
            {
                ClearSelection();
                OnTextChanged(EventArgs.Empty);
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        /// <summary>
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys)Shortcut.CtrlV)
            {
                ClearSelection();

                var text = Clipboard.GetText();
                //                SendKeys.Send(text);

                for (var k = 0; k < text.Length; k++) // can not use SendKeys.Send
                {
                    SendCharKey(text[k]);
                }
                return true;
            }
            if (keyData == (Keys)Shortcut.CtrlC)
            {
                Clipboard.SetText(SelectedText);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (!ReadOnly)
            {
                if (e.KeyData == Keys.Delete || e.KeyData == Keys.Back)
                {
                    if (SelectionLength > 0)
                    {
                        ClearSelection();
                    }
                    else
                    {
                        DeleteText(e.KeyData);
                    }
                    e.SuppressKeyPress = true; // does not transform event to KeyPress, but to KeyUp
                }
            }
        }

        /// <summary>
        /// repostion SelectionStart, recalculate SelectedLength
        /// </summary>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if (ReadOnly)
            {
                return;
            }

            if (e.KeyChar == (char)13 || e.KeyChar == (char)3 || e.KeyChar == (char)22 || e.KeyChar == (char)24)
            {
                return;
            }

            if (m_decimalLength == 0 && e.KeyChar == m_decimalSeparator)
            {
                e.Handled = true;
                return;
            }

            if (!m_allowNegative && e.KeyChar == m_negativeSign && base.Text.IndexOf(m_negativeSign) < 0)
            {
                e.Handled = true;
                return;
            }

            if (!char.IsDigit(e.KeyChar) && e.KeyChar != m_negativeSign && e.KeyChar != m_decimalSeparator)
            {
                e.Handled = true;
                return;
            }

            if (base.Text.Length >= m_MaxValueLength && e.KeyChar != m_negativeSign)
            {
                e.Handled = true;
                return;
            }

            if (e.KeyChar == m_decimalSeparator || e.KeyChar == m_negativeSign) // will position after dot(.) or first
            {
                SelectionLength = 0;
            }

            var isNegative = base.Text[0] == m_negativeSign ? true : false;

            if (isNegative && SelectionStart == 0)
            {
                SelectionStart = 1;
            }

            if (e.KeyChar == m_negativeSign)
            {
                var selStart = SelectionStart;

                if (!isNegative)
                {
                    base.Text = m_negativeSign + base.Text;
                    SelectionStart = selStart + 1;
                }
                else
                {
                    base.Text = base.Text.Substring(1, base.Text.Length - 1);
                    if (selStart >= 1)
                    {
                        SelectionStart = selStart - 1;
                    }
                    else
                    {
                        SelectionStart = 0;
                    }
                }
                e.Handled = true; // minus(-) has been handled
                return;
            }

            var dotPos = base.Text.IndexOf(m_decimalSeparator) + 1;

            if (e.KeyChar == m_decimalSeparator)
            {
                if (dotPos > 0)
                {
                    SelectionStart = dotPos;
                }
                e.Handled = true; // dot has been handled 
                return;
            }

            if (base.Text == "0")
            {
                BaseText.SelectAll(); //清除默认0
                //this.SelectionStart = 0;
                //this.SelectionStart = 1;  // replace thre first char, ie. 0
            }
            else if (base.Text == m_negativeSign + "0")
            {
                SelectionStart = 1;
                SelectionLength = 1; // replace thre first char, ie. 0
            }
            else if (m_decimalLength > 0)
            {
                if (base.Text[0] == '0' && dotPos == 2 && SelectionStart <= 1)
                {
                    SelectionStart = 0;
                    SelectionLength = 1; // replace thre first char, ie. 0
                }
                else if (base.Text.Substring(0, 2) == m_negativeSign + "0" && dotPos == 3 && SelectionStart <= 2)
                {
                    SelectionStart = 1;
                    SelectionLength = 1; // replace thre first char, ie. 0
                }
                else if (SelectionStart == dotPos + m_decimalLength)
                {
                    SelectionStart -= 1;
                    SelectionLength = 1; // last position after text
                }
                else if (SelectionStart >= dotPos)
                {
                    SelectionLength = 1;
                }
                else if (SelectionLength == dotPos + m_decimalLength)
                {
                    ClearSelection();
                    SelectionStart = 0;
                    SelectionLength = 1;
                }
            }
        }

        /// <summary>
        /// reformat the base.Text
        /// </summary>
        protected override void OnLeave(EventArgs e)
        {
            if (string.IsNullOrEmpty(base.Text))
            {
                base.Text = 0.ToString(m_valueFormatStr);
            }
            else
            {
                base.Text = Value.ToString(m_valueFormatStr);
            }
            base.OnLeave(e);
        }

        #endregion

        #region private methods
        private void SetValueFormatStr()
        {
            m_valueFormatStr = "F" + m_decimalLength;
        }

        private void SendCharKey(char c)
        {
            var msg = new Message();
            if (!IsDisposed)
            {
                msg.HWnd = Handle;
                msg.Msg = (int)WindowsMessage.WM_CHAR;
                msg.WParam = (IntPtr)c;
                msg.LParam = IntPtr.Zero;
            }

            base.WndProc(ref msg);
        }

        /// <summary>
        /// Delete operator will be changed to BackSpace in order to
        /// uniformly handle the position of deleted digit.
        /// </summary>
        private void DeleteText(Keys key)
        {
            var selStart = SelectionStart; // base.Text will be delete at selStart - 1

            if (key == Keys.Delete) // Delete key change to BackSpace key, adjust selStart value
            {
                selStart += 1; // adjust position for BackSpace
                if (selStart > base.Text.Length) // text end
                {
                    return;
                }

                if (IsSeparator(selStart - 1)) // next if delete dot(.) or thousands(;)
                {
                    selStart++;
                }
            }
            else // BackSpace key
            {
                if (selStart == 0) // first position
                {
                    return;
                }

                if (IsSeparator(selStart - 1)) // char which will be delete is separator
                {
                    selStart--;
                }
            }

            if (selStart == 0 || selStart > base.Text.Length) // selStart - 1 no digig
            {
                return;
            }

            var dotPos = base.Text.IndexOf(m_decimalSeparator);
            var isNegative = base.Text.IndexOf(m_negativeSign) > -1 ? true : false;

            if (selStart > dotPos && dotPos > -1) // delete digit after dot(.)
            {
                base.Text = base.Text.Substring(0, selStart - 1) +
                            base.Text.Substring(selStart, base.Text.Length - selStart) + "0";
                SelectionStart = selStart - 1; // SelectionStart is unchanged
            }
            else // delete digit before dot(.)
            {
                if (selStart == 1 && isNegative) // delete 1st digit and Text is negative,ie. delete minus(-)
                {
                    if (base.Text.Length == 1) // ie. base.Text is '-'
                    {
                        base.Text = "0";
                    }
                    else if (dotPos == 1) // -.* format
                    {
                        base.Text = "0" + base.Text.Substring(1, base.Text.Length - 1);
                    }
                    else
                    {
                        base.Text = base.Text.Substring(1, base.Text.Length - 1);
                    }
                    SelectionStart = 0;
                }
                else if (selStart == 1 && (dotPos == 1 || base.Text.Length == 1))
                // delete 1st digit before dot(.) or Text.Length = 1
                {
                    base.Text = "0" + base.Text.Substring(1, base.Text.Length - 1);
                    SelectionStart = 1;
                }
                else if (isNegative && selStart == 2 && base.Text.Length == 2) // -* format
                {
                    base.Text = m_negativeSign + "0";
                    SelectionStart = 1;
                }
                else if (isNegative && selStart == 2 && dotPos == 2) // -*.* format
                {
                    base.Text = m_negativeSign + "0" + base.Text.Substring(2, base.Text.Length - 2);
                    SelectionStart = 1;
                }
                else // selStart > 0
                {
                    base.Text = base.Text.Substring(0, selStart - 1) +
                                base.Text.Substring(selStart, base.Text.Length - selStart);
                    SelectionStart = selStart - 1;
                }
            }
        }

        /// <summary>
        /// clear base.SelectedText
        /// </summary>
        private void ClearSelection()
        {
            if (SelectionLength == 0)
            {
                return;
            }

            if (SelectedText.Length == base.Text.Length)
            {
                base.Text = 0.ToString(m_valueFormatStr);
                return;
            }

            var selLength = SelectedText.Length;
            if (SelectedText.IndexOf(m_decimalSeparator) > -1)
            {
                selLength--; // selected text contains dot(.), selected length minus 1
            }

            SelectionStart += SelectedText.Length; // after selected text
            SelectionLength = 0;

            for (var k = 1; k <= selLength; k++)
            {
                DeleteText(Keys.Back);
            }
        }

        private bool IsSeparator(int index)
        {
            return IsSeparator(base.Text[index]);
        }

        private bool IsSeparator(char c)
        {
            if (c == m_decimalSeparator)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}