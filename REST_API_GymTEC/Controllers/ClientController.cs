using Microsoft.AspNetCore.Mvc;
using REST_API_GymTEC.Models;
using REST_API_GymTEC.Database_Resources;
using System.Data;

namespace REST_API_GymTEC.Controllers
{

    /// <summary>
    /// Controller for Client
    /// </summary>
    [ApiController]
    [Route("api")]
    public class ClientController : ControllerBase
    {

        /// <summary>
        /// HTTP POST method to authenticate a client 
        /// </summary>
        /// <param name="credentials"> refers to email and password provided by the user </param>
        /// <returns> json with the information of the client </returns>
        [HttpPost("auth_client")]

        public async Task<ActionResult<JSON_Object>> LoginClient(Credentials_Client credentials)
        {
            JSON_Object ob = new JSON_Object("error", null);
            try
            {
                DataTable dt = DatabaseConnection.ExecuteLoginClient(credentials);

                Client client = new Client();

                foreach (DataRow row in dt.Rows)
                {
                    client.cedula_cliente = row["cedula_cliente"].ToString();
                    client.nombre = row["nombre"].ToString();
                    client.apellido_1 = row["apellido_1"].ToString();
                    client.apellido_2 = row["apellido_2"].ToString();
                    client.direccion = row["direccion"].ToString();
                    client.email = row["email"].ToString();
                    client.password = row["password"].ToString();
                    client.IMC = (double)row["IMC"];
                    client.edad = (int)row["edad"];
                }

                ob.status = "ok";

                ob.result = client;

                return Ok(ob);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ob);
            }

        }

        /// <summary>
        /// HTTP POST method to add a new client to the database 
        /// </summary>
        /// <param name="cliente"> refers to the information of the client to be added to the table </param>
        /// <returns> returns a json with a status message confirming the success of the query </returns>
        [HttpPost("create_client")]
        public async Task<ActionResult<JSON_Object>> CreateClient(Client_Register cliente)
        {
            JSON_Object json = new JSON_Object("error", null);
            bool var = DatabaseConnection.ExecuteCreateClient(cliente);

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
        /// HTTP DELETE to delete a client from the database 
        /// </summary>
        /// <param name="cliente"> refers to the identifier of the client that will be deleted </param>
        /// <returns> returns a json with a status message confirming the success of the query </returns>
        [HttpDelete("delete_client")]
        public async Task<ActionResult<JSON_Object>> DeleteClient(Cedula_Cliente cliente)
        {
            JSON_Object json = new JSON_Object("error", null);
            bool var = DatabaseConnection.ExecuteDeleteClient(cliente);

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
