namespace Domain.Exceptions
{
    public class InvalidFuelException : DomainException
    {
        public InvalidFuelException(string message) : base(message)
        {
        }
    }
}
