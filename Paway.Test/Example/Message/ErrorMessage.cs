using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    public class ErrorMessage : ExampleMessage
    {
        public string FromTypes { get; set; }
        public MType FromType
        {
            get { return EntityHelper.Parse<MType>(FromTypes); }
        }
        public string Message { get; set; }

        public ErrorMessage() : base(MType.Error) { }
        public ErrorMessage(MType type, string ipPort, string msg) : base(MType.Error, ipPort)
        {
            this.FromTypes = type.ToString();
            this.Message = msg;
        }
    }
}
