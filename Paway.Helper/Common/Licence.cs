using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Paway.Helper
{
    /// <summary>
    /// 确定是否授权
    /// </summary>
    public abstract class Licence
    {
        //private static string key = "92f6766f-4b26-40ef-b27c-0b93057d4377";
        /// <summary>
        /// 确定是否授权
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsLicence(string key)
        {
            string host = HardWareHandler.GetCpuId();
            string Licence = ConfigurationManager.AppSettings["licence"];
            return Licence != null && Licence == EncryptHelper.EncryptMD5_16(host + key);
        }

        /// <summary>
        /// 获取许可证号
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetLicence(string key)
        {
            string host = HardWareHandler.GetCpuId();
            return GetLicence(host, key);
        }
        /// <summary>
        /// 获取许可证号
        /// </summary>
        /// <param name="host"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetLicence(string host, string key)
        {
            return EncryptHelper.EncryptMD5_16(host + key);
        }
        /// <summary>
        /// 检查文件创建日期，过期无效
        /// </summary>
        public static bool Checking()
        {
            return Checking(30);
        }
        /// <summary>
        /// 检查文件创建日期，过期无效
        /// </summary>
        /// <param name="days"></param>
        public static bool Checking(int days)
        {
            EncryptHelper.EncryptMD5(days.ToString());
            FileInfo file = new FileInfo(Assembly.GetExecutingAssembly().Location);
            TimeSpan ts = DateTime.Now.Subtract(file.LastWriteTime);
            if (ts < TimeSpan.Zero || ts > new TimeSpan(days, 0, 0, 0))
            {
                return false;
            }
            return true;
        }
    }
}
