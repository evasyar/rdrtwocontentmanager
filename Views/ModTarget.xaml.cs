using System.Windows.Controls;

namespace rdrtwocontentmanager.Views
{
    /// <summary>
    /// Interaction logic for ModTarget.xaml
    /// </summary>
    public partial class ModTarget : UserControl
    {
        public ContentControl ParentContainer { get; set; }
        public ModTarget(object parentContainer)
        {
            InitializeComponent();

            ParentContainer = (parentContainer as ContentControl);
        }

        private void bExit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ParentContainer.Content = null;
        }
    }
}
