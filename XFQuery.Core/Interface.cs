namespace XFQuery.Core
{
    public static class Interface
    {
        public static XfQuery XfQueryBot { get; private set; }

        public static void Initialize()
        {
            if (XfQueryBot != null) return;
            XfQueryBot = new XfQuery();
            XfQueryBot.Load();
        }
    }
}