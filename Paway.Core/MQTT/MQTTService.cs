using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Net.Sockets;
using Paway.Core;
using System.Threading;
using System.IO;
using System.Collections;
using System.Xml;
using MQTTnet.Server;
using System.Threading.Tasks;
using MQTTnet.Protocol;
using MQTTnet;
using System.ComponentModel;

namespace Paway.Core
{
    /// <summary>
    /// 封装MQTT服务端
    /// </summary>
    public partial class MQTTService
    {
        #region 变量
        private IMqttServer mqttServer = null;

        #endregion

        #region 事件
        /// <summary>
        /// 连接验证登陆
        /// </summary>
        public event Action<MqttConnectionValidatorContext> LoginEvent;
        /// <summary>
        /// 启动完成
        /// </summary>
        public event Action<string> StartEvent;
        /// <summary>
        /// 连接事件
        /// </summary>
        public event Action<MqttClientConnectedEventArgs> ClientConnected;
        /// <summary>
        /// 断开事件
        /// </summary>
        public event Action<MqttClientDisconnectedEventArgs> ClientDisconnected;

        #endregion

        #region 公开方法
        /// <summary>
        /// 启动，完成后引发StartEvent
        /// </summary>
        /// <param name="port">服务端口</param>
        public virtual void Start(int port)
        {
            var task = new Task(async () =>
            {
                string result = await CreateMQTTServer(port);
                Console.Title = $"0.0.0.0:{port}";
                StartEvent?.Invoke(result);
            });
            task.Start();
        }
        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            mqttServer?.StopAsync();
        }
        /// <summary>
        /// 获取客户端连接列表
        /// </summary>
        public IList<IMqttClientSessionStatus> Clients()
        {
            return mqttServer.GetClientSessionsStatus();
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        public void Publish(string topic, string data, MqttQualityOfServiceLevel level)
        {
            if (topic == null || data == null) return;
            var message = new MqttApplicationMessage()
            {
                Topic = topic,
                Payload = Encoding.GetEncoding("utf-8").GetBytes(data),
                QualityOfServiceLevel = level,
            };
            mqttServer.PublishAsync(message);
        }
        /// <summary>
        /// 响应消息
        /// </summary>
        protected void Response(MqttApplicationMessageReceivedEventArgs e, string data, MqttQualityOfServiceLevel level)
        {
            if (data == null) return;
            e.ApplicationMessage.Payload = Encoding.GetEncoding("utf-8").GetBytes(data);
            e.ApplicationMessage.QualityOfServiceLevel = level;
        }
        /// <summary>
        /// 消息处理
        /// </summary>
        protected virtual void MessageHandle(MqttApplicationMessageReceivedEventArgs e) { }

        #endregion

        #region private Method
        /// <summary>
        /// 开启服务
        /// </summary>
        private async Task<string> CreateMQTTServer(int port)
        {
            var optionsBuilder = new MqttServerOptionsBuilder();
            try
            {
                //在 MqttServerOptions 选项中，你可以使用 ConnectionValidator 来对客户端连接进行验证。比如客户端ID标识 ClientId，用户名 Username 和密码 Password 等。
                optionsBuilder.WithConnectionValidator(c =>
                {
                    try
                    {
                        if (LoginEvent != null)
                        {
                            LoginEvent?.Invoke(c);
                        }
                        else c.ReturnCode = MqttConnectReturnCode.ConnectionAccepted;
                    }
                    catch
                    {
                        c.ReturnCode = MqttConnectReturnCode.ConnectionRefusedBadUsernameOrPassword;
                        return;
                    }
                });
                //指定端口
                optionsBuilder.WithDefaultEndpointPort(port);
                //连接记录数，默认 一般为2000
                //optionsBuilder.WithConnectionBacklog(2000);
                mqttServer = new MqttFactory().CreateMqttServer();
                //   客户端支持 Connected、Disconnected 和 ApplicationMessageReceived 事件，用来处理客户端与服务端连接、客户端从服务端断开以及客户端收到消息的事情。
                //其中 ClientConnected 和 ClientDisconnected 事件的事件参数一个客户端连接对象 ConnectedMqttClient，通过该对象可以获取客户端ID标识 ClientId 和 MQTT 版本 ProtocolVersion。
                mqttServer.ClientConnected += MqttServer_ClientConnected;
                mqttServer.ClientDisconnected += MqttServer_ClientDisconnected;
                //ApplicationMessageReceived 的事件参数包含了客户端ID标识 ClientId 和 MQTT 应用消息 MqttApplicationMessage 对象，通过该对象可以获取主题 Topic、QoS QualityOfServiceLevel 和消息内容 Payload 等信息。
                mqttServer.ApplicationMessageReceived += MqttServer_ApplicationMessageReceived;
            }
            catch (Exception ex)
            {
                return "MQTT服务创建失败>" + ex.Message;
            }
            await mqttServer.StartAsync(optionsBuilder.Build());
            return "MQTT服务<0.0.0.0:" + port + ">已启动";
        }
        private void MqttServer_ClientConnected(object sender, MqttClientConnectedEventArgs e)
        {
            ClientConnected?.Invoke(e);
        }
        private void MqttServer_ClientDisconnected(object sender, MqttClientDisconnectedEventArgs e)
        {
            ClientDisconnected?.Invoke(e);
        }
        private void MqttServer_ApplicationMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            MessageHandle(e);
        }

        #endregion
    }
}
