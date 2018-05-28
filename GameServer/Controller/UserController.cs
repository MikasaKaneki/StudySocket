using System;
using GameServer.DAO;
using GameServer.Model;
using GameServer.Servers;
using Share;

namespace GameServer.Controller
{
    class UserController : BaseController
    {
        private UserDAO _userDao = new UserDAO();

        public UserController()
        {
            _requestCode = RequestCode.User;
        }


        public string Login(string data, Client client, Server server)
        {
            string[] strs = data.Split('$');
            User user = _userDao.VerifyUser(client.MySqlConnection, strs[0], strs[1]);
            if (user == null)
            {
//                Enum.GetName(typeof(RequestCode), ReturnCode.Fail);
                return ((int) ReturnCode.Fail).ToString();
            }
            else
            {
                return ((int) ReturnCode.Success).ToString();
            }
        }
    }
}