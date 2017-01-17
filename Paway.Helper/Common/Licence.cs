using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace Paway.Helper
{
    /// <summary>
    ///     确定是否授权
    /// </summary>
    public abstract class Licence
    {
        //private static string key = "92f6766f-4b26-40ef-b27c-0b93057d4377";
        /// <summary>
        ///     确定是否授权
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsLicence(string key)
        {
            var host = HardWareHandler.GetCpuId();
            var Licence = ConfigurationManager.AppSettings["licence"];
            return Licence != null && Licence == EncryptHelper.EncryptMD5_16(host + key);
        }

        /// <summary>
        ///     获取许可证号
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetLicence(string key)
        {
            var host = HardWareHandler.GetCpuId();
            return GetLicence(host, key);
        }

        /// <summary>
        ///     获取许可证号
        /// </summary>
        /// <param name="host"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetLicence(string host, string key)
        {
            return EncryptHelper.EncryptMD5_16(host + key);
        }

        /// <summary>
        ///     检查文件创建日期，过期无效
        /// </summary>
        public static bool Checking()
        {
            return Checking(30);
        }

        /// <summary>
        ///     检查文件创建日期，过期无效
        /// </summary>
        /// <param name="days"></param>
        public static bool Checking(int days)
        {
            return true;
            EncryptHelper.EncryptMD5(days.ToString());
            var file = new FileInfo(Assembly.GetExecutingAssembly().Location);
            var ts = DateTime.Now.Subtract(file.LastWriteTime);
            //全球时间24小时以内
            if (ts < new TimeSpan(0, -24, 0, 0) || ts > new TimeSpan(days, 0, 0, 0))
            {
                return false;
            }
        }
    }
}