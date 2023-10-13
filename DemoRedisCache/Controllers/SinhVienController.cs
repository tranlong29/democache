using DemoRedisCache.Datas;
using DemoRedisCache.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiUtils;
using Utils.Attributes;

namespace DemoRedisCache.Controllers
{
    //[Authorize]
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
        [Route("getAllData")]
        [Cache]
        public IActionResult GetAllData()
        {
            var lstData = _dbContext.SinhViens.ToList();
            return Ok(lstData);
        }
        [HttpGet]
        [Route("getDataByName")]
        [Cache]
        public IActionResult GetDataById(string name)
        {
            var lstData = _dbContext.SinhViens.SingleOrDefault(x => x.Name == name);
            if(lstData == null)
            {
                return NotFound("Không tìm thấy Sinh viên");
            }
            return Ok(lstData);
        }
        [HttpPost]
        [Route("SetData")]
        public IActionResult SetData(SinhVienDTO sinhVien)
        {
            var lstData = new SinhVien
            {
                MaSV = sinhVien.MaSv,
                Name = sinhVien.Name,
                DiaChi = sinhVien.DiaChi,
                NgaySinh = sinhVien.NgaySinh,
                MaLop = sinhVien.MaLop
            };
            _dbContext.Add(lstData);
            _dbContext.SaveChanges();
            cacheDuLieu.RemoveKeys("/api/SinhVien/");
            return Ok(lstData);
        }

        [HttpPut]
        [Route("PutById/{id}")]
        public IActionResult PutById(string id, SinhVienDTO sinhVien)
        {
            try
            {
                var sinhViens = _dbContext.SinhViens.SingleOrDefault(x => x.MaSV == id);
                if (sinhViens == null)
                {
                    return NotFound();
                }
                sinhViens.MaSV = sinhVien.MaSv;
                sinhViens.Name = sinhVien.Name;
                sinhViens.DiaChi = sinhVien.DiaChi;
                sinhViens.NgaySinh = sinhVien.NgaySinh;
                sinhViens.MaLop = sinhVien.MaLop;
                _dbContext.SaveChanges();
                cacheDuLieu.RemoveKeys("/api/SinhVien/");
                return Ok(sinhViens);
            }
            catch
            {
                return BadRequest();
            }


        }
        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                var lstdata = _dbContext.SinhViens.SingleOrDefault(x => x.MaSV == id);
                if (lstdata == null)
                {
                    return NotFound();
                }
                _dbContext.Remove(lstdata);
                _dbContext.SaveChanges();
                cacheDuLieu.RemoveKeys("/api/SinhVien/");
                return Ok("Xóa Thành Công");
            }
            catch
            {
                return BadRequest();
            }

        }
    }
}
