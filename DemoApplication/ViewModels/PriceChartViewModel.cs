using System;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;

namespace DemoApplication.ViewModels
{
    public class PriceChartViewModel
    {
        public Func<double, string> XFormatter { get; }
        public SeriesCollection PriceHistory { get; }

        public PriceChartViewModel()
        {
            PriceHistory = new SeriesCollection(Mappers.Xy<PriceHistoryViewModel>()
                .X(dayModel => (double)dayModel.Time.Ticks / TimeSpan.FromHours(1).Ticks)
                .Y(dayModel => dayModel.Price));

            PriceHistory.Add(new LineSeries
            {
                Title = "Price (£)",
                Values = new ChartValues<PriceHistoryViewModel>()
            });

            XFormatter = value => new DateTime((long) (value < 0 ? 0 : value * TimeSpan.FromHours(1).Ticks)).ToString("HH:mm:ss");
        }
    }
}