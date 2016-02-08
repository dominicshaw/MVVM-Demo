using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfApplication3.Annotations;

namespace WpfApplication3
{
    public abstract class Vehicle : INotifyPropertyChanged
    {
        private string _type;
        private string _make;
        private string _model;
        private int _capacity;

        public string Type
        {
            get { return _type; }
            set
            {
                if (value == _type) return;
                _type = value;
                OnPropertyChanged();
            }
        }

        public string Make
        {
            get { return _make; }
            set
            {
                if (value == _make) return;
                _make = value;
                OnPropertyChanged();
            }
        }

        public string Model
        {
            get { return _model; }
            set
            {
                if (value == _model) return;
                _model = value;
                OnPropertyChanged();
            }
        }

        public int Capacity
        {
            get { return _capacity; }
            set
            {
                if (value == _capacity) return;
                _capacity = value;
                OnPropertyChanged();
            }
        }

        protected Vehicle(string type)
        {
            Type = type;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}