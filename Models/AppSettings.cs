namespace AppsettingsValue
{
    public class AppSettings
    {
        public Log log { get; set; }
        public string AllowedHosts { get; set; }
    }

    public class Log
    {
        public string Settings { get; set; }
    }

    public class Settings
    {
        public string RandomApi { get; set; }
        public string BlackList { get; set; }
    }
}