using XFQuery.Core;
using XFQuery.Functions.Interval.PerformanceCheck;
using XFQuery.Functions.OnChannelMove.RegisterChannels;
using XFQuery.Functions.OnClientJoin.WelcomeMessage;
using XFQuery.Functions.OnMultipleEvents.NicknameCheck;
using XFQuery.Functions.OnMultipleEvents.ServerName;

namespace XFQuery.Daemon
{
    internal class Core
    {
        private static void Main()
        {
            Interface.Initialize();
            Interface.XfQueryBot.AddAddon(new WelcomeMessage());
            Interface.XfQueryBot.AddAddon(new PerformanceCheck());
            Interface.XfQueryBot.AddAddon(new ServerName());
            Interface.XfQueryBot.AddAddon(new NicknameCheck());
            Interface.XfQueryBot.AddAddon(new RegisterChannels());
            Interface.XfQueryBot.Run();
        }
    }
}