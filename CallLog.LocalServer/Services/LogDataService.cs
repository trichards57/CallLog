using CallLog.LocalServer.Data;
using CallLog.LocalServer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CallLog.LocalServer.Services
{
    public class LogDataService : ILogService
    {
        private readonly DataContext _dataContext;

        public LogDataService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddLogLine(LineType type, Guid eventId, string message)
        {
            var ev = await _dataContext.Events.Include(e => e.Controllers).FirstOrDefaultAsync(e => e.Id == eventId);

            if (ev == null)
                throw new KeyNotFoundException();

            ev.Log.Add(new LogLine { LineType = type, EventId = eventId, Line = message, DateTime = DateTimeOffset.UtcNow });
            await _dataContext.SaveChangesAsync();
        }

        public async Task AddLogLine(LineType type, Guid eventId, Guid controllerId, string message)
        {
            var ev = await _dataContext.Events.Include(e => e.Controllers).FirstOrDefaultAsync(e => e.Id == eventId);

            if (ev == null)
                throw new KeyNotFoundException();

            ev.Log.Add(new LogLine { LineType = type, EventId = eventId, ControllerId = controllerId, Line = message, DateTime = DateTimeOffset.UtcNow });
            await _dataContext.SaveChangesAsync();
        }
    }
}
