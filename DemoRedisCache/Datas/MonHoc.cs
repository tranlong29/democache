using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoRedisCache.Datas
{
    public class MonHoc
    {
        [Key]
        public string? MaMH { get; set; }
        public string? tenMonHoc { get; set; } 
        public string? MaKhoa { get; set; }
        [ForeignKey("MaKhoa")]
        public Khoa? Khoa { get; set; }
    }
}
