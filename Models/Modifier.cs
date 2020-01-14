namespace rdrtwocontentmanager.Models
{
    public class Modifier : AuditEntry
    {
        public string Id { get; set; }
        public string ModifierId { get; set; }
        public string Source { get; set; }
        public string FileName { get; set; }
        public string Subfolder { get; set; }
    }
}
