using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using DemoApplication.ViewModels;
using log4net;

namespace DemoApplication.Emulators
{
    public class Dispatched : BackgroundEmulator
    {
        public Dispatched(ILog log, ObservableCollection<VehicleViewModel> vehicles) : base(log, vehicles)
        {
        }

        private readonly Random _rand = new Random();

        protected override void Change()
        {
            try
            {
                if (!BackgroundThreadUpdates)
                    return;

                var randomVehicle = GetRandomVehicle();

                Application.Current.Dispatcher.BeginInvoke(
                    new Action(() =>
                    {
                        var adj = _rand.Next(10, 50);

                        if (adj % 2 == 0)
                            randomVehicle.Price = randomVehicle.Price + adj;
                        else
                            randomVehicle.Price = randomVehicle.Price - adj;

                        Increment();
                    }), DispatcherPriority.Normal);
            }
            finally
            {
                _worker.Change(TimeSpan.FromMilliseconds(_frequency), Timeout.InfiniteTimeSpan);
            }
        }
    }
}