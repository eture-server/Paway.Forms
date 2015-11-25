using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public void UPDSendMessage(object message)
        {
            try
            {
                if (udpClient == null)
                {
                    udpClient = new UdpClient();
                }
                byte[] byteData = message is byte[] ? message as byte[] : SctructHelper.GetByteFromObject(message);
                //先处理发送的数据，加上一字节头+四字节长度
                byte[] msgBuffer = new byte[byteData.Length + 5];
                msgBuffer[0] = this.Preamble;
                msgBuffer[1] = (byte)(byteData.Length >> 24);
                msgBuffer[2] = (byte)(byteData.Length >> 16);
                msgBuffer[3] = (byte)(byteData.Length >> 8);
                msgBuffer[4] = (byte)(byteData.Length);
                for (int i = 0; i < byteData.Length; i++)
                {
                    msgBuffer[i + 5] = byteData[i];
                }
                this.udpClient.Send(msgBuffer, msgBuffer.Length, ServerAddress);
            }
            catch (Exception ex)
            {
                OnError(ex.Message, null);
            }
            finally
            {
                udpClient.Close();
                udpClient = null;
            }
        }
    }
}
