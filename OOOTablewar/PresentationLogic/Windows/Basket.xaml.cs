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
        // Удаляем внутренний класс BasketItem и статическую коллекцию
        // Вместо них используем репозиторий
        private BasketRepository basketRepository = new BasketRepository();
        private ObservableCollection<BasketRepository.BasketItem> basketItems;
        private User userlog;

        public Basket(User user)
        {
            userlog = user;
            InitializeComponent();

            // Получаем коллекцию из репозитория
            basketItems = basketRepository.GetAllItems();
            BasketListView.ItemsSource = basketItems;

            UpdateTotal();
            FIO.Text = user.Name;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSampleData();
        }

        private void LoadSampleData()
        {
            // Используем репозиторий для добавления данных
            basketRepository.AddItem(
                1,
                "Ноутбук ASUS",
                "15.6 дюймов, Intel Core i5, 8GB RAM",
                "ASUS",
                45000,
                1,
                "/Images/laptop.jpg"
            );

            basketRepository.AddItem(
                2,
                "Смартфон Samsung",
                "6.2 дюйма, 128GB",
                "Samsung",
                32000,
                2,
                "/Images/phone.jpg"
            );

            UpdateTotal();
        }

        private void UpdateTotal()
        {

            int totalItems = basketRepository.GetTotalItemsCount();
            decimal totalPrice = basketRepository.GetTotalPrice();

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
                    basketRepository.UpdateQuantity(id, item.Quantity + 1);
                    UpdateTotal();
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
                    basketRepository.UpdateQuantity(id, item.Quantity - 1);
                    UpdateTotal();
                    BasketListView.Items.Refresh();
                }
                else if (item != null && item.Quantity == 1)
                {
                    basketRepository.RemoveItem(id);
                    UpdateTotal();
                }
            }
        }

        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int id)
            {
                basketRepository.RemoveItem(id);
                UpdateTotal();
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
                basketRepository.Clear();
                UpdateTotal();
            }
        }

        private void ContinueShoppingButton_Click(object sender, RoutedEventArgs e)
        {
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

            MessageBox.Show($"Заказ оформлен!\n\n" +
                          $"Товаров: {basketRepository.GetTotalItemsCount()}\n" +
                          $"Общая сумма: {basketRepository.GetTotalPrice():C}",
                          "Заказ оформлен",
                          MessageBoxButton.OK,
                          MessageBoxImage.Information);

            basketRepository.Clear();
            UpdateTotal();
        }
        public void AddToBasket(int productId, string name, string description,
                               string manufacturer, decimal price, string image)
        {
            basketRepository.AddItem(productId, name, description, manufacturer, price, 1, image);
            UpdateTotal();
            BasketListView.Items.Refresh();
        }

        public static void AddProductToBasket(int productId, string name, string description,
                                             string manufacturer, decimal price, string image)
        {
            var repository = new BasketRepository();
            repository.AddItem(productId, name, description, manufacturer, price, 1, image);
        }
    }
}