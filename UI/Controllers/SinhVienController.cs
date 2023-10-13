using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;
using UI.Models;

namespace UI.Controllers
{
    public class SinhVienController : Controller
    {
        private readonly ILogger<SinhVienController> _logger;
        private readonly IConfiguration _Configure;
        private readonly string apiBaseUrl;
        public SinhVienController(ILogger<SinhVienController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _Configure = configuration;
            apiBaseUrl = _Configure.GetValue<string>("WebApiBaseUrl"); 
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
               var accessToken = HttpContext.Session.GetString("JWToken");
               JObject jsonObj = JsonConvert.DeserializeObject<JObject>(accessToken);

                // Lấy giá trị của trường "token"
                string token = jsonObj["token"].ToString();
                // Tạo một HttpClient
                HttpClient client = new HttpClient();
                // Đặt địa chỉ cơ sở của API
                client.BaseAddress = new Uri(apiBaseUrl);
                
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                 HttpResponseMessage response = await client.GetAsync("/api/SinhVien/getAllData");
                // Kiểm tra nếu yêu cầu thành công
                if (response.IsSuccessStatusCode)
                {
                    string jsonStr = await response.Content.ReadAsStringAsync();
                    List<SinhVienModel> sinhViens = JsonConvert.DeserializeObject<List<SinhVienModel>>(jsonStr);
                    return View(sinhViens);
                }
                else
                {
                    ViewBag.ErrorMessage = "Không thể truy cập dữ liệu.";
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                ViewBag.ErrorMessage = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        
        public async Task<IActionResult> GetData(string name)
        {
            HttpClient client = new HttpClient();
            List<SinhVienModel> sinhViens;
            client.BaseAddress = new Uri(apiBaseUrl);
            if (!string.IsNullOrEmpty(name))
            {
                HttpResponseMessage response = await client.GetAsync($"/api/SinhVien/getDataByName?name={name}"); // Use 'name' as a parameter
                string jsonStr = await response.Content.ReadAsStringAsync();
                SinhVienModel sinhVien = JsonConvert.DeserializeObject<SinhVienModel>(jsonStr);
                sinhViens = new List<SinhVienModel> { sinhVien };
                return RedirectToAction("Index", "SinhVien");
            }
            else
            {
                // Xử lý lỗi nếu có
                ViewBag.ErrorMessage = "Không thể truy cập dữ liệu.";
                return View();
            }
            
        }
        
        public async Task<IActionResult> Create()
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(apiBaseUrl);
            HttpResponseMessage response = await client.GetAsync("/api/Lop/getAllData"); // Use 'name' as a parameter
            string jsonStr = await response.Content.ReadAsStringAsync();
            List<LopViewModel> lop = JsonConvert.DeserializeObject<List<LopViewModel>>(jsonStr);
            ViewBag.Lop = new SelectList(lop, "MaLop", "MaLop");
            return View();
        }

        public async Task<IActionResult> Creater(SinhVienModel sinhvien)
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            JObject jsonObj = JsonConvert.DeserializeObject<JObject>(accessToken);

            // Lấy giá trị của trường "token"
            string token = jsonObj["token"].ToString();
            using (var httpClient = new HttpClient())
            {
                
                httpClient.BaseAddress = new Uri(apiBaseUrl);
                // Xây dựng URL API bằng cách kết hợp apiBaseUrl và đường dẫn tương ứng
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(sinhvien), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("/api/SinhVien/SetData", stringContent))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Thêm mới thành công!"; // Thông báo thành công hoặc thay đổi tùy theo trường hợp
                        return RedirectToAction("Index", "SinhVien");
                    }
                    else
                    {
                        TempData["Message"] = "Thêm mới thất bại. Vui lòng thử lại."; // Thông báo lỗi hoặc thay đổi tùy theo trường hợp
                        return View();
                    }
                }
            }
        }

    }
}
