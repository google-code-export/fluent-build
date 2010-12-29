using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace FluentBuild.BuildUI
{
    public class StateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch((TaskState)value)
            {
                case TaskState.Normal:
                    return Brushes.Green;
                case TaskState.Warning:
                    return new SolidColorBrush(Color.FromRgb(255, 178, 25));
                case TaskState.Error:
                    return Brushes.Red;
            }
           throw new NotImplementedException("Could not convert state to color");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
