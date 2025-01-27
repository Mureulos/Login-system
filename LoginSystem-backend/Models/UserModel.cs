using System.Text.Json.Serialization;

namespace LoginSystem.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [JsonIgnore] public string Password { get; set; }
        public string Phone { get; set; }
    }
}
