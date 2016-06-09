namespace WpfApplication3
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Services.Tracker.Configure(this)
                .IdentifyAs("MainWindow")
                .AddProperties<MainWindow>(w => w.Height, w => w.Width, w => w.Left, w => w.Top, w => w.WindowState)
                .RegisterPersistTrigger(nameof(Closed))
                .Apply();
        }
    }
}
