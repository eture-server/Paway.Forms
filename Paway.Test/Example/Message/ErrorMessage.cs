using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    public class ErrorMessage : ExampleMessage
    {
        public string From { get; set; }
        public string Message { get; set; }

        public ErrorMessage() : base(nameof(MType.Error)) { }
        public ErrorMessage(string type, string ipPort, string msg) : base(nameof(MType.Error), ipPort)
        {
            this.From = type;
            this.Message = msg;
        }
    }
}
