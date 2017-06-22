using System;

namespace DemoApplication.ViewModels
{
    public class PriceHistoryViewModel
    {
        public double Price { get; }
        public DateTime Time { get; }

        public PriceHistoryViewModel(double price, DateTime time)
        {
            Price = price;
            Time = time;
        }
    }
}