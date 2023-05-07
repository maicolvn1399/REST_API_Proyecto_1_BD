using Microsoft.AspNetCore.Mvc;
using REST_API_GymTEC.Models;
using REST_API_GymTEC.Database_Resources;
using System.Data;

namespace REST_API_GymTEC.Controllers
{
    /// <summary>
    /// Product Controller 
    /// </summary>
    [ApiController]
    [Route("api")]
    public class ProductController : ControllerBase
    {
        /// <summary>
        /// HTTP GET method to get all the products stored in the database 
        /// </summary>
        /// <returns>json with all the products listed </returns>
        [HttpGet("get_all_products")]

        public async Task<ActionResult<JSON_Object>> GetAllProducts()
        {
            JSON_Object ob = new JSON_Object("error", null);
            try
            {
                DataTable dt = DatabaseConnection.ExecuteGetAllProducts();

                List<Shortened_Product> product_list = new List<Shortened_Product>();

                foreach (DataRow row in dt.Rows)
                {
                    Shortened_Product product = new Shortened_Product();

                    product.codigo_barras = row["codigo_barras"].ToString();
                    product.nombre_producto = row["nombre_producto"].ToString();
                    product.costo = (double)row["costo"];



                    product_list.Add(product);
                }

                ob.status = "ok";

                ob.result = product_list;

                return Ok(ob);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ob);
            }

        }
        /// <summary>
        /// HTTP POST method to get a specific product 
        /// </summary>
        /// <param name="barras"> refers to the identifier to get the information for a specific product </param>
        /// <returns>json with the information of the requested product </returns>
        [HttpPost("get_product")]

        public async Task<ActionResult<JSON_Object>> GetProduct(Product_Identifier barras)
        {
            JSON_Object ob = new JSON_Object("error", null);
            try
            {
                DataTable dt = DatabaseConnection.ExecuteGetProduct(barras);

                Product product = new Product();

                foreach (DataRow row in dt.Rows)
                {
                    product.codigo_barras = row["codigo_barras"].ToString();
                    product.nombre_producto = row["nombre_producto"].ToString();
                    product.costo = (double)row["costo"];
                    product.descripcion = row["descripcion"].ToString();


                }

                ob.status = "ok";

                ob.result = product;

                return Ok(ob);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ob);
            }

        }
        /// <summary>
        /// HTTP POST method to add a new product to the database 
        /// </summary>
        /// <param name="new_product"> refers to the new product that will be stored in the database </param>
        /// <returns> returns a json with the status confirming if the query was succesfull or not </returns>
        [HttpPost("add_product")]

        public async Task<ActionResult<JSON_Object>> AddProduct(Product new_product)
        {
            JSON_Object json = new JSON_Object("error", null);
            bool var = DatabaseConnection.ExecuteAddProduct(new_product);

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
        /// HTTP PUT method to update a product 
        /// </summary>
        /// <param name="updated_product"> refers to an updated product</param>
        /// <returns> returns a json with the status confirming if the query was succesfull or not </returns>
        [HttpPut("update_product")]
        public async Task<ActionResult<JSON_Object>> UpdateProduct(Product updated_product)
        {
            JSON_Object json = new JSON_Object("error", null);
            bool var = DatabaseConnection.ExecuteUpdateProduct(updated_product);
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
        /// HTTP DELETE method to delete a product from the database 
        /// </summary>
        /// <param name="barras"> identifier to delete a specific product from the database </param>
        /// <returns> returns a json with the status confirming if the query was succesfull or not </returns>
        [HttpDelete("delete_product")]
        public async Task<ActionResult<JSON_Object>> DeleteProduct(Product_Identifier barras)
        {
            JSON_Object json = new JSON_Object("error", null);
            bool var = DatabaseConnection.ExecuteDeleteProduct(barras);
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
        /// HTTP POST method to associate a product to a branch 
        /// </summary>
        /// <param name="obj"> refers to the product that will be associated with a specific branch </param>
        /// <returns> returns a json with the status confirming if the query was succesfull or not </returns>
        [HttpPost("associate_product")]
        public async Task<ActionResult<JSON_Object>> AssignProduct(Associate_product obj)
        {
            JSON_Object json = new JSON_Object("error", null);
            bool var = DatabaseConnection.ExecuteAssignProduct(obj);
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
