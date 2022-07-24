using CallLog.LocalServer.Data;

namespace CallLog.LocalServer.Services.Interfaces
{
    public interface ILogService
    {
        Task AddLogLine(LineType type, Guid eventId, string message);
        Task AddLogLine(LineType type, Guid eventId, Guid controllerId, string message);
    }
}
