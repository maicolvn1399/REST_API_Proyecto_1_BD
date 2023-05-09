namespace REST_API_GymTEC.Models
{

    //Model to use in the get_employee and delete_employee endpoints 
    public class Employee_Identifier
    {
        public string cedula_empleado { get; set; } = string.Empty;
    }
}
