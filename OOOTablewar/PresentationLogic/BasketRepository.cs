using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace PresentationLogic
{
    public class BasketRepository
    {
        private static ObservableCollection<BasketRepository.BasketItem> basketItems = new ObservableCollection<BasketRepository.BasketItem>();

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

            // Конструктор
            public BasketItem(int id, string name, string description, string manufacturer,
                            decimal price, int quantity, string image)
            {
                Id = id;
                Name = name;
                Description = description;
                Manufacturer = manufacturer;
                Price = price;
                Quantity = quantity;
                Image = image;
            }

            public BasketItem() { }
        }

        public ObservableCollection<BasketItem> GetAllItems()
        {
            return basketItems;
        }

        public void AddItem(int id, string name, string description, string manufacturer,
                           decimal price, int quantity, string image)
        {
            var existingItem = basketItems.FirstOrDefault(item => item.Id == id);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                basketItems.Add(new BasketItem(id, name, description, manufacturer, price, quantity, image));
            }
        }

        public void RemoveItem(int id)
        {
            var itemToRemove = basketItems.FirstOrDefault(item => item.Id == id);
            if (itemToRemove != null)
            {
                basketItems.Remove(itemToRemove);
            }
        }

        public void UpdateQuantity(int id, int newQuantity)
        {
            var item = basketItems.FirstOrDefault(item => item.Id == id);
            if (item != null)
            {
                item.Quantity = newQuantity;
            }
        }
        public void Clear()
        {
            basketItems.Clear();
        }

        public decimal GetTotalPrice()
        {
            return basketItems.Sum(item => item.TotalPrice);
        }

        public int GetTotalItemsCount()
        {
            return basketItems.Sum(item => item.Quantity);
        }

        public bool ContainsItem(int id)
        {
            return basketItems.Any(item => item.Id == id);
        }
    }
}