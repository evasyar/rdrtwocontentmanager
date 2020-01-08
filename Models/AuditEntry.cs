using System;

namespace rdrtwocontentmanager.Models
{
    public interface IAuditEntry
    {
        DateTime creationDate { get; set; }
        string modifiedBy { get; set; }
        DateTime modifiedDate { get; set; }
    }

    public class AuditEntry : IAuditEntry
    {
        public DateTime creationDate { get; set; }
        public DateTime modifiedDate { get; set; }
        public string modifiedBy { get; set; }
    }
}
