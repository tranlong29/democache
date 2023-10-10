using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
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

                // Gửi yêu cầu GET đến API
                HttpResponseMessage response = await client.GetAsync("/api/SinhVien");

                // Kiểm tra nếu yêu cầu thành công
                if (response.IsSuccessStatusCode)
                {
                    // Đọc dữ liệu JSON từ phản hồi
                    string jsonStr = await response.Content.ReadAsStringAsync();
                    List<SinhVienModel> sinhViens = JsonConvert.DeserializeObject<List<SinhVienModel>>(jsonStr);

                    // Trả về dữ liệu đến view
                    return View(sinhViens);
                }
                else
                {
                    // Xử lý lỗi nếu có
                    ViewBag.ErrorMessage = "Không thể truy cập dữ liệu.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                ViewBag.ErrorMessage = "Có lỗi xảy ra: " + ex.Message;
                return View();
            }
        }
    

    }
}
