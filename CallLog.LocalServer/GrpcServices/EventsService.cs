using CallLog.LocalServer.Data;
using CallLog.LocalServer.Logging;
using CallLog.LocalServer.Services.Interfaces;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace CallLog.LocalServer.GrpcServices
{
    public class EventsService : Events.EventsBase
    {
        private readonly Action<ILogger, string, Exception?> _controllerNotFoundLog;
        private readonly IEventService _eventService;
        private readonly Action<ILogger, string, Exception?> _eventNotFoundLog;
        private readonly Action<ILogger, string, string, Exception?> _getControllersLog;
        private readonly Action<ILogger, Mode, string, Exception?> _getEventsLog;
        private readonly Action<ILogger, string, Exception?> _invalidControllerIdLog;
        private readonly Action<ILogger, string, Exception?> _invalidEventIdLog;
        private readonly Action<ILogger, string, string, string, Exception?> _controllerLoggedOnLog;
        private readonly ILogger<EventsService> _logger;

        public EventsService(ILogger<EventsService> logger, IEventService eventService)
        {
            _eventService = eventService;
            _logger = logger;
            _controllerLoggedOnLog = LoggerMessage.Define<string, string, string>(LogLevel.Information, LoggingEvents.ControllerLoggedOn, "Controller with ID {ControllerID} logged on to Event {EventID} from {Address}");
            _controllerNotFoundLog = LoggerMessage.Define<string>(LogLevel.Warning, LoggingEvents.ControllerNotFound, "The requested controller was not found : {ControllerID}");
            _eventNotFoundLog = LoggerMessage.Define<string>(LogLevel.Warning, LoggingEvents.EventNotFound, "The requested event was not found : {EventID}");
            _getControllersLog = LoggerMessage.Define<string, string>(LogLevel.Information, LoggingEvents.GetEventControllers, "List of controllers for event ID {EventId} was requested from {Address}");
            _getEventsLog = LoggerMessage.Define<Mode, string>(LogLevel.Information, LoggingEvents.GotEvents, "List of events in {Mode} mode were requested from {Address}");
            _invalidControllerIdLog = LoggerMessage.Define<string>(LogLevel.Warning, LoggingEvents.InvalidControllerId, "An invalid Controller ID was provided : {ControllerID}");
            _invalidEventIdLog = LoggerMessage.Define<string>(LogLevel.Warning, LoggingEvents.InvalidEventId, "An invalid Event ID was provided : {EventID}");
        }

        public override async Task<GetControllersResponse> GetControllers(GetControllersRequest request, ServerCallContext context)
        {
            _getControllersLog(_logger, request.EventId, context.Peer, null);

            var eventIdValid = Guid.TryParse(request.EventId, out var eventId);
            if (!eventIdValid)
            {
                _invalidEventIdLog(_logger, request.EventId, null);
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Event ID."));
            }

            try
            {
                var result = new GetControllersResponse();
                result.Controllers.AddRange(await _eventService.GetControllersAsync(eventId));
                return result;
            }
            catch (KeyNotFoundException)
            {
                _eventNotFoundLog(_logger, request.EventId, null);
                throw new RpcException(new Status(StatusCode.NotFound, "Event not found"));
            }
        }

        public override async Task<GetEventsResponse> GetEvents(GetEventsRequest request, ServerCallContext context)
        {
            var result = new GetEventsResponse();
            result.Events.AddRange(await _eventService.GetInMode(request.Mode));

            _getEventsLog(_logger, request.Mode, context.Peer, null);

            return result;
        }

        public override async Task<LogOnControllerResponse> LogOnController(LogOnControllerRequest request, ServerCallContext context)
        {
            _controllerLoggedOnLog(_logger, request.ControllerId, request.EventId, context.Peer, null);

            var eventIdValid = Guid.TryParse(request.EventId, out var eventId);
            if (!eventIdValid)
            {
                _invalidEventIdLog(_logger, request.EventId, null);
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Event ID."));
            }

            var controllerIdValid = Guid.TryParse(request.ControllerId, out var controllerId);
            if (!controllerIdValid)
            {
                _invalidControllerIdLog(_logger, request.ControllerId, null);
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Controller ID."));
            }

            try
            {
                await _eventService.LogOn(eventId, controllerId);
                return new LogOnControllerResponse();
            }
            catch (KeyNotFoundException ex)
            {
                if (ex.Message.Contains("event", StringComparison.InvariantCultureIgnoreCase))
                    _eventNotFoundLog(_logger, request.EventId, null);
                if (ex.Message.Contains("controller", StringComparison.InvariantCultureIgnoreCase))
                    _controllerNotFoundLog(_logger, request.ControllerId, null);
                throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
            }
        }
    }
}
