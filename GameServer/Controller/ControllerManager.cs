using System.Collections.Generic;
using Share;
using GameServer.Servers;

namespace GameServer.Controller
{
    public class ControllerManager
    {
        private Dictionary<RequestCode, BaseController> controllerDict = new Dictionary<RequestCode, BaseController>();
        private Server _server;


        public ControllerManager()
        {
            Init();
        }

        void Init()
        {
        }
    }
}