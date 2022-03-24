using Microsoft.AspNetCore.Mvc;
using ProductionPlanApi.Model;
using ProductionPlanApi.Helper;

namespace ProductionPlanApi.Controllers;
// for /hello
[Route("[controller]")]
[ApiController]
public class HelloController : ControllerBase
{
   [HttpGet(Name = "GetHello")]
 public String Get(){
     return new String("Hello from Controller!");
 }
    [HttpGet("{id}")]
 public String Get(int id){     
     return (id < 0 ) ? "Out of rage": id>5 ? "Outof Range": PowerPlant.types[id];
 }
 [HttpPost]
 
     public  void Post([FromBody] Object jsonBody)
    {
        String strdbody= new String("");
        Console.WriteLine("hello post");
       Payloads payloads= PayLoadsHelper.serialParse(jsonBody);
       Console.WriteLine("===============Deseriliazed Payloads==========\n");
       Console.WriteLine(payloads.ToString());
        
     
       //trebuie parsat obiectul asta
       //scoti din el tot elementul fuels si pe el elementul asta il parsezi cu excape caracters. 
       //sa folosesti simple text json
       

    }
}