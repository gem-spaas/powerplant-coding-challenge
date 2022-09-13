namespace powerplant_coding_challenge
{

    /// <summary>
    /// This is the class used to store the data of the fuel imported.
    /// </summary>
    public class InputFuels
    {
        /// <summary>
        /// This is the price rate of the gas.
        /// </summary>
        public Double GasEuroMWH
        {
            get;
        }

        /// <summary>
        /// This is the price rate of the kerosine.
        /// </summary>
        public Double KersosinEuroMWH
        {
            get;
        }

        /// <summary>
        /// This is the price rate of the reject of CO2.
        /// </summary>
        public Double CO2EuroTon
        {
            get;
        }
        
        /// <summary>
        /// This is the percentage of the wind.
        /// </summary>
        public Double WindPercent
        {
            get;
        }

    }
}
