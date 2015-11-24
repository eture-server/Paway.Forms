using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;

namespace Paway.Helper
{
    /// <summary>
    /// 硬件信息控制器
    /// </summary>
    public abstract class HardWareHandler
    {
        /// <summary>
        /// 机器CPU内核数 
        /// </summary>
        public static int CpuCount { get { return System.Environment.ProcessorCount; } }
        /// <summary>
        /// 获取本地主机名
        /// </summary>
        /// <returns></returns>
        public static string GetHostName()
        {
            return System.Net.Dns.GetHostName();
        }
        /// <summary>
        /// 获取CPU编号
        /// </summary>
        /// <returns></returns>
        public static string GetCpuId()
        {
            ManagementClass mc = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mc.GetInstances();
            String strCpuID = string.Empty;
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
        /// <summary>
        /// 获取网卡地址
        /// </summary>
        /// <returns></returns>
        public static string[] GetNetCardMacAddressArray()
        {
            string macAddress = GetNetCardMacAddress();
            if (macAddress == "")
            {
                macAddress = "54:04:A6:C2:7B:9A";
            }
            List<string> lstMac = new List<string>();
            string[] arrMac = macAddress.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string mac in arrMac)
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
        /// 获取MAC地址
        /// </summary>
        /// <returns></returns>
        public static string GetNetCardMacAddress()
        {
            try
            {
                ManagementClass mc;
                ManagementObjectCollection moc;
                mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                moc = mc.GetInstances();
                string str = "";
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                        str += "," + mo["MacAddress"].ToString();
                }
                return str.Length > 0 ? str.Substring(1) : str;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取主硬盘编号
        /// </summary>
        /// <returns></returns>
        public static string GetMainHardDiskId()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
                String strHardDiskID = string.Empty;
                foreach (ManagementObject mo in searcher.Get())
                {
                    strHardDiskID = mo["SerialNumber"].ToString().Trim();
                    if (!string.IsNullOrEmpty(strHardDiskID))
                        break;
                }
                return strHardDiskID;
            }
            catch
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIpAddress()
        {
            try
            {
                IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
                if (ips != null && ips.Length > 0)
                {
                    foreach (IPAddress ip in ips)
                    {
                        if (ip.AddressFamily.ToString().Equals("InterNetwork"))
                            return ip.ToString();
                    }
                }
            }
            catch { }
            return string.Empty;
        }
        /// <summary>
        /// 获取最大线程数
        /// </summary>
        /// <returns></returns>
        public static int GetMaxThreads()
        {
            int threadCount = 4;
            if (CpuCount > 1)
            {
                threadCount = CpuCount * 2;
            }
            threadCount = threadCount > 8 ? 8 : threadCount;
            return threadCount;
        }
        /// <summary>
        /// 验证服务端IP
        /// </summary>
        /// <returns></returns>
        public static bool ValidateIpAddress(string ip, out string msg)
        {
            msg = string.Empty;
            string strReg = @"^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$";
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(strReg);
            if (!regex.IsMatch(ip))
            {
                msg = "配置的IP地址格式不正确!";
                return false;
            }
            return true;
        }
        /// <summary>
        /// 验证端口号
        /// </summary>
        /// <param name="intNum"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool ValidateNum(int intNum, out string msg)
        {
            msg = string.Empty;
            string strReg = @"^([0-9]|[1-9]\d|[1-9]\d{2}|[1-9]\d{3}|[1-5]\d{4}|6[0-4]\d{3}|65[0-4]\d{2}|655[0-2]\d|6553[0-5])$";
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(strReg);
            if (!regex.IsMatch(intNum.ToString()))
            {
                msg = "服务端口请输入0到65535的整数";
                return false;
            }
            return true;
        }
    }
}
