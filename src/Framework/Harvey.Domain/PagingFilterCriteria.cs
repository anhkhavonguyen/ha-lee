namespace Harvey.Domain
{
    public class PagingFilterCriteria : FilterCriteriaBase
    {
        public int Page { get; set; }
        public int NumberItemsPerPage { get; set; }
        public static PagingFilterCriteria Default
        {
            get
            {
                return new PagingFilterCriteria()
                {
                    Page = 0,
                    NumberItemsPerPage = 0
                };
            }
        }
    }
}
