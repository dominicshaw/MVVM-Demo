using System;
using System.Collections.ObjectModel;
using System.Threading;
using DemoApplication.ViewModels;
using log4net;

namespace DemoApplication.Emulators
{
    public abstract class BackgroundEmulator
    {
        protected static readonly object _sync = new object();

        protected readonly Random _random = new Random();
        protected readonly Timer _worker;
        protected readonly ILog _log;
        protected readonly ObservableCollection<VehicleViewModel> _vehicles;
        protected bool _backgroundThreadUpdates;
        protected double _frequency = CalcFreq(20);

        public double BackgroundThreadFrequency
        {
            set
            {
                lock (_sync)
                {
                    _frequency = CalcFreq(value);
                    _worker.Change(TimeSpan.FromMilliseconds(_frequency), Timeout.InfiniteTimeSpan);
                }
            }
        }

        private static double CalcFreq(double value)
        {
            return ((121.0 - value) / 60.0) * 1000;

            //return Interpolate(value, 1, 120, 60, 1);
        }

        private static double Interpolate(double x, double x0, double x1, double y0, double y1)
        {
            if ((x1 - x0) == 0)
            {
                return (y0 + y1) / 2;
            }
            return y0 + (x - x0) * (y1 - y0) / (x1 - x0);
        }

        public bool BackgroundThreadUpdates
        {
            get { return _backgroundThreadUpdates; }
            set
            {
                _backgroundThreadUpdates = value;

                if (_backgroundThreadUpdates)
                {
                    _worker.Change(TimeSpan.FromMilliseconds(_frequency), Timeout.InfiniteTimeSpan);
                }
                else
                {
                    _worker.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
                }
            }
        }

        protected BackgroundEmulator(ILog log, ObservableCollection<VehicleViewModel> vehicles)
        {
            _worker = new Timer(o => Change(), null, Timeout.Infinite, Timeout.Infinite);
            _log = log;
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