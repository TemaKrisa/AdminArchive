using System.Globalization;
using System.Windows.Data;

namespace AdminArchive.Classes
{
    internal class BooleanConverter : IValueConverter
    {
        //public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        //{
        //    if (value is Boolean && (bool)value)
        //    {
        //        return Visibility.Visible;
        //    }
        //    return Visibility.Collapsed;
        //}

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && parameter != null)
            {
                if (value.ToString() == parameter.ToString())
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool && ((bool)value) == true;
        }
    }
}
