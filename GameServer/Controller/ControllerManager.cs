using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Share;
using GameServer.Servers;

namespace GameServer.Controller
{
    public class ControllerManager
    {
        private Dictionary<RequestCode, BaseController> controllerDict = new Dictionary<RequestCode, BaseController>();
        private Server _server;


        public ControllerManager(Server server)
        {
            this._server = server;
            InitController();
        }

        void InitController()
        {
            DefaultController defaultController = new DefaultController();
            controllerDict.Add(defaultController.RequestCode, defaultController);
        }

        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            BaseController controller;
            bool isGet = controllerDict.TryGetValue(requestCode, out controller);
            if (isGet == false)
            {
                Console.WriteLine("无法得到【" + requestCode + "】对应的controller，无法初期请求");
                return;
            }

            string methodName = Enum.GetName(typeof(ActionCode), actionCode);
            MethodInfo methodInfo = controller.GetType().GetMethod(methodName);
            if (methodInfo == null)
            {
                Console.WriteLine("在Controller【" + controller.GetType() + "】中没有对应的处理方法+【" + methodName + "】");
                return;
            }

            object[] parameters = new object[] {data, client, _server};
            object o = methodInfo.Invoke(controller, parameters);
            if (!string.IsNullOrEmpty((string) o))
            {
                _server.SendResponse(client, actionCode, o as string);
            }
        }
    }
}