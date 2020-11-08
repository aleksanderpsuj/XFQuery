using XFQuery.Core.Extensions;

namespace XFQuery.Functions.Interval.PerformanceCheck
{
    public class PerformanceCheckDto : IConfig
    {
        public int Interval { get; set; }
        public uint ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string ChannelDescription { get; set; }
        public bool Enabled { get; set; } = true;
    }
}