using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTTBackend.Models
{
    public class RabbitMQTopic
    {
        public string ExchangeType { get; set; } = "direct";
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }
    }
}