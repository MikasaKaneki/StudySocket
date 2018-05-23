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
        private ControllerManager controllerManager;

        public Server()
        {
        }

        public Server(string ipStr, int port)
        {
            controllerManager = new ControllerManager(this);
            SetIpAndPort(ipStr, port);
        }


        public void SetIpAndPort(string ipStr, int port)
        {
            _ipEndPoint = new IPEndPoint(IPAddress.Parse(ipStr), port);
        }

        public void Start()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(_ipEndPoint);
            serverSocket.Listen(0);
            serverSocket.BeginAccept(AcceptCallback, null);
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            Socket clientSocket = serverSocket.EndAccept(ar);
            Client client = new Client(clientSocket, this);
            _clientList.Add(client);
            client.Start();
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
            controllerManager.HandleRequest(requestCode, actionCode, data, client);
        }
    }
}