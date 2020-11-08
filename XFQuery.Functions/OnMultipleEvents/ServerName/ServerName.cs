using System;
using System.Timers;
using TS3QueryLib.Net.Core.Server.Notification;
using XFQuery.Core.Extensions;

namespace XFQuery.Functions.OnMultipleEvents.ServerName
{
    public class ServerName : Addon
    {
        private static ServerNameFunction _service;

        public ServerName()
        {
            Config = GetConfig<ServerNameDto>();
            if (Config.Enabled) _service = new ServerNameFunction(this, Config);
        }

        public override string Name => "ServerName";

        private static ServerNameDto Config { get; set; }

        protected override void LoadDefaultConfig()
        {
            Log.Warning($"Creating a new configuration file for {Name}!");

            SetConfig(new ServerNameDto
            {
                Enabled = true,
                Interval = 10,
                ServerName =
                    "WowThatWork.nicedomain :: Online ([ONLINEUSERS]/[MAXUSERS]) ([ONLINEPERCENT]%) :: Query ([ONLINEQUERY])"
            });
        }


        public override void RegisterNotifications(NotificationHub notifications)
        {
            var timer = new Timer(TimeSpan.FromSeconds(Config.Interval).TotalMilliseconds);
            timer.Elapsed += MultipleEvents;
            timer.Enabled = Config.Enabled;
            notifications.ClientJoined.Triggered += MultipleEvents;
            notifications.ClientLeft.Disconnected += MultipleEvents;
        }

        private static async void MultipleEvents(object sender, EventArgs useless)
        {
            if (_service != null) { await _service.ServerName(); }
        }
    }
}