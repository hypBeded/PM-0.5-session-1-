using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Data;           // Для IValueConverter
using System.Windows.Media;          // Для Brushes
using System.Windows.Media.Imaging;  // Для BitmapImage

namespace PresentationLogic.Converters
{
    public class StockStatusConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length == 0 || values[0] == null)
                return GetDefaultValue(parameter);

            if (values[0] is int stockQuantity)
            {
                bool isText = parameter?.ToString() == "text";

                if (isText)
                {
                    return stockQuantity > 0 ? "В наличии" : "Нет в наличии";
                }
                else
                {
                    return stockQuantity > 0 ?
                        new SolidColorBrush(Color.FromRgb(76, 175, 80)) : // Зеленый
                        new SolidColorBrush(Color.FromRgb(244, 67, 54));  // Красный
                }
            }

            return GetDefaultValue(parameter);
        }

        private object GetDefaultValue(object parameter)
        {
            bool isText = parameter?.ToString() == "text";
            return isText ? "Неизвестно" : Brushes.Gray;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
