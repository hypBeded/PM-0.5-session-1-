using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace DataBaseLogic
{
    public class Context
    {
        private static SqliteConnection _connection;
        public static SqliteConnection GetConnection()
        {
            if (_connection == null || _connection.State != System.Data.ConnectionState.Open)
            {
                _connection = new SqliteConnection("Data Source=DBTableware.db");
                _connection.Open();
            }
            return _connection;
        }
        public static void OpenConnection()
        {
            GetConnection(); // Просто создаст и откроет соединение
        }
        public static void CloseConnection()
        {
            if (_connection != null && _connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}
