namespace PowerplantCodingChallenge.Business.Extensions
{
    internal static class NumericExtensions
    {
        internal static int ToTenths(this decimal value)
        {
            return (int)Math.Round(value * 10m, MidpointRounding.AwayFromZero);
        }
    }
}
