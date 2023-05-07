using Microsoft.AspNetCore.Mvc;
using REST_API_GymTEC.Models;
using REST_API_GymTEC.Database_Resources;
using System.Data;

namespace REST_API_GymTEC.Controllers
{
    /// <summary>
    /// Payroll controller 
    /// </summary>
    [ApiController]
    [Route("api")]

    public class PayrollController : ControllerBase
    {
        /// <summary>
        /// HTTP GET method to get all the payrolls stored in the database 
        /// </summary>
        /// <returns> json with all the payrolls </returns>
        [HttpGet("get_all_payrolls")]

        public async Task<ActionResult<JSON_Object>> GetAllPayrolls() { 
            JSON_Object ob = new JSON_Object("error", null);
            try
            {
                DataTable dt = DatabaseConnection.ExecuteGetAllPayrolls();

                List<Payroll> payroll_list = new List<Payroll>();

                foreach (DataRow row in dt.Rows)
                {
                    Payroll payroll = new Payroll();

                    payroll.empleado_cedula = row["empleado_cedula"].ToString();
                    payroll.nombre_planilla = row["planilla_tipo"].ToString();
                    payroll.salario = (double)row["salario"];

                    payroll_list.Add(payroll);
                }

                ob.status = "ok";

                ob.result = payroll_list;

                return Ok(ob);

            }
            catch(Exception ex) { 
                Console.WriteLine(ex.Message);
                return BadRequest(ob); }

        }

        /// <summary>
        /// HTTP POST method to get a specific payroll for an employee 
        /// </summary>
        /// <param name="cd"> identifier for employee to get their respective payroll information </param>
        /// <returns>json with the information of the payroll for this specific employee </returns>
        [HttpPost("get_payroll")]
        public async Task<ActionResult<JSON_Object>> GetPayroll(Employee_Identifier cd)
        {
            JSON_Object ob = new JSON_Object("error", null);
            try
            {
                DataTable dt = DatabaseConnection.ExecuteGetPayroll(cd);

                Payroll payroll = new Payroll();

                foreach (DataRow row in dt.Rows)
                {
                    payroll.empleado_cedula = row["empleado_cedula"].ToString();
                    payroll.nombre_planilla = row["planilla_tipo"].ToString();
                    payroll.salario = (double)row["salario"];
                }

                ob.status = "ok";

                ob.result = payroll;

                return Ok(ob);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ob);
            }

        }


    }
}
