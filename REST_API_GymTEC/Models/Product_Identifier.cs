namespace REST_API_GymTEC.Models
{
    //Model to be used in the delete_product, get_product endpoints 
    public class Product_Identifier
    {
        public string codigo_barras { get; set; } = string.Empty;
    }
}
