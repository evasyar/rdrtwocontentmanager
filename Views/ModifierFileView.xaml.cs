using rdrtwocontentmanager.Helper;
using rdrtwocontentmanager.Models;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

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
            LoadModFiles(dgList);
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

        private void btnSelectFiles_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var dlg = new OpenFileDialog()
                {
                    InitialDirectory = !string.IsNullOrWhiteSpace(SelectedMod.Source) ? SelectedMod.Source : @"c:\",
                    Filter = @"All files (*.*)|*.*",
                    Multiselect = true
                };
                if ((dlg.ShowDialog() == DialogResult.OK) && (dlg.FileNames.Length > 0))
                {
                    using var mf = new ModifierFileDbHelper();
                    foreach (var item in dlg.FileNames)
                    {
                        mf.Post(new ModifierFile()
                        {
                            SubFolder = FileFolderHelper.GetChildFolder(Path.GetDirectoryName(item),
                            SelectedMod.Source),
                            FileName = Path.GetFileName(item),
                            ModId = SelectedMod.Id,
                            Source = item
                        });
                    }
                    LoadModFiles(dgList);
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.LogError(ex.Message);
            }
        }

        private void LoadModFiles(object container)
        {
            try
            {
                using var db = new ModifierFileDbHelper();
                ((DataGrid)container).ItemsSource = db.Get().Where(e => e.ModId == SelectedMod.Id).OrderByDescending(ord => ord.creationDate);
                LogHelper.Log(string.Format(@"Modifier files for Modifier:{0} are loaded into grid", SelectedMod.Id));
            }
            catch (System.Exception ex)
            {
                LogHelper.LogError(ex.Message);
            }

        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadModFiles(dgList);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new ModifierFileDbHelper();
                db.Delete(SelectedMod);
                LoadModFiles(dgList);
            }
            catch (System.Exception ex)
            {
                LogHelper.LogError(ex.Message);
            }
        }
    }
}
