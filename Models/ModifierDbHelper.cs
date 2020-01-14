using LiteDB;
using rdrtwocontentmanager.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace rdrtwocontentmanager.Models
{
    public class ModifierDbHelper : IDisposable
    {

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        public Modifier Post(Modifier modifier)
        {
            Modifier retval = new Modifier();
            using (var db = new LiteDatabase(Defaults.DbName))
            {
                try
                {
                    //  if data invalid then throw error                   
                    var mods = db.GetCollection<Modifier>(Defaults.Mods);
                    //  target name and location are required
                    if (string.IsNullOrWhiteSpace(modifier.FileName))
                        throw new Exception(@"Mod target file name cannot be empty");
                    if (string.IsNullOrWhiteSpace(modifier.Source))
                        throw new Exception(@"Mod Modifier source cannot be empty");
                    if (mods.Exists(e => e.Source.ToLower() == modifier.Source.ToLower())) 
                        throw new Exception(string.Format(@"Mod Modifier {0} already exist in DB", modifier.Source));
                    if (mods.Exists(e => e.FileName.ToLower() == modifier.FileName.ToLower()))
                        throw new Exception(string.Format(@"Mod Modifier Filename {0} already used in DB", modifier.FileName));
                    modifier.Id = Guid.NewGuid().ToString();
                    modifier.creationDate = DateTime.Now;
                    modifier.modifiedDate = DateTime.Now;
                    modifier.modifiedBy = UserHelper.GetWinUser();
                    mods.Insert(modifier);
                    LogHelper.Log(string.Format(@"New mod Modifier:{0}, located at file location:{1} posted in DB", modifier.FileName, modifier.Source));
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(ex.Message);
                    retval = new Modifier();
                }
            }
            return retval;
        }

        public void Delete(Modifier modifier)
        {
            using (var db = new LiteDatabase(Defaults.DbName))
            {
                try
                {
                    //  if data invalid then throw error                   
                    var mods = db.GetCollection<Modifier>(Defaults.Mods);
                    mods.Delete(modifier.Id);
                    LogHelper.Log(string.Format(@"Mod Modifier:{0} deleted from DB", modifier.Id));
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(ex.Message);                    
                }
            }
        }

        public List<Modifier> Get()
        {
            List<Modifier> retval = new List<Modifier>();
            using (var db = new LiteDatabase(Defaults.DbName))
            {
                try
                {
                    var mods = db.GetCollection<Modifier>(Defaults.Mods);
                    retval = mods.Find(Query.All("creationDate", Query.Descending)).ToList();
                    LogHelper.Log(@"List of Modifiers returned");
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(ex.Message);
                }
            }
            return retval;
        }

        public List<Modifier> Search(string keyword)
        {
            List<Modifier> retval = new List<Modifier>();
            using (var db = new LiteDatabase(Defaults.DbName))
            {
                try
                {
                    var mods = db.GetCollection<Modifier>(Defaults.Mods);
                    retval = mods.FindAll().Where(e => e.Id.ToLower().Contains(keyword.ToLower())
                    || e.creationDate.ToLongDateString().Contains(keyword)
                    || e.modifiedBy.ToLower().Contains(keyword.ToLower())
                    || e.modifiedDate.ToLongTimeString().Contains(keyword)
                    || e.Source.ToLower().Contains(keyword.ToLower())
                    || e.ModifierId.ToLower().Contains(keyword.ToLower())
                    || e.Subfolder.ToLower().Contains(keyword.ToLower())
                    || e.FileName.ToLower().Contains(keyword.ToLower())).ToList();
                    if (retval.Count < 0)
                    {
                        retval = Get();
                    }
                    LogHelper.Log(@"Search of mod Modifiers completed");
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
        // ~ModifierDbHelper()
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
