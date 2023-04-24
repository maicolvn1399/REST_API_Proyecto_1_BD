using Microsoft.AspNetCore.Mvc;
using REST_API_GymTEC.Models;
using REST_API_GymTEC.Database_Resources;
using System.Data;

namespace REST_API_GymTEC.Controllers
{
    //Controller for Branch

    [ApiController]
    [Route("api")]
    public class BranchController : ControllerBase
    {

        [HttpGet("get_all_branches")]
        public async Task<ActionResult<JSON_Object>> GetAllBranches()
        {
            JSON_Object json = new JSON_Object("error", null);

            try
            {
                DataTable all_branches_table = DatabaseConnection.ExecuteGetAllBranches();
                List<Branch_Identifier> all_branches_list = new List<Branch_Identifier>();

                foreach (DataRow row in all_branches_table.Rows)
                {
                    Branch_Identifier branch_identifier = new Branch_Identifier();
                    branch_identifier.nombre_sucursal = row["Nombre"].ToString();
                    all_branches_list.Add(branch_identifier);
                }
                json.status = "ok";
                json.result = all_branches_list;
                return Ok(json);

            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest(json);
            }



        }


        [HttpGet("get_branch")]
        public async Task<ActionResult<JSON_Object>> GetBranch([FromQuery] Branch_Identifier branch_to_get)
        {
            JSON_Object json = new JSON_Object("error", null);
            try
            {
                DataTable single_branch_table = DatabaseConnection.ExecuteGetBranch(branch_to_get);
                Branch branch = new Branch();
                foreach (DataRow row in single_branch_table.Rows)
                {
                    branch.nombre_sucursal = row["Nombre"].ToString();
                    branch.fecha_apertura = row["Fecha_aper"].ToString();
                    branch.horario = row["Horario"].ToString();
                    branch.cap_max = Convert.ToInt32(row["Cap_max"]);
                    branch.provincia = row["Provincia"].ToString();
                    branch.canton = row["Canton"].ToString();
                    branch.distrito = row["Distrito"].ToString();
                    branch.manager = row["Manager"].ToString();
                    branch.active_spa = Convert.ToBoolean(row["activeSpa"]);
                    branch.active_store = Convert.ToBoolean(row["activeStore"]);

                }

                DataTable phones_table = DatabaseConnection.ExecuteGetPhonesXBranch(branch_to_get.nombre_sucursal);
                List<string> phones_list = new List<string>();
                foreach (DataRow row in phones_table.Rows)
                {
                    phones_list.Add(row["Telefono"].ToString());
                }

                branch.telefonos = phones_list;

                json.status = "ok";
                json.result = branch;
                return Ok(json);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return BadRequest(json);
            }

        }

        [HttpPost("add_branch")]
        public async Task<ActionResult<JSON_Object>> AddBranch(Branch new_branch)
        {
            JSON_Object json = new JSON_Object("error", null);
            bool var = DatabaseConnection.ExecuteAddBranch(new_branch);
            
            if (var)
            {
                DatabaseConnection.ExecuteAddPhonesBranch(new_branch.nombre_sucursal, new_branch.telefonos);
                json.status = "ok";
                return Ok(json);
            }
            else
            {
                return BadRequest(json);
            }

        }

        [HttpPut("update_branch")]
        public async Task<ActionResult<JSON_Object>> UpdateBranch(Branch updated_branch)
        {
            JSON_Object json = new JSON_Object("error", null);
            bool var = DatabaseConnection.ExecuteUpdateBranch(updated_branch);
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

        [HttpDelete("delete_branch")]
        public async Task<ActionResult<JSON_Object>> DeleteBranch(Branch_Identifier branch_to_delete)
        {
            JSON_Object json = new JSON_Object("error", null);
            return BadRequest(json);
        }







        }
}
