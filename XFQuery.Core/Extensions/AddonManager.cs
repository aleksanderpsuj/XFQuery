using System.Collections.Generic;
using TS3QueryLib.Net.Core.Server.Notification;
using XFQuery.Core.Logging;

namespace XFQuery.Core.Extensions
{
    public sealed class AddonManager
    {
        public AddonManager()
        {
            Addons = new List<Addon>();
            Logger = Logger;
        }

        public IList<Addon> Addons { get; }
        private ILogger Logger { get; }

        public void AddAddon(Addon addon)
        {
            Addons.Add(addon);
        }


        public void RegisterNotifications(NotificationHub notifications)
        {
            foreach (var e in Addons) e.RegisterNotifications(notifications);
        }
    }
}