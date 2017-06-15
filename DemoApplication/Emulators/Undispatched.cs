using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using DemoApplication.ViewModels;

namespace DemoApplication.Emulators
{
    public class Undispatched : BackgroundEmulator
    {
        public Undispatched(ObservableCollection<VehicleViewModel> vehicles) : base(vehicles)
        {
        }

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
                        randomVehicle.Make = randomVehicle.Make + "x";
                    }), DispatcherPriority.Normal);
            }
            finally
            {
                _worker.Change(TimeSpan.FromSeconds(6), Timeout.InfiniteTimeSpan);
            }
        }
    }
}