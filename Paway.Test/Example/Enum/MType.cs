using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    public enum MType
    {
        [Description(TConfig.None)]
        None,
        Error,
        Wait,
        Exit,

        Update,

        Win,
        WinDelay,
    }
}
