using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    public class MEventArgs : EventArgs
    {
        [NoShow]
        public bool Result { get; set; }

        [NoShow]
        public MType MType { get; set; }

        [NoShow]
        public string Message { get; set; }

        public MEventArgs() { }
        public MEventArgs(MType type)
        {
            this.MType = type;
        }
    }
}
