using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(IPAddress.Parse("127.0.0.1"), 9010);

            byte[] data = new byte[1024];
            int count = clientSocket.Receive(data);
            string msg = Encoding.UTF8.GetString(data, 0, count);
            Console.WriteLine("[Client] 从服务器收到一条消息:" + msg);
            //TODO test

//            while (true)
//            {
//                Console.Write("请输入需要发送的消息:");
//                string s = Console.ReadLine();
//                if (s == "c")
//                {
//                    clientSocket.Close();
//                    return;
//                }
//
//                Console.WriteLine("[Client] 发送一条消息到服务端：" + s);
//                clientSocket.Send(Message.GetBytes(s));
//            }

            for (int i = 0; i <= 1000; i++)
            {
                string s = i.ToString() + "_";
                Console.WriteLine("[Client] 发送一条消息到服务端：" + s);
                clientSocket.Send(Message.GetBytes(s));
            }


            Console.ReadKey();
            clientSocket.Close();
        }
    }
}