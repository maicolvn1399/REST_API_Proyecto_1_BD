namespace REST_API_GymTEC.Models
{
    //Model to use in get_all_employees endpoint
    public class Employee_Shortened
    {
        public string cedula { get; set; } = string.Empty;
        public string nombre { get; set; } = string.Empty;
        public string apellido_1 { get; set; } = string.Empty;
        public string apellido_2 { get; set; } = string.Empty;
    }
}
