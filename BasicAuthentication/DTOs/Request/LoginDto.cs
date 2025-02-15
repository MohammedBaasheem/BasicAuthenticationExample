using System.ComponentModel.DataAnnotations;

namespace BasicAuthentication.DTOs.Request
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        
    }
}
