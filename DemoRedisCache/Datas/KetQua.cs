using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoRedisCache.Datas
{
    public class KetQua
    {
        [Key]
        [Required]
        public string? MaSv { get; set; }
        public string? MaMH { get; set; }
        public int lanThi { get; set; }
        public int Diem { get; set; }
        [ForeignKey("MaSv")]
        public SinhVien? SinhVien { get; set; } 
        [ForeignKey("MaMH")]
        public MonHoc? MonHoc { get; set; }
    }
}
