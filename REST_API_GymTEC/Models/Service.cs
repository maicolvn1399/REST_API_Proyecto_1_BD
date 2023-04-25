namespace REST_API_GymTEC.Models
{
    //Model to use in add_service, get_services endpoints
    public class Service
    {
        public string nombre_sucursal { get; set; } = string.Empty;
        public string servicio { get; set; } = string.Empty;
    }
}
