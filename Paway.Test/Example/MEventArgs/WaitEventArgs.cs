using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    public class WaitEventArgs : MEventArgs
    {
        public WaitEventArgs() : this(true, null) { }
        public WaitEventArgs(string msg) : this(true, msg) { }
        public WaitEventArgs(bool result, string msg) : base(MType.Wait)
        {
            this.Result = result;
            this.Message = msg;
        }
    }
}
