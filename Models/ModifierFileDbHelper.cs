using LiteDB;
using rdrtwocontentmanager.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace rdrtwocontentmanager.Models
{
    public class ModifierFileDbHelper : IDisposable
    {
        public List<ModifierFile> Get()
        {
            List<ModifierFile> retval = new List<ModifierFile>();
            using (var db = new LiteDatabase(Defaults.DbName))
            {
                try
                {
                    var modfiles = db.GetCollection<ModifierFile>(Defaults.ModFiles);
                    retval = modfiles.Find(Query.All("creationDate", Query.Descending)).ToList();
                    LogHelper.Log(@"List of modifier files returned");
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(ex.Message);
                }
            }
            return retval;
        }

        public List<ModifierFile> Search(string keyword)
        {
            List<ModifierFile> retval = new List<ModifierFile>();
            using (var db = new LiteDatabase(Defaults.DbName))
            {
                try
                {
                    var modfiles = db.GetCollection<ModifierFile>(Defaults.ModFiles);
                    retval = modfiles.FindAll().Where(e => e.Id.ToLower().Contains(keyword.ToLower())
                    || e.creationDate.ToShortDateString().Contains(keyword)
                    || e.modifiedBy.ToLower().Contains(keyword.ToLower())
                    || e.modifiedDate.ToShortDateString().Contains(keyword)
                    || e.FileName.ToLower().Contains(keyword.ToLower())
                    || e.ModId.ToLower().Contains(keyword.ToLower())
                    || e.Source.ToLower().Contains(keyword.ToLower())
                    || e.SubFolder.ToLower().Contains(keyword.ToLower())).ToList();
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

        public void Delete(ModifierFile modfile)
        {
            using (var db = new LiteDatabase(Defaults.DbName))
            {
                try
                {
                    //  if data invalid then throw error                   
                    var modfiles = db.GetCollection<ModifierFile>(Defaults.ModFiles);
                    modfiles.Delete(modfile.Id);
                    LogHelper.Log(string.Format(@"Mod files:{0} deleted from DB", modfile.Id));
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(ex.Message);
                }
            }
        }

        public void Delete(Modifier mod)
        {
            using (var db = new LiteDatabase(Defaults.DbName))
            {
                try
                {
                    //  if data invalid then throw error                   
                    var modfiles = db.GetCollection<ModifierFile>(Defaults.ModFiles);
                    foreach (var item in modfiles.FindAll().Where(e => e.ModId.ToLower() == mod.Id))
                    {
                        modfiles.Delete(item.Id);
                        LogHelper.Log(string.Format(@"Mod file:{0} deleted from DB", item.Id));
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(ex.Message);
                }
            }
        }

        public ModifierFile Post(ModifierFile modfile)
        {
            ModifierFile retval = new ModifierFile();
            using (var db = new LiteDatabase(Defaults.DbName))
            {
                try
                {
                    //  if data invalid then throw error                   
                    var modfiles = db.GetCollection<ModifierFile>(Defaults.ModFiles);
                    //  target name and location are required
                    if (string.IsNullOrWhiteSpace(modfile.FileName))
                        throw new Exception(@"Mod name cannot be empty");
                    if (string.IsNullOrWhiteSpace(modfile.Source))
                        throw new Exception(@"Mod source cannot be empty");
                    if (modfiles.Exists(e => e.Source.ToLower() == modfile.Source.ToLower()))
                        throw new Exception(string.Format(@"Mod source {0} already exist in DB", modfile.Source));
                    if (modfiles.Exists(e => e.FileName.ToLower() == modfile.FileName.ToLower()))
                        throw new Exception(string.Format(@"Mod file {0} already used in DB", modfile.FileName));
                    modfile.Id = Guid.NewGuid().ToString();
                    modfile.creationDate = DateTime.Now;
                    modfile.modifiedDate = DateTime.Now;
                    modfile.modifiedBy = UserHelper.GetWinUser();
                    modfiles.Insert(modfile);
                    LogHelper.Log(string.Format(@"New mod file:{0}, located at file location:{1} posted in DB", modfile.FileName, modfile.Source));
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(ex.Message);
                    retval = new ModifierFile();
                }
            }
            return retval;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

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
        // ~ModifierFileDbHelper()
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
