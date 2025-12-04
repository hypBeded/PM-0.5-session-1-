using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLogic
{
    public static class DatabaseContext
    {
        private static string _connectionString;
        private static string _databaseName = "DBTableware.db";

        public static void Initialize(string baseDirectory)
        {
            string databasePath = FindDatabaseFile(baseDirectory);

            if (!File.Exists(databasePath))
            {
                throw new FileNotFoundException($"База данных не найдена: {databasePath}");
            }

            _connectionString = $"Data Source={databasePath};Version=3;";
        }

        private static string FindDatabaseFile(string baseDirectory)
        {
            // Ищем файл базы данных в разных местах
            string[] possiblePaths =
            {
                Path.Combine(baseDirectory, _databaseName),                    // В корне приложения
                Path.Combine(baseDirectory, "Data", _databaseName),            // В папке Data
                Path.Combine(baseDirectory, "Database", _databaseName),        // В папке Database
                Path.Combine(baseDirectory, "AppData", _databaseName),         // В папке AppData
                Path.Combine(baseDirectory, "Resources", _databaseName),       // В папке Resources
                Path.Combine(baseDirectory, "bin", "Debug", _databaseName),    // В папке Debug
                Path.Combine(baseDirectory, "bin", "Release", _databaseName),  // В папке Release
                _databaseName                                                   // Просто по имени
            };

            foreach (var path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    return path;
                }
            }

            throw new FileNotFoundException($"Файл базы данных '{_databaseName}' не найден в следующих местах:\n{string.Join("\n", possiblePaths)}");
        }

        public static List<Product> GetAllProducts()
        {
            var products = new List<Product>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

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
                    FROM [Product]";

                using (var command = new SqliteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var product = new Product
                        {
                            Article = reader["Артикул"]?.ToString() ?? string.Empty,
                            Name = reader["Наименование"]?.ToString() ?? string.Empty,
                            Unit = reader["Единица измерения"]?.ToString() ?? string.Empty,
                            Manufacturer = reader["Производитель"]?.ToString() ?? string.Empty,
                            Supplier = reader["Поставщик"]?.ToString() ?? string.Empty,
                            Category = reader["Категория товара"]?.ToString() ?? string.Empty,
                            Description = reader["Описание"]?.ToString() ?? string.Empty
                        };

                        // Парсинг числовых значений
                        if (decimal.TryParse(reader["Стоимость"]?.ToString(), out decimal price))
                        {
                            product.Price = price;
                        }

                        if (int.TryParse(reader["Размер максимально возможной скидки"]?.ToString(), out int maxDiscount))
                        {
                            product.MaxDiscount = maxDiscount;
                        }

                        if (int.TryParse(reader["Действующая скидка"]?.ToString(), out int currentDiscount))
                        {
                            product.CurrentDiscount = currentDiscount;
                        }

                        if (int.TryParse(reader["Кол-во на складе"]?.ToString(), out int stockQuantity))
                        {
                            product.StockQuantity = stockQuantity;
                        }

                        // Обработка изображения
                        int imageColumnIndex = reader.GetOrdinal("Изображение");
                        if (!reader.IsDBNull(imageColumnIndex))
                        {
                            product.Image = (byte[])reader["Изображение"];
                        }

                        products.Add(product);
                    }
                }
            }

            return products;
        }

        public static async System.Threading.Tasks.Task<List<Product>> GetAllProductsAsync()
        {
            var products = new List<Product>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

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
                    FROM [Product]";

                using (var command = new SqliteCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var product = new Product
                        {
                            Article = reader["Артикул"]?.ToString() ?? string.Empty,
                            Name = reader["Наименование"]?.ToString() ?? string.Empty,
                            Unit = reader["Единица измерения"]?.ToString() ?? string.Empty,
                            Manufacturer = reader["Производитель"]?.ToString() ?? string.Empty,
                            Supplier = reader["Поставщик"]?.ToString() ?? string.Empty,
                            Category = reader["Категория товара"]?.ToString() ?? string.Empty,
                            Description = reader["Описание"]?.ToString() ?? string.Empty
                        };

                        // Парсинг числовых значений
                        if (decimal.TryParse(reader["Стоимость"]?.ToString(), out decimal price))
                        {
                            product.Price = price;
                        }

                        if (int.TryParse(reader["Размер максимально возможной скидки"]?.ToString(), out int maxDiscount))
                        {
                            product.MaxDiscount = maxDiscount;
                        }

                        if (int.TryParse(reader["Действующая скидка"]?.ToString(), out int currentDiscount))
                        {
                            product.CurrentDiscount = currentDiscount;
                        }

                        if (int.TryParse(reader["Кол-во на складе"]?.ToString(), out int stockQuantity))
                        {
                            product.StockQuantity = stockQuantity;
                        }

                        // Обработка изображения
                        int imageColumnIndex = reader.GetOrdinal("Изображение");
                        if (!reader.IsDBNull(imageColumnIndex))
                        {
                            product.Image = (byte[])reader["Изображение"];
                        }

                        products.Add(product);
                    }
                }
            }

            return products;
        }
    }
}
