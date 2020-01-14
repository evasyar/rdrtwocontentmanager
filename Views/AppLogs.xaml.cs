using rdrtwocontentmanager.Models;
using System.Collections.ObjectModel;
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
        public ObservableCollection<AppLog> ocAppLogs { get; set; } = new ObservableCollection<AppLog>();
        public AppLogs(object parentContainer)
        {
            InitializeComponent();
            ParentContainer = (ContentControl)parentContainer;
            using var db = new AppLogDbHelper();
            ocAppLogs = new ObservableCollection<AppLog>(db.Get());

        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            //
            using var db = new AppLogDbHelper();
            ocAppLogs = new ObservableCollection<AppLog>(db.Search((sender as TextBox).Text));
        }

        private void dgAppLogs_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                using var db = new AppLogDbHelper();
                ocAppLogs = new ObservableCollection<AppLog>(db.Search((sender as TextBox).Text));
            }
        }

        private void bExit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ParentContainer.Content = null;
        }
    }
}
