using System;
using System.Collections.ObjectModel;
using System.Threading;
using DemoApplication.ViewModels;
using log4net;

namespace DemoApplication.Emulators
{
    public class Dispatched : BackgroundEmulator
    {
        public Dispatched(ILog log, ObservableCollection<VehicleViewModel> vehicles) : base(log, vehicles)
        {
        }

        protected override void Change()
        {
            try
            {
                if (!BackgroundThreadUpdates)
                    return;

                var randomVehicle = GetRandomVehicle();

                randomVehicle.Capacity = randomVehicle.Capacity + 1;
            }
            finally
            {
                _worker.Change(TimeSpan.FromMilliseconds(_frequency), Timeout.InfiniteTimeSpan);
            }
        }
    }
}