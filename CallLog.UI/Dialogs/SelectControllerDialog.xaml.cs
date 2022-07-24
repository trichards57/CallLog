using System.Windows;
using CallLog.UI.ViewModels.Dialogs;

namespace CallLog.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for ChooseControllerDialog.xaml
    /// </summary>
    public partial class SelectControllerDialog : Window
    {
        public SelectControllerDialog()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

        public SelectControllerViewModel ViewModel { get; } = new SelectControllerViewModel();

        private void SelectClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
