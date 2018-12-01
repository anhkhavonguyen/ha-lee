using System.Collections.Generic;

namespace Harvey.Domain
{
    public class PagedResult<T>
    {
        public int TotalItems { get; set; }
        public int TotalPages => (TotalItems / NumberItemsPerPage) + ((TotalItems % NumberItemsPerPage) > 0 ? 1 : 0);
        public int CurrentPage { get; set; }
        public int NumberItemsPerPage { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
