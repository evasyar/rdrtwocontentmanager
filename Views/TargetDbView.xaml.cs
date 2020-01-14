using rdrtwocontentmanager.Models;
using System.Windows.Controls;
using System.Windows.Input;

namespace rdrtwocontentmanager.Views
{
    /// <summary>
    /// Interaction logic for TargetDbView.xaml
    /// </summary>
    public partial class TargetDbView : UserControl
    {
        public ContentControl ParentContainer { get; set; }
        public Target SelectedTarget { get; set; } = new Target();
        public TargetDbView(object parentContainer)
        {
            InitializeComponent();
            ParentContainer = (ContentControl)parentContainer;
            using var db = new TargetDbHelper();
            dgTargetDb.ItemsSource = db.Search(tbSearch.Text);
        }

        private void btnExit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ParentContainer.Content = null;
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            using var db = new TargetDbHelper();
            dgTargetDb.ItemsSource = db.Search((sender as TextBox).Text);
        }

        private void tbSearch_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                using var db = new TargetDbHelper();
                dgTargetDb.ItemsSource = db.Search((sender as TextBox).Text);
            }
        }
    }
}
