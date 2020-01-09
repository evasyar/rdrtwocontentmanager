using rdrtwocontentmanager.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace rdrtwocontentmanager.Models
{
    public class TargetDbHelper : IDisposable
    {

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        public Target Post(Target target)
        {
            Target retval = new Target();
            using (TargetDbHelper db = new TargetDbHelper())
            {
                try
                {
                    target.creationDate = DateTime.Now;
                    target.modifiedDate = DateTime.Now;
                    target.modifiedBy = UserHelper.GetWinUser();
                    db.Post(target);
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(ex.Message);
                    retval = new Target();
                }
            }
            return retval;
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
        // ~TargetDbHelper()
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
