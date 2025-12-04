using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Data.Sqlite;

namespace PresentationLogic
{
    public class ProductViewModel
    {
        public class ProductsViewModel : INotifyPropertyChanged
        {
            private ObservableCollection<Product> _products;
            public ObservableCollection<Product> Products
            {
                get => _products;
                set
                {
                    _products = value;
                    OnPropertyChanged();
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    public static class ProductRepository
    {
        private static string connectionString = "Source=Tableware.db";

        public static List<Product> GetProducts()
        {
            var products = new List<Product>();

            try
            {
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    string query = @"
                        SELECT 
                            SELECT 
                            [Артикул],
                            [Наименование],
                            [Единица измерения],
                            [Стоимость],
                            [Размер максимально возможной скидки],
                            [Производитель],
                            [Поставщик],
                            [Категория товара],
                            [Действующая скидка],
                            [Кол-во на складе],
                            [Описание],
                            [Изображение]
                        FROM Product";

                    SqliteCommand command = new SqliteCommand(query, connection);
                    connection.Open();

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var product = new Product
                            {
                                Article = reader["Артикул"].ToString(),
                                Name = reader["Наименование"].ToString(),
                                Unit = reader["Единица измерения"].ToString(),
                                Price = Convert.ToDecimal(reader["Стоимость"]),
                                MaxDiscount = Convert.ToInt32(reader["Размер максимально возможной скидки"]),
                                Manufacturer = reader["Действующая скидка"].ToString(),
                                Supplier = reader["Поставщик"].ToString(),
                                Category = reader["Категория товара"].ToString(),
                                CurrentDiscount = Convert.ToInt32(reader["Действующая скидка"]),
                                StockQuantity = Convert.ToInt32(reader["Кол-во на складе"]),
                                Description = reader["Описание"].ToString()
                            };

                            // Обработка изображения (если оно хранится как varbinary)
                            if (!reader.IsDBNull(reader.GetOrdinal("Image")))
                            {
                                byte[] imageData = (byte[])reader["Image"];
                                product.Image = imageData;
                            }

                            products.Add(product);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }

            return products;
        }
    }
}
