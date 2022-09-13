namespace powerplant_coding_challenge
{

    /// <summary>
    /// This is the class used to store the data of the fuel imported.
    /// </summary>
    public class InputFuels
    {
        // -------------- Constructors --------------
        /// <summary>
        /// This is the main constructor of the class that store the data of the imported fuel.
        /// </summary>
        /// <param name="gasEuroMWH">This is the price rate of the gas.</param>
        /// <param name="kerosinEuroMWH">This is the price rate of the kerosin.</param>
        /// <param name="CO2EuroTon">This is the price rate of the reject of CO2.</param>
        /// <param name="windPercent">This is the wind pourcentage.</param>
        public InputFuels (Double gasEuroMWH, Double kerosinEuroMWH, Double CO2EuroTon, Double windPercent)
        {
            this.GasEuroMWH = gasEuroMWH;
            this.KersosinEuroMWH= kerosinEuroMWH;
            this.CO2EuroTon = CO2EuroTon;
            this.WindPercent = windPercent;
        }

        // -------------- Properties --------------
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
