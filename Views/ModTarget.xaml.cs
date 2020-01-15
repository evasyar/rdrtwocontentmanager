using rdrtwocontentmanager.Helper;
using rdrtwocontentmanager.Models;
using System.Collections.Generic;
using System.Globalization;
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
            dgTarget.ItemsSource = TargetToBind;
            LogHelper.Log("Mod target list initialized");
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
                    RootName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tbModTargetName.Text.ToLower()),
                    Root = tbFileLocation.Text
                });
                TargetToBind = tdb.Get();
                dgTarget.ItemsSource = TargetToBind;
                LogHelper.Log(string.Format(@"Target {0} posted on DB and list is updated", res.RootName));
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
                dgTarget.ItemsSource = tdb.Get();
                LogHelper.Log(string.Format(@"Target {0} removed from DB and list is updated", SelectedTarget.RootName));
            }
            catch (System.Exception ex)
            {
                LogHelper.LogError(ex.Message);
            }
        }

        private void dgTarget_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            using TargetDbHelper tdb = new TargetDbHelper();
            try
            {
                ((DataGrid)sender).ItemsSource = tdb.Get();
                LogHelper.Log(@"Mod target list is loaded");
            }
            catch (System.Exception ex)
            {
                LogHelper.LogError(ex.Message);
            }
        }

        private void dgTarget_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((DataGrid)sender).HasItems)
            {
                SelectedTarget = ((DataGrid)sender).SelectedItem as Target;
                LogHelper.Log(string.Format(@"mod target id:{0}, mod target:{1} selected!", SelectedTarget.Id, SelectedTarget.RootName));
            }
        }

        private void bModSources_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ParentContainer.Content != null) ParentContainer.Content = null;
            ParentContainer.Content = new ModifierView(ParentContainer, SelectedTarget);
        }
    }
}
