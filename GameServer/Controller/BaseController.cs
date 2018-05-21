using GameServer.Servers;
using Share;

namespace GameServer.Controller
{
    abstract class BaseController
    {
        protected RequestCode _requestCode = RequestCode.None;

        public RequestCode RequestCode
        {
            get { return _requestCode; }
        }

        public virtual string DefaultHandle(string data, Client client, Server server)
        {
            return null;
        }
    }
}