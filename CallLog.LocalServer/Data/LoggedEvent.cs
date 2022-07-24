namespace CallLog.LocalServer.Data
{
    public class LoggedEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Mode Mode { get; set; }
        public List<Controller> Controllers { get; set; } = new List<Controller>();
        public List<LogLine> Log { get; set; } = new List<LogLine>();
    }
}
