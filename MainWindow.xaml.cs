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
            ccMainContentContainer.Content = new ModTarget(ccMainContentContainer);
        }
    }
}
