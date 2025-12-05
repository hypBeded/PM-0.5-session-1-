using DataBaseLogic;
using Microsoft.Data.Sqlite;
using PresentationLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLogic.Services
{
    public class DatabaseService : IDatabaseService
    {
        public User AuthenticateUser(string email, string password)
        {
            using (var connection = Context.GetConnection())
            {
                // Исправьте названия полей согласно вашей БД
                const string query = @"
                    SELECT ФИО, Логин, Пароль, Роль_сотрудника 
                    FROM Users 
                    WHERE Логин = @email AND Пароль = @password";

                using (var cmd = new SqliteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", password);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Сопоставьте поля БД с полями класса User
                            return new User(
                                reader["ФИО"].ToString(),      // Name
                                reader["Логин"].ToString(),    // Email
                                reader["Пароль"].ToString(),   // Password
                                reader["Роль_сотрудника"].ToString() // Post
                            );
                        }
                    }
                }
            }
            return null;
        }
    }
}
