using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace 公交线路查询表
{
    class MysqlUtil
    {
        MySqlConnection con;
        public Boolean GetConnection()
        {
            string cs = "server=127.0.0.1;port=33060;userid=root;password=root;database=study";
            con = new MySqlConnection(cs);
            try
            {
                con.Open();
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error when connection database.");
                return false;
            }

            Console.WriteLine("MySQL version : {con.ServerVersion} has been connected.");
            return true;
        }

        public void Close()
        {
            con.Close();
        }

        public MySqlDataReader ReadData(string sql)
        {
            MySqlCommand cmd = new MySqlCommand(sql, con);

            MySqlDataReader rdr = cmd.ExecuteReader();
            return rdr;
        }

        public int InsertData(string table, params String[] values)
        {
            //var sql = "INSERT INTO cars(name, price) VALUES(@name, @price)";
            int length = values.Length;
            String sql = "INSERT INTO " + table + " VALUES(";
            for(int i = 0;i < length; i++)
            {
                sql += "'" + values[i] + "'";
                if(i != length - 1)
                {
                    sql += ",";
                }
            }
            /*foreach(String value in values)
            {
                sql += "`" + value + "`,";
            }我还不知道更好的动态写法*/
            sql += ");";
            MySqlCommand cmd = new MySqlCommand(sql, con);
            return cmd.ExecuteNonQuery();
        }

        public int UpdateData(string sql)
        {
            MySqlCommand cmd = new MySqlCommand(sql, con);
            return cmd.ExecuteNonQuery();
        }

    }
}
