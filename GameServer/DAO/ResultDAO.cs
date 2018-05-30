using System;
using GameServer.Model;
using MySql.Data.MySqlClient;

namespace GameServer.DAO
{
    class ResultDAO
    {
        public Result GetResultByUserId(MySqlConnection conn, int userid)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd =
                    new MySqlCommand("select * from result where userid=@userid", conn);
                cmd.Parameters.AddWithValue("userid", userid);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    int totalCount = reader.GetInt32("totalcount");
                    int winCount = reader.GetInt32("wincount");
                    Result result = new Result(id, userid, totalCount, winCount);
                    return result;
                }
                else
                {
                    Result result = new Result(-1, userid, 0, 0);
                    return result;
                }
            }
            catch (Exception e)
            {
                throw new Exception("在GetResultByUserId的时候出现了错误:" + e.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
    }
}