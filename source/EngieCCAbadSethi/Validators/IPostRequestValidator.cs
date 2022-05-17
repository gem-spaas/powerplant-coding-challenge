namespace CalculatePowerGenerationByPowerPlants.Validators
{
    public interface IPostRequestValidator<T> where T : class
    {
        /// <summary>
        /// Validates the incoming request parameter
        /// </summary>
        /// <param name="request">request parameter of the controller</param>
        void Validate(T request);

    }
}
