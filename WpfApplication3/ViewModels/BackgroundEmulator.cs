using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace DemoApplication.ViewModels
{
    public class BackgroundEmulator
    {
        private readonly object _sync = new object();
        private readonly Random _random = new Random();
        private readonly Timer _worker;
        private readonly Timer _dispatchedWorker;
        private readonly ObservableCollection<Vehicle> _vehicles;
        private bool _backgroundThreadUpdates;

        public bool BackgroundThreadUpdates
        {
            get { return _backgroundThreadUpdates; }
            set
            {
                _backgroundThreadUpdates = value;

                if (_backgroundThreadUpdates)
                {
                    _worker.Change(TimeSpan.FromMilliseconds(100), Timeout.InfiniteTimeSpan);
                    _dispatchedWorker.Change(TimeSpan.FromMilliseconds(100), Timeout.InfiniteTimeSpan);
                }
                else
                {
                    _worker.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
                    _dispatchedWorker.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
                }
            }
        }

        public BackgroundEmulator(ObservableCollection<Vehicle> vehicles)
        {
            _worker = new Timer(o => BackgroundChange(), null, Timeout.Infinite, Timeout.Infinite);
            _dispatchedWorker = new Timer(o => DispatchedChange(), null, Timeout.Infinite, Timeout.Infinite);

            _vehicles = vehicles;
        }

        private void DispatchedChange()
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
                _dispatchedWorker.Change(6000, Timeout.Infinite);
            }
        }

        private void BackgroundChange()
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
                _worker.Change(7000, Timeout.Infinite);
            }
        }

        private Vehicle GetRandomVehicle()
        {
            lock (_sync)
            {
                return _vehicles[_random.Next(0, _vehicles.Count - 1)];
            }
        }

    }
}