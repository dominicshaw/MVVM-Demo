using System;
using System.Windows;
using DemoApplication.Repos;
using DemoApplication.ViewModels;
using log4net;
using Ninject;

namespace DemoApplication
{
    public partial class App
    {
        private readonly StandardKernel _kernel = new StandardKernel();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _kernel.Bind<ILog>().ToMethod(context => LogManager.GetLogger(context.Request.Target?.Member.DeclaringType?.FullName));
            _kernel.Bind<IRepository>().To<SQLiteRepository>().InSingletonScope();

            _kernel.Bind<VehicleViewModel>().To<CarViewModel>().Named("Car");
            _kernel.Bind<VehicleViewModel>().To<TruckViewModel>().Named("Truck");

            InitialiseLogs();
            Start();
        }

        private void Start()
        {
            MainWindow = _kernel.Get<MainWindow>();

            MainWindow.Show();
            MainWindow.Focus();
        }

        private static void InitialiseLogs()
        {
            GlobalContext.Properties["username"] = Environment.UserName;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _kernel.Dispose();

            base.OnExit(e);
        }
    }
}
