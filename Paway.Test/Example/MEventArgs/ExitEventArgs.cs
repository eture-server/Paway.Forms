using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    public class ExitEventArgs : MEventArgs
    {
        public ExitEventArgs() : base(MType.Exit) { }
    }
}
