using System.Collections.Generic;
using System.Windows;
using CallLog.UI.Dialogs;
using CallLog.UI.Model;
using CallLog.UI.Services.Interfaces;

namespace CallLog.UI.Services
{
    internal class DialogService : IDialogService
    {
        private Window? _parentWindow;

        public void SetParent(Window window)
        {
            _parentWindow = window;
        }

        public LocalServer.Mode? SelectMode()
        {
            var connectDialog = new ConnectDialog { Owner = _parentWindow };

            if (connectDialog.ShowDialog() != true)
                return null;

            if (connectDialog.ViewModel.UseLocalOperational)
                return LocalServer.Mode.Operational;
            else
                return LocalServer.Mode.Training;
        }

        public EventSummary? SelectEvent(IEnumerable<EventSummary> _events)
        {
            var selectEventDialog = new SelectEventDialog { Owner = _parentWindow };

            foreach (var ev in _events)
                selectEventDialog.ViewModel.Events.Add(ev);

            if (selectEventDialog.ShowDialog() != true)
                return null;

            return selectEventDialog.ViewModel.SelectedEvent;
        }

        public ControllerSummary? SelectController(IEnumerable<ControllerSummary> _controllers)
        {
            var selectControllerDialog = new SelectControllerDialog { Owner = _parentWindow };

            foreach (var c in _controllers)
                selectControllerDialog.ViewModel.Controllers.Add(c);

            if (selectControllerDialog.ShowDialog() != true)
                return null;

            return selectControllerDialog.ViewModel.SelectedController;
        }

        public void ShowError(string message, string title)
        {
            MessageBox.Show(_parentWindow, message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
