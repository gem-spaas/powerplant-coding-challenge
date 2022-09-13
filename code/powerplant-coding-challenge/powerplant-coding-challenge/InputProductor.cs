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
        /// <param name="aon">This is the all or nothing mode of the productor.</param>
        public InputProductor (String name, String type, Double efficiency, Double pmin, Double pmax, Boolean aon = false)
        {
            this.Name = name;
            this.Type = type;
            this.Efficiency = efficiency;
            this.PMin = pmin;
            this.PMax = pmax;
            this.AON = aon;
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

        /// <summary>
        /// This is the method used to compute the activation function.
        /// If it is true, the productor is in ALL OR NOTHING mode.
        /// If it is false, the productor is in LINEAR mode.
        /// </summary>
        public Boolean AON
        {
            get;
        }
    }
}
