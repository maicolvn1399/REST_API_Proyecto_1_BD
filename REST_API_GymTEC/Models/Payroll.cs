namespace REST_API_GymTEC.Models
{

    //Model to use in the get_all_payrolls, get_payroll endpoints
    public class Payroll
    {
        public string empleado_cedula { get; set; } = string.Empty;
        public int planilla_id { get; set; } = 0;
        public string descripcion { get; set; } = string.Empty;
    }
}
