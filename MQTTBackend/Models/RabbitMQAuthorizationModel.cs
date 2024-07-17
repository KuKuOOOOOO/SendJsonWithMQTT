using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTTBackend.Models
{
    public class RabbitMQAuthorization
    {
        public string EndPoint { get; set; }
        public string Port { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
    }
}