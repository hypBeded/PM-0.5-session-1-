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
    public class ProductsViewModel : INotifyPropertyChanged
        {
        public ProductsViewModel()
        {
            LoadProducts();
        }

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
        
        private void LoadProducts()
        {
            var productsList = ProductRepository.GetProducts();
            Products = new ObservableCollection<Product>(productsList);
        }

        public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
    }
    
    public static class ProductRepository
    {
        private static string connectionString = "Data Source=DBTableware.db";

        public static List<Product> GetProducts()
        {
            var products = new List<Product>();

            try
            {
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    string query = @"
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
                                // Используем безопасные методы преобразования
                                Article = SafeGetString(reader, "Артикул"),
                                Name = SafeGetString(reader, "Наименование"),
                                Unit = SafeGetString(reader, "Единица измерения"),
                                Price = SafeGetDecimal(reader, "Стоимость"),
                                MaxDiscount = SafeGetInt(reader, "Размер максимально возможной скидки"),
                                Manufacturer = SafeGetString(reader, "Производитель"), // Исправлено: было "Действующая скидка"
                                Supplier = SafeGetString(reader, "Поставщик"),
                                Category = SafeGetString(reader, "Категория товара"),
                                CurrentDiscount = SafeGetInt(reader, "Действующая скидка"),
                                StockQuantity = SafeGetInt(reader, "Кол-во на складе"),
                                Description = SafeGetString(reader, "Описание")
                            };

                            // Обработка изображения
                            product.Image = SafeGetBytes(reader, "Изображение");

                            products.Add(product);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}\n\nПодробности: {ex.InnerException?.Message}");
            }

            return products;
        }

        // Вспомогательные методы для безопасного преобразования

        private static string SafeGetString(SqliteDataReader reader, string columnName)
        {
            try
            {
                int columnIndex = reader.GetOrdinal(columnName);
                if (!reader.IsDBNull(columnIndex))
                    return reader[columnName].ToString();
            }
            catch (IndexOutOfRangeException)
            {
                // Столбец не найден
                return string.Empty;
            }
            catch (Exception)
            {
                // Другие ошибки
                return string.Empty;
            }
            return string.Empty;
        }

        private static decimal SafeGetDecimal(SqliteDataReader reader, string columnName)
        {
            try
            {
                int columnIndex = reader.GetOrdinal(columnName);
                if (!reader.IsDBNull(columnIndex))
                {
                    object value = reader[columnName];
                    if (value != null && value != DBNull.Value)
                    {
                        return Convert.ToDecimal(value);
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                // Столбец не найден
                return 0;
            }
            catch (FormatException)
            {
                // Ошибка преобразования
                return 0;
            }
            catch (InvalidCastException)
            {
                // Неправильное приведение типа
                return 0;
            }
            return 0;
        }

        private static int SafeGetInt(SqliteDataReader reader, string columnName)
        {
            try
            {
                int columnIndex = reader.GetOrdinal(columnName);
                if (!reader.IsDBNull(columnIndex))
                {
                    object value = reader[columnName];
                    if (value != null && value != DBNull.Value)
                    {
                        return Convert.ToInt32(value);
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                // Столбец не найден
                return 0;
            }
            catch (FormatException)
            {
                // Ошибка преобразования
                return 0;
            }
            catch (InvalidCastException)
            {
                // Неправильное приведение типа
                return 0;
            }
            return 0;
        }

        private static byte[] SafeGetBytes(SqliteDataReader reader, string columnName)
        {
            try
            {
                int columnIndex = reader.GetOrdinal(columnName);
                if (!reader.IsDBNull(columnIndex))
                {
                    object value = reader[columnName];
                    if (value != null && value != DBNull.Value)
                    {
                        return (byte[])value;
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                // Столбец не найден
                return null;
            }
            catch (InvalidCastException)
            {
                // Неправильное приведение типа
                return null;
            }
            return null;
        }
    }
}
