using System.Text;
using RabbitMQ.Client;
using MQTTBackend.Models;

namespace MQTTBackend.BLLs
{
    public class MessageHelper
    {
        #region Internal Methods
        static ConnectionFactory CreateFactory(RabbitMQAuthorization authorization)
        {
            return new ConnectionFactory
            {
                HostName = authorization.EndPoint,
                UserName = authorization.Account,
                Password = authorization.Password
            };
        }
        #endregion
        public static void SendByQueue(RabbitMQAuthorization authorization, RabbitMQTopic topic, string message)
        {
            var factory = CreateFactory(authorization);

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(topic.RoutingKey, true, false, false, null);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = false;

                    channel.BasicPublish(string.Empty, topic.RoutingKey, properties, Encoding.UTF8.GetBytes(message));
                }
            }
        }
        public static void SendByRouting(RabbitMQAuthorization authorization, RabbitMQTopic topic, string message)
        {
            var factory = CreateFactory(authorization);

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(topic.Exchange, topic.ExchangeType);
                    channel.BasicPublish(topic.Exchange, topic.RoutingKey, null, Encoding.UTF8.GetBytes(message));
                }
            }
        }
        public static void SendByMqtt(RabbitMQAuthorization authorization, RabbitMQTopic topic, string message)
        {
            var factory = CreateFactory(authorization);

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(topic.Exchange, topic.ExchangeType);
                    channel.BasicPublish(topic.Exchange, topic.RoutingKey, null, Encoding.UTF8.GetBytes(message));
                }
            }
        }
        public static void SendByPublishSubscribe(RabbitMQAuthorization authorization, RabbitMQTopic topic, string message)
        {
            var factory = CreateFactory(authorization);

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(topic.Exchange, topic.ExchangeType, true);
                    channel.BasicPublish(topic.Exchange, topic.RoutingKey, null, Encoding.UTF8.GetBytes(message));
                }
            }
        }
    }
}