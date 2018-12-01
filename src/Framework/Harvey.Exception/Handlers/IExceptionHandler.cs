namespace Harvey.Exception.Handlers
{
    public interface IExceptionHandler
    {
        ProblemDetails Handle(System.Exception ex, ref bool hasHandle);
    }
}
