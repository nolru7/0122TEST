using System;
using System.Collections;
using System.IO;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebAPI2.Modules
{
    public class MYsql {
        private MySqlConnection conn;

        public MYsql(){
            this.conn = GetConnection();
        }

        private MySqlConnection GetConnection()
        {
            

            try
            {
                MySqlConnection conn = new MySqlConnection();
                
                string path = "/public/DBInfo.json";
                string result = new StreamReader(File.OpenRead(path)).ReadToEnd();
                JObject jo = JsonConvert.DeserializeObject<JObject>(result);
                Hashtable map = new Hashtable();
                foreach (JProperty col in jo.Properties())
                {
                    Console.WriteLine("{0} : {1}", col.Name, col.Value);
                    map.Add(col.Name, col.Value);
                }
                #region 
                
                string strConnection1 =
                    string.Format("server={0};user={1};password={2};database={3};", map["server"], map["user"], map["password"], map["database"]);
                conn.ConnectionString = strConnection1;
                conn.Open();
                
                #endregion
                return conn;
            }
            catch(MySqlException e)
            {
                Console.WriteLine(e.Message);
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