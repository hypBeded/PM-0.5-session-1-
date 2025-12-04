using PresentationLogic.Converters;
using DataBaseLogic;
using PresentationLogic.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PresentationLogic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Инициализация базы данных
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            try
            {
                ProductService.InitializeDatabase(baseDirectory);
                this.DataContext = new MainViewModel();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка загрузки базы данных",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Product> _products;
        private bool _isLoading;

        public ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ProductsCountText));
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public string ProductsCountText => $"Всего товаров: {Products?.Count ?? 0}";

        public MainViewModel()
        {
            Products = new ObservableCollection<Product>();
            LoadProductsAsync();
        }

        private async Task LoadProductsAsync()
        {
            IsLoading = true;

            try
            {
                // Загрузка данных из базы данных
                var productsFromDb = await ProductService.GetAllProductsAsync();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Products.Clear();
                    foreach (var product in productsFromDb)
                    {
                        Products.Add(product);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}