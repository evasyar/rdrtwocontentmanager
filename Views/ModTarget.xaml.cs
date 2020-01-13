using System.Windows.Controls;
using System.Windows.Forms;

namespace rdrtwocontentmanager.Views
{
    /// <summary>
    /// Interaction logic for ModTarget.xaml
    /// </summary>
    public partial class ModTarget : System.Windows.Controls.UserControl
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

        private void bFileSelect_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    tbFileLocation.Text = fbd.SelectedPath;
                }
            }
        }
    }
}
