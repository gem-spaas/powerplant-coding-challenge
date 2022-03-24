using Microsoft.AspNetCore.Mvc;
using ProductionPlanApi.Model;
using ProductionPlanApi.Helper;

// /productionplan :: GET
namespace ProductionPlanApi.Controllers;
[Route("[controller]")]
[ApiController]
public class ProductionPlanController : ControllerBase
{
   [HttpGet(Name = "GetHProductionPlan")]
 public  IEnumerable<string> Get(){
     return Enumerable.Range(1,5).Select(index =>new String("Production plan"+ index ));
 }
// /productionplan :: POST
    [HttpPost]
     public List<PowerPlantResponseItem> Post([FromBody] Object jsonBody)
    {
        Console.WriteLine("This payload was posted");
        Payloads payloads= PayLoadsHelper.serialParse(jsonBody);
        Console.WriteLine(payloads);
        //calclation algoritm 
        PowerPlan pplan = new PowerPlan(payloads);
        pplan.buildPowerplanList();        
        return pplan.powerplanlist;
    }


   
}