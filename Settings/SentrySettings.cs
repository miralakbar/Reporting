namespace Application.Settings
{
    public class SentrySettings
    {
        public string Dsn { get; set; }
        public bool IsDebugModeEnabled { get; set; }
        public double TracesSampleRate { get; set; }
        public bool AttachStacktrace { get; set; }
        public bool IncludeActivityData { get; set; }
    }
}
