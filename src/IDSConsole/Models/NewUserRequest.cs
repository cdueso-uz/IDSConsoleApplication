using System;

namespace IDSConsole.Models
{
    public class NewUserRequest
    {
        public Guid? UUid { get; set; }
        public int? Id { get; set; }
        public string UserZoomName { get; set; }
        public string Password { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string NickName { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Locale { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Email { get; set; }
        public bool EmailVerified { get; set; }
        public string Sub { get; set; }
    }
}