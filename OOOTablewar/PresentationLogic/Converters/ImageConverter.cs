using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Data;           // Для IValueConverter
using System.Windows.Media;          // Для Brushes
using System.Windows.Media.Imaging;  // Для BitmapImage

namespace PresentationLogic.Converters
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is byte[] imageData && imageData.Length > 0)
                {
                    return ConvertByteArrayToBitmapImage(imageData);
                }

                return LoadDefaultImage();
            }
            catch
            {
                return LoadDefaultImage();
            }
        }

        private BitmapImage ConvertByteArrayToBitmapImage(byte[] imageData)
        {
            using (var memoryStream = new MemoryStream(imageData))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
        }

        private BitmapImage LoadDefaultImage()
        {
            try
            {
                // Пробуем загрузить из ресурсов
                var uri = new Uri("pack://application:,,,/Resources/picture.png", UriKind.Absolute);
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = uri;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
            catch
            {
                // Создаем простую заглушку
                return CreatePlaceholderImage();
            }
        }

        private BitmapImage CreatePlaceholderImage()
        {
            // Простая заглушка с темно-серым фоном и белым текстом
            int width = 200;
            int height = 150;

            var drawingVisual = new DrawingVisual();
            using (var drawingContext = drawingVisual.RenderOpen())
            {
                // Фон
                drawingContext.DrawRectangle(
                    new SolidColorBrush(Color.FromRgb(240, 240, 240)),
                    null,
                    new Rect(0, 0, width, height));

                // Текст
                var formattedText = new FormattedText(
                    "Нет изображения",
                    CultureInfo.CurrentUICulture,
                    FlowDirection.LeftToRight,
                    new Typeface("Arial"),
                    12,
                    Brushes.DarkGray,
                    1.0);

                drawingContext.DrawText(
                    formattedText,
                    new Point(
                        (width - formattedText.Width) / 2,
                        (height - formattedText.Height) / 2));
            }

            var renderTarget = new RenderTargetBitmap(
                width, height, 96, 96, PixelFormats.Pbgra32);
            renderTarget.Render(drawingVisual);

            var bitmapImage = new BitmapImage();
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderTarget));

            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);

                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }

            return bitmapImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
