using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DemoRedisCache.Datas
{
    public class Khoa
    {
        [Key]
        public string? MaKhoa { get; set; }
        public string? TenKhoa { get; set; }
        public int NamThanhLap { get; set; }
    }
}
