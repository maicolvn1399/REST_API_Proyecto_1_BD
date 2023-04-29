namespace REST_API_GymTEC.Models
{
    //Model to use in the get_all_equipment, add_equipment, update_equipment
    public class Equipment_Shortened
    {
        public int num_serie { get; set; } = 0;
        public string tipo_equipo { get; set; } = string.Empty;
        
    }
}
