using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Xml.Serialization;
using GameServer.Controller;
using Share;

namespace GameServer.Servers
{
    public class Server
    {
        private IPEndPoint _ipEndPoint;
        private Socket serverSocket;
        private List<Client> _clientList = new List<Client>();
        private ControllerManager _controllerManager;
        private List<Room> _roomList = new List<Room>();

        public Server()
        {
        }

        public Server(string ipStr, int port)
        {
            _controllerManager = new ControllerManager(this);
            SetIpAndPort(ipStr, port);
            Start();
        }


        public void SetIpAndPort(string ipStr, int port)
        {
            _ipEndPoint = new IPEndPoint(IPAddress.Parse(ipStr), port);
        }

        /// <summary>
        /// 初始化服务器端的参数 开始接受客户端的连接
        /// </summary>
        public void Start()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(_ipEndPoint);
            serverSocket.Listen(0);
            serverSocket.BeginAccept(AcceptCallback, null);
            Console.WriteLine("{Server} initServer Success");
        }

        /// <summary>
        /// 客户端连接的回调
        /// </summary>
        /// <param name="ar"></param>
        private void AcceptCallback(IAsyncResult ar)
        {
            Console.WriteLine("{Server} have Client Conectted");
            Socket clientSocket = serverSocket.EndAccept(ar);
            Client client = new Client(clientSocket, this);
            _clientList.Add(client);
            serverSocket.BeginAccept(AcceptCallback, null);
        }

        public void RemoveClient(Client client)
        {
            lock (_clientList)
            {
                _clientList.Remove(client);
            }
        }

        public void SendResponse(Client client, ActionCode actionCode, string data)
        {
            //给客户端
            client.Send(actionCode, data);
        }


        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            _controllerManager.HandleRequest(requestCode, actionCode, data, client);
        }
    }
}