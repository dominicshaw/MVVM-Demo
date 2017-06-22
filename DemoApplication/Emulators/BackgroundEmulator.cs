using System;
using System.Collections.ObjectModel;
using System.Threading;
using DemoApplication.ViewModels;
using log4net;

namespace DemoApplication.Emulators
{
    public abstract class BackgroundEmulator
    {
        public delegate void IncrementEventHandler(int counter);
        public event IncrementEventHandler Incremented;

        private static readonly object _sync = new object();

        private int _counter;
        private bool _backgroundThreadUpdates;

        private readonly Random _random = new Random();
        private readonly ObservableCollection<VehicleViewModel> _vehicles;

        protected readonly Timer _worker;
        protected readonly ILog _log;

        protected double _frequency = CalcFreq(20);

        public double BackgroundThreadFrequency
        {
            set
            {
                _frequency = CalcFreq(value);
                _worker.Change(TimeSpan.FromMilliseconds(_frequency), Timeout.InfiniteTimeSpan);
            }
        }

        private static double CalcFreq(double value)
        {
            return (60.0 / value) * 1000.0;
        }

        public bool BackgroundThreadUpdates
        {
            get { return _backgroundThreadUpdates; }
            set
            {
                if (_backgroundThreadUpdates == value)
                    return;

                _backgroundThreadUpdates = value;

                _worker.Change(
                    _backgroundThreadUpdates ? TimeSpan.FromMilliseconds(_frequency) : Timeout.InfiniteTimeSpan, 
                    Timeout.InfiniteTimeSpan);
            }
        }

        protected BackgroundEmulator(ILog log, ObservableCollection<VehicleViewModel> vehicles)
        {
            _worker = new Timer(o => Change(), null, Timeout.Infinite, Timeout.Infinite);
            _log = log;
            _vehicles = vehicles;
        }
        
        protected abstract void Change();

        protected void Increment()
        {
            Incremented?.Invoke(_counter++);
        }

        protected VehicleViewModel GetRandomVehicle()
        {
            lock (_sync)
            {
                return _vehicles[_random.Next(0, _vehicles.Count)];
            }
        }
    }
}