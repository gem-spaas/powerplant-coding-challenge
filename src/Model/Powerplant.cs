namespace PowerplantChallenge.Model {

    public class Powerplant
    {
        public string Name { get; set; }

        public PowerplantType PowerplantType { get; set; }

        public decimal Efficiency { get; set; }

        public decimal MaxPower { get ; set; }

        public decimal MinPower { get; set; }

        public decimal ProposedLoad { get; set; }

        public decimal BaseCost { get; set; }

        private decimal CalculateCost(decimal fuelCost, decimal co2Cost){
            
            if(this.PowerplantType.FuelType == FuelType.Wind) {
                return 0;
            }

            return fuelCost / this.Efficiency + (this.PowerplantType.GenerateCO2 ? 0.3M * co2Cost : 0);
        }

        public Powerplant(PowerplantInput input, IEnumerable<(FuelType FuelType, decimal Cost)> fuelCosts){
            this.Name = input.Name;
            this.PowerplantType = PowerplantType.SupportedTypes.First(st => st.TypeLib == input.Type);

            //TODO Manage unsuported powerplant error

            this.Efficiency = input.Efficiency;
            this.MinPower = input.PMin;
            this.MaxPower = this.PowerplantType.FuelType == FuelType.Wind ? 
                fuelCosts.First(fc => fc.FuelType == FuelType.Wind).Cost / 100 * input.PMax : 
                input.PMax;

            this.BaseCost = CalculateCost(fuelCosts.First(fc => fc.FuelType == this.PowerplantType.FuelType).Cost, fuelCosts.First(fc => fc.FuelType == FuelType.Co2).Cost);

        }
    }
}