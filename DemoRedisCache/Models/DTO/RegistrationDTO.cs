using System.ComponentModel.DataAnnotations;

namespace DemoRedisCache.Models.DTO
{
    public class RegistrationDTO
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
