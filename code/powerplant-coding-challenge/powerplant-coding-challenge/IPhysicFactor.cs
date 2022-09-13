namespace powerplant_coding_challenge
{
    /// <summary>
    /// This interface is used to modify the output of a productor thanks to a physic factor : wind, solar, ...
    /// </summary>
    public interface IPhysicFactor
    {
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
