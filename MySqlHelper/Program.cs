using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace MySqlHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            string connCof = "database=game;datasource=127.0.0.1;port=3306;user=root;password=mojinyang;";
            MySqlConnection conn = new MySqlConnection(connCof);
            conn.Open();

            #region 数据库插入

//            InsertSQL(conn);

            #endregion

            #region 数据删除

//            DeleteSQL(conn);

            #endregion

            #region 数据库查询

//            QuerySQL(conn);

            #endregion


            #region 数据库更新

            UpdateSQL(conn);

            #endregion


            conn.Close();
        }


        /// <summary>
        ///  插入SQL
        /// </summary>
        /// <param name="conn"></param>
        static void InsertSQL(MySqlConnection conn)
        {
            for (int i = 1; i <= 100; i++)
            {
                int uid = 1000 + i;
                string name = "guest" + uid;
                int level = uid - 1000;

                MySqlCommand cmd = new MySqlCommand("insert into user set uid=@uid1,name=@name1,level=@level1", conn);

                cmd.Parameters.AddWithValue("uid1", uid);
                cmd.Parameters.AddWithValue("name1", name);
                cmd.Parameters.AddWithValue("level1", level);
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("插入数据结束");
        }

        /// <summary>
        /// 查询SQL
        /// </summary>
        /// <param name="conn"></param>
        static void QuerySQL(MySqlConnection conn)
        {
            MySqlCommand cmd = new MySqlCommand("select * from user", conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            cmd.Connection = conn;

            while (reader.Read())
            {
                int uid = reader.GetInt32("uid");
                string name = reader.GetString("name");
                int level = reader.GetInt32("level");

                Console.WriteLine(uid + ":" + name + ":" + level);
            }

            reader.Close();
            Console.WriteLine("查询数据结束");
        }

        static void DeleteSQL(MySqlConnection conn)
        {
            for (int i = 1; i <= 100; i++)
            {
                MySqlCommand cmd = new MySqlCommand("delete from user where uid = @uid", conn);
                cmd.Parameters.AddWithValue("uid", 1000 + i);
                cmd.ExecuteNonQuery();
            }
        }


        static void UpdateSQL(MySqlConnection conn)
        {
            MySqlCommand cmd = new MySqlCommand("update user set name = @name where uid = 1001", conn);
            cmd.Parameters.AddWithValue("name", "zhangziqing");
            cmd.ExecuteNonQuery();
        }
    }
}