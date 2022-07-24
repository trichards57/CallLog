namespace CallLog.LocalServer.Services.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<ControllerSummary>> GetControllersAsync(Guid id);
        Task<IEnumerable<EventSummary>> GetInMode(Mode mode);
        Task LogOn(Guid eventId, Guid controllerId);
    }
}
