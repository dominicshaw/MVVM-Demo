using System;
using System.Collections.ObjectModel;
using System.Threading;
using DemoApplication.Repositories;
using DemoApplication.ViewModels;
using log4net;
using Ninject;
using NUnit.Framework;

namespace DemoApplication.Tests
{
    [TestFixture]
    public class DatabaseTests : IDisposable
    {
        private readonly StandardKernel _kernel;

        public DatabaseTests()
        {
            _kernel = new StandardKernel();

            _kernel.Bind<ILog>().ToMethod(context => LogManager.GetLogger(context.Request.Target?.Member.DeclaringType?.FullName));
            _kernel.Bind<ObservableCollection<VehicleViewModel>>().ToSelf().InSingletonScope(); // one which is passed to dispatchers as well as mvm
            _kernel.Bind<IRepository>().To<SQLiteRepository>().InSingletonScope();
        }

        [Test, Apartment(ApartmentState.STA)]
        public void DatabaseHasRows()
        {
            var mvm = _kernel.Get<MainViewModel>();

            mvm.Load().GetAwaiter().GetResult();

            Assert.IsNotEmpty(mvm.Vehicles);
        }

        public void Dispose()
        {
            _kernel?.Dispose();
        }
    }
}
