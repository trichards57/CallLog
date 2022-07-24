using System.Collections.ObjectModel;
using PostSharp.Patterns.Model;

namespace CallLog.UI.ViewModels.Dialogs
{
    [NotifyPropertyChanged]
    public class SelectControllerViewModel
    {
        public bool CanSelectController => SelectedController != null;
        public ObservableCollection<Model.ControllerSummary> Controllers { get; } = new ObservableCollection<Model.ControllerSummary>();
        public Model.ControllerSummary? SelectedController { get; set; }
    }
}
