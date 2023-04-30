using Microsoft.AspNetCore.Mvc;
using REST_API_GymTEC.Models;
using REST_API_GymTEC.Database_Resources;
using System.Data;

namespace REST_API_GymTEC.Controllers
{
    [ApiController]
    [Route("api")]
    public class EmployeeController : ControllerBase
    {
        [HttpGet("get_all_employees")]

        public async Task<ActionResult<JSON_Object>> GetAllEmployees()
        {
            JSON_Object ob = new JSON_Object("error", null);
            try
            {
                DataTable dt = DatabaseConnection.ExecuteGetAllEmployees();

                List<Employee_Shortened> employee_list = new List<Employee_Shortened>();

                foreach (DataRow row in dt.Rows)
                {
                    Employee_Shortened employee = new Employee_Shortened();

                    employee.cedula = row["cedula"].ToString();
                    employee.nombre = row["nombre"].ToString();
                    employee.apellido_1 = row["apellido_1"].ToString();
                    employee.apellido_2 = row["apellido_2"].ToString();

                    

                    employee_list.Add(employee);
                }

                ob.status = "ok";

                ob.result = employee_list;

                return Ok(ob);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ob);
            }

        }
        [HttpPost("get_employee")]

        public async Task<ActionResult<JSON_Object>> GetEmployee(Employee_Identifier cedula)
        {
            JSON_Object ob = new JSON_Object("error", null);
            try
            {
                DataTable dt = DatabaseConnection.ExecuteGetEmployee(cedula);

                Employee_Extended employee = new Employee_Extended();

                foreach (DataRow row in dt.Rows)
                {
                    employee.cedula_empleado = row["cedula_empleado"].ToString();
                    employee.nombre = row["nombre"].ToString();
                    employee.apellido_1 = row["apellido_1"].ToString();
                    employee.apellido_2 = row["apellido_2"].ToString();
                    employee.provincia = row["provincia"].ToString();
                    employee.canton = row["canton"].ToString();
                    employee.distrito = row["distrito"].ToString();
                    employee.salario = (double)row["salario"];
                    employee.correo = row["correo"].ToString();
                    employee.password = row["password"].ToString();
                    employee.nombre_sucursal = row["nombre_sucursal"].ToString();
                    employee.puesto_descripcion = row["puesto_descripcion"].ToString();
                    employee.planilla_descripcion = row["planilla_descripcion"].ToString();
                }

                ob.status = "ok";

                ob.result = employee;

                return Ok(ob);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ob);
            }

        }
        [HttpPost("add_employee")]

        public async Task<ActionResult<JSON_Object>> AddEmployee(Employee_Extended new_employee)
        {
            JSON_Object json = new JSON_Object("error", null);
            bool var = DatabaseConnection.ExecuteAddEmployee(new_employee);

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
        [HttpPut("update_employee")]
        public async Task<ActionResult<JSON_Object>> UpdateEmployee(Employee_Extended updated_employee)
        {
            JSON_Object json = new JSON_Object("error", null);
            bool var = DatabaseConnection.ExecuteUpdateEmployee(updated_employee);
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
        [HttpDelete("delete_employee")]
        public async Task<ActionResult<JSON_Object>> DeleteEmployee(Employee_Identifier cedula)
        {
            JSON_Object json = new JSON_Object("error", null);
            bool var = DatabaseConnection.ExecuteDeleteEmployee(cedula);
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
