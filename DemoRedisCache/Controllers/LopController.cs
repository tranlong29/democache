using DemoRedisCache.Datas;
using DemoRedisCache.Models;
using DemoRedisCache.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiUtils;
using Utils.Attributes;

namespace DemoRedisCache.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LopController : ControllerBase
    {
        private readonly MyDBContext _dbContext;
        protected CacheDuLieu cacheDuLieu = CacheDuLieu.Instance;

        public LopController(MyDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        [Route("getAllData")]
        [Cache]
        public IActionResult GetAllData()
        {
            var lstData = _dbContext.Lops.ToList();
            return Ok(lstData);
        }
        [HttpGet]
        [Route("getDataById")]
        [Cache]
        public IActionResult GetDataById(string id)
        {
            var lstData = _dbContext.Lops.SingleOrDefault(x => x.MaLop == id);
            if (lstData == null)
            {
                return NotFound("Không tìm thấy Lớp");
            }
            return Ok(lstData);
        }
        [HttpPost]
        [Route("SetData")]
        public IActionResult SetData(LopDTO lop)
        {
            var lstData = new Lop
            {
                MaLop = lop.MaLop,
                MaKhoaHoc = lop.MaKhoaHoc,
                MaKhoa = lop.MaKhoa,
                MaCT = lop.MaCT,
                soThuTu = lop.soThuTu
            };
            _dbContext.Add(lstData);
            _dbContext.SaveChanges();
            cacheDuLieu.RemoveKeys("/api/Lop/");
            return Ok(lstData);
        }
        [HttpPut]
        [Route("PutById/{id}")]
        public IActionResult PutById(string id, LopDTO lop)
        {
            try
            {
                var lops = _dbContext.Lops.SingleOrDefault(x => x.MaLop  == id);
                if (lops == null)
                {
                    return NotFound();
                }
                lops.MaLop = lop.MaLop;
                lops.MaKhoaHoc = lop.MaKhoaHoc;
                lops.MaKhoa = lop.MaKhoa;
                lops.MaCT = lop.MaCT;
                lops.soThuTu = lop.soThuTu;
                _dbContext.SaveChanges();
                cacheDuLieu.RemoveKeys("/api/Lop/");
                return Ok(lops);
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
                var lstdata = _dbContext.Lops.SingleOrDefault(x => x.MaLop == id);
                if (lstdata == null)
                {
                    return NotFound();
                }
                _dbContext.Remove(lstdata);
                _dbContext.SaveChanges();
                cacheDuLieu.RemoveKeys("/api/Lop/");
                return Ok("Xóa Thành Công");
            }
            catch
            {
                return BadRequest();
            }

        }
    }
}
