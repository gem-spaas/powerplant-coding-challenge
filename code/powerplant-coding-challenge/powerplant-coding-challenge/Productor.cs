namespace powerplant_coding_challenge
{
    /// <summary>
    /// This is the class that allows to holds the data of the productors.
    /// </summary>
    public class Productor
    {
        // -------------- Fields --------------
        /// <summary>
        /// This is the activation factor of the productor.
        /// </summary>
        private Double activation;

        /// <summary>
        /// This is the price of the productor.
        /// </summary>
        private Double price;

        /// <summary>
        /// This is the list of physic factor applied on the productor.
        /// </summary>
        private IList<IPhysicFactor> physicFactors = new List<IPhysicFactor>();

        // -------------- Constructors --------------
        /// <summary>
        /// This si the main constructor of the class productor.
        /// </summary>
        /// <param name="index">This is the index to reorder the productors on the right order.</param>
        /// <param name="name">This is the name of the productor.</param>
        /// <param name="type">This is the type of the productor.</param>
        /// <param name="efficiency">This is the efficiency of the productor.</param>
        /// <param name="pmin">This is the minimum power that the productor produces if it is activated.</param>
        /// <param name="pmax">This is the maximum power that the productor produces if it is activated.</param>
        /// <param name="price">This is the price rate for the current productor.</param>
        /// <param name="aon">This is the all or nothing mode of the productor.</param>
        /// <param name="physicFactor">These are the physics factor to apply on the productor.</param>
        public Productor (Int32 index, String name, String type, Double efficiency, Double pmin, Double pmax, Double price, Boolean aon = false, IList<IPhysicFactor>? physicFactor = null)
        {
            this.Index = index;
            this.Name = name;
            this.Type = type;
            this.Efficiency = efficiency;
            this.PMin = pmin;
            this.PMax = pmax;
            this.price = price;
            this.AON = aon;
            this.activation = 0;

            if (physicFactor != null)
            {
                foreach (IPhysicFactor pf in physicFactor)
                {
                    this.physicFactors.Add(pf);
                }
            }
        }

        // -------------- Properties --------------
        /// <summary>
        /// This is the index of the productor.
        /// </summary>
        public Int32 Index
        {
            get;
        }

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
        /// This is the price of the productor.
        /// </summary>
        public Double PriceRate
        {
            get => this.price * this.Efficiency;
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

        /// <summary>
        /// This is the activation state of the productor.
        /// </summary>
        public Double Activation
        {
            get => this.activation;
            set
            {
                if (this.AON)
                {
                    if (value > 0)
                    {
                        this.activation = 1;
                    }
                    else
                    {
                        this.activation = 0;
                    }
                }
                else
                {
                    if (value < 0)
                    {
                        this.activation = 0;
                    }
                    else if (value > 1)
                    {
                        this.activation = 1;
                    }
                    else
                    {
                        this.activation = value;
                    }
                }
            }
        }

        /// <summary>
        /// This is the output power of the productor.
        /// </summary>
        public Double OutputPower
        {
            get
            {
                Double power = 0;

                if (activation == 0)
                {
                    return power;
                }

                power = this.PMin + this.Activation * (this.PMax - this.PMin);

                foreach (IPhysicFactor pf in this.physicFactors)
                {
                    power = pf.ComputePower(power);
                }

                return Math.Round(power, 1);
            }
        }

    }
}
