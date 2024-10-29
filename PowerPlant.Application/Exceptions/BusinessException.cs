namespace PowerPlant.Application.Exceptions;

public class BusinessException : Exception
{
    public string ErrorMessage { get; set; } = string.Empty;

    public BusinessException() : base("Business failure occured.")
    {
        
    }

    public BusinessException(string errorMessage) : this()
    {
        ErrorMessage = errorMessage;
    }
}