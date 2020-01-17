using rdrtwocontentmanager.Helper;
using rdrtwocontentmanager.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace rdrtwocontentmanager.Views
{
    /// <summary>
    /// Interaction logic for ModifierView.xaml
    /// </summary>
    public partial class ModifierView : System.Windows.Controls.UserControl
    {
        public ContentControl ParentContainer { get; set; }
        public Target CapTarget { get; set; }
        public Modifier SelectedMod { get; set; }
        public ModifierView(object parentContainer, Target ParentTarget)
        {
            InitializeComponent();
            ParentContainer = (ContentControl)parentContainer;
            CapTarget = ParentTarget;
            RefreshList(CapTarget);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            ParentContainer.Content = null;
        }

        private void btnModTarget_Click(object sender, RoutedEventArgs e)
        {
            if (ParentContainer.Content != null) ParentContainer.Content = null;
            ParentContainer.Content = new ModTarget(ParentContainer, CapTarget);
        }

        private void btnFolderSelect_Click(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    tbSource.Text = fbd.SelectedPath;
                }
            }
        }

        private void btnPost_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var mdb = new ModifierDbHelper();
                mdb.Post(new Modifier() { 
                    TargetId = CapTarget.Id, 
                    Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tbName.Text), 
                    ModifierVersion = blVersion.Text, 
                    Source = tbSource.Text, 
                    ReleaseDate = Convert.ToDateTime(dpReleaseDate.SelectedDate)
                });
                RefreshList(CapTarget);
                LogHelper.Log("New target recorded in database");
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //  before deleting mods make sure no mods are applied!
                ModFileFolderHelper.RemoveAllMods(CapTarget);
                using var mdb = new ModifierDbHelper();
                mdb.Delete(CapTarget);
                RefreshList(CapTarget);
                LogHelper.Log(string.Format("Target:{0} removed from database", CapTarget.RootName));
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshList(CapTarget);
        }

        private void RefreshList(Target target)
        {
            try
            {
                using var mdb = new ModifierDbHelper();
                dgList.ItemsSource = mdb.Get().Where(e => e.TargetId == target.Id);
                LogHelper.Log("Modifiers are loaded from database");
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex.Message);
            }
        }

        private void btnModFiles_Click(object sender, RoutedEventArgs e)
        {
            if (ParentContainer.Content != null) ParentContainer.Content = null;
            ParentContainer.Content = new ModifierFileView(ParentContainer, SelectedMod, CapTarget);
        }

        private void dgList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((DataGrid)sender).HasItems)
            {
                SelectedMod = (((DataGrid)sender).SelectedItem as Modifier) == null ? SelectedMod : ((DataGrid)sender).SelectedItem as Modifier;
                LogHelper.Log(string.Format(@"mod id:{0}, mod:{1} selected!", SelectedMod.Id, SelectedMod.Name));
            }
        }
    }
}
