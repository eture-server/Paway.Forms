using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    /// <summary>
    /// 更新消息同步
    /// </summary>
    [Serializable]
    public class UpdateEventArgs : MEventArgs
    {
        public OperType OperType { get; set; }

        public Type ObjType { get; set; }

        public object Obj { get; set; }

        /// <summary>
        /// 更新消息同步
        /// </summary>
        public UpdateEventArgs(Object obj, OperType operType) : base(MType.Update)
        {
            this.Obj = obj;
            this.ObjType = obj.GetType();
            this.OperType = operType;
        }
    }
}
