namespace REST_API_GymTEC.Models
{

    //Model to use in the get_employee, add_employee, update_employee endpoints
    public class Employee_Extended
    {
        public string cedula_empleado { get; set; } = string.Empty;
        public string nombre { get; set; } = string.Empty;
        public string apellido_1 { get; set; } = string.Empty;
        public string apellido_2 { get; set; } = string.Empty;
        public string provincia { get; set; } = string.Empty;
        public string canton { get; set; } = string.Empty;
        public string distrito { get; set; } = string.Empty;
        public double salario { get; set; } = 0.0f;
        public string correo { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public string nombre_sucursal { get; set; } = string.Empty;
        public string puesto_descripcion { get; set; } = string.Empty;
        public string planilla_descripcion { get; set; } = string.Empty;
    }
}
