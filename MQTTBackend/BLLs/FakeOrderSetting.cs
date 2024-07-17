

using MQTTBackend.BLLs.Enum;
using MQTTBackend.Interfaces;
using MQTTBackend.Models;
using Newtonsoft.Json;

namespace MQTTBackend.BLLs
{
    public class FakeOrderSetting : IFakeOrderSetting
    {
        #region Fidles
        public static string RoutingKeyPrefix_PX = "ProxWeb";
        public static string RoutingKeyPrefix_ND = "nidin";
        public static string RoutingKeyPrefix_UE = "uber";
        public static string RoutingKeyPrefix_FP = "foodpanda";
        private int _currentDisplayID = 5000;
        #endregion

        #region Constructor
        public FakeOrderSetting()
        {

        }
        #endregion

        #region Public Methods
        public string CreateDeliveryOrder(PlatfromTypeEnum platfromType)
        {
            string jsonString = string.Empty;
            switch (platfromType)
            {
                case PlatfromTypeEnum.Nidin:
                    NidinMQModel order = CreateNindinMQModel();
                    jsonString = JsonConvert.SerializeObject(order);
                    break;
            }
            return jsonString;
        }

        public PlatfromTypeEnum CheckType(string text)
        {
            if (text.StartsWith(RoutingKeyPrefix_PX))
                return PlatfromTypeEnum.ProxWeb;
            else if (text.StartsWith(RoutingKeyPrefix_ND))
                return PlatfromTypeEnum.Nidin;
            else if (text.StartsWith(RoutingKeyPrefix_UE))
                return PlatfromTypeEnum.UberEats;
            else if (text.StartsWith(RoutingKeyPrefix_FP))
                return PlatfromTypeEnum.Foodpanda;
            else
                return PlatfromTypeEnum.Unknow;
        }

        #endregion

        #region Private Methods
        private NidinMQModel CreateNindinMQModel()
        {
            var currentEnvetType = "order.status_changed";
            NidinMQModel model = new NidinMQModel();

            // 設置屬性值
            model.event_id = OrderCommonHelper.GetGuid().ToString();
            model.event_time = DateTime.Now;
            model.event_type = currentEnvetType;

            _currentDisplayID = OrderCommonHelper.GetNextId(_currentDisplayID);

            model.payload = new NidinMQModel.PayloadData
            {
                type = 1,//之後要相容你訂、Uber
                store_id = 308,
                order_id = _currentDisplayID,
                take_date = DateTime.Now,
                create_time = DateTime.Now
            };
            return model;
        }

        #endregion
    }
}