using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLogic
{
    public class Product : INotifyPropertyChanged
    {
        private string _article;
        private string _name;
        private string _unit;
        private decimal _price;
        private int _maxDiscount;
        private string _manufacturer;
        private string _supplier;
        private string _category;
        private int _currentDiscount;
        private int _stockQuantity;
        private string _description;
        private byte[] _image;

        public string Article
        {
            get => _article;
            set { _article = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string Unit
        {
            get => _unit;
            set { _unit = value; OnPropertyChanged(); }
        }

        public decimal Price
        {
            get => _price;
            set { _price = value; OnPropertyChanged(); }
        }

        public int MaxDiscount
        {
            get => _maxDiscount;
            set { _maxDiscount = value; OnPropertyChanged(); }
        }

        public string Manufacturer
        {
            get => _manufacturer;
            set { _manufacturer = value; OnPropertyChanged(); }
        }

        public string Supplier
        {
            get => _supplier;
            set { _supplier = value; OnPropertyChanged(); }
        }

        public string Category
        {
            get => _category;
            set { _category = value; OnPropertyChanged(); }
        }

        public int CurrentDiscount
        {
            get => _currentDiscount;
            set { _currentDiscount = value; OnPropertyChanged(); }
        }

        public int StockQuantity
        {
            get => _stockQuantity;
            set { _stockQuantity = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        public byte[] Image
        {
            get => _image;
            set { _image = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
