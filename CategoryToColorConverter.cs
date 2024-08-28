using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Elementary
{
    public class CategoryToColorConverter : IValueConverter
    {
        private static readonly Dictionary<string, SolidColorBrush> CategoryColors = new Dictionary<string, SolidColorBrush>
        {
            { "Nonmetal", Brushes.MediumSlateBlue},
            { "Alkali Metal", Brushes.PeachPuff },
            { "Alkaline Earth Metal", Brushes.Wheat },
            { "Transition Metal", Brushes.Khaki},
            { "Lanthanide", Brushes.GreenYellow},
            { "Actinide", Brushes.PaleGreen},
            
            { "Post-transition Metal", Brushes.Gold },
            { "Metalloid", Brushes.LightGreen },            
            { "Noble Gas", Brushes.LightBlue },
            { "Halogen", Brushes.Violet },
            // Agrega más categorías según sea necesario
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string category = value as string;
            return CategoryColors.TryGetValue(category, out var brush) ? brush : Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<KeyValuePair<string, SolidColorBrush>> GetCategoryColors()
        {
            return CategoryColors;
        }
    }
}
