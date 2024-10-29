using FluentValidation.Results;

namespace PowerPlant.Application.Exceptions;

public class ValidationException : Exception
{
    
    public IDictionary<string, string[]> Errors { get; }
    public ValidationException() : base("Validation failures occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }
    
    public ValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        Errors = failures
            .GroupBy(
                ex => ex.PropertyName, 
                ex => ex.ErrorMessage)
            .ToDictionary(
                group => group.Key, 
                group => group.ToArray());
    }
}