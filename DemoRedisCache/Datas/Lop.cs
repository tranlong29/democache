using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoRedisCache.Datas
{
    public class Lop
    {
        [Key]
        [Required]
        public string? MaLop { get; set; }
        public string? MaKhoaHoc { get;}
        public string? MaKhoa { get; set; }
        public string? MaCT { get; set; }
        public int soThuTu { get; set; }

        [ForeignKey("MaKhoaHoc")]
        public KhoaHoc? KhoaHoc { get; set; }
        [ForeignKey("MaKhoa")]
        public Khoa? Khoa { get; set; }
        [ForeignKey("MaCT")]
        public ChuongTrinh? ChuongTrinh { get; set; }
        public ICollection<SinhVien>? SinhViens { get; set; }
    }
}
