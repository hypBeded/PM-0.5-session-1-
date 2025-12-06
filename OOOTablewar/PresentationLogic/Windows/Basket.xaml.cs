using DataBaseLogic;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLogic.Windows
{
    public partial class Basket : Window
    {
        public class BasketItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Manufacturer { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public string Image { get; set; }
            public decimal TotalPrice => Price * Quantity;
        }

        private ObservableCollection<BasketItem> basketItems = new ObservableCollection<BasketItem>();
        private User userlog;
        public Basket(User user)
        {
            userlog = user;
            InitializeComponent();
            BasketListView.ItemsSource = basketItems;
            UpdateTotal();
            FIO.Text = user.Name;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Загрузка тестовых данных (в реальном приложении здесь будет загрузка из БД)
            LoadSampleData();
        }

        private void LoadSampleData()
        {
            // Пример данных
            basketItems.Add(new BasketItem
            {
                Id = 1,
                Name = "Ноутбук ASUS",
                Description = "15.6 дюймов, Intel Core i5, 8GB RAM",
                Manufacturer = "ASUS",
                Price = 45000,
                Quantity = 1,
                Image = "/Images/laptop.jpg"
            });

            basketItems.Add(new BasketItem
            {
                Id = 2,
                Name = "Смартфон Samsung",
                Description = "6.2 дюйма, 128GB",
                Manufacturer = "Samsung",
                Price = 32000,
                Quantity = 2,
                Image = "/Images/phone.jpg"
            });

            UpdateTotal();
        }

        private void UpdateTotal()
        {
            int totalItems = basketItems.Sum(item => item.Quantity);
            decimal totalPrice = basketItems.Sum(item => item.TotalPrice);

            TotalItemsText.Text = totalItems.ToString();
            TotalPriceText.Text = totalPrice.ToString("C");
        }

        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int id)
            {
                var item = basketItems.FirstOrDefault(x => x.Id == id);
                if (item != null)
                {
                    item.Quantity++;
                    UpdateTotal();
                    // Обновляем отображение
                    BasketListView.Items.Refresh();
                }
            }
        }

        private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int id)
            {
                var item = basketItems.FirstOrDefault(x => x.Id == id);
                if (item != null && item.Quantity > 1)
                {
                    item.Quantity--;
                    UpdateTotal();
                    BasketListView.Items.Refresh();
                }
            }
        }

        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int id)
            {
                var item = basketItems.FirstOrDefault(x => x.Id == id);
                if (item != null)
                {
                    basketItems.Remove(item);
                    UpdateTotal();
                }
            }
        }

        private void ClearBasketButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Вы уверены, что хотите очистить корзину?",
                "Очистка корзины",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                basketItems.Clear();
                UpdateTotal();
            }
        }

        private void ContinueShoppingButton_Click(object sender, RoutedEventArgs e)
        {
            // Возвращаемся к окну продуктов
            var productsWindow = new Products(userlog);
            productsWindow.Show();
            this.Close();
        }

        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (!basketItems.Any())
            {
                MessageBox.Show("Корзина пуста!", "Оформление заказа",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Здесь логика оформления заказа
            MessageBox.Show($"Заказ оформлен!\n\n" +
                          $"Товаров: {basketItems.Sum(x => x.Quantity)}\n" +
                          $"Общая сумма: {basketItems.Sum(x => x.TotalPrice):C}",
                          "Заказ оформлен",
                          MessageBoxButton.OK,
                          MessageBoxImage.Information);

            // Очищаем корзину после оформления
            basketItems.Clear();
            UpdateTotal();
        }

        // Метод для добавления товара в корзину (может вызываться из окна продуктов)
        public void AddToBasket(int productId, string name, string description,
                               string manufacturer, decimal price, string image)
        {
            var existingItem = basketItems.FirstOrDefault(x => x.Id == productId);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                basketItems.Add(new BasketItem
                {
                    Id = productId,
                    Name = name,
                    Description = description,
                    Manufacturer = manufacturer,
                    Price = price,
                    Quantity = 1,
                    Image = image
                });
            }

            UpdateTotal();
            BasketListView.Items.Refresh();
        }
    }

    // Converter для форматирования цены (опционально)
    public class PriceConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is decimal price)
            {
                return price.ToString("C", culture);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}