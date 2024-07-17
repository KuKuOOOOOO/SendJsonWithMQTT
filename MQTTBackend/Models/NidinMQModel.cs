namespace MQTTBackend.Models
{
    public class NidinMQModel
    {
        public string event_id { get; set; }
        public DateTime event_time { get; set; }
        public string event_type { get; set; }
        public PayloadData payload { get; set; }

        public class PayloadData
        {
            public int type { get; set; }
            public int store_id { get; set; }
            public int order_id { get; set; }
            public DateTime take_date { get; set; }
            public DateTime create_time { get; set; }
        }
    }
}