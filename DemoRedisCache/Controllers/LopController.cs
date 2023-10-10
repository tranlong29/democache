using Microsoft.AspNetCore.Mvc;

namespace DemoRedisCache.Controllers
{
    public class LopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
