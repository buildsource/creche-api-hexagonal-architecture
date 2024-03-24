namespace Creche.Application.Exceptions;

public class ValidationException : Exception
{
    public IEnumerable<string> Errors { get; private set; }

    public ValidationException(IEnumerable<string> errors)
        : base("Validation error")
    {
        Errors = errors;
    }
}