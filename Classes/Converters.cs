using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;

namespace AdminArchive.Classes
{
    internal class BooleanConverter : IValueConverter
    {
        // Метод Convert преобразует значение value в тип bool и возвращает true, если оно равно параметру parameter, иначе - false.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && parameter != null)
            {
                if (value.ToString() == parameter.ToString()) return true;
                return false;
            }
            return false;
        }
        // Метод ConvertBack преобразует значение value типа bool обратно в исходный тип данных.
        // Если значение равно true, то метод возвращает true, иначе - false.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        { return value is bool boolean && boolean == true; }
    }
    internal class ReverseBooleanConverter : IValueConverter
    {
        // Метод Convert преобразует значение value в тип bool и возвращает true, если оно равно параметру parameter, иначе - false.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && parameter != null)
            {
                if (value.ToString() == parameter.ToString()) return false;
                return true;
            }
            return false;
        }
        // Метод ConvertBack преобразует значение value типа bool обратно в исходный тип данных.
        // Если значение равно true, то метод возвращает true, иначе - false.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        { return value is bool boolean && boolean == true; }
    }
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = (bool)value;
            if (boolValue) return Visibility.Visible;
            else return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibilityValue = (Visibility)value;
            if (visibilityValue == Visibility.Visible) return true;
            else return false;
        }
    }
    public class MediaElementConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is byte[] fileBytes && values[1] is string fileName)
            {
                var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();

                if (fileExtension == ".mp3" || fileExtension == ".wav" || fileExtension == ".mp4" || fileExtension == ".avi" || fileExtension == ".wmv")
                {
                    var base64String = System.Convert.ToBase64String(fileBytes);
                    var uriString = $"data:video/{fileExtension};base64,{base64String}";
                    return new Uri(uriString);
                }
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
