using System.Collections.Generic;
using TS3QueryLib.Net.Core.Server.Notification;
using TS3QueryLib.Net.Core.Server.Notification.EventArgs;
using XFQuery.Core.Extensions;

namespace XFQuery.Functions.OnChannelMove.RegisterChannels
{
    public class RegisterChannels : Addon
    {
        private static RegisterChannelsFunction _service;

        public RegisterChannels()
        {
            Config = GetConfig<RegisterChannelsDto>();
            _service = Config.Enabled ? new RegisterChannelsFunction(this, Config) : null;
        }

        public override string Name => "RegisterChannels";

        private static RegisterChannelsDto Config { get; set; }

        protected override void LoadDefaultConfig()
        {
            Log.Warning($"Creating a new configuration file for {Name}!");
            SetConfig(new RegisterChannelsDto
            {
                Enabled = true,
                DefaultLang = "en",
                KickMessage = new Dictionary<string, string>
                {
                    {"pl", "Posiadasz już rangę lub nie spełniasz wymagań!"},
                    {"en", "You're already have that rank or you dont meet the requirements"}
                },
                SucessMessage = new Dictionary<string, string>
                {
                    {"pl", "Została ci nadana ranga!"},
                    {"en", "You have been assigned with a rank!"}
                },
                RegisterChannels = new Dictionary<int, RegisterChannel>
                {
                    {
                        0,
                        new RegisterChannel
                        {
                            RankId = 0, ConnectionsNeeded = 0, SecondsNeeded = 0, RestrictedRanks = new List<int> {1, 1}
                        }
                    },
                    {
                        1,
                        new RegisterChannel
                        {
                            RankId = 0, ConnectionsNeeded = 0, SecondsNeeded = 0, RestrictedRanks = new List<int> {1, 1}
                        }
                    }
                }
            });
        }


        public override void RegisterNotifications(NotificationHub notifications)
        {
            notifications.ClientMoved.JoiningChannel += ClientJoinToChannel;
            notifications.ClientMoved.JoiningChannelForced += ClientJoinToChannel;
        }

        private static async void ClientJoinToChannel(object sender, ClientMovedEventArgs e)
        {
            if (_service != null) { await _service.RegisterOnChannel(e); }
        }
    }
}