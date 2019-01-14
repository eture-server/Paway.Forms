using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    public enum MType
    {
        [EnumTextValue("None")]
        None,
        Error,
        Wait,
        Exit,

        Update,

        Win,
        WinDelay,
    }
}
