using System;
using System.Diagnostics;
using System.Net.Sockets;
using GameServer.Tool;
using MySql.Data.MySqlClient;
using Share;

namespace GameServer.Servers
{
    public class Client
    {
        private Socket _clientSocket;
        private Server _server;
        private Message msg = new Message();

        private MySqlConnection _mySqlConnection;

        public MySqlConnection MySqlConnection
        {
            get { return _mySqlConnection; }
        }

        public Client()
        {
        }

        public Client(Socket clientSocket, Server server)
        {
            this._clientSocket = clientSocket;
            this._server = server;
            Start();
        }


        public void Start()
        {
            _mySqlConnection = ConnHelper.Connect();
            if (_clientSocket != null && _clientSocket.Connected)
            {
                _clientSocket.BeginReceive(msg.Data, msg.CurDataSize, msg.RemianSize, SocketFlags.None, ReceiveCallback,
                    null);
                Console.WriteLine("开始接收客户端的消息");
            }
            else
            {
                Console.WriteLine("客户端关闭了");
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            Console.WriteLine("接收到了客户端的消息");

            try
            {
                int count = _clientSocket.EndReceive(ar);
                if (count == 0)
                {
                    Close();
                }

                string data = msg.ReadMessage(count, OnProcessMessage);
                Console.WriteLine("ReceiveCallback data is:" + data);
                Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("ReceiveCallback is error:" + e.Message);
            }
        }

        private void OnProcessMessage(RequestCode requestCode, ActionCode actionCode, string data)
        {
            _server.HandleRequest(requestCode, actionCode, data, this);
        }

        private void Close()
        {
            if (_clientSocket != null)
            {
                _clientSocket.Close();
            }

            _server.RemoveClient(this);
        }


        public void Send(ActionCode actionCode, string data)
        {
            try
            {
                byte[] bytes = Message.PackData(actionCode, data);
                _clientSocket.Send(bytes);
            }
            catch (Exception e)
            {
                Console.WriteLine("客户端发送消息失败 error:" + e.Message);
            }
        }
    }
}