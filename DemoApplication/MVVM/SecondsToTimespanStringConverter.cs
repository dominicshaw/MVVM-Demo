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
                answer = string.Format("{0}s", t.Seconds);
            else if (t.TotalHours < 1.0)
                answer = string.Format("{0}m:{1:D2}s", t.Minutes, t.Seconds);
            else
                answer = string.Format("{0}h:{1:D2}m:{2:D2}s", (int) t.TotalHours, t.Minutes, t.Seconds);

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