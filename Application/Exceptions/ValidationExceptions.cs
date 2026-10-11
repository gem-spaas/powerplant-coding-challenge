namespace Application.Exceptions
{
    public sealed class ValidationExceptions : Exception
    {
        public ValidationExceptions(IEnumerable<ValidationError> errors) => Errors = errors;

        public IEnumerable<ValidationError> Errors { get; }
    }
}
