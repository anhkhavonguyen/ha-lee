using System.Net;
namespace Harvey.Exception.Handlers
{
    public class EfUniqueConstraintExceptionHandler : IExceptionHandler
    {
        private const string GENERIC_ERROR_MESSAGE = "Something went wrong! Please contact administrators.";
        public ProblemDetails Handle(System.Exception ex, ref bool hasHandle)
        {
            var exception = ex;
            ProblemDetails result = new ProblemDetails()
            {
                Title = HttpStatusCode.BadRequest.ToString(),
                Status = (int)HttpStatusCode.BadRequest,
                Detail = ""
            };

            if (ex != null && ex.InnerException != null && ex.InnerException.InnerException != null)
            {
                result.Detail = ((dynamic)ex.InnerException?.InnerException).Detail ?? GENERIC_ERROR_MESSAGE;
                hasHandle = true;
                return result;
            }

            if (ex != null && ex.InnerException != null)
            {
                if (ex.InnerException.Message.Contains("23505: duplicate key value violates unique constraint."))
                {
                    result.Detail = ((dynamic)ex.InnerException).Detail ?? GENERIC_ERROR_MESSAGE;
                    hasHandle = true;
                    return result;
                }
            }
            return result;
        }
    }
}
