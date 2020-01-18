using rdrtwocontentmanager.Helper;
using rdrtwocontentmanager.Models;
using System;
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
            ParentContainer.Content = new ModifierView(ParentContainer, SelectedTarget, SelectedMod);
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
                //  before deleting mods make sure mods not applied to target!
                ModFileFolderHelper.RemoveAllMods(SelectedTarget);
                using var db = new ModifierFileDbHelper();
                db.Delete(SelectedMod);
                LoadModFiles(dgList);
            }
            catch (System.Exception ex)
            {
                LogHelper.LogError(ex.Message);
            }
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new ModifierFileDbHelper();
                var modfiles = db.Get().FindAll(row => row.ModId.ToLower() == SelectedMod.Id.ToLower() && !string.IsNullOrWhiteSpace(row.SubFolder));
                foreach (var item in modfiles)
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(SelectedTarget.Root, item.SubFolder));
                    string _dir = System.IO.Path.Combine(SelectedTarget.Root, item.SubFolder);
                    System.IO.File.Copy(item.Source, System.IO.Path.Combine(_dir, item.FileName), true);
                    LogHelper.Log(string.Format("File {0} copied to destination: {1}", item.FileName, SelectedTarget.Root));
                }
                modfiles = db.Get().FindAll(row => row.ModId.ToLower() == SelectedMod.Id.ToLower() && string.IsNullOrWhiteSpace(row.SubFolder));
                foreach (var item in modfiles)
                {
                    System.IO.File.Copy(item.Source, System.IO.Path.Combine(SelectedTarget.Root, item.FileName), true);
                    LogHelper.Log(string.Format("File {0} copied to destination: {1}", item.FileName, SelectedTarget.Root));
                }

                System.Windows.MessageBox.Show(string.Format("Mod files copied to destination: {0}", SelectedTarget.Root));
            }
            catch (System.Exception ex)
            {
                LogHelper.LogError(ex.Message);
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new ModifierFileDbHelper();
                var modfiles = db.Get().FindAll(row => row.ModId.ToLower() == SelectedMod.Id.ToLower() && !string.IsNullOrWhiteSpace(row.SubFolder));
                foreach (var item in modfiles)
                {
                    string _dir = System.IO.Path.Combine(SelectedTarget.Root, item.SubFolder);
                    try
                    {
                        System.IO.File.Delete(System.IO.Path.Combine(_dir, item.FileName));
                        LogHelper.Log(string.Format("File {0} deleted from destination: {1}", item.FileName, SelectedTarget.Root));
                    }
                    catch (Exception ex)
                    {
                        LogHelper.LogError(ex.Message);
                    }
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(item.SubFolder)) System.IO.Directory.Delete(_dir, true);
                        LogHelper.Log(string.Format("Subdirectory {0} deleted from destination: {1}", _dir, SelectedTarget.Root));
                    }
                    catch (Exception ex)
                    {
                        LogHelper.LogError(ex.Message);
                    }
                }
                modfiles = db.Get().FindAll(row => row.ModId.ToLower() == SelectedMod.Id.ToLower() && string.IsNullOrWhiteSpace(row.SubFolder));
                foreach (var item in modfiles)
                {
                    try
                    {
                        System.IO.File.Delete(System.IO.Path.Combine(SelectedTarget.Root, item.FileName));
                        LogHelper.Log(string.Format("File {0} deleted from destination: {1}", item.FileName, SelectedTarget.Root));
                    }
                    catch (Exception ex)
                    {
                        LogHelper.LogError(ex.Message);
                    }
                }

                System.Windows.MessageBox.Show(string.Format("Mod files removed from destination: {0}", SelectedTarget.Root));
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex.Message);
            }
        }

        private void btnRemoveAll_Click(object sender, RoutedEventArgs e)
        {
            ModFileFolderHelper.RemoveAllMods(SelectedTarget);
        }

        private void btnAcf_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SelectedTarget.Root)) throw new Exception("MOD file destination should not be empty!");
                if (string.IsNullOrWhiteSpace(tbCustomFile.Text)) throw new Exception("MOD custom file should not be empty!");
                using var db = new ModifierFileDbHelper();
                db.Post(new ModifierFile() { 
                    ModId = SelectedMod.Id, 
                    Source = System.IO.Path.Combine(SelectedMod.Source, tbCustomFile.Text), 
                    FileName = tbCustomFile.Text
                });
            }
            catch (System.Exception ex)
            {
                LogHelper.LogError(ex.Message);
            }
        }

        private void btnInspectTarget_Click(object sender, RoutedEventArgs e)
        {
            FileInspectorHelper.StartProcess(SelectedTarget.Root);
        }
    }
}
