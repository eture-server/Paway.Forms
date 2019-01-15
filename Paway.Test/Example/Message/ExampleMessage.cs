using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    public class ExampleMessage
    {
        public string Types { get; set; }
        public MType Type
        {
            get { return EntityHelper.Parse<MType>(Types); }
        }
        public bool Result { get; set; }
        public string IpPort { get; set; }

        public ExampleMessage() { }
        public ExampleMessage(MType type)
        {
            this.Types = type.ToString();
        }
        public ExampleMessage(MType type, bool result) : this(type)
        {
            this.Result = result;
        }
        public ExampleMessage(MType type, string ipPort) : this(type)
        {
            this.IpPort = ipPort;
        }
    }
}
