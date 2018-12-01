using System;
using System.Collections.Generic;

namespace Harvey.CRMLoyalty.Application.Extensions.PagingExtensions
{
    public abstract class PagedResultBase
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItem { get; set; }

        public int FirstRowOnPage
        {
            get { return (PageNumber - 1) * PageSize + 1; }
        }

        public int LastRowOnPage
        {
            get { return Math.Min(PageNumber * PageSize, TotalItem); }
        }
    }

    public class PagedResult<T> : PagedResultBase where T : class
    {
        public List<T> Results { get; set; }

        public PagedResult()
        {
            Results = new List<T>();
        }
    }
}
