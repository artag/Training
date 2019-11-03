using System;
using System.ComponentModel;
using System.Globalization;

namespace WiredBrainCoffee.CustomersApp.Wpf2.Model
{
    public class CustomerConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(
            ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string inputString)
            {
                var values = inputString.Split(',');
                return new Customer
                {
                    FirstName = values[0],
                    LastName = values[1],
                    IsDeveloper = bool.Parse(values[2]),
                };
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
