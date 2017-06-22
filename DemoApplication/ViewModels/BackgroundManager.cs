using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using DemoApplication.Emulators;
using DemoApplication.Properties;

namespace DemoApplication.ViewModels
{
    public class BackgroundManager
    {
        private readonly BackgroundEmulator _dispatchedWorker;
        private readonly BackgroundEmulator _undispatchedWorker;
        private bool _backgroundThreadUpdates;

        private int _backgroundThreadFrequency = 20;
        private int _backgroundTreadIncrement;
        private int _backgroundThreadRunning;

        private readonly Timer _runningFor;

        public BackgroundManager(Dispatched dispatched, Undispatched undispatched)
        {
            _dispatchedWorker = dispatched;
            _undispatchedWorker = undispatched;

            _dispatchedWorker.Incremented += _dispatchedWorker_Incremented;

            _runningFor = new Timer(_ => Increment(), null, Timeout.Infinite, Timeout.Infinite);
        }

        private void Increment()
        {
            BackgroundThreadRunning = BackgroundThreadRunning + 1;
        }

        public bool BackgroundThreadUpdates
        {
            get { return _backgroundThreadUpdates; }
            set
            {
                if (value == _backgroundThreadUpdates) return;

                _backgroundThreadUpdates = value;
                OnPropertyChanged();

                SetState();
            }
        }

        private void SetState()
        {
            _dispatchedWorker.BackgroundThreadUpdates = _backgroundThreadUpdates;
            _undispatchedWorker.BackgroundThreadUpdates = _backgroundThreadUpdates;

            if (_backgroundThreadUpdates)
            {
                BackgroundTreadIncrement = 0;
                BackgroundThreadRunning = 0;

                _runningFor.Change(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            }
            else
            {
                _runningFor.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        public int BackgroundThreadFrequency
        {
            get { return _backgroundThreadFrequency; }
            set
            {
                _backgroundThreadFrequency = value;

                _dispatchedWorker.BackgroundThreadFrequency = value;
                _undispatchedWorker.BackgroundThreadFrequency = value;

                OnPropertyChanged();
            }
        }

        public int BackgroundTreadIncrement
        {
            get { return _backgroundTreadIncrement; }
            set
            {
                _backgroundTreadIncrement = value;
                OnPropertyChanged();
            }
        }

        public int BackgroundThreadRunning
        {
            get { return _backgroundThreadRunning; }
            set
            {
                _backgroundThreadRunning = value;
                OnPropertyChanged();
            }
        }

        private void _dispatchedWorker_Incremented(int counter)
        {
            BackgroundTreadIncrement++;
        }

        #region " inpc "
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}