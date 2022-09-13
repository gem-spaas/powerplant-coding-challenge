namespace powerplant_coding_challenge
{
    /// <summary>
    /// This is the class that holds the output data of the productor.
    /// </summary>
    public class OutputProductor
    {
        // -------------- Constructors --------------
        /// <summary>
        /// This is the main constructor of the output productor.
        /// </summary>
        /// <param name="name">This is the name of the productor.</param>
        /// <param name="power">This is the power that produce the productor.</param>
        public OutputProductor (String name, Double power)
        {
            this.name = name;
            this.p = power;
        }

        // -------------- Properties --------------
        /// <summary>
        /// This is the name of the productor.
        /// </summary>
        public String name
        {
            get;
        }

        /// <summary>
        /// This is the power that produce the productor.
        /// </summary>
        public Double p
        {
            get;
        }
    }
}
