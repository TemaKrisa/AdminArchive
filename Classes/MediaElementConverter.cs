using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace AdminArchive.Classes;

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