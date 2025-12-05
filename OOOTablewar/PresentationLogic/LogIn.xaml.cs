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
using PresentationLogic.Interfaces;
using System.Windows;
using PresentationLogic.Services;
namespace PresentationLogic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        private readonly IDatabaseService _databaseService;
        private readonly IMessageService _messageService;
        private readonly INavigationService _navigationService;

        // Сделайте CurrentUser public
        public User CurrentUser { get; private set; }

        // Сделайте TextBox public для тестов
        public System.Windows.Controls.TextBox LoginTextBox { get; private set; }
        public System.Windows.Controls.TextBox PasswordTextBox { get; private set; }
        public LogIn()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            _messageService = new MessageService();
            _navigationService = new PresentationLogic.Services.NavigationService();

            // Сохраняем ссылки на TextBox
            LoginTextBox = Login;
            PasswordTextBox = Password;
        }
        // Конструктор для тестирования
        public LogIn(
            IDatabaseService databaseService,
            IMessageService messageService,
            INavigationService navigationService)
        {
            _databaseService = databaseService;
            _messageService = messageService;
            _navigationService = navigationService;

            // Инициализируем TextBox для тестов
            LoginTextBox = new System.Windows.Controls.TextBox();
            PasswordTextBox = new System.Windows.Controls.TextBox();

            // Только если это реальные сервисы, инициализируем UI
            if (databaseService is DatabaseService)
            {
                InitializeComponent();
                LoginTextBox = Login;
                PasswordTextBox = Password;
            }
        }
        public void SignUp_Click(object sender, RoutedEventArgs e)
        {
            //SignUp sign = new SignUp();
            //sign.Show();
            //this.Close();
            _navigationService.NavigateToSignUp(this);
        }

        public void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            // Используем TextBox из конструктора
            string email = LoginTextBox.Text.Trim();
            string password = PasswordTextBox.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                _messageService.ShowMessage("Введите логин и пароль");
                return;
            }

            try
            {
                User user = _databaseService.AuthenticateUser(email, password);

                if (user != null)
                {
                    CurrentUser = user;
                    _messageService.ShowMessage("Вход выполнен!");
                    _navigationService.NavigateToAddEditProduct(user);
                    this.Close();
                }
                else
                {
                    _messageService.ShowMessage("Неверный логин или пароль");
                }
            }
            catch (Exception ex)
            {
                _messageService.ShowError($"Ошибка: {ex.Message}");
            }
        
        //var login = Login.Text.Trim();
        //var password = Password.Text;

        //if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
        //{
        //    _messageService.ShowMessage("Введите логин и пароль");
        //    return;
        //}

        //try
        //{
        //    var user = _databaseService.AuthenticateUser(login, password);

        //    if (user != null)
        //    {
        //        CurrentUser = user;
        //        _messageService.ShowMessage("Вход выполнен!");
        //        _navigationService.NavigateToAddEditProduct(user);
        //        Close();
        //    }
        //    else
        //    {
        //        _messageService.ShowMessage("Неверный логин или пароль");
        //    }
        //}
        //catch (Exception ex)
        //{
        //    _messageService.ShowError($"Ошибка: {ex.Message}");
        //}
        }

    }
}