using System;

namespace rdrtwocontentmanager.Models
{
    public interface IRecordBase
    {
        DateTime creationDate { get; set; }
        string modifiedBy { get; set; }
        DateTime modifiedDate { get; set; }
    }

    public class RecordBase : IRecordBase
    {
        public DateTime creationDate { get; set; }
        public DateTime modifiedDate { get; set; }
        public string modifiedBy { get; set; }
    }
}
