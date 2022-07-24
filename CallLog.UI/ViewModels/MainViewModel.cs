using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CallLog.LocalServer;
using CallLog.UI.Services.Interfaces;
using Grpc.Core;
using PostSharp.Patterns.Model;

namespace CallLog.UI.ViewModels
{
    [NotifyPropertyChanged]
    public class MainViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly Events.EventsClient _eventsClient;

        public MainViewModel(IDialogService dialogService, Events.EventsClient eventsClient)
        {
            _dialogService = dialogService;
            _eventsClient = eventsClient;
        }

        private void ShowServerError(string message)
        {
            _dialogService.ShowError(message, "Error Communicating with Server");
        }

        public async Task<bool> Startup(CancellationToken cancellationToken = default)
        {
            while (true)
            {
                UpdateTitle();

                var mode = _dialogService.SelectMode();
                if (mode == null)
                    return false;

                SelectedMode = mode.Value;

                UpdateTitle();

                GetEventsResponse? events = null;

                try
                {
                    events = await _eventsClient.GetEventsAsync(new GetEventsRequest { Mode = SelectedMode }, cancellationToken: cancellationToken);
                }
                catch (RpcException ex)
                {
                    ShowServerError($"There was an error connecting to the server.  Error Code : {ex.StatusCode}");
                    Debug.WriteLine(ex);
                    continue;
                }

                var selectedEvent = _dialogService.SelectEvent(events!.Events.Select(e => new Model.EventSummary(e)));

                if (!selectedEvent.HasValue)
                    continue;

                SelectedEvent = selectedEvent.Value;

                UpdateTitle(includeEvent: true);

                GetControllersResponse? controllers = null;

                try
                {
                    controllers = await _eventsClient.GetControllersAsync(new GetControllersRequest { EventId = SelectedEvent.Id.ToString() }, cancellationToken: cancellationToken);
                }
                catch (RpcException ex)
                {
                    if (ex.StatusCode == StatusCode.NotFound)
                        ShowServerError("The selected event no-longer appears to exist.");
                    else
                        ShowServerError($"There was an error connecting to the server.  Error Code : {ex.StatusCode}");

                    Debug.WriteLine(ex);
                    continue;
                }

                var controller = _dialogService.SelectController(controllers!.Controllers.Select(c => new Model.ControllerSummary(c)));

                if (!controller.HasValue)
                    continue;

                SelectedController = controller.Value;

                UpdateTitle(includeEvent: true, includeController: true);

                try
                {
                    await _eventsClient.LogOnControllerAsync(new LogOnControllerRequest { EventId = SelectedEvent.Id.ToString(), ControllerId = SelectedController.Id.ToString() }, cancellationToken: cancellationToken);
                }
                catch (RpcException ex)
                {
                    if (ex.StatusCode == StatusCode.NotFound)
                        ShowServerError("The selected event or controller no-longer appears to exist.");
                    else
                        ShowServerError($"There was an error connecting to the server.  Error Code : {ex.StatusCode}");

                    Debug.WriteLine(ex);
                    continue;
                }

                return true;
            }
        }

        private void UpdateTitle(bool includeEvent = false, bool includeController = false)
        {
            var titleBuilder = new StringBuilder("Call Log");

            if (SelectedMode == Mode.Training)
                titleBuilder.Append(" (Training Mode)");

            if (includeEvent)
                titleBuilder.Append($" - {SelectedEvent.Name}");
            if (includeController)
                titleBuilder.Append($" - {SelectedController.Name}");
        }

        public Mode SelectedMode { get; set; }
        public Model.EventSummary SelectedEvent { get; set; }
        public Model.ControllerSummary SelectedController { get; set; }
        public string WindowTitle { get; set; } = "Call Log";
    }
}
