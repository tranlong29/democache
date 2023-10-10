using DemoRedisCache.Datas;
using DemoRedisCache.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiUtils;
using Utils.Attributes;

namespace DemoRedisCache.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SinhVienController : ControllerBase
    {
        private readonly MyDBContext _dbContext;
        protected CacheDuLieu cacheDuLieu = CacheDuLieu.Instance;

        public SinhVienController(MyDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        [Cache]
        public IActionResult GetAllData()
        {
            var lstData = _dbContext.SinhViens.ToList();
            return Ok(lstData);
        }
        [HttpPost]
        public IActionResult SetData(SinhVienDTO sinhVien)
        {
            var lstData = new SinhVien
            {
                Name = sinhVien.Name,
                DiaChi = sinhVien.DiaChi,
                NgaySinh = sinhVien.NgaySinh
            };
            _dbContext.Add(lstData);
            _dbContext.SaveChanges();
            return Ok(lstData);
        }

        [HttpPut("{id}")]
        public IActionResult PutById(int id, SinhVienDTO sinhVien)
        {
            try
            {
                var sinhViens = _dbContext.SinhViens.SingleOrDefault(x => x.Id == id);
                if (sinhVien == null)
                {
                    return NotFound();
                }
                sinhViens.Name = sinhVien.Name;
                sinhViens.DiaChi = sinhVien.DiaChi;
                sinhViens.NgaySinh = sinhVien.NgaySinh;
                _dbContext.SaveChanges();
                return Ok(sinhViens);
            }
            catch
            {
                return BadRequest();
            }


        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var lstdata = _dbContext.SinhViens.SingleOrDefault(x => x.Id == id);
                if (lstdata == null)
                {
                    return NotFound();
                }
                _dbContext.Remove(lstdata);
                _dbContext.SaveChanges();
                return NoContent();
            }
            catch
            {
                return BadRequest();
            }

        }
    }
}
