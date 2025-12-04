
using DataBaseLogic;
using Microsoft.Data.Sqlite;
using System.Data.Common;

Context CT = new Context();

namespace BusinessLogic
{
    
    public class AuthReg
    {
        private readonly SqliteConnection _connection;
        public AuthReg(SqliteConnection connection)
        {
            _connection = connection;
        }
        
        public bool Auth(string login, string password)
        {
            try
            {
                string query = @"
                    SELECT COUNT(*) 
                    FROM Users 
                    WHERE [Логин] = @login 
                    AND [Пароль] = @password";

                using (var command = new SqliteCommand(query, _connection))
                {
                    
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@password", password);

                    long count = (long)command.ExecuteScalar();
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка авторизации: {ex.Message}");
                return false;
            }
        }






    }
}
