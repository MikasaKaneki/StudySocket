using System;
using System.Text;

namespace Client
{
    public class Message
    {
        public static byte[] GetBytes(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            int dataSize = data.Length;
            byte[] bytesSize = BitConverter.GetBytes(dataSize);

            byte[] dataResult = new byte[data.Length + bytesSize.Length];
            for (int i = 0; i < dataResult.Length; i++)
            {
                if (i < bytesSize.Length)
                {
                    dataResult[i] = bytesSize[i];
                }
                else
                {
                    dataResult[i] = data[i - bytesSize.Length];
                }
            }

            return dataResult;
        }
    }
}