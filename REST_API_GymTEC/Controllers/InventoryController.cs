using Microsoft.AspNetCore.Mvc;
using REST_API_GymTEC.Database_Resources;
using REST_API_GymTEC.Models;
using System.Data;

namespace REST_API_GymTEC.Controllers
{
    /// <summary>
    /// Controller for Inventory 
    /// </summary>
    [ApiController]
    [Route("api")]
    public class InventoryController : ControllerBase
    {
        
        /// <summary>
        /// HTTP GET method to get all the inventories 
        /// </summary>
        /// <returns>json with all the inventories in the databaase </returns>
        [HttpGet("get_all_inventories")]
        public async Task<ActionResult<JSON_Object>> GetAllInventories()
        {
            JSON_Object json = new JSON_Object("error", null);
            try
            {
                DataTable table_all_inventories = DatabaseConnection.ExecuteGetAllInventories();
                List<Inventory_Shortened> list_all_inventories = new List<Inventory_Shortened>();
                foreach (DataRow row in table_all_inventories.Rows)
                {
                    Inventory_Shortened inventory = new Inventory_Shortened();
                    inventory.num_serie = Convert.ToInt32(row["num_serie"]);
                    inventory.marca = row["marca"].ToString();
                    inventory .tipo_equipo = row["description"].ToString();

                    list_all_inventories.Add(inventory);

                }

                json.status = "ok";
                json.result = list_all_inventories;
                return Ok(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(json);
            }

        }

        /// <summary>
        /// HTTP POST to get an inventory in specific 
        /// </summary>
        /// <param name="inventory_Identifier"> refers to the identifier of the inventory that is requested </param>
        /// <returns> json with the respective information for this inventory </returns>
        [HttpPost("get_inventory")]
        public async Task<ActionResult<JSON_Object>> GetInventory(Inventory_Identifier inventory_Identifier)
        {
            JSON_Object json = new JSON_Object("error", null);
            try
            {
                DataTable table_inventory = DatabaseConnection.ExecuteGetInventory(inventory_Identifier);
                Inventory inventory = new Inventory();
                foreach (DataRow row in table_inventory.Rows)
                {
                    inventory.num_serie = Convert.ToInt32(row["num_serie"]);
                    inventory.marca = row["marca"].ToString();
                    inventory.costo = (double)row["costo"];
                    inventory.is_used = Convert.ToBoolean(row["used"]);
                    inventory.tipo_equipo = row["description"].ToString();
                   
                }
                json.status = "ok";
                json.result = inventory;

                return Ok(json);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(json);
            }

        }

        /// <summary>
        /// HTTP POST method to add a new inventory to the database 
        /// </summary>
        /// <param name="new_inventory"> refers to the object of the new inventory </param>
        /// <returns> returns a json with a status message confirming the success of the query </returns>
        [HttpPost("add_inventory")]
        public async Task<ActionResult<JSON_Object>> AddInventory(Inventory new_inventory)
        {
            JSON_Object json = new JSON_Object("error", null);
            bool var = DatabaseConnection.ExecuteAddInventory(new_inventory);
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
        /// HTTP PUT method to update an inventory in the database 
        /// </summary>
        /// <param name="inventory_to_update"> refers to the object of inventory to be updated </param>
        /// <returns>returns a json with a status message confirming the success of the query</returns>
        [HttpPut("update_inventory")]
        public async Task<ActionResult<JSON_Object>> UpdateInventory(Inventory inventory_to_update)
        {
            JSON_Object json = new JSON_Object("error", null);
            bool var = DatabaseConnection.ExecuteUpdateInventory(inventory_to_update);
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
        /// HTTP DELETE method to delete an inventory 
        /// </summary>
        /// <param name="inventory_Identifier"> refers to the identifier of the inventory to be deleted </param>
        /// <returns> returns a json with a status message confirming the success of the query </returns>
        [HttpDelete("delete_inventory")]
        public async Task<ActionResult<JSON_Object>> DeleteInventory(Inventory_Identifier inventory_Identifier)
        {
            JSON_Object json = new JSON_Object("error", null);
            bool var = DatabaseConnection.ExecuteDeleteInventory(inventory_Identifier);
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
        /// HTTP POST method to associate an inventory 
        /// </summary>
        /// <param name="associate_Inventory"> refers to an object to associate an inventory to a branch </param>
        /// <returns>returns a json with a status message confirming the success of the query </returns>
        [HttpPost("associate_inventory")]
        public async Task<ActionResult<JSON_Object>> AssociateInventory(Associate_Inventory associate_Inventory)
        {
            JSON_Object json = new JSON_Object("error",null);
            bool var = DatabaseConnection.ExecuteAssociateInventory(associate_Inventory);
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
        /// HTTP GET method to get all the inventories that are not associated in the database 
        /// </summary>
        /// <returns>returns a json with all the non associated inventories </returns>
        [HttpGet("get_non_associated_inv")]
        public async Task<ActionResult<JSON_Object>> GetNonAssociatedInventories()
        {
            JSON_Object json = new JSON_Object("error",null);
            try
            {
                DataTable table_non_associated_inv = DatabaseConnection.ExecuteGetNonAssociatedInv();
                List<NonAssociatedInventories> list_non_associated = new List<NonAssociatedInventories>();

                foreach (DataRow row in table_non_associated_inv.Rows)
                {
                    NonAssociatedInventories nonAssociated = new NonAssociatedInventories();
                    nonAssociated.num_serie = Convert.ToInt32(row["num_serie"]);
                    nonAssociated.tipo_equipo = row["tipo_equipo"].ToString();
                    list_non_associated.Add(nonAssociated);
                }
                json.status = "ok";
                json.result = list_non_associated;
                return Ok(json);
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(json);
            }
            

        }



    }
}
