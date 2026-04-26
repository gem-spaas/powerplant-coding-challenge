namespace PowerplantCodingChallenge.Business.Models.Exceptions
{
    public sealed class BusinessException : Exception
    {
        public BusinessException(string message) : base(message) { }
    }
}
