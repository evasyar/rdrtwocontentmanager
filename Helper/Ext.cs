using rdrtwocontentmanager.Models;
using System;

namespace rdrtwocontentmanager.Helper
{
    class Ext
    {
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
}
