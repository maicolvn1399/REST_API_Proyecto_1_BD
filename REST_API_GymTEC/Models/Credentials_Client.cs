namespace REST_API_GymTEC.Models
{
    //Model for login of client, should be used in the auth_client endpoint
    public class Credentials_Client
    {
        public string email { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
    }
}
