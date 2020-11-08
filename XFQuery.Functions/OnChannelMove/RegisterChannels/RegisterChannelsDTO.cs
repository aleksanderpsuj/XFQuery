using System.Collections.Generic;
using XFQuery.Core.Extensions;

namespace XFQuery.Functions.OnChannelMove.RegisterChannels
{
    public class RegisterChannelsDto : IConfig
    {
        public string DefaultLang { get; set; } = "en";
        public bool Enabled { get; set; } = true;
        public Dictionary<string, string> KickMessage { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> SucessMessage { get; set; } = new Dictionary<string, string>();
        public Dictionary<int, RegisterChannel> RegisterChannels { get; set; } = new Dictionary<int, RegisterChannel>();
    }

    public class RegisterChannel
    {
        public int RankId { get; set; }
        public int ConnectionsNeeded { get; set; }
        public int SecondsNeeded { get; set; }
        public List<int> RestrictedRanks { get; set; }
    }
}