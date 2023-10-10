using System.ComponentModel.DataAnnotations;

namespace DemoRedisCache.Models
{
    public class SinhVienDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? DiaChi { get; set; }
        public DateTime NgaySinh { get; set; }
    }
}
