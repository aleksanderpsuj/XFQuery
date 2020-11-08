using System;
using System.Collections.Generic;
using System.Timers;
using TS3QueryLib.Net.Core.Server.Notification;
using TS3QueryLib.Net.Core.Server.Notification.EventArgs;
using XFQuery.Core.Extensions;

namespace XFQuery.Functions.OnMultipleEvents.NicknameCheck
{
    public class NicknameCheck : Addon
    {
        private static NicknameCheckFunction _service;

        public NicknameCheck()
        {
            Config = GetConfig<NicknameCheckDto>();
            if (Config.Enabled) _service = new NicknameCheckFunction(this, Config);
        }

        public override string Name => "NicknameCheck";

        private static NicknameCheckDto Config { get; set; }

        protected override void LoadDefaultConfig()
        {
            Log.Warning($"Creating a new configuration file for {Name}!");

            SetConfig(new NicknameCheckDto
            {
                Enabled = true,
                Interval = 30,
                BannedWords = new[] {"TeamSpeakUser", "Fuck", ".eu", ".com", ".pl"},
                PokeOrKick = "Poke",
                Reason = new Dictionary<string, string>
                {
                    {"pl", "Używasz zakazanego nicku ([NICKNAME])!"},
                    {"en", "You're using restricted nickname ([NICKNAME])!"}
                }
            });
        }


        public override void RegisterNotifications(NotificationHub notifications)
        {
            var timer = new Timer(TimeSpan.FromSeconds(Config.Interval).TotalMilliseconds);
            timer.Elapsed += OnInterval;
            timer.Enabled = Config.Enabled;
            notifications.ClientJoined.Triggered += OnJoin;
        }

        private static async void OnJoin(object sender, ClientJoinedEventArgs e)
        {
            if (_service != null) { await _service.CheckOnJoin(e); }
        }

        private static void OnInterval(object sender, EventArgs useless)
        {
            _service.CheckOnInterval();
        }
    }
}