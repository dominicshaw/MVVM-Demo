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

                randomVehicle.Capacity = randomVehicle.Capacity + 1;

                // could Increment() here to also count these changes but no need. Ensure thread-safe if done in future.
            }
            finally
            {
                _worker.Change(TimeSpan.FromMilliseconds(_frequency), Timeout.InfiniteTimeSpan);
            }
        }
    }
}