namespace REST_API_GymTEC.Models
{

    //Model to be used in the associate_product endpoint 

    public class Associate_product
    {
        public string sucursal { get; set; } = string.Empty;
        public string product { get; set; } = string.Empty;

    }
}
