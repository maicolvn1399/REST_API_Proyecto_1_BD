namespace REST_API_GymTEC.Models
{

    //Model for login of worker, should be used in the auth_worker endpoint
    public class Credentials_Worker
    {
        public string cedula { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
    }
}
