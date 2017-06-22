using System;

namespace DemoApplication.ViewModels
{
    public class PriceHistoryViewModel
    {
        public decimal Price { get; }
        public DateTime Time { get; }

        public PriceHistoryViewModel(decimal price, DateTime time)
        {
            Price = price;
            Time = time;
        }
    }
}