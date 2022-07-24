using System;

namespace CallLog.UI.Model
{
    public readonly record struct ControllerSummary
    {
        public ControllerSummary(LocalServer.ControllerSummary controller)
        {
            Id = Guid.Parse(controller.Id);
            Name = controller.Name;
        }

        public Guid Id { get; init; }
        public string Name { get; init; }
    }
}
