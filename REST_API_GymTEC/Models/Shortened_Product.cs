namespace REST_API_GymTEC.Models
{
    
    //Model to use in the get_all_products
    public class Shortened_Product
    {
        public string codigo_barras { get; set; } = string.Empty;
        public string nombre_producto { get; set; } = string.Empty;
        public double costo { get; set; }
    }
}
