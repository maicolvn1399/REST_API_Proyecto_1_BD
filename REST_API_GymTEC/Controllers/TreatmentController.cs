using Microsoft.AspNetCore.Mvc;
using REST_API_GymTEC.Database_Resources;
using REST_API_GymTEC.Models;
using System.Data;

namespace REST_API_GymTEC.Controllers
{

    /// <summary>
    /// Treatment Controller 
    /// </summary>
    [ApiController]
    [Route("api")]
    public class TreatmentController : ControllerBase
    {

        /// <summary>
        /// HTTP POST method to associate a treatment to a branch 
        /// </summary>
        /// <param name="associate_Treatment"> refers to object of treatment to be associated with a branch </param>
        /// <returns>returns a json with the status confirming if the query was succesfull or not</returns>
        [HttpPost("associate_treatment")]
        public async Task<ActionResult<JSON_Object>> AssociateTreatment(Associate_treatment associate_Treatment)
        {
            JSON_Object json = new JSON_Object("error", null);
            bool var = DatabaseConnection.ExecuteAssociateTreatment(associate_Treatment);
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
        /// HTTP GET method to get all the treatments from the database 
        /// </summary>
        /// <returns>json with all the treatments stored in the database </returns>
        [HttpGet("get_all_treatments")]
        public async Task<ActionResult<JSON_Object>> GetAllTreatments()
        {
            JSON_Object json = new JSON_Object("error", null);

            try
            {

                DataTable all_treatment_table = DatabaseConnection.ExecuteGetAllTreatments();
                List<Treatment> list_all_treatments = new List<Treatment>();

                foreach (DataRow row in all_treatment_table.Rows)
                {
                    Treatment treatment_ = new Treatment();
                    treatment_.id = Convert.ToInt32(row["ID"]);
                    treatment_.tratamiento = row["Descripcion"].ToString();

                    list_all_treatments.Add(treatment_);

                }
                
                json.status = "ok";
                json.result = list_all_treatments;
                return Ok(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(json);
            }

        }
    }

   


}
