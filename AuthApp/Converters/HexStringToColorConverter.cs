﻿using System;
using System.Globalization;
using Xamarin.Forms;

namespace AuthApp.Converters
{
    public class HexStringToColorConverter : IValueConverter
    {
        public HexStringToColorConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string valueAsString = value?.ToString();
            switch (valueAsString)
            {
                case (null):
                case (""):
                    {
                        return Color.Default;
                    }
                case ("Accent"):
                    {
                        return Color.Accent;
                    }
                default:
                    {
                        return Color.FromHex(value.ToString());
                    }
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { return null; }
    }
}
