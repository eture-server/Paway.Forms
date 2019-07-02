using System;
using System.Collections.Generic;
using System.Management;
using System.Net;
using System.Text.RegularExpressions;

namespace Paway.Helper
{
    /// <summary>
    ///     硬件信息控制器
    /// </summary>
    public abstract class HardWareHandler
    {
        /// <summary>
        ///     机器CPU内核数
        /// </summary>
        public static int CpuCount
        {
            get { return Environment.ProcessorCount; }
        }

        /// <summary>
        ///     获取本地主机名
        /// </summary>
        /// <returns></returns>
        public static string GetHostName()
        {
            return Dns.GetHostName();
        }

        /// <summary>
        ///     获取CPU编号
        /// </summary>
        /// <returns></returns>
        public static string GetCpuId()
        {
            using (var mc = new ManagementClass("Win32_Processor"))
            {
                var moc = mc.GetInstances();
                var strCpuID = string.Empty;
                foreach (ManagementObject mo in moc)
                {
                    if (mo.Properties["ProcessorId"] != null)
                    {
                        strCpuID = mo.Properties["ProcessorId"].Value.ToString();
                        break;
                    }
                }
                return strCpuID;
            }
        }

        /// <summary>
        ///     获取网卡地址
        /// </summary>
        /// <returns></returns>
        public static string[] GetNetCardMacAddressArray()
        {
            var macAddress = GetNetCardMacAddress();
            if (macAddress == "")
            {
                macAddress = "54:04:A6:C2:7B:9A";
            }
            var lstMac = new List<string>();
            var arrMac = macAddress.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (var mac in arrMac)
            {
                if (!lstMac.Contains(mac))
                    lstMac.Add(mac);
            }
            return lstMac.ToArray();
            //if (macAddress.Length > 0)
            //{
            //    List<string> lstMac = new List<string>();
            //    string[] arrMac = macAddress.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            //    foreach (string mac in arrMac)
            //    {
            //        if (!lstMac.Contains(mac))
            //            lstMac.Add(mac);
            //    }
            //    return lstMac.ToArray();
            //}
            //else
            //{
            //    return null;
            //}
        }

        /// <summary>
        ///     获取MAC地址
        /// </summary>
        /// <returns></returns>
        public static string GetNetCardMacAddress()
        {
            using (var mc = new ManagementClass("Win32_NetworkAdapterConfiguration"))
            {
                var moc = mc.GetInstances();
                var str = "";
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"])
                        str += "," + mo["MacAddress"];
                }
                return str.Length > 0 ? str.Substring(1) : str;
            }
        }

        /// <summary>
        ///     获取主硬盘编号
        /// </summary>
        /// <returns></returns>
        public static string GetMainHardDiskId()
        {
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia"))
            {
                var strHardDiskID = string.Empty;
                foreach (ManagementObject mo in searcher.Get())
                {
                    strHardDiskID = mo["SerialNumber"].ToStrs().Trim();
                    if (!string.IsNullOrEmpty(strHardDiskID))
                        break;
                }
                return strHardDiskID;
            }
        }

        /// <summary>
        ///     获取IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIpAddress()
        {
            var ips = Dns.GetHostAddresses(Dns.GetHostName());
            if (ips != null && ips.Length > 0)
            {
                foreach (var ip in ips)
                {
                    if (ip.AddressFamily.ToString().Equals("InterNetwork"))
                        return ip.ToString();
                }
            }
            return string.Empty;
        }

        /// <summary>
        ///     获取最大线程数
        /// </summary>
        /// <returns></returns>
        public static int GetMaxThreads()
        {
            var threadCount = 4;
            if (CpuCount > 1)
            {
                threadCount = CpuCount * 2;
            }
            threadCount = threadCount > 8 ? 8 : threadCount;
            return threadCount;
        }

        /// <summary>
        ///     验证服务端IP
        /// </summary>
        /// <returns></returns>
        public static bool ValidateIpAddress(string ip, out string msg)
        {
            msg = string.Empty;
            var strReg =
                @"^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$";
            var regex = new Regex(strReg);
            if (!regex.IsMatch(ip))
            {
                msg = "配置的IP地址格式不正确!";
                return false;
            }
            return true;
        }

        /// <summary>
        ///     验证端口号
        /// </summary>
        /// <param name="intNum"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool ValidateNum(int intNum, out string msg)
        {
            msg = string.Empty;
            var strReg =
                @"^([0-9]|[1-9]\d|[1-9]\d{2}|[1-9]\d{3}|[1-5]\d{4}|6[0-4]\d{3}|65[0-4]\d{2}|655[0-2]\d|6553[0-5])$";
            var regex = new Regex(strReg);
            if (!regex.IsMatch(intNum.ToString()))
            {
                msg = "服务端口请输入0到65535的整数";
                return false;
            }
            return true;
        }
    }
}