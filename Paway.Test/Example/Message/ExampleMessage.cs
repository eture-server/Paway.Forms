using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    public class ExampleMessage
    {
        public string Type { get; set; }
        public bool Result { get; set; }
        public string IpPort { get; set; }

        public ExampleMessage() { }
        public ExampleMessage(string type)
        {
            this.Type = type;
        }
        public ExampleMessage(string type, bool result)
        {
            this.Type = type;
            this.Result = result;
        }
        public ExampleMessage(string type, string ipPort)
        {
            this.Type = type;
            this.IpPort = ipPort;
        }
    }
}
