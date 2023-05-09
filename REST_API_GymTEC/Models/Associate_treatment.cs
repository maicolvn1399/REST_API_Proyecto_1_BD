namespace REST_API_GymTEC.Models
{

    //Model to be used in the associate_treatment endpoint 

    public class Associate_treatment
    {
        public string sucursal { get; set; } = string.Empty;
        public int tratamiento_id { get; set; } = 0;
    }
}
