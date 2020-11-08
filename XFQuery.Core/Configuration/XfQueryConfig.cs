namespace XFQuery.Core.Configuration
{
    public class XfQueryConfig
    {
        public XfQueryConfig()
        {
            // Default config
            Server = new QuerySettings
                {Host = "localhost", ServerPort = 9987, QueryPort = 10011, Login = "serveradmin", Password = ""};
            Bot = new BotSettings {Name = "<XFQuery> Jeff", DefaultChannelId = 1, DefaultLang = "en"};
            Database = new DatabaseSettings
                {Host = "localhost", Port = 27017, Login = "", Password = "", Database = "", ConnectionString = ""};
        }

        public QuerySettings Server { get; }
        public BotSettings Bot { get; }
        public DatabaseSettings Database { get; }
    }

    public class QuerySettings
    {
        public string Host { get; set; }
        public ushort QueryPort { get; set; }
        public ushort ServerPort { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class DatabaseSettings
    {
        public string Host { get; set; }
        public ushort Port { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
        public string ConnectionString { get; set; }
    }

    public class BotSettings
    {
        public string Name { get; set; }
        public ushort DefaultChannelId { get; set; }
        public string DefaultLang { get; set; }
    }
}