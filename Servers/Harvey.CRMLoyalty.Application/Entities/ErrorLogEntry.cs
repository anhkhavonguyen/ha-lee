using System;

namespace Harvey.CRMLoyalty.Application.Entities
{
    public class ErrorLogEntry
    {
        public string Id { get; set; }
        public string Detail { get; set; }
        public string Caption { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int ErrorSourceId { get; set; }
        public virtual ErrorLogSource ErrorLogSource { get; set; }
    }

    public enum SourceErrorLog
    {
        AdminApp = 1,
        MemberApp = 2,
        StoreApp = 3,
        BackEnd = 4,
        FrontEnd = 5
    }
}
