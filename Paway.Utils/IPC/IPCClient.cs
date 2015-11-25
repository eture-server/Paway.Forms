using Paway.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using System.Security.Permissions;
using System.Text;

namespace Paway.Utils.Pdf
{
    /// <summary>
    /// IPC客户端
    /// </summary>
    /// <typeparam name="T">接口</typeparam>
    public class IPCClient<T> where T : class
    {
        private IpcChannel channel = null;
        /// <summary>
        /// 接口实例
        /// </summary>
        public T obj { get; private set; }
        /// <summary>
        /// 连接状态
        /// </summary>
        public bool IsConnected { get; private set; }
        /// <summary>
        /// 创建一个IPC信道。
        /// </summary>
        public void Connect()
        {
            try
            {
                BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
                BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
                serverProvider.TypeFilterLevel = TypeFilterLevel.Full;
                IDictionary props = new Hashtable();
                props["name"] = "ServerChannel";
                props["portName"] = string.Format("ServerChannel-Client.{0}", TConfig.Name);
                channel = new IpcChannel(props, clientProvider, serverProvider);
                System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(channel, true);
                obj = Activator.GetObject(typeof(T), string.Format("ipc://ServerChannel-Server.{0}/IPCObject", TConfig.Name)) as T;
                if (obj != null)
                    IsConnected = true;
            }
            catch (Exception)
            {
                Disconnect();
            }
        }
        /// <summary>
        /// 断开IPC连接
        /// </summary>
        public void Disconnect()
        {
            if (channel != null)
            {
                ChannelServices.UnregisterChannel(channel);
                channel.StopListening(null);
            }
            channel = null;
            obj = null;
            IsConnected = false;
        }
    }
}
