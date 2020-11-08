using System.IO;
using Newtonsoft.Json;
using TS3QueryLib.Net.Core.Server.Notification;
using XFQuery.Core.Logging;

namespace XFQuery.Core.Extensions
{
    public abstract class Addon
    {
        protected static readonly ILogger Log = Interface.XfQueryBot.Logger;
        private IConfig _config;
        protected DatabaseManager MongoClient = Interface.XfQueryBot.DatabaseManager;
        public abstract string Name { get; }
        private AddonManager Manager { get; set; }
        public abstract void RegisterNotifications(NotificationHub notifications);
        protected abstract void LoadDefaultConfig();

        public void HandleAddedToManager(AddonManager manager)
        {
            Manager = manager;
        }

        private void SaveConfig()
        {
            ExtConfig.Write(Name, _config);
        }

        protected T GetConfig<T>() where T : class
        {
            if (_config == null) InitConfig<T>();

            return (T) _config;
        }

        protected void SetConfig(IConfig cfg)
        {
            _config = cfg;
        }

        private void InitConfig<T>()
        {
            if (!ExtConfig.Exists(Name))
            {
                LoadDefaultConfig();
                SaveConfig();
            }

            _config = (IConfig) ExtConfig.Read<T>(Name);
        }

        private class ExtConfig
        {
            public static T Read<T>(string name)
            {
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(Path(name)));
            }

            public static void Write(string name, IConfig config)
            {
                File.WriteAllText(Path(name), JsonConvert.SerializeObject(config, Formatting.Indented));
            }

            public static bool Exists(string name)
            {
                return File.Exists(Path(name));
            }

            private static string Path(string name)
            {
                return $"Configs/Functions/{name}.json";
            }
        }
    }
}