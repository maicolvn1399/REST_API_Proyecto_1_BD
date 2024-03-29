﻿using Microsoft.AspNetCore.Mvc;
using REST_API_GymTEC.Models;
using REST_API_GymTEC.Database_Resources;
using System.Data;

namespace REST_API_GymTEC.Controllers

{
    /// <summary>
    /// Service Controller 
    /// </summary>
    [ApiController]
    [Route("api")]
    public class ServiceController : ControllerBase
    {
        /// <summary>
        /// HTTP GET method to get all the services available in the gym 
        /// </summary>
        /// <returns> returrns a json with all the services available </returns>
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

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ob);
            }
        }

        /// <summary>
        /// HTTP POST method to add a new service into the database 
        /// </summary>
        /// <param name="serviceAdd">refers to the object of the new service to be added </param>
        /// <returns> returns a json with the status confirming if the query was succesfull or not </returns>
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
        /// <summary>
        /// HTTP POST method to associate a service to a branch 
        /// </summary>
        /// <param name="service"> refers to the service that will be associated to a branch </param>
        /// <returns>returns a json with the status confirming if the query was succesfull or not</returns>
        [HttpPost("associate_service")]
        public async Task<ActionResult<JSON_Object>> AssociateService(Associate_Service service)
        {
            JSON_Object json = new JSON_Object("error", null);
            bool var = DatabaseConnection.ExecuteAssociateService(service);
            if (var)
            {
                json.status = "ok";
                return Ok(json);
            }
            else
            {
                return BadRequest(json);
            }

        }
    }
}
