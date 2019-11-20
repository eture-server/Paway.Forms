using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    [Serializable]
    [Table("Users")]
    public class UserInfo : IName, IFind<UserInfo>
    {
        [NoShow]
        public int Id { get; set; }

        [Text("用户名")]
        public string Name { get; set; }

        [NoShow]
        public string Pad { get; set; }

        [NoShow]
        public bool Statu { get; set; }

        [NoShow]
        public UserType UserType { get; set; }

        [Text("最后登陆")]
        public DateTime DateTime { get; set; }

        [NoShow]
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
