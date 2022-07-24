namespace CallLog.LocalServer.Data
{
    public enum LineType
    {
        Misc = 0,
        Controller = 1
    }


    public class LogLine
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Guid? ControllerId { get; set; }
        public string Line { get; set; } = string.Empty;
        public DateTimeOffset DateTime { get; set; }
        public LineType LineType { get; set; }
    }
}
