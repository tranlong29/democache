
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DemoRedisCache.Datas
{
    public class MyDBContext : IdentityDbContext<ApplicationUser>
    {
        public MyDBContext(DbContextOptions options) : base(options) { }

        public DbSet<SinhVien> SinhViens { get; set; }
        public DbSet<TokenInfo> TokenInfos { get; set; }
        public DbSet<Lop> Lops { get; set; }
        public DbSet<ChuongTrinh> chuongTrinhs { get; set; }
        public DbSet<Khoa> khoas { get; set; }
        public DbSet<KhoaHoc> khoaHocs { get; set; }
        public DbSet<MonHoc> MonHocs { get; set; }
        public DbSet<KetQua> KetQuas { get; set; }
    }
}
