using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Elementary
{
    public class CategoryToColorConverter : IValueConverter
    {
        private static Dictionary<string, SolidColorBrush> _currentColors = new Dictionary<string, SolidColorBrush>();

        public static void LoadColorsFromJson(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var colors = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                _currentColors = new Dictionary<string, SolidColorBrush>();

                foreach (var color in colors)
                {
                    _currentColors[color.Key] = (SolidColorBrush)(new BrushConverter().ConvertFromString(color.Value));
                }
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string category = value as string;
            return _currentColors.TryGetValue(category, out var brush) ? brush : Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<KeyValuePair<string, SolidColorBrush>> GetCategoryColors()
        {
            return _currentColors;
        }

        public static SolidColorBrush GetBrushForCategory(string category)
        {
            return _currentColors.TryGetValue(category, out var brush) ? brush : Brushes.Transparent;
        }
    }
}
