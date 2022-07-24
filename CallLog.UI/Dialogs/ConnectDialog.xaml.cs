using System.Windows;
using CallLog.UI.ViewModels.Dialogs;

namespace CallLog.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for ConnectDialog.xaml
    /// </summary>
    public partial class ConnectDialog : Window
    {
        public ConnectDialog()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

        public ConnectViewModel ViewModel { get; } = new ConnectViewModel();

        private void ConnectClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
