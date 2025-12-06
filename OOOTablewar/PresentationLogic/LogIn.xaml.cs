using System.Text;
using DataBaseLogic;
using Microsoft.Data.Sqlite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PresentationLogic.Windows;
namespace PresentationLogic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        public User user;
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
                using (var connection = Context.GetConnection())
                {









                    // Простой запрос для проверки
                    string query = @"
                    SELECT ФИО, Логин, Пароль, Роль_сотрудника 
                    FROM Users 
                    WHERE Логин = @login AND Пароль = @password";

                    using (var cmd = new SqliteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@login", login);
                        cmd.Parameters.AddWithValue("@password", password);
                        // Просто проверяем, вернулось ли что-то
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) // Если есть хотя бы одна строка
                            {
                                // Заполняем объект User данными из базы
                                user = new User(
                                    reader["ФИО"].ToString(),
                                    reader["Логин"].ToString(),
                                    reader["Пароль"].ToString(),
                                    reader["Роль_сотрудника"].ToString()
                                );


                                MessageBox.Show("Вход выполнен!");

                                AdminProduct adminProduct =  new AdminProduct(user);
                                adminProduct.Show();
                                this.Close();

                               /*  Windows.AddEditProbuct addEditProbuct = new Windows.AddEditProbuct(user);
                                addEditProbuct.Show();
                                this.Close(); */

                                /* Windows.Products products = new Windows.Products(user);
                                 products.Show();
                                 this.Close(); */
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