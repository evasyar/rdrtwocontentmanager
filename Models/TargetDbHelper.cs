using LiteDB;
using rdrtwocontentmanager.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace rdrtwocontentmanager.Models
{
    public class TargetDbHelper : IDisposable
    {

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        public Target Post(Target target)
        {
            Target retval = new Target();
            using (var db = new LiteDatabase(Defaults.DbName))
            {
                try
                {
                    //  if data invalid then throw error                   
                    var targets = db.GetCollection<Target>(Defaults.Targets);
                    //  target name and location are required
                    if (string.IsNullOrWhiteSpace(target.Root))
                        throw new Exception(@"Mod target file location cannot be empty");
                    if (string.IsNullOrWhiteSpace(target.RootName))
                        throw new Exception(@"Mod target name cannot be empty");
                    if (targets.Exists(e => e.Root.ToLower() == target.Root.ToLower())) 
                        throw new Exception(string.Format(@"Mod Target {0} already exist in DB", target.Root));
                    if (targets.Exists(e => e.RootName.ToLower() == target.RootName.ToLower()))
                        throw new Exception(string.Format(@"Mod Target Name {0} already used in DB", target.RootName));
                    target.Id = Guid.NewGuid().ToString();
                    target.creationDate = DateTime.Now;
                    target.modifiedDate = DateTime.Now;
                    target.modifiedBy = UserHelper.GetWinUser();
                    targets.Insert(target);
                    LogHelper.Log(string.Format(@"New mod target:{0}, located at file location:{1} posted in DB", target.RootName, target.Root));
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(ex.Message);
                    retval = new Target();
                }
            }
            return retval;
        }

        public void Delete(Target target)
        {
            using (var db = new LiteDatabase(Defaults.DbName))
            {
                try
                {
                    //  if data invalid then throw error                   
                    var targets = db.GetCollection<Target>(Defaults.Targets);
                    //  delete child records first!
                    using var md = new ModifierDbHelper();
                    md.Delete(target);
                    //  now delete the parent!
                    targets.Delete(target.Id);
                    LogHelper.Log(string.Format(@"Mod target:{0} deleted from DB", target.Id));
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(ex.Message);                    
                }
            }
        }

        public List<Target> Get()
        {
            List<Target> retval = new List<Target>();
            using (var db = new LiteDatabase(Defaults.DbName))
            {
                try
                {
                    var targets = db.GetCollection<Target>(Defaults.Targets);
                    retval = targets.Find(Query.All("creationDate", Query.Descending)).ToList();
                    LogHelper.Log(@"List of mod targets returned");
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(ex.Message);
                }
            }
            return retval;
        }

        public List<Target> Search(string keyword)
        {
            List<Target> retval = new List<Target>();
            using (var db = new LiteDatabase(Defaults.DbName))
            {
                try
                {
                    var targets = db.GetCollection<Target>(Defaults.Targets);
                    retval = targets.FindAll().Where(e => e.Id.ToLower().Contains(keyword.ToLower())
                    || e.creationDate.ToShortDateString().Contains(keyword)
                    || e.modifiedBy.ToLower().Contains(keyword.ToLower())
                    || e.modifiedDate.ToShortDateString().Contains(keyword)
                    || e.Root.ToLower().Contains(keyword.ToLower())
                    || e.RootName.ToLower().Contains(keyword.ToLower())).ToList();
                    if (retval.Count < 0)
                    {
                        retval = Get();
                    }
                    LogHelper.Log(@"Search of mod targets completed");
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(ex.Message);
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
