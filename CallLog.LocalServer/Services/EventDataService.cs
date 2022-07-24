using CallLog.LocalServer.Data;
using CallLog.LocalServer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CallLog.LocalServer.Services
{
    public class EventDataService : IEventService
    {
        private readonly DataContext _dataContext;
        private readonly ILogService _logService;

        public EventDataService(DataContext dataContext, ILogService logService)
        {
            _dataContext = dataContext;
            _logService = logService;
        }

        public Task<IEnumerable<EventSummary>> GetInMode(Mode mode)
        {
            return Task.FromResult<IEnumerable<EventSummary>>(_dataContext.Events.Where(e => e.Mode == (Data.Mode)mode)
                .Select(e => new EventSummary { Id = e.Id.ToString(), Name = e.Name }));
        }

        public async Task LogOn(Guid eventId, Guid controllerId)
        {
            var ev = await _dataContext.Events.Include(e => e.Controllers).FirstOrDefaultAsync(e => e.Id == eventId);

            if (ev == null)
                throw new KeyNotFoundException("Event not found");

            if (!ev.Controllers.Any(e => e.Id == controllerId))
                throw new KeyNotFoundException("Controller not found or not expected");

            await _logService.AddLogLine(LineType.Controller, eventId, controllerId, "Logged on.");
        }

        public async Task<IEnumerable<ControllerSummary>> GetControllersAsync(Guid id)
        {
            var ev = await _dataContext.Events.Include(e => e.Controllers).FirstOrDefaultAsync(e => e.Id == id);

            if (ev == null)
                throw new KeyNotFoundException();

            await _logService.AddLogLine(LineType.Misc, id, "List of controllers retrieved.");

            return ev.Controllers.Select(e => new ControllerSummary { Id = e.Id.ToString(), Name = e.Name });
        }
    }
}
