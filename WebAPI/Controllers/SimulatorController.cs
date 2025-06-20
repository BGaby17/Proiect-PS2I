using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DateModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
         
    [Route("api/[controller]")]
    [ApiController]
    public class SimulatorController : ControllerBase
    {
        private static List<ProcessStatusEvent> stamps = null;

        public SimulatorController()
        {
            if(stamps==null)
            {
                stamps = new List<ProcessStatusEvent>();
            }
        }
        
        //private static object ReceivedData;
        // GET: api/Simulator
        [HttpGet]
        public IEnumerable<ProcessStatusEvent> Get()
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            //return new string[] { "value1", "value2" };
            return stamps;
        }

        // GET: api/Simulator/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value   ESTI LA 5 BOSSS";
        }

        // POST: api/Simulator    
        [HttpPost()]
        public void Post([FromBody] ProcessStatusEvent value)
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            stamps.Add(value);
        }

        // PUT: api/Simulator/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }    
}
