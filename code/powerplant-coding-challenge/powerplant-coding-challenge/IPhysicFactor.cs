namespace powerplant_coding_challenge
{
    /// <summary>
    /// This interface is used to modify the output of a productor thanks to a physic factor : wind, solar, ...
    /// </summary>
    public interface IPhysicFactor
    {
        // -------------- Methods --------------
        /// <summary>
        /// This method is used to compute the final power of the productor
        /// </summary>
        /// <param name="powerInput">This is the input power of the productor.</param>
        /// <returns>The output power of the productor.</returns>
        Double ComputePower (Double powerInput);

        // -------------- Properties --------------
        /// <summary>
        /// This is the current physic value.
        /// </summary>
        Double PhysicValue
        {
            get;
            set;
        }
    }
}
