using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;           // Для IValueConverter
using System.Windows.Media;          // Для Brushes
using System.Windows.Media.Imaging;  // Для BitmapImage
using System.IO;
namespace PresentationLogic.Converters
{
    public class PriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal price)
            {
                if (parameter != null && int.TryParse(parameter.ToString(), out int discount) && discount > 0)
                {
                    decimal discountedPrice = price * (100 - discount) / 100;
                    return discountedPrice.ToString("N2", culture);
                }
                return price.ToString("N2", culture);
            }
            return "0.00";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
