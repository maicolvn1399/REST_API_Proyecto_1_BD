namespace REST_API_GymTEC.Models
{

    //Model to be used in the create_class endpoint and get_all_classes 
    public class Class
    {
        public int clase_id { get; set; } = 0;
        public string servicio { get; set; } = string.Empty;
        public string modo { get; set; } = string.Empty;
        public int capacidad { get; set; } = 0;
        public string fecha { get; set; } = string.Empty;
        public string hora_ingreso { get; set; } = string.Empty;
        public string hora_salida { get; set; } = string.Empty;
        public string encargado { get; set; } = string.Empty;
    }
}
