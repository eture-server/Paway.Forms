using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Paway.Test
{
    /// <summary>
    /// 本地保存数据
    /// </summary>
    [Serializable]
    public class LocationInfo : ILocationWin
    {
        #region IUpdate
        public const int VUpdate = 0;
        public int IUpdate { get; set; }

        #endregion

        #region IWin

        #endregion

        #region Public
        /// <summary>
        /// 保存本地自定义配置文件
        /// </summary>
        public void Save()
        {
            var xmlFile = AppDomain.CurrentDomain.FriendlyName.Replace(".vshost", string.Empty).Replace("exe", "xml");
            xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xmlFile);
            switch (Application.ProductName)
            {
                case "Paway.Test.Win":
                    XmlHelper.Save<ILocationWin>(this, xmlFile);
                    break;
            }
        }

        #endregion
    }
    public interface ILocationWin : IUpdate { }
    public interface IUpdate
    {
        [Description("自动更新")]
        int IUpdate { get; set; }
    }
}
