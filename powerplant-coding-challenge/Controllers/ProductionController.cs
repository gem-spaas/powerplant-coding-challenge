
using powerplant_coding_challenge.Models;
﻿using System;
using System.Text;
using System.Data;
using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Data;
using Microsoft.AspNetCore.Mvc;


namespace powerplant_coding_challenge.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PowerController : ControllerBase
    {
        ///This takes the post of the payload calculates the result and saves it to JSON
        [HttpPost("productionplan")]
        public IActionResult UpLoadJson(IFormFile file){
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
           //Here we parse the input producing a string of the jSON
           string payload;

            using(var reader = new StreamReader(file.OpenReadStream()))
            {
                payload = reader.ReadToEnd();

            }
            

            if(payload==null){
               throw new Exception("Not uploaded"); 
            }
            //Here the JSON data is extracted using the model
            Payload payloadOutput = System.Text.Json.JsonSerializer.Deserialize<Payload>(payload, options);
            if(payloadOutput!=null){
                ///The result is calculated
                var answer = CalculateResponse.Calculate(payloadOutput);
                if (answer!=null){
                    //Here it is wriiten to JSON
                    string writeText= System.Text.Json.JsonSerializer.Serialize(answer, options);
                    if (writeText!=null){
                    System.IO.File.WriteAllText("response.JSON", writeText);
                    return Ok();
                    }
                    else{
                        throw new Exception("Couldn't Write");
                    }
                }
                else{
                   throw new Exception("No calculation");
                }
            }
            else{
                throw new Exception("Not parsed");
            }
        }
        //This transmits the result to who ever requests it
        [HttpGet("productionplan")]
        public object GetResponse(){
            var allText = System.IO.File.ReadAllText("response.JSON");
            object jsonObject = allText;
           return jsonObject;
        }
    }
}