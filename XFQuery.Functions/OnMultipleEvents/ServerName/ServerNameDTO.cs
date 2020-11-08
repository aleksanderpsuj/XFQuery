using XFQuery.Core.Extensions;

namespace XFQuery.Functions.OnMultipleEvents.ServerName
{
    public class ServerNameDto : IConfig
    {
        public string ServerName { get; set; }
        public int Interval { get; set; }
        public bool Enabled { get; set; } = true;
    }
}