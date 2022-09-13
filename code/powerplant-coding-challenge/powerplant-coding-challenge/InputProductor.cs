namespace powerplant_coding_challenge
{
    /// <summary>
    /// This class is used to store the data of the productors.
    /// </summary>
    public class InputProductor
    {

        // -------------- Constructors --------------
        /// <summary>
        /// This is the main constructor of the class productor.
        /// </summary>
        /// <param name="name">This is the name of the productor.</param>
        /// <param name="type">This is the type of the productor.</param>
        /// <param name="efficiency">This is the efficiency of the productor.</param>
        /// <param name="pmin">This is the minimum power that the productor produces if it is activated.</param>
        /// <param name="pmax">This is the maximum power that the productor produces if it is activated.</param>
        public InputProductor (String name, String type, Double efficiency, Double pmin, Double pmax)
        {
            this.Name = name;
            this.Type = type;
            this.Efficiency = efficiency;
            this.PMin = pmin;
            this.PMax = pmax;
        }

        // -------------- Methods --------------
        /// <summary>
        /// This method allows to create a Productor object thanks to the data stored inside the current InputProductor object.
        /// </summary>
        /// <param name="physicFactors">These are the optional physics factor.</param>
        /// <returns>The freshly created Productor object.</returns>
        public Productor CreateProductor (IList<IPhysicFactor>? physicFactors = null)
        {
            return new Productor(this.Name, this.Type, this.Efficiency, this.PMin, this.PMax, physicFactors);
        }

        // -------------- Properties --------------
        /// <summary>
        /// This is the name of the productor.
        /// </summary>
        public String Name
        {
            get;
        }

        /// <summary>
        /// This is the type of the productor.
        /// </summary>
        public String Type
        {
            get;
        }

        /// <summary>
        /// This is the efficiency of the productor.
        /// </summary>
        public Double Efficiency
        {
            get;
        }

        /// <summary>
        /// This is the pmin of the productor.
        /// </summary>
        public Double PMin
        {
            get;
        }

        /// <summary>
        /// This is the pmax of the productor.
        /// </summary>
        public Double PMax
        {
            get;
        }
    }
}
