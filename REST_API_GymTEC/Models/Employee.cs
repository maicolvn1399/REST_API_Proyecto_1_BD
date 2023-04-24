namespace REST_API_GymTEC.Models
{
    //Model to represent an employee
    public class Employee
    {
        public string cedula { get; set; } = string.Empty;
        public string nombre { get; set; } = string.Empty;
        public string apellido_1 { get; set; } = string.Empty;
        public string apellido_2 { get; set; } = string.Empty;
        public string provincia { get; set; } = string.Empty;
        public string canton { get; set; } = string.Empty;
        public string distrito { get; set; } = string.Empty;
        public double salario { get; set; }
        public string correo { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
    }
}
