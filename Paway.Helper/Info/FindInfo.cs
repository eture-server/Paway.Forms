using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Helper
{
    /// <summary>
    /// Loading
    /// </summary>
    [Serializable]
    public class FindInfo
    {
        /// <summary>
        /// </summary>
        [Property(IShow = false)]
        public int Id { get; set; }

        /// <summary>
        /// </summary>
        [Property(Text = "State")]
        public string State { get; set; }

        /// <summary>
        /// </summary>
        public FindInfo()
        {
            this.State = "Loading...";
        }
        /// <summary>
        /// </summary>
        public FindInfo(string name)
        {
            this.State = name;
        }
    }
}
