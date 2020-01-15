using rdrtwocontentmanager.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace rdrtwocontentmanager.Views
{
    /// <summary>
    /// Interaction logic for ModifierView.xaml
    /// </summary>
    public partial class ModifierView : UserControl
    {
        public ContentControl ParentContainer { get; set; }
        public Target CapTarget { get; set; }
        public ModifierView(object parentContainer, Target ParentTarget)
        {
            InitializeComponent();
            ParentContainer = (ContentControl)parentContainer;
            CapTarget = ParentTarget;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            ParentContainer.Content = null;
        }

        private void btnModTarget_Click(object sender, RoutedEventArgs e)
        {
            if (ParentContainer.Content != null) ParentContainer.Content = null;
            ParentContainer.Content = new ModTarget(ParentContainer);
        }
    }
}
