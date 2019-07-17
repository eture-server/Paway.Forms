using System;
using System.Net.Sockets;

namespace Paway.Helper
{
    /// <summary>
    /// UDPClient广播客户端信息
    /// </summary>
    public class UDPClient : UDPBase
    {
        /// <summary>
        /// 广播客户端信息
        /// </summary>
        public void UPDSendMessage(object message)
        {
            UdpClient udpClient = null;
            try
            {
                udpClient = new UdpClient();
                var byteData = message is byte[] ? message as byte[] : StructHelper.GetByteFromObject(message);
                //先处理发送的数据，加上一字节头+四字节长度
                var msgBuffer = new byte[byteData.Length + 5];
                msgBuffer[0] = Preamble;
                msgBuffer[1] = (byte)(byteData.Length >> 24);
                msgBuffer[2] = (byte)(byteData.Length >> 16);
                msgBuffer[3] = (byte)(byteData.Length >> 8);
                msgBuffer[4] = (byte)byteData.Length;
                for (var i = 0; i < byteData.Length; i++)
                {
                    msgBuffer[i + 5] = byteData[i];
                }
                udpClient.Send(msgBuffer, msgBuffer.Length, ServerAddress);
            }
            catch (Exception ex)
            {
                OnError(ex.Message, null);
            }
            finally
            {
                if (udpClient != null)
                {
                    udpClient.Close();
                }
            }
        }
    }
}