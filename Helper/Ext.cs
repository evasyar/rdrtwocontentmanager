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
}
