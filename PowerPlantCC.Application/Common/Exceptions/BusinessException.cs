namespace PowerPlantCC.Application.Common.Exceptions;

public class BusinessException : Exception
{
    public string ErrorMessage { get; set; } = string.Empty;

    public BusinessException() : base("A business failure has occured.")
    { }

    public BusinessException(string errorMessage) : this()
    {
        ErrorMessage = errorMessage;
    }

}