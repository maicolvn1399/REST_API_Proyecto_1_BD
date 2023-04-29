using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using REST_API_GymTEC.Database_Resources;
using REST_API_GymTEC.Models;
using System.Data;
using System.Text.Json.Serialization;

namespace REST_API_GymTEC.Controllers
{

    [ApiController]
    [Route("api")]
    public class LoginWorkerController : ControllerBase
    {

        [HttpPost("auth_worker")]
        public async Task<ActionResult<JSON_Object>> AuthWorker(Credentials_Worker credentials)
        {

            JSON_Object json = new JSON_Object("error", null);
            try
            {
                DataTable login_worker_table = DatabaseConnection.ExecuteLoginWorker(credentials);
                Employee employee = new Employee();
                foreach (DataRow row in login_worker_table.Rows)
                {
                    employee.cedula = row["Cedula"].ToString();
                    employee.nombre = row["Nombre"].ToString();
                    employee.apellido_1 = row["Apellido1"].ToString();
                    employee.apellido_2 = row["Apellido2"].ToString();
                    employee.provincia = row["Provincia"].ToString();
                    employee.canton = row["Canton"].ToString();
                    employee.distrito = row["Distrito"].ToString();
                    employee.salario = (double)row["Salario"];
                    employee.correo = row["Correo"].ToString();
                    employee.password = row["Password"].ToString();
                }

                json.status = "ok";
                json.result = employee;
                return Ok(json);
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest(json);
            }
            

        }


    }
}
