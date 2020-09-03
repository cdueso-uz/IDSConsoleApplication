namespace IDSConsole.Models
{
    public class LoginRequest
    {
        public string Client_Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Realm { get; set; }
        public string Credential_Type { get; set; }
    }
}
