using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Paway.Helper
{
    /// <summary>
    /// UDPClient广播客户端信息
    /// </summary>
    public class UDPClient : UDPBase
    {
        private UdpClient udpClient = null;
        private static UDPClient instance = null;
        /// <summary>
        /// </summary>
        public static UDPClient Default
        {
            get
            {
                if (instance == null)
                {
                    instance = new UDPClient();
                }
                return instance;
            }
        }

        /// <summary>
        /// 广播客户端信息
        /// </summary>
        public void UPDSendMessage()
        {
            try
            {
                if (udpClient == null)
                {
                    udpClient = new UdpClient();
                }
                this.udpClient.Send(this.PackageData, this.PackageData.Length, this.ServerAddress);
            }
            catch (Exception ex)
            {
                throw new Exception("UdpClient广播错误", ex);
            }
            finally
            {
                udpClient.Close();
                udpClient = null;
            }
        }
    }
}
