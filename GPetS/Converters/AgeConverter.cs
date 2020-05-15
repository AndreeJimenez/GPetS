using GPetS.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace GPetS.Converters
{
    public class AgeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime birthdate = (DateTime)value;
            if(value == null || string.IsNullOrEmpty(value.ToString()) || DateTime.Today < birthdate)
            {
                return "0";
            }
            int petAge = DateTime.Today.Year - birthdate.Year;
            if( birthdate.Month > DateTime.Today.Month)
            {
                --petAge;
            }
            return petAge;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}