namespace Harvey.Exception
{
    public class IndexingException : ProductionException
    {
        public dynamic Item { get; set; }
        public IndexingException()
        {
        }

        public IndexingException(string message) :
            base(message)
        {
        }

        public IndexingException(string message, System.Exception inner) :
            base(message, inner)
        {
        }
    }
}
