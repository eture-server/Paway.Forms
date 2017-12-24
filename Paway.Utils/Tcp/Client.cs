using System.Net.Sockets;

namespace Paway.Utils.Tcp
{
    /// <summary>
    ///     封装Socket通讯客户端
    /// </summary>
    public class Client : SocketClient
    {
        /// <summary>
        ///     构造
        /// </summary>
        /// <param name="IpAddress"></param>
        /// <param name="port"></param>
        public Client(string IpAddress, int port) : base(IpAddress, port) { }
    }
}