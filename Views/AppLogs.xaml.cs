using rdrtwocontentmanager.Models;
using System.Windows.Controls;
using System.Windows.Input;

namespace rdrtwocontentmanager.Views
{
    /// <summary>
    /// Interaction logic for AppLogs.xaml
    /// </summary>
    public partial class AppLogs : UserControl
    {
        public ContentControl ParentContainer { get; set; }
        public AppLog SelectedAppLog { get; set; } = new AppLog();
        public AppLogs(object parentContainer)
        {
            InitializeComponent();
            ParentContainer = (ContentControl)parentContainer;
            using var db = new AppLogDbHelper();
            dgAppLogs.ItemsSource = db.Get();

        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            //
            using var db = new AppLogDbHelper();
            dgAppLogs.ItemsSource = db.Search((sender as TextBox).Text);
        }

        private void dgAppLogs_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                using var db = new AppLogDbHelper();
                dgAppLogs.ItemsSource = db.Search((sender as TextBox).Text);
            }
        }

        private void bExit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ParentContainer.Content = null;
        }

        private void dgAppLogs_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            using var db = new AppLogDbHelper();
            var grid = sender as DataGrid;
            grid.ItemsSource = db.Get();
        }

        private void dgAppLogs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((DataGrid)sender).HasItems)
                SelectedAppLog = ((DataGrid)sender).SelectedItem as AppLog;
        }
    }
}
