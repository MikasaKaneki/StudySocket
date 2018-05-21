using System;
using System.Net.Sockets;
using Share;

namespace GameServer.Servers
{
    public class Client
    {
        private Socket _clientSocket;
        private Server _server;
        private Message msg = new Message();

        public Client()
        {
        }

        public Client(Socket clientSocket, Server server)
        {
            this._clientSocket = clientSocket;
            this._server = server;
        }


        public void Start()
        {
            _clientSocket.BeginReceive(msg.Data, msg.CurDataSize, msg.RemianSize, SocketFlags.None, ReceiveCallback,
                null);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                int count = _clientSocket.EndReceive(ar);
                if (count == 0)
                {
                    Close();
                }

                msg.ReadMessage(count);
                Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
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