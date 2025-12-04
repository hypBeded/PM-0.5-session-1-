using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace DataBaseLogic
{
    public class Context
    {
        public void OpenConnection()
        {
            
                using (var connect = new SqliteConnection("Data Source=DBTableware.db"))
                {
                    connect.Open();
                }
            
        }
    }
}
