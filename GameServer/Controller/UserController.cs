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
        private ResultDAO _resultDao = new ResultDAO();

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
                return ((int) ReturnCode.Fail).ToString();
            }
            else
            {
                Result result = _resultDao.GetResultByUserId(client.MySqlConnection, user.id);
                return string.Format("{0}${1}${2}${3}", ((int) ReturnCode.Success)
                    .ToString(), user.UserName, result.TotalCount, result.WinCount);
            }
        }


        public string Register(string data, Client client, Server server)
        {
            string[] strs = data.Split('$');
            string userName = strs[0];
            string password = strs[1];
            bool isExit = _userDao.GetUserByUserName(client.MySqlConnection, userName);
            if (isExit)
            {
                return ((int) ReturnCode.Fail).ToString();
            }
            else
            {
                _userDao.AddUser(client.MySqlConnection, userName, password);
                return ((int) ReturnCode.Success).ToString();
            }
        }
    }
}