namespace REST_API_GymTEC.Models
{
    //Model to use in the get_inventory, add_inventory and update_inventory endpoints
    public class Inventory
    {
        public int num_serie { get; set; }
        public string marca { get; set; }
        public float costo { get; set; }
        public bool is_used { get; set; }
        public string tipo_equipo { get; set; }
        public string nombre_sucursal { get; set; }
    }
}
