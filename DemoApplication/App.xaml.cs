using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using DemoApplication.Repositories;
using DemoApplication.ViewModels;
using Jot;
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
            _kernel.Bind<ObservableCollection<VehicleViewModel>>().ToSelf().InSingletonScope(); // one which is passed to dispatchers as well as mvm
            _kernel.Bind<IRepository>().To<SQLiteRepository>().InSingletonScope();
            _kernel.Bind<StateTracker>().ToSelf().InSingletonScope(); // only ever need one jot tracker

            InitialiseLogs();
            Start();

            var log = LogManager.GetLogger(GetType());

            Current.DispatcherUnhandledException +=
                (s, ex) => log.Fatal("Dispatcher Unhandled Exception: {0}", ex.Exception);
            AppDomain.CurrentDomain.UnhandledException +=
                (s, ex) => log.Fatal($"AppDomain.CurrentDomain Exception: {ex.ExceptionObject}");
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
            GlobalContext.Properties["version"] = Assembly.GetExecutingAssembly().GetName().Version;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _kernel.Dispose();

            base.OnExit(e);
        }
    }
}
