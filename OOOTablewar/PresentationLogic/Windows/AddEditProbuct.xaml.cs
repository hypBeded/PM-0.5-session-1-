using DataBaseLogic;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
using static System.Net.Mime.MediaTypeNames;

namespace PresentationLogic.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEditProbuct.xaml
    /// </summary>
    public partial class AddEditProbuct : Window
    {
        private User userlog;
        byte[] imageBytes; //Временное хранение изображения в байтах
        public AddEditProbuct(User user)
        {
            InitializeComponent();
            userlog = user;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string art = Artikul.Text;
            string name = Name.Text;
            string izmer = "шт.";
            decimal price = Convert.ToDecimal(Price.Text);
            int maxDiscount = Convert.ToInt32(MaxDiscount.Text);
            string manufacturer = Manufacturer.Text;
            string supplier = Supplier.Text;
            string category = Category.Text;
            int currentDiscount = Convert.ToInt32(CurrentDiscount.Text);
            int stockQuantity = Convert.ToInt32(StockQuantity.Text);
            string description = Description.Text;
            byte[] image = imageBytes;
            using (var connect = Context.GetConnection())
            {
                try
                {;
                    var command = connect.CreateCommand();
                    command.CommandText = "Insert INTO Product" +
                        "(Артикул, Наименование,[Единица измерения],Стоимость,[Размер максимально возможной скидки],Производитель,Поставщик,[Категория товара],[Действующая скидка],[Кол-во на складе],Описание,Изображение) " +
                        "values  (@Artikul,@Name,@Izmer,@Price,@MaxDiscount,@Manufacturer,@Supplier,@Category,@CurrentDiscount,@StockQuantity,@Description,@Image)";

                    command.Parameters.AddWithValue("@Artikul", art);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Izmer", izmer);
                    command.Parameters.AddWithValue("@Price", price);
                    command.Parameters.AddWithValue("@MaxDiscount", maxDiscount);
                    command.Parameters.AddWithValue("@Manufacturer", manufacturer);
                    command.Parameters.AddWithValue("@Supplier", supplier);
                    command.Parameters.AddWithValue("@Category", category);
                    command.Parameters.AddWithValue("@CurrentDiscount", currentDiscount);
                    command.Parameters.AddWithValue("@StockQuantity", stockQuantity);
                    command.Parameters.AddWithValue("@Description", description);
                    command.Parameters.AddWithValue("@Image", image);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Товар добавлена");
                    Windows.Products products = new Windows.Products(userlog);
                    products.Show();
                    this.Close();

                }
                catch (Exception ex) //Обработка исключений
                {
                    MessageBox.Show("Убедитесь что вы заполнили все " + ex.Message);
                    return;
                }
            }

            }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.png;" // Фильтр .jpg и .png
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName; //Получение пути к изображению
                imageBytes = File.ReadAllBytes(filePath); //Преобразование изображения в байты

                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = stream;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze(); // Замораживаем изображение для использования в UI
                    Image.Source = bitmap; // Заменяем текущее изображение
                }
            }
        }
    }
}
