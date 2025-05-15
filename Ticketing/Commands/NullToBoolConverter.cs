// Modification du namespace du convertisseur NullToBoolConverter pour cohérence
using System;
using System.Globalization;
using System.Windows.Data;

namespace Ticketing.Converters
{
    public class NullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}