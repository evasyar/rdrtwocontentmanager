using System;

namespace rdrtwocontentmanager.Models
{
    public class AppLog : AuditEntry
    {
        public int Id { get; set; }
        public string LogType { get; set; }
        public string Log { get; set; }
    }
}
