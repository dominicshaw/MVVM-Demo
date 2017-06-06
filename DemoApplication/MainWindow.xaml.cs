using DemoApplication.ViewModels;

namespace DemoApplication
{
    public partial class MainWindow
    {
        private readonly MainViewModel _model;

        public MainWindow()
        {
            InitializeComponent();

            Services.Tracker.Configure(this)
                .IdentifyAs("MainWindow")
                .AddProperties<MainWindow>(w => w.Height, w => w.Width, w => w.Left, w => w.Top, w => w.WindowState)
                .RegisterPersistTrigger(nameof(Closed))
                .Apply();

            _model = new MainViewModel();
            DataContext = _model;

            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await _model.Load();
        }
    }
}
