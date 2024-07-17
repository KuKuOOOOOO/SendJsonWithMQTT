using MQTTBackend.BLLs.Enum;

namespace MQTTBackend.Interfaces
{
    public interface IFakeOrderSetting
    {
        string CreateDeliveryOrder(PlatfromTypeEnum platfromType);
        PlatfromTypeEnum CheckType(string text);
    }
}