namespace CallLog.LocalServer.Logging
{
    public static class LoggingEvents
    {
        public static EventId ControllerNotFound { get; } = new(12, "Controller Not Found");
        public static EventId EventNotFound { get; } = new(11, "Event Not Found");
        public static EventId ControllerLoggedOn { get; } = new(103, "Controller Logged On");
        public static EventId GetEventControllers { get; } = new(102, "Get Event Controllers");
        public static EventId GotEvents { get; } = new(101, "Got Events");
        public static EventId InvalidControllerId { get; } = new(1, "Invalid Controller ID Provided");
        public static EventId InvalidEventId { get; } = new(2, "Invalid Event ID Provided");
    }
}
