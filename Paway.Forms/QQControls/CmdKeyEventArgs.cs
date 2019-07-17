using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    /// 键盘消息
    /// </summary>
    public class CmdKeyEventArgs : EventArgs
    {
        /// <summary>
        /// 事件类型构造
        /// </summary>
        public CmdKeyEventArgs(Message msg, Keys key)
        {
            Message = msg;
            KeyCode = key;
        }
        /// <summary>
        /// 消息
        /// </summary>
        public Message Message { get; set; }

        /// <summary>
        /// 按键
        /// </summary>
        public Keys KeyCode { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        public bool Result { get; set; }
    }
}
