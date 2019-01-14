using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    public class MEventArgs : EventArgs
    {
        [Property(IShow = false)]
        public bool Result { get; set; }

        [Property(IShow = false)]
        public MType MType { get; set; }

        [Property(IShow = false)]
        public string Message { get; set; }

        public MEventArgs() { }
        public MEventArgs(MType type)
        {
            this.MType = type;
        }
    }
}
