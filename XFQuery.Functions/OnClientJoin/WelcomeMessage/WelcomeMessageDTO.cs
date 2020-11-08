using System.Collections.Generic;
using XFQuery.Core.Extensions;

namespace XFQuery.Functions.OnClientJoin.WelcomeMessage
{
    public class WelcomeMessageDto : IConfig
    {
        public string DefaultLang { get; set; } = "en";
        public Dictionary<string, string[]> FirstConnectionMessage { get; set; } = new Dictionary<string, string[]>();
        public Dictionary<string, string[]> ConnectionMessage { get; set; } = new Dictionary<string, string[]>();
        public bool Enabled { get; set; } = true;
    }
}