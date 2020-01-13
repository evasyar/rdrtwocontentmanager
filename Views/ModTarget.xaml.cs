using rdrtwocontentmanager.Helper;
using rdrtwocontentmanager.Models;
using System.Collections.Generic;
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
        public Target SelectedTarget { get; set; }
        public List<Target> TargetToBind { get; set; }
        public ModTarget(object parentContainer)
        {
            InitializeComponent();

            ParentContainer = (parentContainer as ContentControl);
            using TargetDbHelper db = new TargetDbHelper();
            TargetToBind = db.Get();
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

        private void bPost_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            using TargetDbHelper tdb = new TargetDbHelper();
            try
            {
                var res = tdb.Post(new Target()
                {
                    RootName = tbModTargetName.Text,
                    Root = tbFileLocation.Text
                });
                TargetToBind = tdb.Get();
            }
            catch (System.Exception ex)
            {
                LogHelper.LogError(ex.Message);
            }
        }

        private void bDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            using TargetDbHelper tdb = new TargetDbHelper();
            try
            {
                tdb.Delete(SelectedTarget);
                TargetToBind = tdb.Get();
            }
            catch (System.Exception ex)
            {
                LogHelper.LogError(ex.Message);
            }
        }
    }
}
