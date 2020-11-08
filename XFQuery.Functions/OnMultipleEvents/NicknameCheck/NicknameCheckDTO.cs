using System.Collections.Generic;
using XFQuery.Core.Extensions;

namespace XFQuery.Functions.OnMultipleEvents.NicknameCheck
{
    public class NicknameCheckDto : IConfig
    {
        public string DefaultLang { get; set; } = "en";
        public int Interval { get; set; }
        public string[] BannedWords { get; set; }
        public string PokeOrKick { get; set; }
        public Dictionary<string, string> Reason { get; set; }
        public bool Enabled { get; set; } = true;
    }
}