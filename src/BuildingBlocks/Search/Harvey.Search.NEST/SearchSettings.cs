using Harvey.Search.Abstractions;

namespace Harvey.Search.NEST
{
    public class SearchSettings : ISearchSettings
    {
        public string Url { get; set; }
        public SearchSettings(string Url)
        {
            this.Url = Url;
        }
    }
}
