using DataBaseLogic;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
namespace BusinessLogic
{
    public class ProductService
    {
        public static void InitializeDatabase(string baseDirectory)
        {
            DatabaseContext.Initialize(baseDirectory);
        }

        public static List<Product> GetAllProducts()
        {
            return DatabaseContext.GetAllProducts();
        }

        public static async Task<List<Product>> GetAllProductsAsync()
        {
            return await DatabaseContext.GetAllProductsAsync();
        }

        public static bool HasImage(Product product)
        {
            return product?.Image != null && product.Image.Length > 0;
        }
    }
}
