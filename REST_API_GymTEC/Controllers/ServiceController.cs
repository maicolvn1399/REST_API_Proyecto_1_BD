using Microsoft.AspNetCore.Mvc;
using REST_API_GymTEC.Models;
using REST_API_GymTEC.Database_Resources;
using System.Data;

namespace REST_API_GymTEC.Controllers

{
    [ApiController]
    [Route("api")]
    public class ServiceController : ControllerBase
    {
        [HttpGet("get_services")]

        public async Task<ActionResult<JSON_Object>> Get_Services()
        {
            JSON_Object ob = new JSON_Object("error", null);
            try
            {
                DataTable dt = DatabaseConnection.ExecuteGetServices();

                List<Service> service_list = new List<Service>();

                foreach (DataRow row in dt.Rows)
                {
                    Service service = new Service();
                    service.nombre_sucursal = row["nombre_sucursal"].ToString();
                    service.servicio = row["servicio"].ToString();

                    service_list.Add(service);
                }

                ob.status = "ok";

                ob.result = service_list;

                return Ok(ob);

            }
            catch(Exception ex) { 
                Console.WriteLine(ex.Message);
                return BadRequest(ob); }

        }
        [HttpPost("add_service")]

        public async Task<ActionResult<JSON_Object>> Add_Service(ServiceAdd serviceAdd)
        {
            JSON_Object ob = new JSON_Object("error", null);
            bool var = DatabaseConnection.ExecuteAddService(serviceAdd);

            if (var)
            {
                ob.status = "ok";
                return Ok(ob);
            }
            else
            {
                return BadRequest(ob);
            }

        }
    }
}
