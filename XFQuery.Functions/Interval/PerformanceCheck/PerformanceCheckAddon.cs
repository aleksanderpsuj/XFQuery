using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using TS3QueryLib.Net.Core.Server.Commands;
using TS3QueryLib.Net.Core.Server.Entitities;
using XFQuery.Core;
using XFQuery.Core.Logging;

namespace XFQuery.Functions.Interval.PerformanceCheck
{
    internal class PerformanceCheckFuncion
    {
        private readonly PerformanceCheck _addon;
        private readonly PerformanceCheckDto _config;
        private readonly ILogger _log = Interface.XfQueryBot.Logger;

        public PerformanceCheckFuncion(PerformanceCheck addon, PerformanceCheckDto config)
        {
            _addon = addon;
            _config = config;
        }

        public async Task CalculateUsage(ElapsedEventArgs e)
        {
            try
            {
                Process p = Process.GetCurrentProcess();
                double cpuload = p.TotalProcessorTime.TotalMilliseconds / 1000;
                var ramload = p.PrivateMemorySize64 / 1024 / 1024;

                var channelModification = new ChannelModification
                {
                    Name = _config.ChannelName
                        .Replace("[CHECKTIME]", DateTime.Now.ToShortTimeString())
                        .Replace("[RAMUSAGE]", ramload.ToString("D"))
                        .Replace("[CPUUSAGE]", cpuload.ToString("F1")),
                    Description = _config.ChannelDescription
                        .Replace("[CHECKTIME]", DateTime.Now.ToShortTimeString())
                        .Replace("[RAMUSAGE]", ramload.ToString("D"))
                        .Replace("[CPUUSAGE]", cpuload.ToString("F1"))
                };
                await new ChannelEditCommand(_config.ChannelId, channelModification).ExecuteAsync(Interface.XfQueryBot
                    .QueryClient);
            }
            catch (Exception ex)
            {
                _log.Error($"({_addon.Name}) : {ex}");
            }
        }
    }
}