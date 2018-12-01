namespace Harvey.Exception
{
    public class DebugProblemDetails : ProblemDetails
    {
        public string DebugInfo { get; set; }

        public DebugProblemDetails(ProblemDetails problemDetails)
        {
            this.Detail = problemDetails.Detail;
            this.Status = problemDetails.Status;
            this.Title = problemDetails.Title;
        }
    }
}
