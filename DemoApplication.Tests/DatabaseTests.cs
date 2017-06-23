using System;
using System.Collections.ObjectModel;
using System.Threading;
using DemoApplication.Repositories;
using DemoApplication.ViewModels;
using Jot;
using log4net;
using Ninject;
using NUnit.Framework;

namespace DemoApplication.Tests
{
    [TestFixture, Apartment(ApartmentState.STA)]
    public class DatabaseTests : IDisposable
    {
        private readonly StandardKernel _kernel;

        public DatabaseTests()
        {
            _kernel = new StandardKernel();

            _kernel.Bind<ILog>().ToMethod(context => LogManager.GetLogger(context.Request.Target?.Member.DeclaringType?.FullName));
            _kernel.Bind<ObservableCollection<VehicleViewModel>>().ToSelf().InSingletonScope(); // one which is passed to dispatchers as well as mvm
            _kernel.Bind<IRepository>().To<SQLiteRepository>().InSingletonScope();
            _kernel.Bind<StateTracker>().ToSelf().InSingletonScope(); // only ever need one jot tracker
        }

        [Test]
        public void DatabaseHasRows()
        {
            var repo = _kernel.Get<IRepository>();
            repo.Initialise().GetAwaiter().GetResult();
            var vehicles = repo.GetAll().GetAwaiter().GetResult();
            Assert.IsNotEmpty(vehicles);
        }

        public void Dispose()
        {
            _kernel?.Dispose();
        }
    }
}
