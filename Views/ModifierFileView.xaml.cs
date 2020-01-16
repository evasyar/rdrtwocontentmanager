using rdrtwocontentmanager.Models;
using System.Windows;
using System.Windows.Controls;

namespace rdrtwocontentmanager.Views
{
    /// <summary>
    /// Interaction logic for ModifierFileView.xaml
    /// </summary>
    public partial class ModifierFileView : System.Windows.Controls.UserControl
    {
        public ContentControl ParentContainer { get; set; }
        public Modifier SelectedMod { get; set; }
        public Target SelectedTarget { get; set; }
        public ModifierFileView()
        {
            InitializeComponent();
        }

        public ModifierFileView(object parentContainer, Modifier mod, Target target) : this()
        {
            ParentContainer = parentContainer as ContentControl;
            SelectedMod = mod;
            SelectedTarget = target;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            ParentContainer.Content = null;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (ParentContainer.Content != null) ParentContainer.Content = null;
            ParentContainer.Content = new ModifierView(ParentContainer, SelectedTarget);
        }
    }
}
