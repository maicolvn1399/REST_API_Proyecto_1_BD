using Microsoft.AspNetCore.Mvc;
using REST_API_GymTEC.Models;
using REST_API_GymTEC.Database_Resources;
using System.Data;

namespace REST_API_GymTEC.Controllers
{
    [ApiController]
    [Route("api")]
    public class ProductController : ControllerBase
    {
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
