namespace powerplant_coding_challenge
{
    /// <summary>
    /// This is the class used to add a wind factor to a productor.
    /// </summary>
    public class WindFactor : IPhysicFactor
    {

        // -------------- Constructors --------------
        /// <summary>
        /// This is the main constructor of the wind factor object.
        /// </summary>
        /// <param name="windValue">This is the wind value.</param>
        public WindFactor (Double windValue)
        {
            this.PhysicValue = windValue;
        }

        // -------------- Methods --------------

        public double ComputePower(double powerInput)
        {
            return powerInput * this.PhysicValue;
        }


        // -------------- Methods --------------

        public Double PhysicValue
        {
            get;
            set;
        } = 0;
    }
}
