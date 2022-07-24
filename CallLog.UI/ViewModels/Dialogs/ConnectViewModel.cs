using PostSharp.Patterns.Model;

namespace CallLog.UI.ViewModels.Dialogs
{
    [NotifyPropertyChanged]
    public class ConnectViewModel
    {
        public bool UseLocalOperational { get; set; } = true;
        public bool UseLocalTraining { get => !UseLocalOperational; set => UseLocalOperational = !value; }
    }
}
