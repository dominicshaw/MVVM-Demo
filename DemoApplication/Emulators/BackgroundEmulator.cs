using System;
using System.Collections.ObjectModel;
using System.Threading;
using DemoApplication.ViewModels;

namespace DemoApplication.Emulators
{
    public abstract class BackgroundEmulator
    {
        protected static readonly object _sync = new object();

        protected readonly Random _random = new Random();
        protected readonly Timer _worker;
        protected readonly ObservableCollection<VehicleViewModel> _vehicles;
        protected bool _backgroundThreadUpdates;

        public bool BackgroundThreadUpdates
        {
            get { return _backgroundThreadUpdates; }
            set
            {
                _backgroundThreadUpdates = value;

                if (_backgroundThreadUpdates)
                {
                    _worker.Change(TimeSpan.FromMilliseconds(100), Timeout.InfiniteTimeSpan);
                }
                else
                {
                    _worker.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
                }
            }
        }

        protected BackgroundEmulator(ObservableCollection<VehicleViewModel> vehicles)
        {
            _worker = new Timer(o => Change(), null, Timeout.Infinite, Timeout.Infinite);
            _vehicles = vehicles;
        }
        
        protected abstract void Change();

        protected VehicleViewModel GetRandomVehicle()
        {
            lock (_sync)
            {
                return _vehicles[_random.Next(0, _vehicles.Count)];
            }
        }
    }
}