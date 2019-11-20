using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    /// <summary>
    /// 管理数据
    /// </summary>
    [Serializable]
    public class AdminInfo
    {
        public string Name { get; set; }
        public int Version { get; set; }
        public string MacId { get; set; }
    }
    /// <summary>
    /// 管理数据结构
    /// </summary>
    [Serializable]
    [Table("Admins")]
    public class AdminBaseInfo : IInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public DateTime DateTime { get; set; }

        public AdminBaseInfo()
        {
            this.DateTime = DateTime.Now;
        }
    }
}
