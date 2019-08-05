﻿using System;
using System.Text;
using System.Reflection;
using System.Net.Sockets;
using log4net;
using MQTTnet.Client;
using MQTTnet;
using System.Threading;
using MQTTnet.Protocol;
using MQTTnet.Adapter;
using System.Collections.Generic;
using Paway.Helper;

namespace Paway.Utils
{
    /// <summary>
    /// 封装MQTT客户端
    /// </summary>
    public partial class MQTTClient
    {
        #region 变量
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IMqttClient mqttClient = null;
        private readonly string topic;
        private readonly int keepAlivePeriod;
        private string host;
        private int port;
        /// <summary>
        /// 用户Id
        /// </summary>
        protected int userId;
        /// <summary>
        /// 连接状态
        /// </summary>
        public bool IConnected { get { return mqttClient != null && mqttClient.IsConnected; } }
        /// <summary>
        /// 客户端ID
        /// </summary>
        public readonly string ClientId;

        #endregion

        #region 事件
        /// <summary>
        /// 连接(断开)后事件
        /// </summary>
        public event Action<bool, Exception> ConnectEvent;
        /// <summary>
        /// 外部登陆
        /// </summary>
        public event Func<Tuple<bool, string, string>> LoginEvent;
        /// <summary>
        /// 更多订阅事件
        /// </summary>
        public event Func<string[]> TopicEvent;

        #endregion

        #region 公开方法
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="topic">订阅主题</param>
        /// <param name="keepAlivePeriod">保活时长(s)</param>
        public MQTTClient(string topic, int keepAlivePeriod)
        {
            this.topic = topic;
            this.keepAlivePeriod = keepAlivePeriod;
            ClientId = Guid.NewGuid().ToString();
            mqttClient = new MqttFactory().CreateMqttClient();
            mqttClient.ApplicationMessageReceived += MqttClient_ApplicationMessageReceived;
            mqttClient.Connected += MqttClient_Connected;
            mqttClient.Disconnected += MqttClient_Disconnected;
        }
        /// <summary>
        /// 断开
        /// </summary>
        public void Disconnect()
        {
            mqttClient.DisconnectAsync();
        }
        /// <summary>
        /// 自动登陆连接
        /// </summary>
        /// <param name="host">主机</param>
        /// <param name="port">端口</param>
        /// <param name="userId">用户Id</param>
        public virtual void Connect(string host, int port, int userId)
        {
            Connect(host, port, userId, null, null);
        }
        /// <summary>
        /// 登陆连接
        /// </summary>
        /// <param name="host">主机</param>
        /// <param name="port">端口</param>
        /// <param name="userName">用户名</param>
        /// <param name="userPad">用户密码</param>
        public virtual void Connect(string host, int port, string userName, string userPad)
        {
            Connect(host, port, 0, userName, userPad);
        }
        /// <summary>
        /// 登陆连接
        /// </summary>
        /// <param name="host">主机</param>
        /// <param name="port">端口</param>
        /// <param name="userId">用户Id</param>
        /// <param name="userName">用户名</param>
        /// <param name="userPad">用户密码</param>
        public void Connect(string host, int port, int userId, string userName, string userPad)
        {
            this.host = host;
            this.port = port;
            this.userId = userId;
            if (LoginEvent != null)
            {
                Tuple<bool, string, string> tuple = LoginEvent?.Invoke();
                if (!tuple.Item1) return;
                userName = tuple.Item2;
                userPad = tuple.Item3;
            }
            var options = new MqttClientOptions
            {
                ClientId = this.ClientId,
                ChannelOptions = new MqttClientTcpOptions()
                {
                    Server = host,
                    Port = port
                },
                Credentials = new MqttClientCredentials()
                {
                    Username = userId > 0 ? userId + "" : userName,
                    Password = userId > 0 ? null : userPad,
                },
                CleanSession = true,
            };
            options.KeepAlivePeriod = TimeSpan.FromSeconds(this.keepAlivePeriod);
            //options.KeepAliveSendInterval = TimeSpan.FromSeconds(15);
            mqttClient.ConnectAsync(options);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        public void Send(string data, MqttQualityOfServiceLevel level, string topic = null)
        {
            Send(Encoding.GetEncoding("utf-8").GetBytes(data), level, topic);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        public void Send(byte[] buffer, MqttQualityOfServiceLevel level, string topic = null)
        {
            //开始发布消息
            var appMsg = new MqttApplicationMessage()
            {
                Topic = topic ?? this.topic + "/" + this.ClientId,
                Payload = buffer,
                QualityOfServiceLevel = level,
                Retain = false
            };
            mqttClient.PublishAsync(appMsg);
        }
        /// <summary>
        /// 消息处理
        /// </summary>
        protected virtual void MessageHandle(string data) { }

        #endregion

        #region private Method
        private void MqttClient_Connected(object sender, MqttClientConnectedEventArgs e)
        {
            var topicList = new List<TopicFilter> {
                new TopicFilter(this.topic, MqttQualityOfServiceLevel.ExactlyOnce),
                new TopicFilter(this.topic + "/" + this.ClientId, MqttQualityOfServiceLevel.ExactlyOnce)
            };
            var topics = TopicEvent?.Invoke();
            if (topics != null)
            {
                foreach (var item in topics)
                {
                    topicList.Add(new TopicFilter(item, MqttQualityOfServiceLevel.ExactlyOnce));
                }
            }
            mqttClient.SubscribeAsync(topicList);
            ConnectEvent?.Invoke(true, null);
        }
        private void MqttClient_Disconnected(object sender, MqttClientDisconnectedEventArgs e)
        {
            ConnectEvent?.Invoke(false, e.Exception);
            if (e.Exception.InnerException is SocketException error)
            {
                switch (error.SocketErrorCode)
                {
                    case SocketError.HostUnreachable:
                    case SocketError.ConnectionAborted:
                    case SocketError.NetworkUnreachable:
                    case SocketError.ConnectionRefused:
                        Thread.Sleep(3000);
                        break;
                    default:
                        Thread.Sleep(1000);
                        break;
                }
            }
            else Thread.Sleep(125);
            if (e.Exception is MqttConnectingFailedException code && code.ReturnCode == MqttConnectReturnCode.ConnectionRefusedBadUsernameOrPassword)
            {
                this.userId = 0;
            }
            else
            {
                Connect(this.host, this.port, this.userId);
            }
        }
        private void MqttClient_ApplicationMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            try
            {
                var buffer = e.ApplicationMessage.Payload;
                string data = Encoding.GetEncoding("utf-8").GetString(buffer);
                MessageHandle(data);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        #endregion
    }
}
