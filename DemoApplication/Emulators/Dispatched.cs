using System;
using System.Collections.ObjectModel;
using System.Threading;
using DemoApplication.ViewModels;

namespace DemoApplication.Emulators
{
    public class Dispatched : BackgroundEmulator
    {
        public Dispatched(ObservableCollection<VehicleViewModel> vehicles) : base(vehicles)
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
                _worker.Change(TimeSpan.FromSeconds(7), Timeout.InfiniteTimeSpan);
            }
        }
    }
}