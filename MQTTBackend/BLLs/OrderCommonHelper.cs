namespace MQTTBackend.BLLs
{
    public class OrderCommonHelper
    {
        public static Guid GetGuid()
        {
            return Guid.NewGuid();
        }
        public static string GetOrderId(int currentNum, out int newNum)
        {
            string prefix = "SN";
            string paddedNumber = currentNum.ToString().PadLeft(10, '0');
            string nextNumber = prefix + paddedNumber; // 組合新號碼
            currentNum = currentNum + 1;
            newNum = currentNum;
            return nextNumber;
        }
        public static string GetDisplayID(int currentDisplayID, out int newNum)
        {
            string prefix = "W";
            string paddedNumber = currentDisplayID.ToString().PadLeft(4, '0');
            string nextNumber = prefix + paddedNumber; // 組合新號碼
            currentDisplayID = currentDisplayID + 1;
            newNum = currentDisplayID;
            return nextNumber;
        }
        public static int GetNextId(int id)
        {
            return id + 1;
        }
    }
}