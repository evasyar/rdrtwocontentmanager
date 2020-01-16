using System;
using System.Collections.Generic;
using System.Text;

namespace rdrtwocontentmanager.Models
{
    public class ModifierFile : RecordBase
    {
        public string Id { get; set; }
        public string ModId { get; set; }
        public string Source { get; set; }
        public string FileName { get; set; }
        public string SubFolder { get; set; }
    }
}
