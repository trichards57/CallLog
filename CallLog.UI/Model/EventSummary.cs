using System;

namespace CallLog.UI.Model
{
    public readonly record struct EventSummary
    {
        public EventSummary(LocalServer.EventSummary summary)
        {
            Name = summary.Name;
            Id = Guid.Parse(summary.Id);
        }

        public Guid Id { get; init; }
        public string Name { get; init; }
    }
}
