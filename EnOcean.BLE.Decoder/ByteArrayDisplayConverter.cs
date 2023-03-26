using System;
using System.Globalization;
using System.Windows.Data;

namespace EnOcean.BLE.Decoder
{
    public class ByteArrayDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is byte[] bytes)
            {
                return System.Convert.ToHexString(bytes);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
