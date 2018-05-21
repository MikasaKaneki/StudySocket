using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("启动Server");
            StartServerAsync();
            Console.ReadKey();
        }

        static Message m_message = new Message();

        /// <summary>
        /// 异步接收消息
        /// </summary>
        static void StartServerAsync()
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9010);
            serverSocket.Bind(ipEndPoint); //绑定ip和端口号
            serverSocket.Listen(0); //开始监听端口号
            serverSocket.BeginAccept(AcceptCallback, serverSocket);
        }


        static void AcceptCallback(IAsyncResult ar)
        {
            Socket serverSocket = ar.AsyncState as Socket;
            Socket clientSocket = serverSocket.EndAccept(ar);

            string msg = "Hello client！你好....";
            byte[] data = Encoding.UTF8.GetBytes(msg);

            Console.WriteLine("[Server] 发送给客户端的消息是:" + msg);
            clientSocket.Send(data);
            clientSocket.BeginReceive(m_message.Data, m_message.CurDataSize, m_message.RemianSize, SocketFlags.None,
                ReceiveCallBack, clientSocket);
            serverSocket.BeginAccept(AcceptCallback, serverSocket);
        }

        static void ReceiveCallBack(IAsyncResult ar)
        {
            Socket clientSocket = ar.AsyncState as Socket;
            try
            {
                int count = clientSocket.EndReceive(ar);
                if (count == 0)
                {
                    clientSocket.Close();
                    return;
                }

                m_message.AddCount(count);

                string msgReceive = m_message.ReadMessage();
                if (!string.IsNullOrEmpty(msgReceive))
                {
                    Console.WriteLine("[Server] 收到了客户端的一条消息：" + msgReceive);
                }

                clientSocket.BeginReceive(m_message.Data, m_message.CurDataSize, m_message.RemianSize, SocketFlags.None,
                    ReceiveCallBack, clientSocket);
            }
            catch (Exception e)
            {
                Console.WriteLine("[Server] 关闭客户端:" + e.Message);
                if (clientSocket != null)
                {
                    clientSocket.Close();
                }
            }
        }

        static void StartServerSync()
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("192.168.6.18"), 9010);
            serverSocket.Bind(ipEndPoint); //绑定ip和端口号
            serverSocket.Listen(0); //开始监听端口号
            Socket clientSocket = serverSocket.Accept(); //接收一个客户端链接

            //向客户端发送一条消息
            string msg = "Hello client！你好....";
            byte[] data = Encoding.UTF8.GetBytes(msg);
            clientSocket.Send(data);

            //接收客户端的一条消息
            byte[] dataBuffer = new byte[1024];
            int count = clientSocket.Receive(dataBuffer);
            string msgReceive = Encoding.UTF8.GetString(dataBuffer, 0, count);
            Console.WriteLine("[Server] 收到了客户端的一条消息：" + msgReceive);
            clientSocket.Close();
            serverSocket.Close();
        }
    }
}