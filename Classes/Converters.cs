using System.Globalization;
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
}