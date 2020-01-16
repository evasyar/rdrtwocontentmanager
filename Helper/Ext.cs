using rdrtwocontentmanager.Models;
using System;
using System.Drawing;
using System.IO;

namespace rdrtwocontentmanager.Helper
{
    class Ext
    {
    }

    public static class Defaults
    {
        public static string DbName { get; set; } = @"Rdr2ModsDb";
        public static string Targets { get; set; } = @"targets";
        public static string Mods { get; set; } = @"mods";
        public static string ModFiles { get; set; } = @"modfiles";
        public static string Logs { get; set; } = @"logs";
    }

    public class PlaceHolderTextBox : System.Windows.Forms.TextBox
    {

        bool isPlaceHolder = true;
        string _placeHolderText;
        public string PlaceHolderText
        {
            get { return _placeHolderText; }
            set
            {
                _placeHolderText = value;
                setPlaceholder();
            }
        }

        public new string Text
        {
            get => isPlaceHolder ? string.Empty : base.Text;
            set => base.Text = value;
        }

        //when the control loses focus, the placeholder is shown
        private void setPlaceholder()
        {
            if (string.IsNullOrEmpty(base.Text))
            {
                base.Text = PlaceHolderText;
                this.ForeColor = Color.Gray;
                this.Font = new Font(this.Font, FontStyle.Italic);
                isPlaceHolder = true;
            }
        }

        //when the control is focused, the placeholder is removed
        private void removePlaceHolder()
        {

            if (isPlaceHolder)
            {
                base.Text = "";
                this.ForeColor = System.Drawing.SystemColors.WindowText;
                this.Font = new Font(this.Font, FontStyle.Regular);
                isPlaceHolder = false;
            }
        }
        public PlaceHolderTextBox()
        {
            GotFocus += removePlaceHolder;
            LostFocus += setPlaceholder;
        }

        private void setPlaceholder(object sender, EventArgs e)
        {
            setPlaceholder();
        }

        private void removePlaceHolder(object sender, EventArgs e)
        {
            removePlaceHolder();
        }
    }

    public static class UserHelper
    {
        public static string GetWinUser()
        {
            return System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        }
    }

    public static class LogHelper
    {
        public static void Log(string infoMessage)
        {
            using (AppLogDbHelper db = new AppLogDbHelper())
            {
                try
                {
                    db.Post(new AppLog() { LogType = "INFO", Log = infoMessage });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public static void LogError(string errorMessage)
        {
            using (AppLogDbHelper db = new AppLogDbHelper())
            {
                try
                {
                    db.Post(new AppLog() { LogType = "ERROR", Log = errorMessage });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }

    public static class FileFolderHelper
    {
        public static bool IsChildFolder(string child, string parent)
        {
            bool retval = false;
            //  detect if sub folder
            //  get the sub folder name then assign to appropriate data property
            if (new DirectoryInfo(child).Name.ToLower() != new DirectoryInfo(parent).Name.ToLower())
            {
                retval = true;
            }
            return retval;
        }

        public static string GetChildFolder(string child, string parent)
        {
            return (IsChildFolder(child, parent)) ? new DirectoryInfo(child).Name : string.Empty;
        }
    }
}
