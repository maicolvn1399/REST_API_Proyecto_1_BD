using Microsoft.AspNetCore.Mvc;
using REST_API_GymTEC.Database_Resources;
using REST_API_GymTEC.Models;
using System.Data;

namespace REST_API_GymTEC.Controllers
{
    /// <summary>
    /// Controller for class 
    /// </summary>
    [ApiController]
    [Route("api")]
    public class ClassController : ControllerBase
    {

        /// <summary>
        /// HTTP POST method to create a new class 
        /// </summary>
        /// <param name="new_class"> new class to add into the database </param>
        /// <returns> returns a json with a status message confirming the success of the query </returns>
        [HttpPost("create_class")]
        public async Task<ActionResult<JSON_Object>> CreateClass(Class new_class)
        {
            JSON_Object json = new JSON_Object("error", null);
            bool var = DatabaseConnection.ExecuteCreateClass(new_class);
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

        /// <summary>
        /// HTTP GET method to get all the classes from a table 
        /// </summary>
        /// <returns> json with all the classes </returns>
        [HttpGet("get_all_classes")]
        public async Task<ActionResult<JSON_Object>> GetAllClasses()
        {
            JSON_Object json = new JSON_Object("error", null);
           
            try
            {

                DataTable all_classes_table = DatabaseConnection.ExecuteGetAllClasses();
                List<Class> list_all_classes = new List<Class>();

                foreach (DataRow row in all_classes_table.Rows)
                {
                    Class _class = new Class();
                    _class.clase_id = Convert.ToInt32(row["id"]);
                    _class.servicio = row["servicio"].ToString();
                    _class.modo = row["modo"].ToString();
                    _class.capacidad = Convert.ToInt32(row["capacidad"]);
                    _class.fecha = row["fecha"].ToString();
                    _class.hora_ingreso = row["hora_ing"].ToString();
                    _class.hora_salida = row["hora_sal"].ToString();
                    _class.encargado = row["encargado"].ToString();

                    list_all_classes.Add(_class);
                }
                json.status = "ok";
                json.result = list_all_classes;
                return Ok(json);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(json);
            }


        }

        /// <summary>
        /// HTTP POST method to get the classes filtered 
        /// </summary>
        /// <param name="filters"> filters of branch and date </param>
        /// <returns> json with all the classes that fit the filters </returns>
        [HttpPost("filter_class")]
        public async Task<ActionResult<JSON_Object>> FilterClass(FilterClass filters)
        {
            JSON_Object json = new JSON_Object("error", null);


            try
            {
                DataTable filtered_classes_table = DatabaseConnection.ExecuteFilterClasses(filters);
                List<Class> filtered_classes = new List<Class>();
                foreach (DataRow row in filtered_classes_table.Rows)
                {
                    Class filtered_class = new Class();
                    filtered_class.servicio = row["servicio"].ToString();
                    filtered_class.clase_id = Convert.ToInt32(row["id"]);
                    filtered_class.modo = row["modo"].ToString();
                    filtered_class.capacidad = Convert.ToInt32(row["capacidad"]);
                    filtered_class.fecha = row["fecha"].ToString();
                    filtered_class.hora_ingreso = row["hora_ing"].ToString();
                    filtered_class.hora_salida = row["hora_sal"].ToString();
                    filtered_class.encargado = row["encargado"].ToString();

                    filtered_classes.Add(filtered_class);
                }

                json.status = "ok";
                json.result = filtered_classes;
                return Ok(json);

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(json);
            }
        }


        /// <summary>
        /// HTTP POST method to enroll a client to a class 
        /// </summary>
        /// <param name="enroll_class"> information to enroll a client into a class </param>
        /// <returns> returns a json with a status message confirming the success of the query </returns>
        [HttpPost("enroll_class")]
        public async Task<ActionResult<JSON_Object>> EnrollClass(Enroll_Class enroll_class)
        {
            JSON_Object json = new JSON_Object("error", null);
            bool var = DatabaseConnection.ExecuteEnrollClass(enroll_class);
            if (var)
            {
                json.status = "ok";
                return Ok(json);
            }
            else
            {
                json.status = "You are already enrolled in this class!";
                return BadRequest(json);
            }
            
        }
        
    }
}
