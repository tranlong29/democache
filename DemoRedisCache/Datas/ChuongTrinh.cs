using System.ComponentModel.DataAnnotations;

namespace DemoRedisCache.Datas
{
    public class ChuongTrinh
    {
        [Key]
        [Required]
        public string? MaCT { get; set; }
        public string? TenCT { get; set;}
    }
}
