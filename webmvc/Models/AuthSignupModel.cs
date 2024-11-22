using System.ComponentModel.DataAnnotations;

namespace webmvc.Models
{
    public class AuthSignupModel
    {
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Role { get; set; }
    }

}
