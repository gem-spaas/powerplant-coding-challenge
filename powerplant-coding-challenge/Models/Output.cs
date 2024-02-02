namespace powerplant_coding_challenge.Models
{

    
    public class PowerStation{
                public string Name {get;set;}
                public double P {get;set;}


                public PowerStation()
                {
                    if (Name == null)
                    {
                        Name = "";
                    }

                }
        }

     public class PowerUnit{
            public string Name {get;set;}
            public string Type {get;set;}
            public double Output {get;set;}=0;
            public double MinOutput {get;set;}=0;
            public double Cost {get;set;}=0;


            public PowerUnit()
            {
                if (Name == null)
                {
                    Name = "";
                }
                if (Type == null)
                {
                    Type = "";
                }

            }
        }
        
} 