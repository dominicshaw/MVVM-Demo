using System;
using Jot;
using Jot.Storage;
using Jot.Triggers;

namespace WpfApplication3
{
    static class Services
    {
        public static StateTracker Tracker { get; } = new StateTracker(new FileStore(Environment.SpecialFolder.UserProfile), new DesktopPersistTrigger());
    }
}