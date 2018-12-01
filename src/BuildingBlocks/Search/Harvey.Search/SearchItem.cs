using System;
using Harvey.Search.Abstractions;

namespace Harvey.Search
{
    public abstract class SearchItem : ISearchItem
    {
        public Guid Id { get; set; }

        public SearchItem()
        {

        }

        public SearchItem(Guid id) : this()
        {
            Id = id;
        }
    }
}
