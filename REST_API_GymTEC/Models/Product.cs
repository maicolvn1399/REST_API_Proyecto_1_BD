namespace REST_API_GymTEC.Models
{
    public class Product
    {

        //Model to use in the get_product, add_product, update_product endpoints
        public string codigo_barras { get; set; } = string.Empty;
        public string nombre_producto { get; set; } = string.Empty;
        public float costo { get; set; }
        public string descripcion { get; set; } = string.Empty;

    }
}
