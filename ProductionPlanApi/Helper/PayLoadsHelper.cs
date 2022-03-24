namespace ProductionPlanApi.Helper;
using  ProductionPlanApi.Model;
using System.Text.Json;
//using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Reflection;

public class PayLoadsHelper{
    public static List<PowerPlantResponseItem> GetSample(){
        List<PowerPlantResponseItem> responselist = new List<PowerPlantResponseItem>
        {
            new PowerPlantResponseItem("windpark1",75), 
            new PowerPlantResponseItem("windpark2",18),
            new PowerPlantResponseItem("gasfiredbig1",200), 
            new PowerPlantResponseItem("gasfiredbig1",0),
            new PowerPlantResponseItem("tj1",0), 
            new PowerPlantResponseItem("tj2",0)

        };
        return responselist;

    }

    /*
    The json Objects needs to be parsed and prepared to fit the  ProductionPlanApi.Model.Paylods model
    */
    public static Payloads serialParse(Object jsonBody){        
         var options = new JsonSerializerOptions
            {
                IncludeFields = true,
                PropertyNameCaseInsensitive = true
            };

        Payloads? payloads = JsonSerializer.Deserialize<Payloads>(PayLoadsHelper.PrepareBody(jsonBody), options);
        return payloads;
    }
/*
(abc) needs to be eliminated in order for the object to be deserialisable
*/
     public static String PrepareBody(Object jsonBody){
         
        String strbody= "";         
        if (jsonBody != null ){

            strbody= ""+jsonBody;                  
            string replacement = "";             
            string pattern =@"[(](.*)[)]";
            Regex rgx = new Regex(pattern,RegexOptions.IgnoreCase);                                
           strbody=rgx.Replace(strbody, replacement);                 
         }                
        return strbody;
    }

 public static String PowerplantsListToJson( List<PowerPlant> powerPlantList){
         
         string jsonString = JsonSerializer.Serialize(powerPlantList);
         return jsonString;
    }
   public static String  PowerPlantResponseToJson( List<PowerPlantResponseItem> PowerPlantResponseList){
         
         string jsonString = JsonSerializer.Serialize(PowerPlantResponseList);
         return jsonString;
    }
    public static SortedList<double, string>   FuelSortedListFactory( Fuels fuels){
               
        //transform key value sorter
        SortedList<double, string> sortedFuelsCost = new SortedList<double, string>();
        if(fuels !=null){
            double gas= fuels.gas.HasValue ? fuels.gas.Value : 0d;
            sortedFuelsCost.Add(gas, "gas");   

            double kerosine= fuels.kerosine.HasValue ? fuels.kerosine.Value : 0d;
            sortedFuelsCost.Add(kerosine, "kerosine");  

            double co2= fuels.co2.HasValue ? fuels.co2.Value : 0d;   
            sortedFuelsCost.Add(co2, "co2");    

            double wind= fuels.wind.HasValue ? fuels.wind.Value : 0d;
            sortedFuelsCost.Add(wind, "wind");  
        }
        
        return sortedFuelsCost; 
    }


}