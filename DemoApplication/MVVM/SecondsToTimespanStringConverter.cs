using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace DemoApplication.MVVM
{
    [ValueConversion(typeof(int), typeof(string))]
    public class SecondsToTimespanStringConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string))
                throw new InvalidOperationException("The target must be System.Windows.Visibility");

            var seconds = value as int?;

            if (seconds == null)
                return string.Empty;

            var t = TimeSpan.FromSeconds(seconds.Value);
            string answer;
            if (t.TotalMinutes < 1.0)
                answer = $"{t.Seconds}s";
            else if (t.TotalHours < 1.0)
                answer = $"{t.Minutes}m:{t.Seconds:D2}s";
            else
                answer = $"{(int) t.TotalHours}h:{t.Minutes:D2}m:{t.Seconds:D2}s";

            return answer;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}