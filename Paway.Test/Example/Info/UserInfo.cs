using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    [Serializable]
    [Table(Table = "Users")]
    public class UserInfo : IName, IFind<UserInfo>
    {
        [Property(IShow = false)]
        public int Id { get; set; }

        [Property(Text = "用户名")]
        public string Name { get; set; }

        [Property(IShow = false)]
        public string Pad { get; set; }

        [Property(IShow = false)]
        public bool Statu { get; set; }

        [Property(IShow = false)]
        public UserType UserType { get; set; }

        [Property(Text = "最后登陆")]
        public DateTime DateTime { get; set; }

        [Property(IShow = false)]
        public DateTime CreateDate { get; set; }

        public UserInfo()
        {
            this.Statu = true;
            this.CreateDate = DateTime.Now;
        }

        public Func<UserInfo, bool> Find(string value)
        {
            throw new NotImplementedException();
        }
    }
}
