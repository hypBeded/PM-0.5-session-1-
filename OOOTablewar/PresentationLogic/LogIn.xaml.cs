
using Microsoft.Data.Sqlite;
using PresentationLogic.Windows;
using System.Security.Policy;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace PresentationLogic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        public LogIn()
        {
            InitializeComponent();
        }   
        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            
            SignUp sign = new SignUp();
            sign.Show();
            this.Close();
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            string login = Login.Text.Trim();
            string password = Password.Text;

            // Проверка на пустые значения
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль");
                return;
            }

            try
            {
                // Временное упрощенное подключение
                using (var connection = new SqliteConnection("Data Source=DBTableware.db"))
                {
                    connection.Open();

                    // Простой запрос для проверки
                    string query = "SELECT 1 FROM Users WHERE Логин = @login AND Пароль = @password";

                    using (var cmd = new SqliteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@login", login);
                        cmd.Parameters.AddWithValue("@password", password);

                        // Просто проверяем, вернулось ли что-то
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) // Если есть хотя бы одна строка
                            {
                                MessageBox.Show("Вход выполнен!");
                                SignUp sign = new SignUp();
                                sign.Show();
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Неверный логин или пароль");
                            }
                        }
                    }
                }
            }
            catch (SqliteException sqlEx)
            {
                MessageBox.Show($"SQL ошибка: {sqlEx.Message}\nКод: {sqlEx.SqliteErrorCode}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}