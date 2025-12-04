using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DataBaseLogic;
using Microsoft.Data.Sqlite;

namespace PresentationLogic.Windows
{
    /// <summary>
    /// Логика взаимодействия для SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private void logIn_Click(object sender, RoutedEventArgs e)
        {

            LogIn logIn = new LogIn();
            logIn.Show();
            this.Close();

        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateEmail(Login.Text)) //Валидация почты
            {
                MessageBox.Show("Некорректный email.", "Ошибка");
                return;
            }
            string email = Login.Text;
            string lasname = LasName.Text;
            string name = Name.Text;
            string ojtchestvo = Otchestvo.Text;
            string password = Password.Text;
            string FIO = $"{lasname} {name} {ojtchestvo}";
            string rol = "Клиент";
            using (var connect = Context.GetConnection())
            { 
                var command = connect.CreateCommand();
                command.CommandText = "INSERT INTO Users" +
                    "(Роль_сотрудника, ФИО, Логин, Пароль)" +
                    "values (@rol, @FIO, @email, @password)";
                command.Parameters.AddWithValue("@rol", rol);
                command.Parameters.AddWithValue("@FIO", FIO);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password", password);
                command.ExecuteNonQuery();
                MessageBox.Show("Запись добавлена");
            }
            LogIn logIn = new LogIn();
            logIn.Show();
            this.Close();
        }
        private readonly string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"; //паттерн для проверки почты
        public bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            return Regex.IsMatch(email, emailPattern);
        } //метод для проверки почты
    }
}
