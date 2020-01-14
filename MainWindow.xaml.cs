using rdrtwocontentmanager.Views;
using System.Windows;

namespace rdrtwocontentmanager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BModTargetLauncher_Click(object sender, RoutedEventArgs e)
        {
            if (ccMainContentContainer.Content != null) ccMainContentContainer.Content = null;
            ccMainContentContainer.Content = new ModTarget(ccMainContentContainer);
        }

        private void BLogsLauncher_Click(object sender, RoutedEventArgs e)
        {
            if (ccMainContentContainer.Content != null) ccMainContentContainer.Content = null;
            ccMainContentContainer.Content = new AppLogs(ccMainContentContainer);
        }

        private void BTargetDbLauncher_Click(object sender, RoutedEventArgs e)
        {
            if (ccMainContentContainer.Content != null) ccMainContentContainer.Content = null;
            ccMainContentContainer.Content = new TargetDbView(ccMainContentContainer);
        }
    }
}
