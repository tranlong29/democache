using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using UI.Models;

namespace UI.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _Configure;
        private readonly string apiBaseUrl;
        public LoginController(IConfiguration configure)
        {
            _Configure = configure;
            apiBaseUrl = _Configure.GetValue<string>("WebApiBaseUrl");
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> LoginUser(LoginViewModel login)
        {
            using (var httpClient = new HttpClient())
            {
                // Xây dựng URL API bằng cách kết hợp apiBaseUrl và đường dẫn tương ứng
                var apiUrl = $"{apiBaseUrl}/api/Authorzation/Login";

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(apiUrl, stringContent))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var token = await response.Content.ReadAsStringAsync();
                        var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(token);
                        if (!string.IsNullOrEmpty(loginResponse.Token))
                        {
                            // Lưu token vào session
                            HttpContext.Session.SetString("JWToken", token);
                            return Redirect("~/Home/Index");
                        }
                        else
                        {
                            TempData["Message"] = "Mật khẩu Hoặc Tài Khoản không chính xác!!!"; // Hoặc thông báo lỗi tùy theo trường hợp
                            return Redirect("~/Login/Index");
                        }
                    }
                    else
                    {
                        TempData["Message"] = "API request failed"; // Hoặc thông báo lỗi tùy theo trường hợp
                        return Redirect("~/Login/Index");
                    }
                }
            }
        }

        public IActionResult Registration()
        {
            return View();
        }

        public async Task<IActionResult> RegistrationUser(RegistrationViewModel registration)
        {
            using (var httpClient = new HttpClient())
            {
                // Xây dựng URL API bằng cách kết hợp apiBaseUrl và đường dẫn tương ứng
                var apiUrl = $"{apiBaseUrl}/api/Authorzation/Registration";

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(registration), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(apiUrl, stringContent))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Đăng ký thành công!"; // Thông báo thành công hoặc thay đổi tùy theo trường hợp
                        return RedirectToAction("Index", "Login");
                    }
                    else
                    {
                        TempData["Message"] = "Đăng ký thất bại. Vui lòng thử lại."; // Thông báo lỗi hoặc thay đổi tùy theo trường hợp
                        return View();
                    }
                }
            }
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("JWToken");
            return Redirect("~/Login/Index");
        }
    }
}
