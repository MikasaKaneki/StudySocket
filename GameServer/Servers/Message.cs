using System;
using System.Text;
using Share;

namespace GameServer.Servers
{
    class Message
    {
        private byte[] data = new byte[1024];
        private int curDataSize = 0; //我们存取了多少字节的数据在数组里面

        private const int flagSize = 4;

        public byte[] Data
        {
            get { return data; }
        }


        /// <summary>
        /// 获取现在存在的数据的长度
        /// </summary>
        public int CurDataSize
        {
            get { return curDataSize; }
        }

        public int RemianSize
        {
            get { return data.Length - curDataSize; }
        }


        public string ReadMessage(int newDataAmount, Action<RequestCode, ActionCode, string> processDataCallback)
        {
            curDataSize += newDataAmount;
            string message = null;
            while (true)
            {
                if (curDataSize <= flagSize)
                {
                    return message;
                }
                else
                {
                    //当前的数据的长度
                    int count = BitConverter.ToInt32(data, 0);
                    if (curDataSize - flagSize >= count)
                    {
                        RequestCode requestCode = (RequestCode) BitConverter.ToInt32(data, 4);
                        ActionCode actionCode = (ActionCode) BitConverter.ToInt32(data, 8);

                        message = Encoding.UTF8.GetString(data, flagSize + 8, count - 8);
                        processDataCallback(requestCode, actionCode, message);
                        Array.Copy(data, count + flagSize, data, 0, curDataSize - flagSize - count);
                        curDataSize -= (count + flagSize);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return message;
        }


        public static byte[] PackData(ActionCode actionCode, string data)
        {
            byte[] requestCodeBytes = BitConverter.GetBytes((int) actionCode);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            int dataAmount = requestCodeBytes.Length + dataBytes.Length;
            byte[] dataAmountBytes = BitConverter.GetBytes(dataAmount);

            byte[] result = new byte[dataAmountBytes.Length + requestCodeBytes.Length + dataBytes.Length];
            dataAmountBytes.CopyTo(result, 0);
            requestCodeBytes.CopyTo(result, dataAmountBytes.Length);
            dataBytes.CopyTo(result, dataAmountBytes.Length + requestCodeBytes.Length);
            return result;
        }
    }
}