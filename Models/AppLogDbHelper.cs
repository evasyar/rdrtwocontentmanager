using LiteDB;
using rdrtwocontentmanager.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rdrtwocontentmanager.Models
{
    public class AppLogDbHelper : IDisposable
    {

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        public List<AppLog> Get()
        {
            var retval = new List<AppLog>();
            try
            {
                using (var db = new LiteDatabase(@"Rdr2ModsDB"))
                {
                    retval = db.GetCollection<AppLog>("logs").FindAll().OrderByDescending(row => row.creationDate).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return retval;
        }

        public AppLog Get(AppLog model)
        {
            return Get().FirstOrDefault(e => e.Id == model.Id);
        }

        public List<AppLog> Search(string keyword)
        {
            return Get()
                .Where(e => Convert.ToString(e.Id).ToLower().Contains(keyword.ToLower())
                    || e.Log.ToLower().Contains(keyword.ToLower())
                    || e.LogType.ToLower().Contains(keyword.ToLower())
                    || e.modifiedBy.ToLower().Contains(keyword.ToLower())
                    || e.creationDate.ToLongDateString().Contains(keyword)
                    || e.modifiedDate.ToLongDateString().Contains(keyword))
                .ToList();
        }

        public void Delete(object model)
        {
            try
            {
                using (var db = new LiteDatabase(@"Rdr2ModsDB"))
                {
                    var appLogs = db.GetCollection<AppLog>("logs");
                    appLogs.Delete((model as AppLog).Id);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Post(AppLog PostCandidate)
        {
            try
            {
                using (var db = new LiteDatabase(@"Rdr2ModsDB"))
                {
                    Console.WriteLine(String.Format("Writing {0} log on {1}", PostCandidate.LogType, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss tt")));
                    var mods = db.GetCollection<AppLog>("logs");
                    PostCandidate.creationDate = DateTime.Now;
                    PostCandidate.modifiedDate = DateTime.Now;
                    PostCandidate.modifiedBy = UserHelper.GetWinUser();
                    mods.Insert(PostCandidate);
                    Console.WriteLine(String.Format("Completed {0} log", PostCandidate.LogType));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~AppLogDbHelper()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
