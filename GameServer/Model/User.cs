namespace GameServer.Model
{
    class User
    {
        public int id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }


        public User(int id, string username, string password)
        {
            this.id = id;
            this.UserName = username;
            this.Password = password;
        }
    }
}