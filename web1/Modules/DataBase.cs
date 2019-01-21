using System;
using MySql.Data.MySqlClient;

namespace WebAPI2.Modules
{
    public class MYsql {
        private MySqlConnection conn;

        public MYsql(){
            this.conn = GetConnection();
        }

        private MySqlConnection GetConnection()
        {
            string host = "192.168.3.37";
            string user = "root";
            string pwd = "1234";
            string db = "test";

            string connStr = string.Format(@"server={0};user={1};password={2};database={3}", host, user, pwd, db);
            MySqlConnection conn = new MySqlConnection(connStr);

            try
            {
                conn.Open();
                return conn;
            }
            catch
            {
                return null;
            }

        }
        public bool ConnectionClose()
        {
            try
            {
                conn.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool NonQuery(string sql)
        {
            try
            {
                if (conn != null)
                {
                    MySqlCommand comm = new MySqlCommand(sql, conn);
                    comm.ExecuteNonQuery();
                    //Console.WriteLine("ExecuteNonQuery 성공1");
                    return true;
                }
                else
                {
                    //Console.WriteLine("ExecuteNonQuery 실패1");
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public MySqlDataReader Reader(string sql)
        {
            try
            {
                if (conn != null)
                {
                    MySqlCommand comm = new MySqlCommand(sql, conn);
                    //Console.WriteLine("comm.ExecuteReader() 실행 성공");
                    return comm.ExecuteReader();
                }
                else
                {
                    //Console.WriteLine("comm.ExecuteReader() 실행 실패");
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public void ReaderClose(MySqlDataReader reader)
        {
            reader.Close();
        }
    }
}