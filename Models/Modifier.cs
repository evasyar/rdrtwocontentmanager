using System;

namespace rdrtwocontentmanager.Models
{
    public class Modifier : RecordBase
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
        public string ModifierVersion { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string TargetId { get; set; }
    }
}
