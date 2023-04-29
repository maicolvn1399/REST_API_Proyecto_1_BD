using Microsoft.AspNetCore.Mvc;
using REST_API_GymTEC.Models;

namespace REST_API_GymTEC.Controllers
{
    [ApiController]
    [Route("api")]
    public class TreatmentController : ControllerBase
    {


        [HttpPost("associate_treatment")]
        public async Task<ActionResult<JSON_Object>> AssociateTreatment(Associate_treatment associate_Treatment)
        {
            JSON_Object json = new JSON_Object("error",null);
            return Ok(json);

        }
    }
}
