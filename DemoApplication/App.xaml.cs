using System.Windows;
using DemoApplication.Repos;
using Ninject;

namespace DemoApplication
{
    public partial class App
    {
        private readonly StandardKernel _kernel = new StandardKernel();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _kernel.Bind<IRepository>().To<LiveRepository>().InSingletonScope();

            Start();
        }

        private void Start()
        {
            MainWindow = _kernel.Get<MainWindow>();
            MainWindow.Show();
            MainWindow.Focus();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _kernel.Dispose();

            base.OnExit(e);
        }
    }
}
