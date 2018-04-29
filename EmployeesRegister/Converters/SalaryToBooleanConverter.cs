using System;
using System.Globalization;
using System.Windows.Data;

namespace EmployeesRegister.Converters
{
    /// <summary>
    /// значения оклада в тыс.руб
    /// </summary>
    class SalaryToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (decimal.TryParse(value.ToString(), out decimal salary))
                return salary > 100;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
