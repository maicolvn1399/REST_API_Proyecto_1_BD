namespace REST_API_GymTEC.Models
{

    //Model to be used in the associate_inventory endpoint 
    public class Associate_Inventory
    {
        public string sucursal { get; set; } = string.Empty;
        public int num_serie { get; set; } = 0;
    }
}
