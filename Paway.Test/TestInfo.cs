using Paway.Helper;
using Paway.Test.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    [Table("Users")]
    public interface ITestInfo : IId
    {
        int ParentId { get; set; }

        [Column("Pad")]
        string NewPad { get; set; }

        bool Statu { get; set; }

        UserType UserType { get; set; }

        int? Value { get; set; }

        Image Image { get; set; }

        FindInfo FindInfo { get; set; }
        List<FindInfo> List { get; }
        List<FindInfo> List2 { get; set; }
    }
    public class TestBase : IParent
    {
        [NoShow]
        public int Id { get; set; }
        [NoShow]
        public int ParentId { get; set; }
        public virtual string Name { get; set; }
    }
    [Serializable, Table("Users")]
    public class TestInfo : TestBase, ITestInfo, IFind<TestInfo>
    {
        [Text("名称")]
        public override string Name { get => base.Name; set => base.Name = value; }

        [Column("Pad")]
        [IButton(nameof(Images))]
        public string NewPad { get; set; }
        [NoSelect]
        [IButton]
        public Image Images { get { return Resources.close; } }

        [ICheckBox]
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
            return c => (c.Name != null && c.Name.IndexOf(value, StringComparison.OrdinalIgnoreCase) != -1);
        }
    }
}
