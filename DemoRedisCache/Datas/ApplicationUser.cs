using Microsoft.AspNetCore.Identity;

namespace DemoRedisCache.Datas
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
    }
}
