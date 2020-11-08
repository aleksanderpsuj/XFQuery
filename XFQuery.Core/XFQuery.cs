using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TS3QueryLib.Net.Core;
using TS3QueryLib.Net.Core.Common;
using TS3QueryLib.Net.Core.Common.Responses;
using TS3QueryLib.Net.Core.Server.Commands;
using TS3QueryLib.Net.Core.Server.Entitities;
using TS3QueryLib.Net.Core.Server.Notification;
using XFQuery.Core.Configuration;
using XFQuery.Core.Extensions;
using XFQuery.Core.Logging;
using events = TS3QueryLib.Net.Core.Server.Notification.EventArgs;

namespace XFQuery.Core
{
    public class XfQuery
    {
        private AddonManager _addonManager;
        public IQueryClient QueryClient;
        private XfQueryConfig Config { get; set; }
        public ConsoleLogger Logger { get; private set; }
        public DatabaseManager DatabaseManager { get; set; }

        public void Load()
        {
            Logger = new ConsoleLogger();
            var currentDir = Directory.GetCurrentDirectory();
            Config = new XfQueryConfig();

            Logger.Ok("Loading XFQuery Core");

            var configDir = Directory.CreateDirectory($"{currentDir}/Configs");
            var functionsConfig = Directory.CreateDirectory($"{configDir}/Functions");
            if (configDir.Exists && functionsConfig.Exists)
            {
                if (File.Exists($"{currentDir}/Configs/MainConfig.json"))
                {
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("Configs/MainConfig.json", false, true)
                        .AddEnvironmentVariables();
                    var configuration = builder.Build();
                    configuration.Bind(Config);
                }
                else
                {
                    LoadDefaultConfig($"{currentDir}/Configs");
                }
            }
            else
            {
                if (configDir.Exists && functionsConfig.Exists)
                {
                    LoadDefaultConfig($"{currentDir}/Configs");
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile(@"Configs/MainConfig.json", false, true)
                        .AddEnvironmentVariables();
                    var configuration = builder.Build();
                    configuration.Bind(Config);
                }
                else
                {
                    throw new Exception("Cannot create an configs folder");
                }
            }

            Logger.Ok("Connecting to MongoDB");
            DatabaseManager = new DatabaseManager(Config);
            _addonManager = new AddonManager();
            Logger.Ok("Loading extensions");
        }

        private void LoadDefaultConfig(string path)
        {
            Logger.Warning("Main config file doesn't exist, creating one");
            File.WriteAllText($"{path}/MainConfig.json",
                JsonConvert.SerializeObject(Config, Formatting.Indented));
            Logger.Info("Please fill new configuration file and then restart");
            throw new Exception("Please fill new configuration file and then restart");
        }

        public void AddAddon(Addon addon)
        {
            _addonManager.AddAddon(addon);
            addon.HandleAddedToManager(_addonManager);
        }

        public IList<Addon> Addons()
        {
            return _addonManager.Addons;
        }

        public void Run()
        {
            _addonManager.Addons.ForEach(addon => Logger.Ok($"({addon.Name}) : Loaded"));
            Logger.Ok("Registering notifications");
            var notifications = new NotificationHub();
            notifications.UnknownNotificationReceived.Triggered += UnknownNotificationReceived_Triggered;
            _addonManager.RegisterNotifications(notifications);


            QueryClient = new QueryClient(notificationHub: notifications, keepAliveInterval: TimeSpan.FromSeconds(30),
                host: Config.Server.Host, port: Config.Server.QueryPort);
            QueryClient.BanDetected += Client_BanDetected;
            QueryClient.ConnectionClosed += Client_ConnectionClosed;
            Connect(QueryClient);
            Logger.Ok(
                $"Query login : {!new LoginCommand(Config.Server.Login, Config.Server.Password).Execute(QueryClient).IsErroneous}");
            Logger.Info(
                $"Switch to server with port {Config.Server.ServerPort} : {!new UseCommand(Config.Server.ServerPort).Execute(QueryClient).IsErroneous}");
            new ClientUpdateCommand(new ClientModification {Nickname = Config.Bot.Name}).ExecuteAsync(QueryClient);
            Logger.Ok(
                $"Notifications [Server] : {!new ServerNotifyRegisterCommand(ServerNotifyRegisterEvent.Server, 0).Execute(QueryClient).IsErroneous}");
            Logger.Ok(
                $"Notifications [Channel] : {!new ServerNotifyRegisterCommand(ServerNotifyRegisterEvent.Channel, 0).Execute(QueryClient).IsErroneous}");
            Logger.Ok(
                $"Notifications [Channel-Text] :  {!new ServerNotifyRegisterCommand(ServerNotifyRegisterEvent.TextChannel, 1).Execute(QueryClient).IsErroneous}");
            Logger.Ok(
                $"Notifications [Server-Text] : {!new ServerNotifyRegisterCommand(ServerNotifyRegisterEvent.TextServer).Execute(QueryClient).IsErroneous}");
            Logger.Ok(
                $"Notifications [Private-Text] : {!new ServerNotifyRegisterCommand(ServerNotifyRegisterEvent.TextPrivate).Execute(QueryClient).IsErroneous}");
            Logger.Ok(
                $"Notifications [TokenUsed] : {!new ServerNotifyRegisterCommand(ServerNotifyRegisterEvent.TokenUsed).Execute(QueryClient).IsErroneous}");

            Logger.Info("Type a command or press [ENTER] to exit");


            do
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write($"<{Config.Server.Login}:{QueryClient.Host}:{QueryClient.Port}> Send: ");
                Console.ResetColor();

                var commandText = Console.ReadLine();

                if (commandText != null && commandText.Length == 0)
                {
                    new LogoutCommand().Execute(QueryClient);
                    break;
                }

                var response = QueryClient.Send(commandText);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("Response: ");
                Console.ResetColor();
                Console.WriteLine(response);
            } while (QueryClient.Connected);

            Console.WriteLine("Exiting now...");
        }

        private static void Client_ConnectionClosed(object sender, EventArgs<string> e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Connection lost: {e.Value}");
            Console.ResetColor();
        }

        private static void Client_BanDetected(object sender, EventArgs<ICommandResponse> e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Flood Ban Detected!");
            Console.ResetColor();
        }

        private static void UnknownNotificationReceived_Triggered(object sender, events.UnknownNotificationEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Unknown notification: [Name:{e.Name}] [ResponseText:{e.ResponseText}]");
            Console.ResetColor();
        }

        private static void Connect(IQueryClient client)
        {
            client.Connect();
        }
    }
}