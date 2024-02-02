
using powerplant_coding_challenge.Models;

namespace powerplant_coding_challenge.Models
{
   public class CalculateResponse{ 

    public static List<PowerStation> Calculate(Payload input)
    {   
        var response = new List<PowerStation>();
        double load = (double)input.load;

        if(input.powerplants==null){
            throw new Exception("No Power Plants " +input.ToString());
        }
        else{

        List<PowerUnit>  PowerUnits = new List<PowerUnit>();
        List<PowerUnit>  WindPowerUnits = new List<PowerUnit>();
        List<PowerUnit>  GasPowerUnits = new List<PowerUnit>();
        List<PowerUnit>  TurboPowerUnits = new List<PowerUnit>();
         //Calculate the realative properties of each power station
        foreach (var pp in input.powerplants)
        {
            PowerUnit a = new PowerUnit();
           switch (pp.type)
           {
                case "windturbine":
                a.Name = (string)pp.name;
                a.Type = (string)pp.type;
                a.Output = ((double)pp.pmax*(double)pp.efficiency*(double)input.fuels.wind)/100;
                a.MinOutput = ((double)pp.pmin*(double)pp.efficiency*(double)input.fuels.wind)/100;
                a.Cost = 0;
                WindPowerUnits.Add(a);
                break;
                case "gasfired":
                a.Name = (string)pp.name;
                a.Type = (string)pp.type;
                a.Output = (double)pp.pmax*(double)pp.efficiency;
                a.MinOutput = (double)pp.pmin*(double)pp.efficiency;
                a.Cost = (double)pp.efficiency*(double)input.fuels.gaseuroMWh;
                GasPowerUnits.Add(a);
                break;
                case "turbojet":
                a.Name = (string)pp.name;
                a.Type = (string)pp.type;
                a.Output = (double)pp.pmax*(double)pp.efficiency;
                a.MinOutput = (double)pp.pmin*(double)pp.efficiency;;
                a.Cost = (double)pp.efficiency*(double)input.fuels.kerosineeuroMWh;
                TurboPowerUnits.Add(a);
                break;
                default:
                break;
           } 
        }
        //Create a cost ordered list of power sources
        PowerUnits.AddRange(WindPowerUnits);
        if(input.fuels.gaseuroMWh>input.fuels.kerosineeuroMWh){
            PowerUnits.AddRange(TurboPowerUnits);
            PowerUnits.AddRange(GasPowerUnits);
        }
        else{
            PowerUnits.AddRange(GasPowerUnits);
            PowerUnits.AddRange(TurboPowerUnits);
        }
        
        //Work out the most cost effective order of production
        for(int i=0;i<PowerUnits.Count();i++)
        {
            var pu = PowerUnits[i];
            if(load==0){
                break;
            }
            if(pu.MinOutput<=load){
                 var p = new PowerStation();
                 p.Name = pu.Name;
                 if(pu.Output<load){
                    p.P = pu.Output;
                 }
                 else{
                    p.P=load;
                 }
                 load -= p.P;
                 response.Add(p);
            }

        }
        

        return response;
        }
    } 

   }
}

 