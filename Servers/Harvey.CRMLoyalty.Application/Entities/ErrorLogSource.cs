using System.Collections.Generic;

namespace Harvey.CRMLoyalty.Application.Entities
{
    public class ErrorLogSource
    {
        public int Id { get; set; }
        public string SourceName { get; set; }
        public virtual ICollection<ErrorLogEntry> ErrorLogEntries { get; set; }
    }
}
