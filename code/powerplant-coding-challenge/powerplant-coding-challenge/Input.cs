namespace powerplant_coding_challenge
{
    /// <summary>
    /// This is the class used to store the data of the input.
    /// </summary>
    public class Input
    {
        // -------------- Constructor --------------
        /// <summary>
        /// This is the main constructor of the object that holds the input data.
        /// </summary>
        /// <param name="load">This is the value of the expected load.</param>
        /// <param name="fuels">These are the data about the fuels.</param>
        /// <param name="productors">These are the data about the productors.</param>
        public Input (Double load, InputFuels fuels, IList<InputProductor> productors)
        {
            this.Load = load;
            this.Fuels = fuels;
            this.Productors = productors;
        }

        // -------------- Properties --------------
        /// <summary>
        /// This is the expected load of the grid.
        /// </summary>
        public Double Load
        {
            get;
        }

        /// <summary>
        /// These are the data about the fuels.
        /// </summary>
        public InputFuels Fuels
        {
            get;
        }

        /// <summary>
        /// These are the data about the productors.
        /// </summary>
        public IList<InputProductor> Productors
        {
            get;
        }
    }
}
