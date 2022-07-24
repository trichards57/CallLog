using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CallLog.LocalServer;

namespace CallLog.UI.Services.Interfaces
{
    public interface IDialogService
    {
        void SetParent(Window window);
        Mode? SelectMode();
        Model.EventSummary? SelectEvent(IEnumerable<Model.EventSummary> _eventSummaries);
        Model.ControllerSummary? SelectController(IEnumerable<Model.ControllerSummary> _controllers);
        void ShowError(string message, string title);
    }
}
