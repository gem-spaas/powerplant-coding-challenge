namespace ProductionPlanApi.Helper;
using  ProductionPlanApi.Model;

public class PowerPlan{
     public List<PowerPlantResponseItem>? powerplanlist {get; set;} 
     public Payloads payloads;
    public double Load { get; set; }

    public  PowerPlan(Payloads payloads){
        this.payloads = payloads;
        this.powerplanlist =new List<PowerPlantResponseItem>();
        this.Load =0;
        //???sort patyloads by Pminim
    }

/*
* After Class Instantiation the buildPowerplanList() should be called to produce a powerplan
*/
    public void buildPowerplanList(){
       
         //minimum power is produced regardless
        // Thus the minimum power is always in the power plan
         List<PowerPlant> eficiencyPowerPlantList = new List<PowerPlant>();

        foreach( var plantitem in this.payloads.PowerPlants){
              //minimum power per load
            if(this.payloads.Load > this.Load){                
                double pmin= plantitem.Pmin.HasValue ? plantitem.Pmin.Value : 0d;
                this.Load+=pmin;
                //Console.WriteLine( this.Load);
                PowerPlantResponseItem currresponseItem = new PowerPlantResponseItem(plantitem.Name,pmin);
                this.powerplanlist.Add(currresponseItem);   
                
                //create the eficiency /cost list                
                double pmax= plantitem.Pmax.HasValue ? plantitem.Pmax.Value : 0d;
                double eff= plantitem.Efficiency.HasValue ? plantitem.Efficiency.Value : 0d;
                double efficiency = eff;
                 if (plantitem.Name.Contains("wind")){   
                    double wind= this.payloads.Fuels.wind.HasValue ? this.payloads.Fuels.wind.Value : 0d;                    
                    if (wind != 0 ) efficiency = eff/wind;                                                       
                  
                 }
                
                else if (plantitem.Name.Contains("gas")){
                     double gas= this.payloads.Fuels.gas.HasValue ? this.payloads.Fuels.gas.Value : 0d;
                     if (gas != 0 )  efficiency = eff/gas;
                     
                }               
                else if (plantitem.Name.Contains("tj")){
                    double tj= this.payloads.Fuels.kerosine.HasValue ? this.payloads.Fuels.kerosine.Value : 0d;
                     if (tj != 0 )  efficiency = eff/tj;                                     
                }             
                else {
                    double co2= this.payloads.Fuels.co2.HasValue ? this.payloads.Fuels.co2.Value : 0d;
                    //later you may want to add fro CO2                      
                } 
                plantitem.Efficiency = efficiency;
                eficiencyPowerPlantList.Add(plantitem);       
            }              
        }
        
        //Console.WriteLine( "----->Load is "+this.Load);
        //Console.WriteLine( PayLoadsHelper.PowerPlantResponseToJson(this.powerplanlist));   

        // SORT to efficiency/cost first 
        eficiencyPowerPlantList =  this.payloads.PowerPlants.OrderByDescending(o=>o.Efficiency).ToList();
    
        //Console.WriteLine("Eficiencylist");
        //Console.WriteLine( PayLoadsHelper.PowerplantsListToJson(eficiencyPowerPlantList));

        // !!!!!!! see if it is still necesary- maybe the minimum power was enough
        if(this.payloads.Load > this.Load)
            foreach( var effplantItem in eficiencyPowerPlantList){               
                if(this.payloads.Load > this.Load){                
                   

                    //Max Power to be added per plant
                    //double power = pmax - pmin;
                    double power = 0d;
                    //making sure to treat them all the same
                    //power to be adjusted by the number of plants
                    List<PowerPlant> sameEffList = eficiencyPowerPlantList.Where(e => 
                                    e.Efficiency.Equals(effplantItem.Efficiency)).ToList();                                    
                    Console.WriteLine("Same members on cost eficiency rank "+sameEffList.Count);                    
                    power =(double) (this.payloads.Load - this.Load)/sameEffList.Count;

                    foreach( var sameEffListItem in sameEffList){
                      
                         double powerLocal = power;
                         double pmin= sameEffListItem.Pmin.HasValue ? sameEffListItem.Pmin.Value : 0d;
                         double pmax= sameEffListItem.Pmax.HasValue ? sameEffListItem.Pmax.Value : 0d;
                         Console.WriteLine("trying ..... "+sameEffListItem.Name+ "  with localPower "+powerLocal); 
                        //make sure you do not over power the plant with a higher demand       
                        if (powerLocal > (pmax - pmin)){
                            Console.WriteLine("do not over pmax= "+pmax+" pmin =  "+pmin+ "local demand "+powerLocal);
                            powerLocal = pmax - pmin;
                        }
                        //make sure you do not over power the plant                    
                        if(this.payloads.Load < (this.Load+powerLocal)){ 
                            Console.WriteLine("this.payloads.Load "+this.payloads.Load +"< (this.Load+powerLocal)=  "+(this.Load+powerLocal) );             
                            double maxPayLoads= this.payloads.Load.HasValue ? this.payloads.Load.Value : 0d;                    
                            powerLocal= maxPayLoads- this.Load;
                            }

                        //update  powerplanlist adding power    
                        //only if you to dot overpower 
                        double futurepower = (double) powerLocal + (double)powerplanlist.Find(x => x.Name == sameEffListItem.Name).P; 
                         if( futurepower <=  pmax){    
                             this.Load+=powerLocal;
                            Console.WriteLine("updating ..... "+sameEffListItem.Name+ "  with localPower "+powerLocal); 
                            Console.WriteLine( "Load is now planed to "+this.Load);   
                            powerplanlist.Find(x => x.Name == sameEffListItem.Name).P += powerLocal;

                         }
                        
                    }
                    
                } 
            }
                

    }

}