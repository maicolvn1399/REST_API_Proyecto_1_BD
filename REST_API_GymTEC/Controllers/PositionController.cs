using Microsoft.AspNetCore.Mvc;
using REST_API_GymTEC.Database_Resources;
using REST_API_GymTEC.Models;
using System.Data;

namespace REST_API_GymTEC.Controllers
{
    [ApiController]
    [Route("api")]
    public class PositionController : ControllerBase
    {
        [HttpGet("get_positions")]

        public async Task<ActionResult<JSON_Object>> GetPositions()
        {
            JSON_Object ob = new JSON_Object("error", null);
            try
            {
                DataTable dt = DatabaseConnection.ExecuteGetPositions();

                List<Position_Identifier> position_list = new List<Position_Identifier>();

                foreach (DataRow row in dt.Rows)
                {
                    Position_Identifier position = new Position_Identifier();

                    position.nombre_posicion = row["tipo_puesto"].ToString();

                    position_list.Add(position);
                }

                ob.status = "ok";

                ob.result = position_list;

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
