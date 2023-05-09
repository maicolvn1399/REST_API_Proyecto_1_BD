namespace REST_API_GymTEC.Models
{

    //Model to use in the add_branch, update_branch, get_branch endpoints
    public class Branch
    {
        public string nombre_sucursal { get; set; } = string.Empty;
        public string fecha_apertura { get; set; } = string.Empty;
        public string horario { get; set; } = string.Empty;
        public int cap_max { get; set; } = 0;
        public string provincia { get; set; } = string.Empty;
        public string canton { get; set; } = string.Empty;
        public string distrito { get; set; } = string.Empty;
        public string manager { get; set; } = string.Empty;
        public List<string> telefonos { get; set; } = new List<string>();
        public bool active_spa { get; set; } = false;
        public bool active_store { get; set; } = false;
    }
}
