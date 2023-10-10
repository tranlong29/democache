using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DemoRedisCache.Datas
{
    public class KhoaHoc
    {
        [Key]
        public string? MaKhoaHoc { get; set; }
        public int NamBatDau { get; set; }
        public int NamKetThuc { get; set; }

    }
}
