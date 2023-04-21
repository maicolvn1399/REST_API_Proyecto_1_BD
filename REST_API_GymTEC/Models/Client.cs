namespace REST_API_GymTEC.Models
{

    //Model to be used in the create_client, auth_client 
    public class Client
    {
        public string cedula_cliente { get; set; } = string.Empty;
        public string nombre { get; set; } = string.Empty; 
        public string apellido_1 { get; set; } = string.Empty;
        public string apellido_2 { get; set; } = string.Empty;
        public string direccion { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public int altura { get; set; } = 0;
        public float peso { get; set; } 
        public string fecha_nacimiento { get; set; } = string.Empty;

    }
}
