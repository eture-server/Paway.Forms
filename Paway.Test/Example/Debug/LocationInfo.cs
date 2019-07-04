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
        private static string xmlFile = "temp.xml";
        /// <summary>
        /// 加载本地自定义配置文件
        /// </summary>
        public static void Load()
        {
            xmlFile = AppDomain.CurrentDomain.FriendlyName.Replace("exe", "xml");
            xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xmlFile);
            if (File.Exists(xmlFile))
            {
                Config.Location = XmlHelper.Load<LocationInfo>(xmlFile);
                if (Config.Location.IUpdate != LocationInfo.VUpdate)
                {
                    Config.Location.IUpdate = LocationInfo.VUpdate;
                    Save();
                }
            }
            else
            {
                Config.Location = new LocationInfo();
                Save();
            }
        }
        /// <summary>
        /// 保存本地自定义配置文件
        /// </summary>
        public static void Save()
        {
            string name = Application.ProductName.Replace("Paway.Test.", string.Empty);
            switch (name)
            {
                case "Win":
                    XmlHelper.Save<ILocationWin>(xmlFile, Config.Location);
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
