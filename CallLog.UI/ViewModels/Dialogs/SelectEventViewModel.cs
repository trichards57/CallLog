using System.Collections.ObjectModel;
using PostSharp.Patterns.Model;

namespace CallLog.UI.ViewModels.Dialogs
{
    [NotifyPropertyChanged]
    public class SelectEventViewModel
    {
        public bool CanSelectEvent => SelectedEvent != null;
        public ObservableCollection<Model.EventSummary> Events { get; } = new ObservableCollection<Model.EventSummary>();
        public Model.EventSummary? SelectedEvent { get; set; }
    }
}
