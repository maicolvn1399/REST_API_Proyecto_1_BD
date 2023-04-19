namespace REST_API_GymTEC.Models
{

    //Model to be used in the associate_inventory endpoint 
    public class Associate_Inventory
    {
        public string sucursal { get; set; } = string.Empty;
        public string equipo { get; set; } = string.Empty;
    }
}
