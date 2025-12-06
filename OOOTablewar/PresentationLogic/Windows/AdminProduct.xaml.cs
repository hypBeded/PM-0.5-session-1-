using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Data.Sqlite;
using DataBaseLogic;
using System.Windows.Media.TextFormatting;
namespace PresentationLogic.Windows
{
    /// <summary>
    /// Логика взаимодействия для Products.xaml
    /// </summary>

    public partial class AdminProduct : Window
    {

        private User userlog;
        private readonly ProductsViewModel _viewModel;
        private string _currentSortField;
        private bool _sortAscending = true;
        public AdminProduct(User user)
        {
            InitializeComponent();
            _viewModel = new ProductsViewModel();
            this.DataContext = _viewModel;
            UpdateSortText();
            userlog = user;
            FIO.Text = userlog.Name;
        }

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {

            var cartWindow = new Basket(userlog);
            cartWindow.Show();
            this.Show();


        }



        private void SortByNameButton_Click(object sender, RoutedEventArgs e)
        {
            SortProducts("Name", "по названию");
        }

        private void SortByPriceButton_Click(object sender, RoutedEventArgs e)
        {
            SortProducts("Price", "по цене");
        }

        private void SortByQuantityButton_Click(object sender, RoutedEventArgs e)
        {
            SortProducts("StockQuantity", "по количеству");
        }

        private void SortByManufacturerButton_Click(object sender, RoutedEventArgs e)
        {
            SortProducts("Manufacturer", "по производителю");
        }
        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
           
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void AddBasket_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ResetSortButton_Click(object sender, RoutedEventArgs e)
        {

        }







        private void SortProducts(string fieldName, string displayName)
        {
            if (_viewModel.Products == null || _viewModel.Products.Count == 0)
                return;

            // Если сортируем по тому же полю, меняем направление
            if (_currentSortField == fieldName)
            {
                _sortAscending = !_sortAscending;
            }
            else
            {
                _currentSortField = fieldName;
                _sortAscending = true;
            }

            // Создаем отсортированную коллекцию
            List<Product> sortedList;

            switch (fieldName)
            {
                case "Name":
                    sortedList = _sortAscending
                        ? _viewModel.Products.OrderBy(p => p.Name).ToList()
                        : _viewModel.Products.OrderByDescending(p => p.Name).ToList();
                    break;

                case "Price":
                    sortedList = _sortAscending
                        ? _viewModel.Products.OrderBy(p => p.Price).ToList()
                        : _viewModel.Products.OrderByDescending(p => p.Price).ToList();
                    break;

                case "StockQuantity":
                    sortedList = _sortAscending
                        ? _viewModel.Products.OrderBy(p => p.StockQuantity).ToList()
                        : _viewModel.Products.OrderByDescending(p => p.StockQuantity).ToList();
                    break;

                case "Manufacturer":
                    sortedList = _sortAscending
                        ? _viewModel.Products.OrderBy(p => p.Manufacturer).ToList()
                        : _viewModel.Products.OrderByDescending(p => p.Manufacturer).ToList();
                    break;

                default:
                    return;
            }

            // Обновляем коллекцию
            _viewModel.Products = new System.Collections.ObjectModel.ObservableCollection<Product>(sortedList);

            // Обновляем текст сортировки
            UpdateSortText();
        }

        private void UpdateSortText()
        {
            if (string.IsNullOrEmpty(_currentSortField))
            {
                CurrentSortText.Text = "(без сортировки)";
                CurrentSortText.Foreground = System.Windows.Media.Brushes.Gray;
            }
            else
            {
                string sortDirection = _sortAscending ? "↑" : "↓";
                string fieldName = _currentSortField switch
                {
                    "Name" => "названию",
                    "Price" => "цене",
                    "StockQuantity" => "количеству",
                    "Manufacturer" => "производителю",
                    _ => _currentSortField
                };

                CurrentSortText.Text = $"Сортировка {fieldName} {sortDirection}";
                CurrentSortText.Foreground = System.Windows.Media.Brushes.DarkBlue;
            }
        }
    }
}
