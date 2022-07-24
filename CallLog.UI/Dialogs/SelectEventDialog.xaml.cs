using System.Windows;
using CallLog.UI.ViewModels.Dialogs;

namespace CallLog.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for SelectEventDialog.xaml
    /// </summary>
    public partial class SelectEventDialog : Window
    {
        public SelectEventDialog()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

        public SelectEventViewModel ViewModel { get; } = new SelectEventViewModel();

        private void SelectClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
