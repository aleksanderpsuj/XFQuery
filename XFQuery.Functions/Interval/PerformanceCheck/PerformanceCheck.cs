using System;
using System.Timers;
using TS3QueryLib.Net.Core.Server.Notification;
using XFQuery.Core.Extensions;

namespace XFQuery.Functions.Interval.PerformanceCheck
{
    public class PerformanceCheck : Addon
    {
        private static PerformanceCheckFuncion _service;

        public PerformanceCheck()
        {
            Config = GetConfig<PerformanceCheckDto>();
            if (Config.Enabled) _service = new PerformanceCheckFuncion(this, Config);
        }

        public override string Name => "PerformanceCheck";

        private static PerformanceCheckDto Config { get; set; }

        protected override void LoadDefaultConfig()
        {
            Log.Warning($"Creating a new configuration file for {Name}!");

            SetConfig(new PerformanceCheckDto
            {
                Enabled = true,
                Interval = 60,
                ChannelId = 0,
                ChannelName = "([CHECKTIME]) : RAM([RAMUSAGE]MB) CPU([CPUUSAGE]%)",
                ChannelDescription =
                    "[hr][b] Last Checked : [CHECKTIME]\r\nRAM USAGE: [RAMUSAGE]\r\nCPU USAGE: [CPUUSAGE][/b]"
            });
        }


        public override void RegisterNotifications(NotificationHub notifications)
        {
            var timer = new Timer(TimeSpan.FromSeconds(Config.Interval).TotalMilliseconds);
            timer.Elapsed += OnTimedEvent;
            timer.Enabled = Config.Enabled;
        }

        private static async void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (_service != null) { await _service.CalculateUsage(e); }
        }
    }
}