using System;
using System.Collections.ObjectModel;
using System.Threading;
using DemoApplication.ViewModels;
using log4net;

namespace DemoApplication.Emulators
{
    public class Undispatched : BackgroundEmulator
    {
        public Undispatched(ILog log, ObservableCollection<VehicleViewModel> vehicles) : base(log, vehicles)
        {
        }

        protected override void Change()
        {
            try
            {
                if (!BackgroundThreadUpdates)
                    return;

                var randomVehicle = GetRandomVehicle();

                randomVehicle.Make = randomVehicle.Make + "x";
            }
            finally
            {
                _worker.Change(TimeSpan.FromMilliseconds(_frequency), Timeout.InfiniteTimeSpan);
            }
        }
    }
}