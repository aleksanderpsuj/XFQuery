using System.Collections.Generic;
using TS3QueryLib.Net.Core.Server.Notification;
using TS3QueryLib.Net.Core.Server.Notification.EventArgs;
using XFQuery.Core.Extensions;

namespace XFQuery.Functions.OnClientJoin.WelcomeMessage
{
    public class WelcomeMessage : Addon
    {
        private static WelcomeMessageFunction _service;

        public WelcomeMessage()
        {
            Config = GetConfig<WelcomeMessageDto>();
            _service = Config.Enabled ? new WelcomeMessageFunction(this, Config) : null;
        }

        public override string Name => "WelcomeMessage";

        private static WelcomeMessageDto Config { get; set; }

        protected override void LoadDefaultConfig()
        {
            Log.Warning($"Creating a new configuration file for {Name}!");

            SetConfig(new WelcomeMessageDto
            {
                Enabled = true,
                ConnectionMessage = new Dictionary<string, string[]>
                {
                    {
                        "pl", new[]
                        {
                            "○ Witaj [b][Nickname][/b] na [b]XFQuery - Dev![/b] ○",
                            "[S]",
                            "[u][b]○ ○ ○ O Tobie ○ ○ ○[/b][/u]",
                            "[S]",
                            "» Database ID: [b][DatabaseId][/b]",
                            "» Unique ID: [b][UniqueIdentifier][/b]",
                            "» Pierwsze Połączenie: [b][Created][/b]",
                            "» Wszystkich Połączeń: [b][TotalConnectionCount][/b]",
                            "» Ostatnie Połączenie: [b][LastConnected][/b]",
                            "» Platforma: [b][Platform][/b]",
                            "» IP: [b][ConnectionIp][/b]",
                            "» Wersja: [b][Version][/b]"
                        }
                    },
                    {
                        "en", new[]
                        {
                            "○ Welcome [b][Nickname][/b] on [b]XFQuery - Dev![/b] ○",
                            "[S]",
                            "[u][b]○ ○ ○ About You ○ ○ ○[/b][/u]",
                            "[S]",
                            "» Database ID: [b][DatabaseId][/b]",
                            "» Unique ID: [b][UniqueIdentifier][/b]",
                            "» First Connection: [b][Created][/b]",
                            "» Total Connections: [b][TotalConnectionCount][/b]",
                            "» Last Connection: [b][LastConnected][/b]",
                            "» Platform: [b][Platform][/b]",
                            "» IP: [b][ConnectionIp][/b]",
                            "» Version: [b][Version][/b]"
                        }
                    }
                },
                FirstConnectionMessage = new Dictionary<string, string[]>
                {
                    {
                        "pl", new[]
                        {
                            "○ Witaj [b][Nickname][/b] po raz pierwszy na [b]XFQuery - Dev![/b] ○",
                            "[S]",
                            "[u][b]○ ○ ○ O Tobie ○ ○ ○[/b][/u]",
                            "[S]",
                            "» Database ID: [b][DatabaseId][/b]",
                            "» Unique ID: [b][UniqueIdentifier][/b]",
                            "» Pierwsze Połączenie: [b][Created][/b]",
                            "» Wszystkich Połączeń: [b][TotalConnectionCount][/b]",
                            "» Ostatnie Połączenie: [b][LastConnected][/b]",
                            "» Platforma: [b][Platform][/b]",
                            "» IP: [b][ConnectionIp][/b]",
                            "» Wersja: [b][Version][/b]"
                        }
                    },
                    {
                        "en", new[]
                        {
                            "○ Welcome [b][Nickname][/b] for the first time On [b]XFQuery - Dev![/b] ○",
                            "[S]",
                            "[u][b]○ ○ ○ About You ○ ○ ○[/b][/u]",
                            "[S]",
                            "» Database ID: [b][DatabaseId][/b]",
                            "» Unique ID: [b][UniqueIdentifier][/b]",
                            "» First Connection: [b][Created][/b]",
                            "» Total Connections: [b][TotalConnectionCount][/b]",
                            "» Last Connection: [b][LastConnected][/b]",
                            "» Platform: [b][Platform][/b]",
                            "» IP: [b][ConnectionIp][/b]",
                            "» Version: [b][Version][/b]"
                        }
                    }
                }
            });
        }


        public override void RegisterNotifications(NotificationHub notifications)
        {
            notifications.ClientJoined.Triggered += ClientJoinToServer;
        }

        private static async void ClientJoinToServer(object sender, ClientJoinedEventArgs e)
        {
            if (_service != null) { await _service.WelcomeMessage(e); }
        }
    }
}