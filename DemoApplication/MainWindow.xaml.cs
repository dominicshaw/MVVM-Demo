using DemoApplication.ViewModels;

namespace DemoApplication
{
    public partial class MainWindow
    {
        private readonly MainViewModel _model;

        public MainWindow(MainViewModel model)
        {
            InitializeComponent();

            Services.Tracker.Configure(this)
                .IdentifyAs("MainWindow")
                .AddProperties<MainWindow>(w => w.Height, w => w.Width, w => w.Left, w => w.Top, w => w.WindowState)
                .RegisterPersistTrigger(nameof(Closed))
                .Apply();

            _model = model;
            DataContext = _model;

            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
        }

        private async void MainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await _model.Load();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _model.BackgroundManager.BackgroundThreadUpdates = false;
            _model.BackgroundManager.BackgroundThreadFrequency = 1;
        }
    }
}
