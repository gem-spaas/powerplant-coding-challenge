namespace Domain.Exceptions
{
    public class InvalidPowerplantException : DomainException
    {
        public InvalidPowerplantException(string message) : base(message)
        {
        }
    }
}
