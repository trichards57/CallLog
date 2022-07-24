using System.Threading;
using System.Windows;
using System.Windows.Input;
using CallLog.UI.Services.Interfaces;
using CallLog.UI.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace CallLog.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CancellationTokenSource _cancelSource;

        public MainWindow()
        {
            InitializeComponent();
            _cancelSource = new CancellationTokenSource();
            Ioc.Default.GetRequiredService<IDialogService>().SetParent(this);
            DataContext = ViewModel;
        }

        public MainViewModel ViewModel { get; } = Ioc.Default.GetRequiredService<MainViewModel>();

        private void WindowClosed(object sender, System.EventArgs e)
        {
            _cancelSource.Cancel();
        }

        private async void WindowLoaded(object sender, RoutedEventArgs e)
        {
            var previousCursor = Cursor;
            Cursor = Cursors.Wait;

            var res = await ViewModel.Startup();

            if (!res)
                Close();

            Cursor = previousCursor;
        }
    }
}
