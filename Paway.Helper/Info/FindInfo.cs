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
    public class FindInfo : IParent
    {
        /// <summary>
        /// </summary>
        [Property(IShow = false)]
        public int Id { get; set; }
        /// <summary>
        /// </summary>
        [Property(IShow = false)]
        public int ParentId { get; set; }

        /// <summary>
        /// </summary>
        public string AutoStates { get; set; }

        /// <summary>
        /// </summary>
        public FindInfo()
        {
            this.Id = 1;
            this.AutoStates = "Loading...";
        }
        /// <summary>
        /// </summary>
        public FindInfo(string name)
        {
            this.Id = 1;
            this.AutoStates = name;
        }
    }
}
