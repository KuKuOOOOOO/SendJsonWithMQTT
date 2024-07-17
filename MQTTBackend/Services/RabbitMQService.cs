using MQTTBackend.BLLs;
using MQTTBackend.BLLs.Enum;
using MQTTBackend.Interfaces;
using MQTTBackend.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace MQTTBackend.Services
{
    public class RabbitMQService : IDisposable
    {
        #region Fields
        private readonly IFakeOrderSetting _fakeOrderSetting;
        string _title = string.Empty;
        string _exchange = string.Empty;
        string _routingKey = string.Empty;
        string _endpoint = string.Empty;
        string _account = string.Empty;
        string _password = string.Empty;
        string _message = "";
        MessageModeEnum _messageMode = MessageModeEnum.Routing;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        #endregion

        #region Constructors
        public RabbitMQService(IFakeOrderSetting fakeOrderSetting)
        {
            _fakeOrderSetting = fakeOrderSetting;
        }
        #endregion

        #region Public Methods
        public string CreateOrderJson(string routingKey)
        {
            if (string.IsNullOrEmpty(routingKey))
                return string.Empty;
            string json = string.Empty;
            PlatfromTypeEnum platfromType = _fakeOrderSetting.CheckType(routingKey);
            switch (platfromType)
            {
                case PlatfromTypeEnum.Nidin:
                    json = _fakeOrderSetting.CreateDeliveryOrder(platfromType);
                    _routingKey = routingKey;
                    break;
            }
            return json;
        }
        public void SendMessage(string message)
        {
            var authorization = this.GetAuthorization();
            var topic = this.GetTopic();
            switch (_messageMode)
            {
                case MessageModeEnum.Queue:
                    MessageHelper.SendByQueue(authorization, topic, message);
                    this.InsertMessage(topic, message);
                    break;
                case MessageModeEnum.Routing:
                    MessageHelper.SendByRouting(authorization, topic, message);
                    this.InsertMessage(topic, message);
                    break;
                case MessageModeEnum.Mqtt:
                    MessageHelper.SendByMqtt(authorization, topic, message);
                    this.InsertMessage(topic, message);
                    break;
                case MessageModeEnum.Publish:
                    MessageHelper.SendByPublishSubscribe(authorization, topic, message);
                    this.InsertMessage(topic, message);
                    break;
            }
        }
        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
        #endregion

        #region  Private Methods
        private RabbitMQAuthorization GetAuthorization()
        {
            return new RabbitMQAuthorization
            {
                EndPoint = this._endpoint,
                Account = this._account,
                Password = this._password
            };
        }
        private RabbitMQTopic GetTopic()
        {
            return new RabbitMQTopic
            {
                ExchangeType = "direct",
                Exchange = this._exchange,
                RoutingKey = this._routingKey
            };
        }
        private void InsertMessage(RabbitMQTopic topic, string data)
        {
            var timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private string RefreshMessage()
        {
            try
            {
                PlatfromTypeEnum platfromType = _fakeOrderSetting.CheckType(_routingKey);
                var text = _fakeOrderSetting.CreateDeliveryOrder(platfromType);
                return text;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
        private void RabbitMqRecover(object sender, EventArgs e)
        {
            Console.WriteLine("Rabbit_MQ 斷線重連成功");
        }
        private void RabbitMqRecoverError(object sender, ConnectionRecoveryErrorEventArgs e)
        {
            Console.WriteLine("Rabbit_MQ 斷線重連失敗");
        }
        private void RabbitMqConnectionShutDown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("Rabbit_MQ 斷線");
        }
        #endregion
    }
}