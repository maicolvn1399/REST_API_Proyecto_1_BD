namespace REST_API_GymTEC.Models
{

    //Model to use in the get_all_payrolls, get_payroll endpoints
    public class Payroll
    {
        public string empleado_cedula { get; set; } = string.Empty;
        public string nombre_planilla { get; set; } = string.Empty;

        public double salario { get; set; }
    }
}
