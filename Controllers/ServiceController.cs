using Microsoft.AspNetCore.Mvc;

namespace ThaiYVien.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
