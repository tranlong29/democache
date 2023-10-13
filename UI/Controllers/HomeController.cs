using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using UI.Models;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _Configure;
        private readonly string apiBaseUrl;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _Configure = configuration;
            apiBaseUrl = _Configure.GetValue<string>("WebApiBaseUrl");
        }

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
                HttpResponseMessage responsel = await client.GetAsync("/api/Lop/getAllData");
                HttpResponseMessage responseK = await client.GetAsync("/api/Khoa/getAllData");
                // Kiểm tra nếu yêu cầu thành công
                if (response.IsSuccessStatusCode && responsel.IsSuccessStatusCode && responseK.IsSuccessStatusCode)
                {
                    string jsonStr = await response.Content.ReadAsStringAsync();
                    string jsonStrl = await responsel.Content.ReadAsStringAsync();
                    string jsonStrK = await responseK.Content.ReadAsStringAsync();
                    List<SinhVienModel> sinhViens = JsonConvert.DeserializeObject<List<SinhVienModel>>(jsonStr);
                    List<LopViewModel> lops = JsonConvert.DeserializeObject<List<LopViewModel>>(jsonStrl);
                    List<KhoaViewModel> khoas = JsonConvert.DeserializeObject<List<KhoaViewModel>>(jsonStrK);
                    if (sinhViens != null && lops != null && khoas != null)
                    {
                        ViewBag.SSV = sinhViens.Count();
                        ViewBag.SLop = lops.Count();
                        ViewBag.SKhoa = khoas.Count();
                    }
                    else
                    {
                        ViewBag.SSV = 0;
                        ViewBag.SLop = 0;
                        ViewBag.SKhoa = 0;
                    }
                    return View();
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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}