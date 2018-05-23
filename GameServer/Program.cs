using GameServer.Servers;

namespace GameServer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Server servers = new Server("127.0.0.1", 8090);
        }
    }
}