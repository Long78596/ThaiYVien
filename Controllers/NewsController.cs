using Microsoft.AspNetCore.Mvc;

namespace ThaiYVien.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DetailNews()
        {
            return View();
        }
    }
}
