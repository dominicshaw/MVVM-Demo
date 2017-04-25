using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfApplication3.Annotations;

namespace WpfApplication3.ViewModels
{
    public class WorkingViewModel : INotifyPropertyChanged
    {
        #region " singleton pattern "
        private static readonly WorkingViewModel _instance = new WorkingViewModel();
        static WorkingViewModel() { }
        private WorkingViewModel() { }
        public static WorkingViewModel Instance => _instance;
        #endregion

        private bool _working;

        public bool Working
        {
            get { return _working; }
            set
            {
                if (value == _working) return;
                _working = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}