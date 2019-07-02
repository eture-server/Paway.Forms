﻿using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    [Table(Table = "Users", Key = "Id")]
    public interface ITestInfo : IId
    {
        [Property(Column = "Pad")]
        string NewPad { get; set; }

        bool Statu { get; set; }

        UserType UserType { get; set; }

        int? Value { get; set; }

        Image Image { get; set; }

        FindInfo FindInfo { get; set; }
        List<FindInfo> List { get; }
        List<FindInfo> List2 { get; set; }
    }
    public class TestBase : IId
    {
        public long Id { get; set; }
        public virtual string Name { get; set; }
    }
    [Serializable, Table(Table = "Users", Key = "Id")]
    public class TestInfo : TestBase, ITestInfo, IFind<TestInfo>
    {
        public override string Name { get => base.Name; set => base.Name = value; }

        [Property(Column = "Pad")]
        public string NewPad { get; set; }

        public bool Statu { get; set; }

        public UserType UserType { get; set; }

        public int? Value { get; set; }

        public DateTime DateTime { get; set; }

        public Image Image { get; set; }

        public FindInfo FindInfo { get; set; }
        public List<FindInfo> List { get; } = new List<FindInfo>();
        public List<FindInfo> List2 { get; set; }

        public TestInfo()
        {
            this.DateTime = DateTime.Now;
        }

        public Func<TestInfo, bool> Find(string value)
        {
            throw new NotImplementedException();
        }
    }
}
