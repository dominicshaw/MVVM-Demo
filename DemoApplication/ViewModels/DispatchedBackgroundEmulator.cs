using System;
using System.Collections.ObjectModel;
using System.Threading;

namespace DemoApplication.ViewModels
{
    public class DispatchedBackgroundEmulator : BackgroundEmulator
    {
        public DispatchedBackgroundEmulator(ObservableCollection<VehicleViewModel> vehicles) : base(vehicles)
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