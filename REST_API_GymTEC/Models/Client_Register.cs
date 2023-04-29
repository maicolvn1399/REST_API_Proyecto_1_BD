namespace REST_API_GymTEC.Models
{
    public class Client_Register
    {
        public string cedula_cliente { get; set; } = string.Empty;
        public string nombre { get; set; } = string.Empty;
        public string apellido_1 { get; set; } = string.Empty;
        public string apellido_2 { get; set; } = string.Empty;
        public string direccion { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public int altura { get; set; }
        public double peso { get; set; }
        public string fecha_nac { get; set;} = string.Empty;
    }
}
