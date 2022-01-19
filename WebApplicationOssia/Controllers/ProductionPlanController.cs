using Microsoft.AspNetCore.Mvc;
using WebApplicationOssia.Models;

using System.Net.Http.Json;

namespace WebApplicationOssia.Controllers
{
    [ApiController]
    [Route("api/ProductionPlan")]
    public class ProductionPlanController : Controller
    {
         

        #region Public Methods
        // GET: api/ProductionPlan
        [HttpGet]

        public ActionResult<IEnumerable<string>> Get()
        {
            return new List<string>() { "API - Power production-plan" };
        }

         

        // POST: api/ProductionPlan
        [HttpPost]
        public ActionResult<IEnumerable<ProductionPlanItem>> Post(Payload payload)
        {
            try
            {
                ProductionPlanManager productionPlanManager = new ProductionPlanManager(payload);
                ProductionPlan productionPlan = productionPlanManager.CalculatePowerProduction();
                return CreatedAtAction(nameof(Get), productionPlan.ProductionPlanItems);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

            
        }

        
        #endregion
    }
}
