using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Added
using System.Windows.Data;
using System.Windows.Controls;
using System.Globalization;
using System.Windows.Media;

namespace ClusteringEnsembleSuite.Code.Converters
{
    
    public sealed class LVAttributes_BGrConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            ListViewItem item = (ListViewItem)value;
            ListView listView = ItemsControl.ItemsControlFromItemContainer(item) as ListView;



            // Get the index of a ListViewItem
            int index = listView.ItemContainerGenerator.IndexFromContainer(item);

            if (index % 2 == 0)
            {
                //return Brushes.LightBlue;
                return new SolidColorBrush(Color.FromArgb(255, 240, 240, 255));
            }
            else
            {
                //return Brushes.Beige;
                return new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
